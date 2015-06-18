using InoDrive.Domain.Entities;
using InoDrive.Domain.Models.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InoDrive.Api.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Register()
        {
            AutoMapper.Mapper.CreateMap<Trip, OutputMyTripModel>()
                .ForMember(pv => pv.OriginPlaceName, opt => opt.MapFrom(src => src.OriginPlace.Name))
                .ForMember(pv => pv.DestinationPlaceName, opt => opt.MapFrom(src => src.DestinationPlace.Name))
                .ForMember(pv => pv.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(pv => pv.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(pv => pv.TotalPlaces, opt => opt.MapFrom(src => src.PeopleCount))
                .ForMember(pv => pv.IsEnded, opt => opt.MapFrom(src => src.EndDate < DateTimeOffset.Now))
                .ForMember(pv => pv.IsStarted, opt => opt.MapFrom(src => src.LeavingDate < DateTimeOffset.Now))
                .ForMember(pv => pv.FreePlaces, opt => opt.MapFrom(src => src.PeopleCount - src.Bids.Count(b => b.IsAccepted == true)));

        }
    }
}