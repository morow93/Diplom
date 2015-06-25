using InoDrive.Domain.Entities;
using InoDrive.Domain.Models;
using InoDrive.Domain.Models.InputModels;
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






            AutoMapper.Mapper.CreateMap<Trip, OutputFindTripModel>()
                .ForMember(pv => pv.OriginPlaceName, opt => opt.MapFrom(src => src.OriginPlace.Name))
                .ForMember(pv => pv.DestinationPlaceName, opt => opt.MapFrom(src => src.DestinationPlace.Name))
                //.ForMember(pv => pv.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(pv => pv.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(pv => pv.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(pv => pv.TotalPlaces, opt => opt.MapFrom(src => src.PeopleCount))
                .ForMember(pv => pv.IsEnded, opt => opt.MapFrom(src => src.EndDate < DateTimeOffset.Now))
                .ForMember(pv => pv.IsStarted, opt => opt.MapFrom(src => src.LeavingDate < DateTimeOffset.Now))
                .ForMember(pv => pv.FreePlaces, opt => opt.MapFrom(src => src.PeopleCount - src.Bids.Count(b => b.IsAccepted == true)))
                .ForMember(pv => pv.Rating, opt => opt.MapFrom(src =>
                              ((double)(src.User.Trips.SelectMany(lk => lk.Commnents).Select(n => n.Vote).Sum()) /
                                (double)(src.User.Trips.SelectMany(l => l.Commnents).Count() * 5)) * 100                    
                ));                             
            





            AutoMapper.Mapper.CreateMap<Trip, InputEditTripModel>()
                .ForMember(pv => pv.WayPoints, opt => opt.MapFrom(src => new List<PlaceModel>()))
                .ForMember(pv => pv.RawOriginPlace, opt => opt.MapFrom(src => new PlaceModel
                {
                    PlaceId = src.OriginPlace.PlaceId,
                    Name = src.OriginPlace.Name
                }))
                .ForMember(pv => pv.RawDestinationPlace, opt => opt.MapFrom(src => new PlaceModel
                {
                    PlaceId = src.DestinationPlace.PlaceId,
                    Name = src.DestinationPlace.Name
                }));

            



            AutoMapper.Mapper.CreateMap<Trip, OutputTripModel>()
                .ForMember(pv => pv.Initials, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(pv => pv.OriginPlace, opt => opt.MapFrom(src => new PlaceModel
                {
                    PlaceId = src.OriginPlaceId,
                    Name = src.OriginPlace.Name,
                    Lat = src.OriginPlace.Latitude,
                    Lng = src.OriginPlace.Longitude
                }))
                .ForMember(pv => pv.DestinationPlace, opt => opt.MapFrom(src => new PlaceModel
                {
                    PlaceId = src.DestinationPlaceId,
                    Name = src.DestinationPlace.Name,
                    Lat = src.DestinationPlace.Latitude,
                    Lng = src.DestinationPlace.Longitude
                }))
                .ForMember(pv => pv.TotalPlaces, opt => opt.MapFrom(src => src.PeopleCount))
                .ForMember(pv => pv.FreePlaces, opt => opt.MapFrom(src => src.PeopleCount - src.Bids.Count(b => b.IsAccepted == true)))
                .ForMember(pv => pv.IsEnded, opt => opt.MapFrom(src => src.EndDate < DateTimeOffset.Now))
                .ForMember(pv => pv.WayPoints, opt => opt.MapFrom(src => src.WayPoints.OrderBy(w => w.WayPointIndex).Select(wp => new PlaceModel
                {
                    PlaceId = wp.PlaceId,
                    Name = wp.Place.Name,
                    Lat = wp.Place.Latitude,
                    Lng = wp.Place.Longitude
                }).ToList<PlaceModel>()))
                .ForMember(pv => pv.UserIndicators, opt => opt.MapFrom(src => src.Bids.Where(b => b.IsAccepted == true).Select(ui => new UserIndicator
                {
                    UserId = ui.UserId,
                    AvatarImage = ui.User.AvatarImage,
                    FirstName = ui.User.FirstName,
                    LastName = ui.User.LastName,
                    WasTriped =
                        (ui.User.Trips.Any(t => !t.IsDeleted && t.EndDate < DateTimeOffset.Now) ||
                        ui.User.Bids.Any(b => b.IsAccepted == true && b.Trip.EndDate < DateTimeOffset.Now))

                }).ToList<UserIndicator>()));


        }
    }
}