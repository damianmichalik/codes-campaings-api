Feature: Campaigns management
    As a developer
    I want to have a set of endpoints to manage campaigns
    
    Background:
        Given the following campaigns exist:
          | Id                                   | Name         |
          | 11111111-1111-1111-1111-111111111111 | Campaign One |
          | 22222222-2222-2222-2222-222222222222 | Campaign Two |

    Scenario: Get all campaigns
        When I send a GET request to "/api/campaigns"
        Then the response status code should be 200
        And the response should match JSON:
        """
        [
          { "id": "11111111-1111-1111-1111-111111111111", "name": "Campaign One" },
          { "id": "22222222-2222-2222-2222-222222222222", "name": "Campaign Two" }
        ]
        """
        
    Scenario: Get single campaign by ID
        When I send a GET request to "/api/campaigns/11111111-1111-1111-1111-111111111111"
        Then the response status code should be 200
        And the response should match JSON:
        """
        { "id": "11111111-1111-1111-1111-111111111111", "name": "Campaign One" }
        """
        
    Scenario: Get single campaign by unexisting ID
        When I send a GET request to "/api/campaigns/33333333-3333-3333-3333-333333333333"
        Then the response status code should be 404
        
    Scenario: Create a new campaign
        When I send a POST request to "/api/campaigns" with body:
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
        When I send a POST request to "/api/campaigns" with body:
        """
        {
          "name": ""
        }
        """
        Then the response status code should be 400
        
    Scenario: Update a campaign
        When I send a PUT request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with body:
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
        When I send a PUT request to "/api/campaigns/11111111-1111-1111-1111-111111111111" with body:
        """
        {
          "name": ""
        }
        """
        Then the response status code should be 400
        
    Scenario: Update a not existing campaign
        When I send a PUT request to "/api/campaigns/33333333-3333-3333-3333-333333333333" with body:
        """
        {
          "name": "Updated campaign"
        }
        """
        Then the response status code should be 404
        
    Scenario: Delete a campaign
        When I send a DELETE request to "/api/campaigns/11111111-1111-1111-1111-111111111111"
        Then the response status code should be 204
        Then there are following Campaigns in the database
          | Name                 |
          | Campaign Two         |
        
    Scenario: Delete a not existing campaign
        When I send a DELETE request to "/api/campaigns/33333333-3333-3333-3333-333333333333"
        Then the response status code should be 404