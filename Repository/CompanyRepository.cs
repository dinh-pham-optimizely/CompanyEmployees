using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    { }

    // Step 2: Implement ICompanyRepositort's GetAllCompanies method in CompanyRepository.
    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) => await FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .ToListAsync();

    public async Task<Company> GetCompanyAsync(Guid id, bool trackChanges) => await FindByCondition(c => c.Id.Equals(id), trackChanges)
        .SingleOrDefaultAsync();

    public void CreateCompany(Company company) => Create(company);

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await FindByCondition(c => ids.Contains(c.Id), trackChanges)
        .ToListAsync();

    public void DeleteCompany(Company company) => Delete(company);
}
