namespace Service.Contracts;

public interface ICompanyService
{
    // Step 3: Define GetAllCompanies method inside ICompanyService.
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
}
