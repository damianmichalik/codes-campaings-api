namespace CodesCampaigns.Application.Exceptions;

public sealed class InvalidEntityIdException(object id) 
    : Exception($"Cannot set: {id} as entity identifier.");