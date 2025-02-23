using AutoMapper;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Core.Enitities;

namespace EmployeeManagementSystem.Application.Mappings;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<EmployeeDTO, Employee>();
            config.CreateMap<Employee, EmployeeDTO>();
        });
        return mappingConfig;
    }
}