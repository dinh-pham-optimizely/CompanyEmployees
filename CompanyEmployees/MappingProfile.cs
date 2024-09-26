using AutoMapper;
using Shared.DataTransferObjects;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create a mapper for FullAddress field.
        CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
            opt => opt.MapFrom(x => string.Join(" - ", x.Address, x.Country)));

        // Create a mapper for all fields.
        CreateMap<Employee, EmployeeDto>();
    }
}