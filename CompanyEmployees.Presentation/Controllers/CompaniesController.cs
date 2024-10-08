﻿using Asp.Versioning;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace CompanyEmployees.Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/companies")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;

    public CompaniesController(IServiceManager service) => _service = service;

    // Last step: Define an api endpoint to get all companies
    [HttpGet(Name = "GetCompanies")]
    /*[ResponseCache(Duration = 60)]*/
    [DisableRateLimiting]
    [OutputCache(Duration = 60)]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType<IEnumerable<CompanyDto>>(200)]
    public async Task<IActionResult> GetCompanies([FromQuery] CompanyParameters companyParameters)
    {
        var pagedResult = await _service.CompanyService.GetAllCompaniesAsync(companyParameters, trackChanges: false);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

        return Ok(pagedResult.companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    /*[ResponseCache(CacheProfileName = "120SecondsDuration")]*/
    [OutputCache(PolicyName = "120SecondsDuration")]
    [EnableRateLimiting(policyName: "SpecificPolicy")]
    [ProducesResponseType<CompanyDto>(200)]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
        return Ok(company);
    }

    [HttpPost(Name = "CreateCompany")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType<CompanyDto>(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
    {
        var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);

        // Return 201 - Created Status and a link to retrived the created company as Location in Headers.
        return CreatedAtRoute("CompanyById", new {id = createdCompany.Id}, createdCompany);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    [ProducesResponseType<IEnumerable<CompanyDto>>(200)]
    public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
    {
        var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);

        return Ok(companies);
    }

    [HttpPost("collection", Name = "CreateCompanyCollection")]
    [ProducesResponseType<IEnumerable<CompanyDto>>(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateCompanyColletion([FromBody] IEnumerable<CompanyForCreationDto> companies)
    {
        var result = await _service.CompanyService.CreateCompanyCollectionAsync(companies, false);

        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }

    [HttpDelete("{id:guid}", Name = "DeleteCompany")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _service.CompanyService.DeleteCompanyAsync(id, false);

        return NoContent();
    }

    [HttpPut("{id:guid}", Name = "UpdateCompany")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
    {
        await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);

        return NoContent();
    }
}
