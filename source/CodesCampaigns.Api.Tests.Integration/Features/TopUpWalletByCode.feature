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

  Scenario: Unsuccessful wallet top-up more than 3 times in a month per user and campaign
    Given Today's date is "2017-01-28"
    And there is used top up code "54d86883-d5ef-45c3-b96f-756cdd7fe1e9" of 100 PLN for campaign "TEST" at "2017-01-01"
    And there is used top up code "cb9e4ee3-5d3f-432b-aca5-85eb2301a72c" of 100 PLN for campaign "TEST" at "2017-01-23"
    And there is used top up code "404b8249-c3f8-4edd-b832-a5942f308504" of 100 PLN for campaign "TEST" at "2017-01-28"
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
      "success": false,
      "error_code": "usage_limit_exceeded"
    }
    """
    And the top-up code "826d3c3e-f0ff-4f3e-b2f3-8f5cb97ee57d" is not used

  Scenario: Successful 4th wallet top-up after three in the last month within the same campaign and user
    Given Today's date is "2017-02-01"
    And there is used top up code "54d86883-d5ef-45c3-b96f-756cdd7fe1e9" of 100 PLN for campaign "TEST" at "2017-01-01"
    And there is used top up code "cb9e4ee3-5d3f-432b-aca5-85eb2301a72c" of 100 PLN for campaign "TEST" at "2017-01-23"
    And there is used top up code "404b8249-c3f8-4edd-b832-a5942f308504" of 100 PLN for campaign "TEST" at "2017-01-28"
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

  Scenario: Successful wallet top-up more than 3 times in a month per user using different campaign
    Given there is used top up code "1406b31b-5c0f-4e1e-bd04-589422b7708c" of 100 PLN for campaign "ANOTHERCAMPAIGN"
    And there is used top up code "57db4e26-a77b-4cef-b575-7768cc7df50d" of 100 PLN for campaign "ANOTHERCAMPAIGN"
    And there is used top up code "e1a38a05-e984-4a64-8b52-1c9043b52026" of 100 PLN for campaign "ANOTHERCAMPAIGN"
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

  Scenario: Unsuccessful wallet top-up more than 3 times in a month per user and campaign on the first day of the month
    Given Today's date is "2017-01-01 11:24:30"
    And there is used top up code "54d86883-d5ef-45c3-b96f-756cdd7fe1e9" of 100 PLN for campaign "TEST" at "2017-01-01 08:00:00"
    And there is used top up code "cb9e4ee3-5d3f-432b-aca5-85eb2301a72c" of 100 PLN for campaign "TEST" at "2017-01-01 09:00:00"
    And there is used top up code "404b8249-c3f8-4edd-b832-a5942f308504" of 100 PLN for campaign "TEST" at "2017-01-01 10:00:00"
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
      "success": false,
      "error_code": "usage_limit_exceeded"
    }
    """
    And the top-up code "826d3c3e-f0ff-4f3e-b2f3-8f5cb97ee57d" is not used
