﻿using InoDrive.Domain.Contexts;
using InoDrive.Domain.Entities;
using InoDrive.Domain.Exceptions;
using InoDrive.Domain.Helpers;
using InoDrive.Domain.Models;
using InoDrive.Domain.Models.InputModels;
using InoDrive.Domain.Models.OutputModels;
using InoDrive.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Data.Entity;

namespace InoDrive.Domain.Repositories.Concrete
{
    public class TripsRepository : ITripsRepository
    {
        public TripsRepository(InoDriveContext dataContext)
        {
            _ctx = dataContext;
        }
        private InoDriveContext _ctx;
        
        public CarModel GetCar(ShortUserModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                var result = new CarModel
                {
                    Car = user.Car,
                    CarImage = user.CarImage,
                    CarImageExtension = user.CarImageExtension,
                    CarClass = user.CarClass
                };
                return result;
            }
            else
            {
                throw new RedirectException(AppConstants.USER_NOT_FOUND);
            }
        }

        public InputEditTripModel GetTripForEdit(InputManageTripModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user == null)
            {
                throw new RedirectException(AppConstants.USER_NOT_FOUND);
            }
            var trip = user.Trips.FirstOrDefault(t => t.TripId == model.TripId);
            if (trip == null)
            {
                throw new RedirectException(AppConstants.USER_HASNT_SUCH_TRIP);
            }
            var result = Mapper.Map<Trip, InputEditTripModel>(trip);

            if (trip.WayPoints != null && trip.WayPoints.Count != 0)
            {
                result.WayPoints = new List<PlaceModel>();
                foreach (var item in trip.WayPoints.OrderBy(w => w.WayPointIndex).ToList())
                {
                    result.WayPoints.Add(new PlaceModel {
                        PlaceId = item.Place.PlaceId,
                        Name    = item.Place.Name
                    });
                }
            }
            return result;
        }

        #region Select

        //public DetailsTripModel GetDetailsTrip(ManageTripModel model)
        //{
        //    var trip = _ctx.Trips.FirstOrDefault(t => t.TripId == model.TripId && !t.IsDeleted);
        //    if (trip != null)
        //    {
        //        var isBidded = false;
        //        if (!String.IsNullOrEmpty(model.UserId))
        //        {
        //            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
        //            if (user == null)
        //            {
        //                throw new RedirectException("Нет такого пользователя!");
        //            }
        //            var isExistBid = user.Bids.FirstOrDefault(b => b.TripId == model.TripId);
        //            if (isExistBid != null)
        //            {
        //                isBidded = true;
        //            }
        //        }

        //        var tripProfile = trip.TripProfile;

        //        var result = new DetailsTripModel
        //        {
        //            TripId = model.TripId,
        //            UserId = trip.UserId,
        //            OriginCity = new GeoCityModel
        //            {
        //                CityId = trip.OriginCityId,
        //                CityName = trip.OriginCity.RuCityName,
        //                RegionName = trip.OriginCity.Region.RuRegionName,
        //                Latitude = trip.OriginCity.Latitude,
        //                Longitude = trip.OriginCity.Longitude
        //            },
        //            DestinationCity = new GeoCityModel
        //            {
        //                CityId = trip.DestinationCityId,
        //                CityName = trip.DestinationCity.RuCityName,
        //                RegionName = trip.DestinationCity.Region.RuRegionName,
        //                Latitude = trip.DestinationCity.Latitude,
        //                Longitude = trip.DestinationCity.Longitude
        //            },
        //            LeavingDate = trip.LeavingDate,
        //            CreationDate = trip.CreationDate,
        //            CarDescription = tripProfile == null ? String.Empty : tripProfile.CarDescription,
        //            CarImage = tripProfile == null ? String.Empty : tripProfile.CarImage,
        //            TotalPlaces = trip.PeopleCount,
        //            FreePlaces = trip.PeopleCount - trip.Bids.Count(b => b.IsAccepted == true),
        //            Pay = trip.PayForOne,
        //            IsBidded = isBidded,
        //            WayPoints = trip.WayPoints.OrderBy(w => w.IndexNumber).Select(wp => new GeoCityModel
        //            {
        //                CityId = wp.CityId,
        //                CityName = wp.City.RuCityName,
        //                RegionName = wp.City.Region.RuRegionName,
        //                Latitude = wp.City.Latitude,
        //                Longitude = wp.City.Longitude
        //            }).ToList<GeoCityModel>(),
        //            UserIndicators = trip.Bids.Where(b => b.IsAccepted == true).Select(ui => new UserIndicator
        //            {
        //                UserId = ui.UserId,
        //                AvatarImage = ui.User.UserProfile == null ? String.Empty : ui.User.UserProfile.AvatarImage,
        //                FirstName = ui.User.FirstName,
        //                LastName = ui.User.LastName,
        //                WasTriped =
        //                    (ui.User.Trips.Any(t => !t.IsDeleted && t.LeavingDate.Date < DateTime.Now.Date) ||
        //                    ui.User.Bids.Any(b => b.IsAccepted == true && b.Trip.LeavingDate.Date < DateTime.Now.Date))

        //            }).ToList<UserIndicator>()
        //        };

        //        return result;
        //    }
        //    else
        //    {
        //        throw new RedirectException("Нет такой поездки!");
        //    }
        //}

        public OutputList<OutputFindTripModel> FindTrips(InputFindTripsModel model)
        {
            var result = new OutputList<OutputFindTripModel>();

            if (model.DestinationPlaceId != null && model.OriginPlaceId != null && model.Places != 0 && model.LeavingDate != null)
            {
                var page = Math.Max(model.Page, 1);

                var trips = _ctx.Trips.Where(t =>
                    !t.IsDeleted &&
                    DbFunctions.TruncateTime(t.LeavingDate) >= DbFunctions.TruncateTime(DateTimeOffset.Now) &&
                    DbFunctions.TruncateTime(t.LeavingDate) == DbFunctions.TruncateTime(model.LeavingDate) &&
                    t.OriginPlaceId == model.OriginPlaceId &&
                    t.DestinationPlaceId == model.DestinationPlaceId  &&
                    ((t.PeopleCount - t.Bids.Count(b => b.IsAccepted == true)) >= model.Places) &&

                    ((model.PriceBottom == null && model.PriceTop == null) || (t.Pay >= model.PriceBottom && t.Pay <= model.PriceTop)) &&

                    //((model.IsAllowdedDeviation == null) || (t.IsAllowdedDeviation == model.IsAllowdedDeviation)) &&
                    ((model.IsAllowdedChildren == null) || (t.IsAllowdedChildren == model.IsAllowdedChildren)) &&
                    ((model.IsAllowdedDrink == null) || (t.IsAllowdedDrink == model.IsAllowdedDrink)) &&
                    ((model.IsAllowdedMusic == null) || (t.IsAllowdedMusic == model.IsAllowdedMusic)) &&
                    ((model.IsAllowdedSmoke == null) || (t.IsAllowdedSmoke == model.IsAllowdedSmoke)) &&
                    ((model.IsAllowdedPets == null) || (t.IsAllowdedPets == model.IsAllowdedPets)) &&
                    ((model.IsAllowdedEat == null) || (t.IsAllowdedEat == model.IsAllowdedEat))

                ).ToList();
                
                if (model.IsAllowdedDeviation == true)
                {
                    for (var i = 0; i < trips.Count; i++)
                    {
                        var tripWayPoints = trips[i].WayPoints.OrderBy(x => x.WayPointIndex).ToList();

                        var isContained = false;
                        for (var j = 0; j < model.WayPoints.Count; j++)
                        {                            
                            if (tripWayPoints[j].PlaceId == model.WayPoints[j])
                            {
                                isContained = true;
                                break;
                            }
                        }
                        if (!isContained) trips.RemoveAt(i--);
                    }
                }
                else if (model.IsAllowdedDeviation == false)
                {
                    for (var i = 0; i < trips.Count; i++)
                    {
                        if (trips[i].WayPoints.Count == model.WayPoints.Count)
                        {
                            var tripWayPoints = trips[i].WayPoints.OrderBy(x => x.WayPointIndex).ToList();
                            
                            for (var j = 0; j < model.WayPoints.Count; j++)
                            {
                                if (tripWayPoints[j].PlaceId != model.WayPoints[j])
                                {
                                    trips.RemoveAt(i--);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            trips.RemoveAt(--i);
                        }
                    }
                }
                              
                var totalCount = trips.Count();

                if (totalCount != 0)
                {
                    result.TotalCount = totalCount;
                    result.Results = Mapper.Map<List<Trip>, List<OutputFindTripModel>>
                    (
                        trips
                        .OrderBy(model.SortField + (model.SortOrder ? " ascending" : " descending"))
                        .Skip(model.PerPage * (page - 1))
                        .Take(model.PerPage)
                        .ToList()
                    );
                }
                return result;
            }
            else
            {
                throw new Exception(AppConstants.FIND_TRIPS_ERROR);
            }
        }

        public OutputList<OutputMyTripModel> GetAllTrips(InputPageSortModel<Int32> model)
        {
            var result = new OutputList<OutputMyTripModel>();

            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                var page = Math.Max(model.Page, 1);
                var perPage = Math.Max(model.PerPage, 1);
                var countExcluded = Math.Max(model.CountExcluded, 0);

                var driverTrips =
                    user.
                    Trips.
                    Where(t => !t.IsDeleted && (model.ShowEnded || t.EndDate > DateTimeOffset.Now));

                var passengerTrips =
                    user.
                    Bids.
                    Where(b => b.IsAccepted == true).
                    Select(b => b.Trip).
                    Where(t => !t.IsDeleted && (model.ShowEnded || t.EndDate > DateTimeOffset.Now));

                var allTrips =
                    driverTrips.
                    Concat(passengerTrips)
                    .OrderByDescending(t => t.CreationDate)
                    .AsQueryable();   

                var totalCount = allTrips.Count();

                if (totalCount != 0)
                {
                    result.TotalCount = totalCount;
                    result.Results = Mapper.Map<IQueryable<Trip>, List<OutputMyTripModel>>(allTrips.Skip(perPage * (page - 1) - countExcluded).Take(perPage));

                    foreach (var trip in result.Results)                    
                        if (!trip.IsStarted && (model.IsOwner && model.UserId == trip.UserId))                        
                            trip.AllowManage = true;                                            
                }
                return result;
            }
            else
            {
                throw new RedirectException(AppConstants.USER_NOT_FOUND);
            }
        }

        public OutputList<OutputMyTripModel> GetDriverTrips(InputPageSortModel<Int32> model)
        {
            var result = new OutputList<OutputMyTripModel>();

            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                var page = Math.Max(model.Page, 1);
                var perPage = Math.Max(model.PerPage, 1);
                var countExcluded = Math.Max(model.CountExcluded, 0);

                var driverTrips =
                    user.
                    Trips.
                    Where(t => !t.IsDeleted && (model.ShowEnded || t.EndDate > DateTimeOffset.Now))
                    .OrderByDescending(t => t.CreationDate)
                    .AsQueryable();

                var totalCount = driverTrips.Count();

                if (totalCount != 0)
                {
                    result.TotalCount = totalCount;
                    result.Results = Mapper.Map<IQueryable<Trip>, List<OutputMyTripModel>>(driverTrips.Skip(perPage * (page - 1) - countExcluded).Take(perPage));

                    foreach (var trip in result.Results)
                        if (!trip.IsStarted && (model.IsOwner && model.UserId == trip.UserId))
                            trip.AllowManage = true; 
                }
                return result;
            }
            else
            {
                throw new RedirectException(AppConstants.USER_NOT_FOUND);
            }
        }

        public OutputList<OutputMyTripModel> GetPassengerTrips(InputPageSortModel<Int32> model)
        {
            var result = new OutputList<OutputMyTripModel>();

            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                var page = Math.Max(model.Page, 1);
                var perPage = Math.Max(model.PerPage, 1);
                var countExcluded = Math.Max(model.CountExcluded, 0);

                var passengerTrips =
                    user.
                    Bids.
                    Where(b => b.IsAccepted == true).
                    Select(b => b.Trip).
                    Where(t => !t.IsDeleted && (model.ShowEnded || t.EndDate > DateTimeOffset.Now))
                    .OrderByDescending(t => t.CreationDate)
                    .AsQueryable();

                var totalCount = passengerTrips.Count();

                if (totalCount != 0)
                {
                    result.TotalCount = totalCount;
                    result.Results = Mapper.Map<IQueryable<Trip>, List<OutputMyTripModel>>(passengerTrips.Skip(perPage * (page - 1) - countExcluded).Take(perPage));

                    foreach (var trip in result.Results)
                        if (!trip.IsStarted && (model.IsOwner && model.UserId == trip.UserId))
                            trip.AllowManage = true; 
                }
                return result;
            }
            else
            {
                throw new RedirectException(AppConstants.USER_NOT_FOUND);
            }
        }

        #endregion

        #region Modifications
        
        private void AddNotExistedPlaces(InputCreateTripModel model)
        {
            var originPlace = _ctx.Places.FirstOrDefault(p => p.PlaceId == model.OriginPlace.PlaceId);
            if (originPlace == null)
            {
                originPlace = new Place
                {
                    PlaceId = model.OriginPlace.PlaceId,
                    Name = model.OriginPlace.Name
                };
                _ctx.Places.Add(originPlace);
                _ctx.SaveChanges();
            }

            var destinationPlace = _ctx.Places.FirstOrDefault(p => p.PlaceId == model.DestinationPlace.PlaceId);
            if (destinationPlace == null)
            {
                destinationPlace = new Place
                {
                    PlaceId = model.DestinationPlace.PlaceId,
                    Name = model.DestinationPlace.Name
                };
                _ctx.Places.Add(destinationPlace);
                _ctx.SaveChanges();
            }

            if (model.SelectedPlaces != null)
            {
                for (int i = 0; i < model.SelectedPlaces.Count; i++)
                {
                    var id = model.SelectedPlaces[i].PlaceId;
                    var place = _ctx.Places.FirstOrDefault(p => p.PlaceId == id);

                    if (place == null)
                    {
                        place = new Place
                        {
                            PlaceId = model.SelectedPlaces[i].PlaceId,
                            Name = model.SelectedPlaces[i].Name
                        };
                        _ctx.Places.Add(place);
                        _ctx.SaveChanges();
                    }
                }
            }
        }

        private void UpdateCarWhenCreateTrip(InputCreateTripModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
 
            user.Car = model.Car;
            user.CarImage = model.CarImage;
            user.CarImageExtension = model.CarImageExtension;
            user.CarClass = model.CarClass;

            _ctx.SaveChanges();
        }

        public void CreateTrip(InputCreateTripModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                AddNotExistedPlaces(model);
                UpdateCarWhenCreateTrip(model);

                List<WayPoint> wayPointsToAdd = null;
                if (model.SelectedPlaces != null)
                {
                    wayPointsToAdd = new List<WayPoint>();
                    var i = 0;
                    foreach (var wp in model.SelectedPlaces)
                    {
                        var wayPoint = new WayPoint
                        {
                            PlaceId = wp.PlaceId,
                            WayPointIndex = (++i)                        
                        };
                        wayPointsToAdd.Add(wayPoint);
                    }
                }

                var trip = new Trip
                {
                    IsAllowdedDeviation = model.IsAllowdedDeviation,//
                    IsAllowdedChildren = model.IsAllowdedChildren,
                    IsAllowdedPets = model.IsAllowdedPets,                   
                    IsAllowdedMusic = model.IsAllowdedMusic,
                    IsAllowdedDrink = model.IsAllowdedDrink,
                    IsAllowdedEat = model.IsAllowdedEat,
                    IsAllowdedSmoke = model.IsAllowdedSmoke,
                    About = model.About,
                    Car = model.Car,
                    CarImage = model.CarImage,
                    CarImageExtension = model.CarImageExtension,
                    CarClass = model.CarClass,
                    UserId = model.UserId,
                    OriginPlaceId = model.OriginPlace.PlaceId,//
                    DestinationPlaceId = model.DestinationPlace.PlaceId,//
                    CreationDate = DateTimeOffset.Now,
                    LeavingDate = 
                        model.
                        LeavingDate.
                        AddHours(DateTimeOffset.Now.Hour).
                        AddMinutes(DateTimeOffset.Now.Minute).
                        AddSeconds(DateTimeOffset.Now.Second),
                    EndDate = 
                        model.
                        LeavingDate.
                        AddDays(3).
                        AddHours(DateTimeOffset.Now.Hour).
                        AddMinutes(DateTimeOffset.Now.Minute).
                        AddSeconds(DateTimeOffset.Now.Second),
                    PeopleCount = model.PeopleCount,
                    Pay = model.Pay,
                    WayPoints = wayPointsToAdd//
                };

                try
                {
                    _ctx.Trips.Add(trip);
                    _ctx.SaveChanges();
                }
                catch
                {
                    throw new AlertException(AppConstants.TRIP_CREATE_ERROR);
                }
            }
            else
            {
                throw new AlertException(AppConstants.USER_NOT_FOUND);
            }
        }

        public void EditTrip(InputCreateTripModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                var trip = user.Trips.FirstOrDefault(t => t.TripId == model.TripId);
                if (trip == null)
                {
                    throw new AlertException(AppConstants.USER_HASNT_SUCH_TRIP);
                }

                AddNotExistedPlaces(model);

                if (trip.WayPoints != null && trip.WayPoints.Count != 0)
                {
                    foreach (var wayPoint in trip.WayPoints.ToList())
                    {
                        _ctx.WayPoint.Remove(wayPoint);
                    }
                }
                
                List<WayPoint> wayPointsToAdd = null;
                if (model.SelectedPlaces != null)
                {
                    wayPointsToAdd = new List<WayPoint>();
                    var i = 0;
                    foreach (var wp in model.SelectedPlaces)
                    {
                        var wayPoint = new WayPoint
                        {
                            PlaceId = wp.PlaceId,
                            WayPointIndex = (++i)
                        };
                        wayPointsToAdd.Add(wayPoint);
                    }
                }

                trip.IsAllowdedDeviation = model.IsAllowdedDeviation;
                trip.IsAllowdedChildren = model.IsAllowdedChildren;
                trip.IsAllowdedPets = model.IsAllowdedPets;
                trip.IsAllowdedMusic = model.IsAllowdedMusic;
                trip.IsAllowdedDrink = model.IsAllowdedDrink;
                trip.IsAllowdedEat = model.IsAllowdedEat;
                trip.IsAllowdedSmoke = model.IsAllowdedSmoke;

                trip.About = model.About;
                trip.Car = model.Car;
                trip.CarImage = model.CarImage;
                trip.CarImageExtension = model.CarImageExtension;
                trip.CarClass = model.CarClass;
                trip.UserId = model.UserId;
                trip.OriginPlaceId = model.OriginPlace.PlaceId;
                trip.DestinationPlaceId = model.DestinationPlace.PlaceId;

                trip.CreationDate = DateTimeOffset.Now;

                trip.LeavingDate =
                    model.
                    LeavingDate.
                    AddHours(DateTimeOffset.Now.Hour).
                    AddMinutes(DateTimeOffset.Now.Minute).
                    AddSeconds(DateTimeOffset.Now.Second);

                trip.EndDate =
                    model.
                    LeavingDate.
                    AddDays(3).
                    AddHours(DateTimeOffset.Now.Hour).
                    AddMinutes(DateTimeOffset.Now.Minute).
                    AddSeconds(DateTimeOffset.Now.Second);

                trip.PeopleCount = model.PeopleCount;
                trip.Pay = model.Pay;
                trip.WayPoints = wayPointsToAdd;

                try
                {
                    _ctx.SaveChanges();
                }
                catch
                {
                    throw new AlertException(AppConstants.TRIP_EDIT_ERROR);
                }
            }
            else
            {
                throw new AlertException(AppConstants.USER_NOT_FOUND);
            }
        }

        public void RemoveTrip(InputManageTripModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                var trip = user.Trips.FirstOrDefault(t => t.TripId == model.TripId);
                if (trip != null)
                {
                    if (trip.LeavingDate < DateTimeOffset.Now)
                    {
                        throw new AlertException(AppConstants.TRIP_REMOVE_CAUSE_DATE_ERROR);
                    }
                    try
                    {
                        trip.IsDeleted = true;
                        _ctx.SaveChanges();
                    }
                    catch
                    {
                        throw new AlertException(AppConstants.TRIP_REMOVE_ERROR);
                    }
                }
                else
                {
                    throw new AlertException(AppConstants.USER_HASNT_SUCH_TRIP);
                }
            }
            else
            {
                throw new AlertException(AppConstants.USER_NOT_FOUND);
            }
        }

        public void RecoverTrip(InputManageTripModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                var trip = user.Trips.FirstOrDefault(t => t.TripId == model.TripId);
                if (trip != null)
                {
                    try
                    {
                        trip.IsDeleted = false;
                        _ctx.SaveChanges();
                    }
                    catch
                    {
                        throw new AlertException(AppConstants.TRIP_RECOVER_ERROR);
                    }
                }
                else
                {
                    throw new AlertException(AppConstants.USER_HASNT_SUCH_TRIP);
                }
            }
            else
            {
                throw new AlertException(AppConstants.USER_NOT_FOUND);
            }
        }

        //public void VoteForTrip(VoteTripModel model)
        //{
        //    var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
        //    if (user != null)
        //    {
        //        var trip = _ctx.Trips.FirstOrDefault(t => t.TripId == model.TripId);
        //        if (trip != null)
        //        {
        //            if (trip.UserId != model.UserId)
        //            {
        //                var vote = model.Vote;
        //                if (vote > 5)
        //                {
        //                    vote = 5;
        //                }
        //                else if (vote < 1)
        //                {
        //                    vote = 1;
        //                }

        //                var like = user.Likes.FirstOrDefault(l => l.TripId == model.TripId);
        //                if (like != null)
        //                {
        //                    like.Vote = vote;
        //                }
        //                else
        //                {
        //                    like = new Like { TripId = model.TripId, Vote = vote };
        //                    user.Likes.Add(like);
        //                }
        //                _ctx.SaveChanges();
        //            }
        //            else
        //            {
        //                throw new RedirectException("Нельзя проголосовать за свою поездку!");
        //            }
        //        }
        //        else
        //        {
        //            throw new RedirectException("Такой поездки не существует!");
        //        }
        //    }
        //    else
        //    {
        //        throw new RedirectException("Такого пользователя не существует!");
        //    }
        //}

        #endregion

    }
}
