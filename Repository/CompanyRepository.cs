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

    public Company GetCompany(Guid id, bool trackChanges) => FindByCondition(c => c.Id.Equals(id), trackChanges)
        .SingleOrDefault();

    public void CreateCompany(Company company) => Create(company);

    public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) => FindByCondition(c => ids.Contains(c.Id), trackChanges)
        .ToList();
}
