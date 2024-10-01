using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<EmployeeDto> _dataShaper;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }

    public async Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange) throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExists(companyId, trackChanges);

        var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
        var shapedData = _dataShaper.ShapeData(employeesDto, employeeParameters.Fields);

        return (employees: shapedData, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        Employee employee = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeEntity = _mapper.Map<Employee>(employee);

        _repository.Employee.CreateEmployee(companyId, employeeEntity);
        await _repository.SaveAsync();

        var employeeToResult = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToResult;
    }

    public async Task DeleteEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        Employee employeeForCompany = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

        _repository.Employee.DeleteEmployee(employeeForCompany);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges: compTrackChanges);

        Employee employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges: empTrackChanges);

        _mapper.Map(employeeForUpdate, employeeEntity);

        await _repository.SaveAsync();

    }

    public async Task<(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges: compTrackChanges);

        Employee employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges: empTrackChanges);

        var employeeForPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        return (employeeForPatch, employeeEntity);
    }

    public async Task SaveChangesForPatch(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeForPatch, employeeEntity);

        await _repository.SaveAsync();
    }

    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }
    }


    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
    {
        var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);

        if (employee is null)
        {
            throw new EmployeeNotFoundException(companyId);
        }

        return employee;
    }
}
