using Shared.RequestFeatures;

namespace Contracts;

public interface ICompanyRepository
{
    // Step 1: Create a definition for GetAllCompanies method in repository interface.
    Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges);
    Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges);

    void CreateCompany(Company company);

    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

    void DeleteCompany(Company company);
}
