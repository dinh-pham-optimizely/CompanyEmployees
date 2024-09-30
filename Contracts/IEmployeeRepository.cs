﻿namespace Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges);

    Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

    void CreateEmployee(Guid companyId, Employee employee);

    void DeleteEmployee(Employee employee);
}
