﻿using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

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

        // Create a mapper from CompanyForCreationDto to Company.
        CreateMap<CompanyForCreationDto, Company>();

        CreateMap<EmployeeForCreationDto, Employee>();

        CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

        CreateMap<CompanyForUpdateDto, Company>();

        CreateMap<UserForRegistrationDto, User>();
    }
}