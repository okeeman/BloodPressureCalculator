import http from 'k6/http';
import { check, sleep } from 'k6';
import { parseHTML } from 'k6/html';

const baseUrl = (__ENV.BASE_URL || 'https://bloodpressurewebapp-dshyerhxb9h4bwgk.italynorth-01.azurewebsites.net/').replace(/\/+$/, '');
const maxVUs = Number.parseInt(__ENV.MAX_VUS || '10', 10);

const readings = [
  { systolic: 80, diastolic: 50, category: 'Low:' },
  { systolic: 110, diastolic: 70, category: 'Ideal:' },
  { systolic: 125, diastolic: 75, category: 'Pre-High:' },
  { systolic: 150, diastolic: 95, category: 'High:' },
];

export const options = {
  scenarios: {
    calculator_load: {
      executor: 'ramping-vus',
      startVUs: 0,
      stages: [
        { duration: '30s', target: maxVUs },
        { duration: '1m', target: maxVUs },
        { duration: '15s', target: 0 },
      ],
    },
  },
  thresholds: {
    checks: ['rate>0.99'],
    http_req_duration: ['p(95)<1000'],
    http_req_failed: ['rate<0.01'],
  },
   cloud: {
    projectID: 8495420,
  },
};

export default function () {
  const page = http.get(`${baseUrl}/`);
  const pageLoaded = check(page, {
    'calculator page responds successfully': (response) => response.status === 200,
  });

  if (!pageLoaded) {
    return;
  }

  const token = parseHTML(page.body)
    .find('input[name="__RequestVerificationToken"]')
    .attr('value');

  if (!check(token, {
    'calculator page includes an antiforgery token': (value) => Boolean(value),
  })) {
    return;
  }

  const reading = readings[__ITER % readings.length];
  const result = http.post(
    `${baseUrl}/`,
    {
      '__RequestVerificationToken': token,
      'BP.Systolic': String(reading.systolic),
      'BP.Diastolic': String(reading.diastolic),
    },
    {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
    },
  );

  check(result, {
    'calculator submission responds successfully': (response) => response.status === 200,
    [`reading is classified as ${reading.category}`]: (response) =>
      response.body.includes(reading.category),
  });

  sleep(1);
}
