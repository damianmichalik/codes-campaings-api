using CodesCampaigns.Api.DTO;
using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Entities;
using CodesCampaigns.Application.Queries;
using CodesCampaigns.Application.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodesCampaigns.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCampaigns()
    {
        var campaigns = await mediator.Send(new GetCampaignsQuery());
        var campaignDtos = campaigns.Select((campaign) => new CampaignDto
        {
            Id = campaign.Id.ToString(),
            Name = campaign.Name,
        });
        return Ok(campaignDtos);
    }

    [HttpGet("{campaignId:guid}")]
    public async Task<ActionResult> GetCampaign(Guid campaignId)
    {
        var campaign = await mediator.Send(new GetCampaignQuery(campaignId));
        var campaignDto = new CampaignDto
        {
            Id = campaign.Id.ToString(),
            Name = campaign.Name,
        };
        return Ok(campaignDto);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCampaignDto createCampaignDto)
    {
        var campaignId = CampaignId.Create();
        await mediator.Send(new CreateCampaignCommand(campaignId, createCampaignDto.Name));

        return CreatedAtAction(nameof(GetCampaigns), new { id = campaignId.ToString() });
    }
    
    [HttpDelete("{campaignId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid campaignId)
    {
        await mediator.Send(new DeleteCampaignCommand(campaignId));

        return NoContent();
    }
    
    [HttpPut("{campaignId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid campaignId, [FromBody] UpdateCampaignDto updateCampaignDto)
    {
        await mediator.Send(new UpdateCampaignCommand(campaignId, updateCampaignDto.Name));

        return NoContent();
    }
}