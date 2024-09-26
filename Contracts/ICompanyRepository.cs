namespace Contracts;

public interface ICompanyRepository
{
    // Step 1: Create a definition for GetAllCompanies method in repository interface.
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    Company GetCompany(Guid companyId, bool trackChanges);
}
