using CodesCampaigns.Application.Abstractions;

namespace CodesCampaigns.Application.Commands;

public record UseTopUpCodeCommand(string PartnerCode, Guid Code, string Email) : ICommand;
