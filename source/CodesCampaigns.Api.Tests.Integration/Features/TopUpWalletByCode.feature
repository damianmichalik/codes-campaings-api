Feature: Use wallet top up code
  As a API client
  I want to request with top up code

  Background:
    Given there is top up code to use "826d3c3e-f0ff-4f3e-b2f3-8f5cb97ee57d" of 100 PLN for campaign "TEST"
    And Today's date is "2017-01-28"
    And I added json content type to header

  Scenario: Successful top up wallet
    When I send a POST request to "/api/v1/top_up_codes" with json:
    """
    {
      "partnerCode": "PARTNER_CODE",
      "code": "826d3c3e-f0ff-4f3e-b2f3-8f5cb97ee57d",
      "email": "test@domain.com"
    }
    """
    Then the response status code should be 200
    And the JSON should be equal to:
    """
    {
      "success": true
    }
    """
    And the top up code "826d3c3e-f0ff-4f3e-b2f3-8f5cb97ee57d" is used
