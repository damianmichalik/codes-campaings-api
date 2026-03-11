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
      "error_code": "usage_limit_exceeded_in_campaign"
    }
    """
    And the top-up code "826d3c3e-f0ff-4f3e-b2f3-8f5cb97ee57d" is not used

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
      "error_code": "usage_limit_exceeded_in_campaign"
    }
    """
    And the top-up code "826d3c3e-f0ff-4f3e-b2f3-8f5cb97ee57d" is not used
  
  Scenario: Unsuccessful top up wallet when email address is incorrect
    When I send a POST request to "/api/v1/top_up_codes" with json:
    """
    {
      "partnerCode": "PARTNER_CODE",
      "code": "826d3c3e-f0ff-4f3e-b2f3-8f5cb97ee57d",
      "email": "invalid_email"
    }
    """
    Then the response status code should be 200
    And the JSON should be equal to:
    """
    {
      "success": false,
      "error_code": "invalid_data"
    }
    """
    
  Scenario: Unsuccessful top up wallet when top up code is already used
    Given there is used top up code "3da10731-4a2e-41d3-a35c-a0f71ceee54c" of 100 PLN without campaign
    When I send a POST request to "/api/v1/top_up_codes" with json:
    """
    {
      "partnerCode": "PARTNER_CODE",
      "code": "3da10731-4a2e-41d3-a35c-a0f71ceee54c",
      "email": "test@domain.com"
    }
    """
    Then the response status code should be 200
    And the JSON should be equal to:
    """
    {
      "success": false,
      "error_code": "invalid_data"
    }
    """
    
  Scenario: Unsuccessful top up wallet when top up code does not exist
    When I send a POST request to "/api/v1/top_up_codes" with json:
    """
    {
      "partnerCode": "PARTNER_CODE",
      "code": "3da10731-4a2e-41d3-a35c-a0f71ceee54c",
      "email": "test@domain.com"
    }
    """
    Then the response status code should be 200
    And the JSON should be equal to:
    """
    {
      "success": false,
      "error_code": "invalid_data"
    }
    """
    
  Scenario: Unsuccessful top up wallet when top up code is not active yet
    Given there is top up code "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d" of 100 PLN that is active from "2017-01-29 00:00:00"
    When I send a POST request to "/api/v1/top_up_codes" with json:
    """
    {
      "partnerCode": "PARTNER_CODE",
      "code": "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d",
      "email": "test@domain.com"
    }
    """
    Then the response status code should be 200
    And the JSON should be equal to:
    """
    {
      "success": false,
      "error_code": "invalid_data"
    }
    """
    And the top-up code "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d" is not used
    
  Scenario: Successful top up wallet when top up code is active
    Given there is top up code "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d" of 100 PLN that is active from "2017-01-29 00:00:00"
    And Today's date is "2017-01-29 00:01:00"
    When I send a POST request to "/api/v1/top_up_codes" with json:
    """
    {
      "partnerCode": "PARTNER_CODE",
      "code": "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d",
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
    And the top up code "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d" is used
    
  Scenario: Unsuccessful top up wallet when top up code is expired
    Given there is top up code "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d" of 100 PLN that is expired at "2017-02-01 00:00:00"
    And Today's date is "2017-02-01 00:01:00"
    When I send a POST request to "/api/v1/top_up_codes" with json:
    """
    {
      "partnerCode": "PARTNER_CODE",
      "code": "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d",
      "email": "test@domain.com"
    }
    """
    Then the response status code should be 200
    And the JSON should be equal to:
    """
    {
      "success": false,
      "error_code": "code_expired"
    }
    """
    And the top-up code "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d" is not used
    
  Scenario: Successful top up wallet when top up code is not expired
    Given there is top up code "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d" of 100 PLN that is expired at "2017-02-02 23:59:59"
    And Today's date is "2017-02-02 01:23:45"
    When I send a POST request to "/api/v1/top_up_codes" with json:
    """
    {
      "partnerCode": "PARTNER_CODE",
      "code": "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d",
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
    And the top up code "e45bbd55-83e6-4d77-ba2d-6ee8cae5434d" is used
    
  Scenario: Successful top up wallet with wallet expiration date
    Given there is top up code "41deaa8e-17dd-4444-8b66-aae06f51aa27" of 100 PLN with wallet expiration date "2017-02-02 23:59:59"
    And Today's date is "2017-02-02 01:23:45"
    When I send a POST request to "/api/v1/top_up_codes" with json:
    """
    {
      "partnerCode": "PARTNER_CODE",
      "code": "41deaa8e-17dd-4444-8b66-aae06f51aa27",
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
    And the top up code "41deaa8e-17dd-4444-8b66-aae06f51aa27" is used
    
  Scenario: Unsuccessful top up wallet when campaign limit is reached for user
    Given there is top up campaign with code "TEST" and name "Test campaign" and max number of top ups per user is 1
    And there is used top up code "6d981503-7deb-4592-a26d-0871653b74c7" of 100 PLN for campaign "TEST" at "2017-01-01"
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
      "error_code": "usage_limit_exceeded_in_campaign"
    }
    """