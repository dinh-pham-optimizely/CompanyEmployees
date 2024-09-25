using Contracts;
namespace Repository;

// By inheriting from RepositoryBase, a repository will have access to all the methods from it.
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    { }
}
