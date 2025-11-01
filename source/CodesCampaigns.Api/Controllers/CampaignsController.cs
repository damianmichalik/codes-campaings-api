using CodesCampaigns.Api.Authentication;
using CodesCampaigns.Api.DTO;
using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Queries;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace CodesCampaigns.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiKey]
public class CampaignsController() : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCampaigns(
        IQueryHandler<GetCampaignsQuery, IEnumerable<Campaign>> queryHandler,
        CancellationToken cancellationToken
    )
    {
        var campaigns = await queryHandler.Handle(
            new GetCampaignsQuery(), 
            cancellationToken
        );
        var campaignDtos = campaigns.Select((campaign) => new CampaignDto
        {
            Id = campaign.Id.ToString(),
            Name = campaign.Name,
        });
        return Ok(campaignDtos);
    }

    [HttpGet("{campaignId:guid}")]
    public async Task<ActionResult> GetCampaign(
        Guid campaignId,
        IQueryHandler<GetCampaignQuery, Campaign> queryHandler,
        CancellationToken cancellationToken
    )
    {
        var campaign = await queryHandler.Handle(new GetCampaignQuery(campaignId), cancellationToken);
        var campaignDto = new CampaignDto
        {
            Id = campaign.Id.ToString(),
            Name = campaign.Name,
        };
        return Ok(campaignDto);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCampaignDto createCampaignDto,
        ICommandHandler<CreateCampaignCommand> commandHandler,
        CancellationToken cancellationToken)
    {
        var campaignId = CampaignId.Create();
        await commandHandler.Handle(new CreateCampaignCommand(campaignId, createCampaignDto.Name), cancellationToken);

        return CreatedAtAction(nameof(GetCampaigns), new { id = campaignId.ToString() });
    }
    
    [HttpDelete("{campaignId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid campaignId,
        ICommandHandler<DeleteCampaignCommand> commandHandler,
        CancellationToken cancellationToken
    )
    {
        await commandHandler.Handle(new DeleteCampaignCommand(campaignId), cancellationToken);

        return NoContent();
    }
    
    [HttpPut("{campaignId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid campaignId, 
        [FromBody] UpdateCampaignDto updateCampaignDto,
        ICommandHandler<UpdateCampaignCommand> commandHandler,
        CancellationToken cancellationToken
    )
    {
        await commandHandler.Handle(new UpdateCampaignCommand(campaignId, updateCampaignDto.Name), cancellationToken);

        return NoContent();
    }
    
    [HttpPost("{campaignId:guid}/codes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GenerateCodes(
        Guid campaignId, 
        [FromBody] GenerateTopUpCodesDto generateTopUpCodesDto,
        ICommandHandler<GenerateTopUpCodesCommand> commandHandler,
        CancellationToken cancellationToken
    )
    {
        await commandHandler.Handle(new GenerateTopUpCodesCommand(
            campaignId, 
            generateTopUpCodesDto.Count,
            generateTopUpCodesDto.Value,
            generateTopUpCodesDto.Currency
        ), cancellationToken);

        return NoContent();
    }

    [HttpGet("{campaignId:guid}/codes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetCodes(
        Guid campaignId,
        IQueryHandler<GetCampaignCodesQuery, IEnumerable<TopUp>> queryHandler,
        CancellationToken cancellationToken
    )
    {
        var topUps = await queryHandler.Handle(new GetCampaignCodesQuery(
            campaignId
        ), cancellationToken);

        var topUpDtos = topUps.Select((topUp) => new TopUpDto
        {
            Amount = topUp.Value.Amount,
            Currency = topUp.Value.CurrencyCode.Code,
            Code = topUp.Code,
            CampaignId = topUp.CampaignId.Value
        });
        return Ok(topUpDtos);
    }
}
