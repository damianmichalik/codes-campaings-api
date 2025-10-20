using CodesCampaigns.Application.Entities;
using CodesCampaigns.Application.ValueObjects;
using MediatR;

namespace CodesCampaigns.Application.Queries;

public record GetCampaignQuery(CampaignId CampaignId) : IRequest<Campaign>;