using System;
using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>();
            this.CreateMap<Talk, TalkModel>();
            this.CreateMap<Speaker, SpeakerModel>();
            this.CreateMap<Location, LocationModel>();

            this.CreateMap<CampModel, Camp>();
            this.CreateMap<TalkModel, Talk>();
            this.CreateMap<SpeakerModel, Speaker>();
            this.CreateMap<LocationModel, Location>();
        }
        
    }
}
