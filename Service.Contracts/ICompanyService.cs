using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    // Step 3: Define GetAllCompanies method inside ICompanyService.
    // IEnumerable<Company> GetAllCompanies(bool trackChanges); // Replace entity with the new DTO
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);

    CompanyDto GetCompany(Guid id, bool trackChanges);

    CompanyDto CreateCompany(CompanyForCreationDto company);

    IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

    (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies, bool trackChanges);

    void DeleteCompany(Guid id, bool trackChanges);
}
