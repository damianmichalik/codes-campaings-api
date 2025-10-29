Feature: Campaigns management
  As a developer
  I want to have a set of endpoints to manage campaigns
  
  Background:
    Given the following campaigns exist:
      | Id                                   | Name         |
      | 11111111-1111-1111-1111-111111111111 | Campaign One |
      | 22222222-2222-2222-2222-222222222222 | Campaign Two |

  Scenario: Get all campaigns
    When I send a GET request to "/api/campaigns" with headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    Then the response status code should be 200
    And the response should match JSON:
    """
    [
      { "id": "11111111-1111-1111-1111-111111111111", "name": "Campaign One" },
      { "id": "22222222-2222-2222-2222-222222222222", "name": "Campaign Two" }
    ]
    """

  Scenario: Unauthorized access is denied when no API key is provided for retrieving all campaigns
    When I send a GET request to "/api/campaigns"
    Then the response status code should be 401

  Scenario: Access is forbidden when an invalid API key is provided for retrieving all campaigns  
    When I send a GET request to "/api/campaigns" with headers:
      | Key       | Value         |
      | X-API-KEY | invalid-key   |
    Then the response status code should be 403
      
  Scenario: Get single campaign by ID
    When I send a GET request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    Then the response status code should be 200
    And the response should match JSON:
    """
    { "id": "11111111-1111-1111-1111-111111111111", "name": "Campaign One" }
    """
      
  Scenario: Get single campaign by unexisting ID
    When I send a GET request to "/api/campaigns/33333333-3333-3333-3333-333333333333" with headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    Then the response status code should be 404

  Scenario: Unauthorized access is denied when no API key is provided for retrieving single campaign
    When I send a GET request to "/api/campaigns/11111111-1111-1111-1111-111111111111"
    Then the response status code should be 401

  Scenario: Access is forbidden when an invalid API key is provided for retrieving single campaign  
    When I send a GET request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with headers:
      | Key       | Value       |
      | X-API-KEY | invalid-key |
    Then the response status code should be 403
        
  Scenario: Create a new campaign
    When I set the following headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    And I send a POST request to "/api/campaigns" with body:
    """
    {
      "name": "New Campaign"
    }
    """
    Then the response status code should be 201
    Then there are following Campaigns in the database
      | Name         |
      | Campaign One |
      | Campaign Two |
      | New Campaign |
        
  Scenario: Validation for creating campaing
    When I set the following headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    And I send a POST request to "/api/campaigns" with body:
    """
    {
      "name": ""
    }
    """
    Then the response status code should be 400

  Scenario: Unauthorized access is denied when no API key is provided for creating campaign
    When I send a POST request to "/api/campaigns" with body:
    """
    {
      "name": "new"
    }
    """
    Then the response status code should be 401

  Scenario: Access is forbidden when an invalid API key is provided for creating campaign  
    When I set the following headers:
      | Key       | Value        |
      | X-API-KEY | invalid-key  |
    When I send a POST request to "/api/campaigns" with body:
    """
    {
      "name": "new"
    }
    """
    Then the response status code should be 403
        
  Scenario: Update a campaign
    When I set the following headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    And I send a PUT request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with body:
    """
    {
      "name": "Updated campaign one"
    }
    """
    Then the response status code should be 204
    Then there are following Campaigns in the database
      | Name                 |
      | Updated campaign one |
      | Campaign Two         |
        
  Scenario: Validation for updating campaing
    When I set the following headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    And I send a PUT request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with body:
    """
    {
      "name": ""
    }
    """
    Then the response status code should be 400
      
  Scenario: Update a not existing campaign
    When I set the following headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    And I send a PUT request to "/api/campaigns/33333333-3333-3333-3333-333333333333" with body:
    """
    {
      "name": "Updated campaign"
    }
    """
    Then the response status code should be 404

  Scenario: Unauthorized access is denied when no API key is provided for updating campaign
    When I send a PUT request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with body:
    """
    {
      "name": "new"
    }
    """
    Then the response status code should be 401

  Scenario: Access is forbidden when an invalid API key is provided for updating campaign  
    When I set the following headers:
      | Key       | Value       |
      | X-API-KEY | invalid-key |
    When I send a PUT request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with body:
    """
    {
      "name": "new"
    }
    """
    Then the response status code should be 403
      
  Scenario: Delete a campaign
    When I send a DELETE request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    Then the response status code should be 204
    Then there are following Campaigns in the database
      | Name                 |
      | Campaign Two         |
      
  Scenario: Delete a not existing campaign
    When I send a DELETE request to "/api/campaigns/33333333-3333-3333-3333-333333333333" with headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    Then the response status code should be 404

  Scenario: Unauthorized access is denied when no API key is provided for deleting campaign
    When I send a DELETE request to "/api/campaigns/11111111-1111-1111-1111-111111111111"
    Then the response status code should be 401

  Scenario: Access is forbidden when an invalid API key is provided for deleting campaign  
    When I send a DELETE request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with headers:
      | Key       | Value       |
      | X-API-KEY | invalid-key |
    Then the response status code should be 403
        
  Scenario: Create codes in campaign
    When I set the following headers:
      | Key       | Value               |
      | X-API-KEY | my-super-secret-key |
    And I send a POST request to "/api/campaigns/11111111-1111-1111-1111-111111111111/codes" with body:
    """
    {
      "count": 10,
      "value": 20,
      "currency": "PLN"
    }
    """
    And I wait for the jobs to complete
    Then the response status code should be 204
    And there are 10 TopUps elements in the database