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
    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        var companies = _repository.Company.GetAllCompanies(trackChanges);
        // var companiesDto = companies.Select(c =>
        //   new CompanyDto(c.Id, c.Name ?? "", string.Join(" - ", c.Address, c.Country))).ToList();

        // Using mapper to map the entity to dto.
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public CompanyDto GetCompany(Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(id, trackChanges);

        if (company == null)
        {
            throw new CompanyNotFoundException(id);
        }

        var companyDto = _mapper.Map<CompanyDto>(company);

        return companyDto;
    }

    public CompanyDto CreateCompany(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);

        _repository.Company.CreateCompany(companyEntity);
        _repository.Save();

        var companyToResult = _mapper.Map<CompanyDto>(companyEntity);

        return companyToResult;
    }

    public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var companies = _repository.Company.GetByIds(ids, trackChanges);

        if (ids.Count() != companies.Count())
            throw new CollectionByIdsBadRequestException();

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies, bool trackChanges)
    {
        if (companies is null) throw new CompanyCollectionBadRequest();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companies);

        foreach (var company in companyEntities)
        {
            _repository.Company.CreateCompany(company);
        }

        _repository.Save();

        var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        
        var ids = string.Join(",", companiesToReturn.Select(company => company.Id));

        return (companies: companiesToReturn, ids);
    }

    public void DeleteCompany(Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(id, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(id);
        }

        _repository.Company.DeleteCompany(company);
        _repository.Save();
    }
}
