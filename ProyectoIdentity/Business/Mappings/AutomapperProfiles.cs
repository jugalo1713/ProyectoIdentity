using AutoMapper;
using ProyectoIdentity.Models;
using ProyectoIdentity.Models.ViewModels;

namespace ProyectoIdentity.Business.Mappings
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<AppUser, RegisterViewModel>();
            CreateMap<RegisterViewModel, AppUser>()
                .ForMember(x => x.UserName, p => p.MapFrom(a => a.Email));
        }
    }
}
