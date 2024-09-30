
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges);
    EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);

    void DeleteEmployee(Guid companyId, Guid id, bool trackChanges);

    void UpdateEmployee(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTractChanges, bool emTrackChanges);

    (EmployeeForUpdateDto employeeForPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

    void SaveChangesForPatch(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity);
}
