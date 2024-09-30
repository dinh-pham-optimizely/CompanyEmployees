﻿using Contracts;
using Microsoft.EntityFrameworkCore;
namespace Repository;

// By inheriting from RepositoryBase, a repository will have access to all the methods from it.
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    { }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .OrderBy(e => e.Name).ToListAsync();

        return employees;
    }

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var employee = await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .FirstOrDefaultAsync();

        return employee;
    }

    public void CreateEmployee(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        Delete(employee);
    }
}
