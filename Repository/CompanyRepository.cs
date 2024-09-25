using Contracts;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    { }

    // Step 2: Implement ICompanyRepositort's GetAllCompanies method in CompanyRepository.
    public IEnumerable<Company> GetAllCompanies(bool trackChanges) => FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .ToList();
}
