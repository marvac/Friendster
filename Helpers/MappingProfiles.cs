using AutoMapper;
using Friendster.Controllers.Resources;
using Friendster.Models;
using Friendster.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendster.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, DetailUserResource>()
                .ForMember(x => x.PhotoUrl, options =>
                {
                    options.MapFrom(source => source.Photos.FirstOrDefault(photo => photo.IsMain).Url);
                });

            CreateMap<User, ListUserResource>()
                .ForMember(x => x.PhotoUrl, options =>
                {
                    options.MapFrom(source => source.Photos.FirstOrDefault(photo => photo.IsMain).Url);
                })
                .ForMember(x => x.Age, options =>
                {
                    options.MapFrom(source => source.BirthDate.CalculateAge());
                });

            CreateMap<Photo, DetailPhotoResource>();
        }
    }
}
