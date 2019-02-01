﻿using AutoMapper;
using Friendster.Controllers.Resources;
using Friendster.Models;
using System.Linq;

namespace Friendster.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Model to Resource

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

            #endregion

            #region Resource to Model

            CreateMap<UpdateUserResource, User>();

            CreateMap<AddPhotoResource, Photo>();

            CreateMap<RegisterUserResource, User>();
            

            #endregion
        }
    }
}
