
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);

    Task DeleteEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

    Task UpdateEmployeeAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTractChanges, bool emTrackChanges);

    Task<(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

    Task SaveChangesForPatch(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity);
}
