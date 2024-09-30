using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    // Step 4: Implement service layer.
    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
    {
        var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);
        // var companiesDto = companies.Select(c =>
        //   new CompanyDto(c.Id, c.Name ?? "", string.Join(" - ", c.Address, c.Country))).ToList();

        // Using mapper to map the entity to dto.
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
    {
        Company company = await GetCompanyAndCheckIfItExists(id, trackChanges);

        var companyDto = _mapper.Map<CompanyDto>(company);

        return companyDto;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);

        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAsync();

        var companyToResult = _mapper.Map<CompanyDto>(companyEntity);

        return companyToResult;
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var companies = await _repository.Company.GetByIdsAsync(ids, trackChanges);

        if (ids.Count() != companies.Count())
            throw new CollectionByIdsBadRequestException();

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies, bool trackChanges)
    {
        if (companies is null) throw new CompanyCollectionBadRequest();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companies);

        foreach (var company in companyEntities)
        {
            _repository.Company.CreateCompany(company);
        }

        await _repository.SaveAsync();

        var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        
        var ids = string.Join(",", companiesToReturn.Select(company => company.Id));

        return (companies: companiesToReturn, ids);
    }

    public async Task DeleteCompanyAsync(Guid id, bool trackChanges)
    {
        var company = await GetCompanyAndCheckIfItExists(id, trackChanges);

        _repository.Company.DeleteCompany(company);
        await _repository.SaveAsync();
    }

    public async Task UpdateCompanyAsync(Guid id, CompanyForUpdateDto company, bool trackChanges)
    {
        var companyEntity = await GetCompanyAndCheckIfItExists(id, trackChanges);

        _mapper.Map(company, companyEntity);

        await _repository.SaveAsync();
    }

    private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(id, trackChanges);

        if (company == null)
        {
            throw new CompanyNotFoundException(id);
        }

        return company;
    }
}
