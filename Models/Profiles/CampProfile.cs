using AutoMapper;
using CoreCodeCamp.Data;

namespace AspRestApiWorkshop.Models.Profiles
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            CreateMap<Camp, CampModel>();
        }
    }
}
