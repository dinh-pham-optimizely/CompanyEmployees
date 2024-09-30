using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId}/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeesController(IServiceManager service) => _service = service;

    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var employees = _service.EmployeeService.GetEmployees(companyId, false);

        return Ok(employees);
    }

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id, false);

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreation)
    {
        if (employeeForCreation is null) return BadRequest("EmployeeForCreationDto object is null");

        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        var employee = _service.EmployeeService.CreateEmployee(companyId, employeeForCreation, false);

        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employee.id }, employee);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployee(Guid companyId, Guid id)
    {
        _service.EmployeeService.DeleteEmployee(companyId, id, false);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployee(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee is null)
        {
            return BadRequest("EmployeeForUpdateDto object is null");
        }

        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        _service.EmployeeService.UpdateEmployee(companyId, id, employee, compTractChanges: false, emTrackChanges: true);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public IActionResult PartiallyUpdateEmployee(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc is null) return BadRequest("patchDoc object sent from client is null.");

        var result = _service.EmployeeService.GetEmployeeForPatch(companyId, id, compTrackChanges: false, empTrackChanges: true);

        patchDoc.ApplyTo(result.employeeForPatch);

        TryValidateModel(result.employeeForPatch);

        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        _service.EmployeeService.SaveChangesForPatch(result.employeeForPatch, result.employeeEntity);

        return NoContent();
    }
}
