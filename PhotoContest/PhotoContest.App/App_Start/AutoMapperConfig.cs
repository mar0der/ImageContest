using System;
using PhotoContest.App.Models.BindingModels.Contests;

namespace PhotoContest.App
{
    using AutoMapper;
    using PhotoContest.App.Models.BindingModels.Users;
    using PhotoContest.App.Models.ViewModels;
    using PhotoContest.Models.Models;

    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<User, ProfileViewModel>();
            Mapper.CreateMap<ContestBindingModel, Contest>();
            Mapper.CreateMap<User, EditProfileModel>();
        }
    }
}