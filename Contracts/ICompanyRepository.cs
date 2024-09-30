namespace Contracts;

public interface ICompanyRepository
{
    // Step 1: Create a definition for GetAllCompanies method in repository interface.
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    Company GetCompany(Guid companyId, bool trackChanges);

    void CreateCompany(Company company);

    IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

    void DeleteCompany(Company company);
}
