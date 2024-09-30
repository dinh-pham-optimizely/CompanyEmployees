using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface ICompanyService
{
    // Step 3: Define GetAllCompanies method inside ICompanyService.
    // IEnumerable<Company> GetAllCompanies(bool trackChanges); // Replace entity with the new DTO
    Task<(IEnumerable<CompanyDto> companies, MetaData metaData)> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges);

    Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges);

    Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);

    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies, bool trackChanges);

    Task DeleteCompanyAsync(Guid id, bool trackChanges);

    Task UpdateCompanyAsync(Guid id, CompanyForUpdateDto companyForUpdate, bool trackChanges);
}
