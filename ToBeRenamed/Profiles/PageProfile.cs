using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ToBeRenamed.Commands;
using ToBeRenamed.Models;
using ToBeRenamed.Pages.Roles;

namespace ToBeRenamed.Profiles
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<IEnumerable<string>, ISet<Privilege>>()
                .ConstructUsing(src => Privilege.All().Where(p => src.Contains(p.Alias)).ToHashSet());

            CreateMap<IndexModel.UpdateMemberRequest, UpdateRoleOfMember>()
                //.ForMember(dest => dest, opt => opt.MapFrom(src => src.DisplayName ?? ""))
                .ConvertUsing(src => src.DisplayName ?? string.Empty);
        }
    }
}
