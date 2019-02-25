using AutoMapper;
using Friendster.Controllers.Resources;
using Friendster.Models;
using System.Linq;

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
            CreateMap<Photo, PhotoResource>();
            CreateMap<UpdateUserResource, User>();
            CreateMap<AddPhotoResource, Photo>();
            CreateMap<RegisterUserResource, User>();
            CreateMap<SendMessageResource, Message>().ReverseMap();

        }
    }
}
