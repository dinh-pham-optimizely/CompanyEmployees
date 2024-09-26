using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    // Step 3: Define GetAllCompanies method inside ICompanyService.
    // IEnumerable<Company> GetAllCompanies(bool trackChanges); // Replace entity with the new DTO
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);

    CompanyDto GetCompany(Guid id, bool trackChanges);
}
