namespace CodesCampaigns.Application.Exceptions;

public sealed class CampaignNotFoundException(Guid id) 
    : Exception($"Campaign with ID: {id} was not found.");