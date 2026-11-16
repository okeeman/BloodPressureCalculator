import http from 'k6/http';
import { check, sleep } from 'k6';
import { parseHTML } from 'k6/html';

const baseUrl = (__ENV.BASE_URL || 'https://bloodpressurewebapp-hnd9b7dafpfeawa7.francecentral-01.azurewebsites.net/').replace(/\/+$/, '');
const maxVUs = Number.parseInt(__ENV.MAX_VUS || '10', 10);

const readings = [
  { systolic: [70, 89], diastolic: [40, 59], category: 'Low:' },
  { systolic: [90, 119], diastolic: [60, 79], category: 'Ideal:' },
  { systolic: [120, 139], diastolic: [60, 89], category: 'Pre-High:' },
  { systolic: [140, 190], diastolic: [90, 100], category: 'High:' },
];

if (!Number.isInteger(maxVUs) || maxVUs < readings.length) {
  throw new Error(`MAX_VUS must be an integer of at least ${readings.length} so every category can run concurrently.`);
}

const baseVUsPerScenario = Math.floor(maxVUs / readings.length);
const additionalVUs = maxVUs % readings.length;
const scenarios = Object.fromEntries(
  readings.map((_, index) => [
    `category_${index + 1}`,
    {
      executor: 'ramping-vus',
      exec: `testCategory${index + 1}`,
      startVUs: 0,
      stages: [
        { duration: '30s', target: baseVUsPerScenario + (index < additionalVUs ? 1 : 0) },
        { duration: '1m', target: baseVUsPerScenario + (index < additionalVUs ? 1 : 0) },
        { duration: '15s', target: 0 },
      ],
    },
  ]),
);

export const options = {
  scenarios,
  thresholds: {
    checks: [{ threshold: 'rate>0.99', abortOnFail: true, delayAbortEval: '10s' }],
    http_req_duration: [{ threshold: 'p(95)<1000', abortOnFail: true, delayAbortEval: '10s' }],
    http_req_failed: [{ threshold: 'rate<0.01', abortOnFail: true, delayAbortEval: '10s' }],
  },
  cloud: {
    projectID: 8495420,
    distribution: {
      dublin: { loadZone: 'amazon:ie:dublin', percent: 100 },
    },
  },
};

function submitReading(reading) {
  const systolic = randomValue(reading.systolic);
  const diastolic = randomValue(reading.diastolic);
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

  const result = http.post(
    `${baseUrl}/`,
    {
      '__RequestVerificationToken': token,
      'BP.Systolic': String(systolic),
      'BP.Diastolic': String(diastolic),
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

function randomValue([minimum, maximum]) {
  return Math.floor(Math.random() * (maximum - minimum + 1)) + minimum;
}

export function testCategory1() {
  submitReading(readings[0]);
}

export function testCategory2() {
  submitReading(readings[1]);
}

export function testCategory3() {
  submitReading(readings[2]);
}

export function testCategory4() {
  submitReading(readings[3]);
}
