Feature: Blood pressure category calculator
  As a visitor to the calculator
  I want to submit blood pressure readings
  So that I can see their category and get feedback for invalid readings

  Background:
    Given the calculator page is open

  Scenario Outline: A valid reading displays its blood pressure category
    When I submit a reading with systolic <systolic> and diastolic <diastolic>
    Then the page displays "<category>"

    Examples:
      | systolic | diastolic | category                                                                                                               |
      | 85       | 55        | Low: Consider consulting a healthcare professional if you experience dizziness, fainting, or other symptoms.          |
      | 110      | 70        | Ideal: Maintain your healthy habits and continue regular monitoring.                                                   |
      | 120      | 80        | Pre-High: Adopt lifestyle changes such as reducing salt intake and increasing physical activity.                       |
      | 140      | 90        | High: Consult a healthcare professional about managing your blood pressure and reducing health risks.                  |

  Scenario: A systolic reading that is not greater than diastolic is rejected
    When I submit a reading with systolic 100 and diastolic 100
    Then the page displays the validation error "Systolic must be greater than Diastolic"

  Scenario: A reading outside the accepted range is rejected
    When I submit a reading with systolic 69 and diastolic 60
    Then the page displays the validation error "Invalid Systolic Value"
