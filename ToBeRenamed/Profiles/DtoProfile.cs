using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ToBeRenamed.Dtos;
using ToBeRenamed.Models;

namespace ToBeRenamed.Profiles
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<IEnumerable<RoleDto>, Role>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.First().Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.First().Title))
                .ForMember(dest => dest.Privileges, opt => opt
                    .MapFrom(src => src.Select(r => Privilege.All().Single(p => p.Alias == r.PrivilegeAlias))
                    .ToHashSet()));

            CreateMap<IEnumerable<RoleDto>, IEnumerable<Role>>()
                .ConvertUsing((src, dst, ctx) => src.GroupBy(r => r.Id).Select(ctx.Mapper.Map<Role>));
        }
    }
}
