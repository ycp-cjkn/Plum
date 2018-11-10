using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ToBeRenamed.Models;

namespace ToBeRenamed.Profiles
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<IEnumerable<string>, ISet<Privilege>>()
                .ConstructUsing(src => Privilege.All().Where(p => src.Contains(p.Alias)).ToHashSet());
        }
    }
}
