using Asp.Versioning;
using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Dynamic;
using System.Text.Json;

namespace CompanyEmployees.Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/companies/{companyId}/employees")]
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeesController(IServiceManager service) => _service = service;

    [HttpGet(Name = "GetEmpoyees")]
    [ProducesResponseType<IEnumerable<EmployeeDto>>(200)]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
    {
        var pagedResult = await _service.EmployeeService.GetEmployeesAsync(companyId, employeeParameters, false);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

        return Ok(pagedResult.employees);
    }

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    [ProducesResponseType<EmployeeDto>(200)]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = await _service.EmployeeService.GetEmployeeAsync(companyId, id, false);

        return Ok(employee);
    }

    [HttpPost(Name = "CreateEmployee")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType<EmployeeDto>(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreation)
    {
        var employee = await _service.EmployeeService.CreateEmployeeAsync(companyId, employeeForCreation, false);

        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employee.id }, employee);
    }

    [HttpDelete("{id:guid}", Name = "DeleteEmployee")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid id)
    {
        await _service.EmployeeService.DeleteEmployeeAsync(companyId, id, false);

        return NoContent();
    }

    [HttpPut("{id:guid}", Name = "UpdateEmployee")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> UpdateEmployee(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        await _service.EmployeeService.UpdateEmployeeAsync(companyId, id, employee, compTractChanges: false, emTrackChanges: true);

        return NoContent();
    }

    [HttpPatch("{id:guid}", Name = "PartiallyUpdateEmployee")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> PartiallyUpdateEmployee(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc is null) return BadRequest("patchDoc object sent from client is null.");

        var result = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id, compTrackChanges: false, empTrackChanges: true);

        patchDoc.ApplyTo(result.employeeForPatch);

        TryValidateModel(result.employeeForPatch);

        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        await _service.EmployeeService.SaveChangesForPatch(result.employeeForPatch, result.employeeEntity);

        return NoContent();
    }
}
