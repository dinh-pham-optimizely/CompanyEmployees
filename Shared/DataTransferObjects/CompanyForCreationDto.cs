using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record CompanyForCreationDto
{
    [Required(ErrorMessage = "Company name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Address is a required field.")]
    public string? Address { get; init; }

    [Required(ErrorMessage = "Country")]
    public string? Country { get; init; }

    public IEnumerable<EmployeeForCreationDto>? Employees;
}