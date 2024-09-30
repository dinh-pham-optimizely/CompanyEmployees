using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employees = _repository.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return employeesDto;
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employee = _repository.Employee.GetEmployee(companyId, id, trackChanges);

        if (employee is null)
        {
            throw new EmployeeNotFoundException(companyId);
        }

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        return employeeDto;
    }

    public EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeEntity = _mapper.Map<Employee>(employee);

        _repository.Employee.CreateEmployee(companyId, employeeEntity);
        _repository.Save();

        var employeeToResult = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToResult;
    }

    public void DeleteEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeForCompany = _repository.Employee.GetEmployee(companyId, id, trackChanges);

        if (employeeForCompany is null)
        {
            throw new EmployeeNotFoundException(id);
        }

        _repository.Employee.DeleteEmployee(employeeForCompany);
        _repository.Save();
    }

    public void UpdateEmployee(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges: compTrackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeEntity = _repository.Employee.GetEmployee(companyId, id, trackChanges: empTrackChanges);

        if (employeeEntity is null)
        {
            throw new EmployeeNotFoundException(id);
        }

        _mapper.Map(employeeForUpdate, employeeEntity);

        _repository.Save();

    }

    public (EmployeeForUpdateDto employeeForPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges: compTrackChanges);

        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeEntity = _repository.Employee.GetEmployee(companyId, id, trackChanges: empTrackChanges);

        if (employeeEntity is null) throw new EmployeeNotFoundException(id);

        var employeeForPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        return (employeeForPatch, employeeEntity);
    }

    public void SaveChangesForPatch(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeForPatch, employeeEntity);

        _repository.Save();
    }
}
