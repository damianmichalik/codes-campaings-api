using System.Reflection;
using System.Text;
using System.Text.Json;
using CodesCampaigns.Api.Tests.Integration.Hooks;
using CodesCampaigns.Infrastructure.DAL;
using CodesCampaigns.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;

namespace CodesCampaigns.Api.Tests.Integration.StepDefinitions;

[Binding]
internal sealed class CampaignsSteps
{
    private HttpResponseMessage? _response;
    private readonly Dictionary<string, string> _headers = new(StringComparer.OrdinalIgnoreCase);
    
    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = true
    };
    
    [BeforeScenario]
    public void ClearHeaders()
        => _headers.Clear();
    
    [Given(@"I set the following headers:")]
    public void GivenISetTheFollowingHeaders(Table table)
    {
        _headers.Clear();
        foreach (var row in table.Rows)
        {
            var key = row["Key"];
            var value = row["Value"];
            _headers[key] = value;
        }
    }
    
    [Given(@"the following campaigns exist:")]
    public static void GivenTheFollowingCampaignsExist(Table table)
    {
        using var scope = FeatureHooks.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        foreach (var row in table.Rows)
        {
            var id = Guid.Parse((string)row["Id"]);
            var name = row["Name"];
            var campaign = new Campaign
            {
                Id = id,
                Name = name,
            };
            db.Campaigns.Add(campaign);
        }

        db.SaveChanges();
    }
    
    [Given(@"the following topups exist:")]
    public static void GivenTheFollowingTopUpsExist(Table table)
    {
        using var scope = FeatureHooks.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        foreach (var row in table.Rows)
        {
            var campaignId = Guid.Parse((string)row["CampaignId"]);
            var code = Guid.Parse((string)row["Code"]);
            var amount = Convert.ToDecimal((string?)row["Amount"], null);
            var campaign = new TopUp
            {
                Code = code,
                CampaignId = campaignId ,
                Amount = amount,
                Currency = row["Currency"]
            };
            db.TopUps.Add(campaign);
        }

        db.SaveChanges();
    }
    
    [When(@"I set the following headers:")]
    public void WhenISetTheFollowingHeaders(Table table)
    {
        _headers.Clear();
        foreach (var row in table.Rows)
        {
            var key = row["Key"];
            var value = row["Value"];
            _headers[key] = value;
        }
    }

    [When(@"I send a (GET|DELETE) request to ""(.*)""")]
    public async Task WhenISendARequestTo(string method, string endpoint)
        => await SendRequestAsync(method, endpoint, null);

    [When(@"I send a (POST|PUT|PATCH) request to ""(.*)"" with body:")]
    public async Task WhenISendARequestToWithBody(string method, string endpoint, string body)
        => await SendRequestAsync(method, endpoint, body);

    [When(@"I send a (POST|PUT|PATCH) request to ""(.*)"" with json:")]
    public async Task WhenISendARequestToWithJson(string method, string endpoint, string body)
        => await SendRequestAsync(method, endpoint, body);
    
    [When(@"I send a (GET|DELETE|POST|PUT|PATCH) request to ""(.*)"" with headers:")]
    public async Task WhenISendARequestToWithHeaders(string method, string endpoint, Table table)
    {
        foreach (var row in table.Rows)
        {
            _headers[row["Key"]] = row["Value"];
        }

        await SendRequestAsync(method, endpoint, null);
    }

    [When(@"I wait for the jobs to complete")]
    public static async Task WhenIWaitForTheJobsToComplete()
        => await Task.Delay(200);

    [Then(@"the response status code should be (\d+)")]
    public async Task ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
    {
        Assert.NotNull(_response);
        var actualStatusCode = (int)_response!.StatusCode;
        if (actualStatusCode != expectedStatusCode)
        {
            var body = await _response.Content.ReadAsStringAsync();
            Assert.Fail($"Expected status {expectedStatusCode} but got {actualStatusCode}. Body: {body}");
        }
    }
    
    [Then(@"the JSON should be equal to:")]
    public async Task ThenTheJsonShouldBeEqualTo(string expectedJson)
        => await AssertJsonEqual(expectedJson);

    [Then(@"the response should match JSON:")]
    public async Task ThenTheResponseShouldMatchJson(string expectedJson)
        => await AssertJsonEqual(expectedJson);

    private async Task AssertJsonEqual(string expectedJson)
    {
        var actualJson = await _response!.Content.ReadAsStringAsync();

        // Parse both to JsonDocument for structural comparison
        using var expectedDoc = JsonDocument.Parse(expectedJson);
        using var actualDoc = JsonDocument.Parse(actualJson);

        // Serialize normalized JSON for comparison
        static string Normalize(JsonDocument doc) =>
            JsonSerializer.Serialize(doc, WriteOptions);

        Assert.Equal(Normalize(expectedDoc), Normalize(actualDoc));
    }
    
    [Then(@"there are following (.*) in the database")]
    public static void ThenThereAreFollowingEntitiesInTheDatabase(string entityName, Table table)
    {
        // Normalize (handle plural forms like "Campaigns")
        var singularName = entityName.TrimEnd('s');

        // Resolve AppDbContext from DI
        using var scope = FeatureHooks.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Find entity type (e.g. "Campaign" -> typeof(Campaign))
        var entityType = Assembly.GetAssembly(typeof(Campaign))!
            .GetTypes()
            .FirstOrDefault(t => string.Equals(t.Name, singularName, StringComparison.OrdinalIgnoreCase));

        Assert.NotNull(entityType);

        // Find DbSet<T> property in DbContext
        var dbSetProp = Enumerable
            .FirstOrDefault<PropertyInfo>(db.GetType().GetProperties(), p =>
                p.PropertyType.IsGenericType &&
                p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                p.PropertyType.GetGenericArguments()[0] == entityType);

        Assert.NotNull(dbSetProp);

        // Get IQueryable<T> from the DbSet<T>
        var dbSetValue = dbSetProp.GetValue(db);
        var queryable = dbSetValue as IQueryable ?? throw new InvalidOperationException("DbSet is not queryable");

        // Materialize entities as List<object>
        var toListMethod = typeof(Enumerable).GetMethod("ToList", BindingFlags.Static | BindingFlags.Public)!
            .MakeGenericMethod(entityType);

        var entities = (IEnumerable<object>)toListMethod.Invoke(null, new object[] { queryable })!;

        // Compare counts
        var expectedRows = table.Rows.ToList();
        Assert.Equal(expectedRows.Count, entities.Count());

        // Compare property values
        foreach (var expected in expectedRows)
        {
            bool found = entities.Any(e =>
                Enumerable.All<string>(expected.Keys, col =>
                {
                    var prop = entityType.GetProperty(col,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (prop == null)
                    {
                        return false;
                    }

                    var actualValue = prop.GetValue(e)?.ToString() ?? "";
                    return string.Equals(expected[col], actualValue, StringComparison.OrdinalIgnoreCase);
                }));

            Assert.True(found,
                $"Expected entity with values [{string.Join(", ", Enumerable.Select<KeyValuePair<string, string>, string>(expected, kv => $"{kv.Key}={kv.Value}"))}] not found in database.");
        }
    }
    
    [Then(@"there are (\d+) (.*) elements in the database")]
    public static void ThenThereAreFollowingEntitiesInTheDatabase(int expectedCount, string entityName)
    {
        // Normalize (handle plural forms like "Campaigns")
        var singularName = entityName.TrimEnd('s');

        // Resolve AppDbContext from DI
        using var scope = FeatureHooks.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Find entity type (e.g. "Campaign" -> typeof(Campaign))
        var entityType = Assembly.GetAssembly(typeof(Campaign))!
            .GetTypes()
            .FirstOrDefault(t => string.Equals(t.Name, singularName, StringComparison.OrdinalIgnoreCase));

        Assert.NotNull(entityType);

        // Find DbSet<T> property in DbContext
        var dbSetProp = Enumerable
            .FirstOrDefault<PropertyInfo>(db.GetType().GetProperties(), p =>
                p.PropertyType.IsGenericType &&
                p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                p.PropertyType.GetGenericArguments()[0] == entityType);

        Assert.NotNull(dbSetProp);

        // Get IQueryable<T> from the DbSet<T>
        var dbSetValue = dbSetProp.GetValue(db);
        var queryable = dbSetValue as IQueryable ?? throw new InvalidOperationException("DbSet is not queryable");

        // Materialize entities as List<object>
        var toListMethod = typeof(Enumerable).GetMethod("ToList", BindingFlags.Static | BindingFlags.Public)!
            .MakeGenericMethod(entityType);

        var entities = (IEnumerable<object>)toListMethod.Invoke(null, new object[] { queryable })!;

        // Compare counts
        Assert.Equal(expectedCount, entities.Count());
    }
    
    [Given(@"there is top up code to use ""(.*)"" of (\d+) PLN for campaign ""(.*)""")]
    public static void GivenThereIsTopUpCodeForCampaign(string code, int amount, string campaignName)
    {
        var campaignId = Guid.NewGuid();

        using (var scope = FeatureHooks.Factory!.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (!db.Campaigns.Any(c => c.Name == campaignName))
            {
                db.Campaigns.Add(new Campaign { Id = campaignId, Name = campaignName });
                db.SaveChanges();
            }
            else
            {
                campaignId = db.Campaigns
                    .AsNoTracking()
                    .Where(c => c.Name == campaignName)
                    .Select(c => c.Id)
                    .First();
            }
        }

        using var topUpScope = FeatureHooks.Factory!.Services.CreateScope();
        var topUpDb = topUpScope.ServiceProvider.GetRequiredService<AppDbContext>();
        topUpDb.TopUps.Add(new TopUp
        {
            Code = Guid.Parse(code),
            Amount = amount,
            Currency = "PLN",
            CampaignId = campaignId
        });
        topUpDb.SaveChanges();
    }

    [Given(@"there is top up code to use ""(.*)"" of (\d+) PLN without campaign")]
    public static void GivenThereIsTopUpCodeWithoutCampaign(string code, int amount)
    {
        using var scope = FeatureHooks.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.TopUps.Add(new TopUp
        {
            Code = Guid.Parse(code),
            Amount = amount,
            Currency = "PLN"
        });
        db.SaveChanges();
    }

    [Given(@"Today's date is ""(.*)""")]
    public static void GivenTodaysDateIs(string date)
    {
        var parsed = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal);
        FeatureHooks.Factory!.FakeClock.Current = parsed;
    }

    [Given(@"I added json content type to header")]
    public static void GivenIAddedJsonContentTypeToHeader()
    {
        // Content-Type is automatically set to application/json by StringContent in SendRequestAsync
    }

    [Then(@"the top up code ""(.*)"" is used")]
    public static void ThenTheTopUpCodeIsUsed(string code)
    {
        using var scope = FeatureHooks.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var topUp = db.TopUps.FirstOrDefault(t => t.Code == Guid.Parse(code));
        Assert.NotNull(topUp);
        Assert.NotNull(topUp.UsedAt);
    }

    private async Task SendRequestAsync(string method, string endpoint, string? body)
    {
        var client = FeatureHooks.Factory!.CreateClient();

        var request = new HttpRequestMessage(new HttpMethod(method), endpoint);
        
        foreach (var (key, value) in _headers)
        {
            request.Headers.Add(key, value);
        }

        if (!string.IsNullOrWhiteSpace(body))
        {
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
        }

        _response = await client.SendAsync(request);
        request.Dispose();
    }
}
