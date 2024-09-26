using Contracts;
namespace Repository;

// By inheriting from RepositoryBase, a repository will have access to all the methods from it.
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    { }

    public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
    {
        var employees = FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .OrderBy(e => e.Name).ToList();

        return employees;
    }

    public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var employee = FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .FirstOrDefault();

        return employee;
    }
}
