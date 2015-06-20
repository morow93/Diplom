using InoDrive.Domain.Contexts;
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
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

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

        /// <summary>
        /// Get trip details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get full trip info for edit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public EditTripModel GetFullTripInfo(ManageTripModel model)
        //{
        //    var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
        //    if (user != null)
        //    {
        //        var trip = user.Trips.FirstOrDefault(t => t.TripId == model.TripId);
        //        if (trip != null)
        //        {
        //            var tripProfile = trip.TripProfile;

        //            var result = new EditTripModel
        //            {
        //                TripId = model.TripId,
        //                UserId = model.UserId,
        //                Date = trip.LeavingDate,
        //                Pay = (int?)trip.PayForOne,
        //                Places = trip.PeopleCount,

        //                OriginCity = new CityModel
        //                {
        //                    CityId = trip.OriginCityId,
        //                    CityName = trip.OriginCity.RuCityName,
        //                    RegionName = trip.OriginCity.Region.RuRegionName
        //                },

        //                DestinationCity = new CityModel
        //                {
        //                    CityId = trip.DestinationCityId,
        //                    CityName = trip.DestinationCity.RuCityName,
        //                    RegionName = trip.DestinationCity.Region.RuRegionName
        //                },

        //                SelectedWayPoints = trip.WayPoints.Select(n => new CityModel
        //                {
        //                    CityId = n.CityId,
        //                    CityName = n.City.RuCityName,
        //                    RegionName = n.City.Region.RuRegionName
        //                }).ToList<CityModel>()
        //            };
        //            if (tripProfile != null)
        //            {
        //                result.CarDescription = tripProfile.CarDescription;
        //                result.CarImage = tripProfile.CarImage;
        //                result.Baggage = tripProfile.Baggage;
        //                result.Comfort = tripProfile.Comfort;
        //                result.Stage = tripProfile.Stage;
        //                result.AllowDeviates = tripProfile.IsAllowdedDeviation;
        //                result.AllowPets = tripProfile.ISAllowdedPets;
        //                result.AllowMusic = tripProfile.IsAllowdedMusic;
        //                result.AllowEat = tripProfile.IsAllowdedEat;
        //                result.AllowDrink = tripProfile.IsAllowdedDrink;
        //                result.AllowSmoke = tripProfile.IsAllowdedSmoke;
        //                result.Info = tripProfile.MoreInfo;
        //            }
        //            return result;
        //        }
        //        else
        //        {
        //            throw new RedirectException("У пользователя нет такой поездки!");
        //        }
        //    }
        //    else
        //    {
        //        throw new RedirectException("Такого пользователя не существует!");
        //    }
        //}

        /// <summary>
        /// Find trips by some params
        /// </summary>
        /// <param name="model">origin city, destination city, date, free places</param>
        /// <returns></returns>
        //public ResultFindTripsModel FindTrips(FindTripsPagedOrderModel model)
        //{
        //    var result = new ResultFindTripsModel();

        //    if (model.DestinationCity != null && model.OriginCity != null && model.Places != 0)
        //    {
        //        var page = Math.Max(model.PageModel.Page, 1);

        //        var trips = _ctx.Trips.Where(t =>
        //            !t.IsDeleted &&
        //            DbFunctions.TruncateTime(t.LeavingDate) >= DbFunctions.TruncateTime(DateTime.Now) &&
        //            DbFunctions.TruncateTime(t.LeavingDate) == DbFunctions.TruncateTime(model.Date) &&
        //            t.OriginCityId == model.OriginCity.CityId &&
        //            t.DestinationCityId == model.DestinationCity.CityId &&
        //            ((t.PeopleCount - t.Bids.Count(b => b.IsAccepted == true)) >= model.Places)
        //        ).ToList<Trip>();

        //        var totalCount = trips.Count();

        //        if (totalCount != 0)
        //        {
        //            result.TotalCount = totalCount;

        //            var resultTrips = trips.Select(t => new FindTripModel
        //            {
        //                TripId = t.TripId,
        //                Pay = t.PayForOne,
        //                LeavingDate = t.LeavingDate,
        //                CreationDate = t.CreationDate,
        //                UserOwner = new UserModel
        //                {
        //                    UserId = t.UserId,
        //                    FirstName = t.User.FirstName,
        //                    LastName = t.User.LastName
        //                },
        //                OriginCity = new CityModel
        //                {
        //                    CityId = t.OriginCityId,
        //                    CityName = t.OriginCity.RuCityName,
        //                    RegionName = t.OriginCity.Region.RuRegionName
        //                },
        //                DestinationCity = new CityModel
        //                {
        //                    CityId = t.DestinationCityId,
        //                    CityName = t.DestinationCity.RuCityName,
        //                    RegionName = t.DestinationCity.Region.RuRegionName
        //                },
        //                TotalPlaces = t.PeopleCount,
        //                FreePlaces = t.PeopleCount - t.Bids.Count(b => b.IsAccepted == true),
        //                UserRating =
        //                    ((double)(t.User.Trips.SelectMany(lk => lk.Likes).Select(n => n.Vote).Sum()) /
        //                    (double)(t.User.Trips.SelectMany(l => l.Likes).Count() * 5)) * 100
        //            })
        //            .OrderBy(model.SortOption.Field + (model.SortOption.Order ? " ascending" : " descending"))
        //            .Skip(model.PageModel.PerPage * (page - 1)).Take(model.PageModel.PerPage)
        //            .ToList<FindTripModel>();

        //            result.FindTrips = resultTrips;
        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// Extend trips find
        /// </summary>
        /// <param name="model">
        /// origin city, destination city, date, free places, etc.
        /// </param>
        /// <returns></returns>
        //public ResultFindTripsModel ExtendFindTrips(ExtendTripsPagedOrderModel model)
        //{
        //    var result = new ResultFindTripsModel();

        //    if (model.DestinationCity != null && model.OriginCity != null && model.Places != 0)
        //    {
        //        var page = Math.Max(model.PageModel.Page, 1);

        //        var trips = _ctx.Trips.Where(t =>
        //            !t.IsDeleted &&
        //            DbFunctions.TruncateTime(t.LeavingDate) >= DbFunctions.TruncateTime(DateTime.Now) &&
        //            DbFunctions.TruncateTime(t.LeavingDate) == DbFunctions.TruncateTime(model.Date) &&
        //            t.OriginCityId == model.OriginCity.CityId &&
        //            t.DestinationCityId == model.DestinationCity.CityId &&
        //            ((t.PeopleCount - t.Bids.Count(b => b.IsAccepted == true)) >= model.Places) &&

        //            ((model.PayBottom == null && model.PayTop == null) || (t.PayForOne >= model.PayBottom && t.PayForOne <= model.PayTop)) &&

        //            ((model.WasTriped == null) || ((t.User.Trips.Any(tt => !tt.IsDeleted && DbFunctions.TruncateTime(tt.LeavingDate) < DbFunctions.TruncateTime(DateTime.Now)) ||
        //            t.User.Bids.Any(b => b.IsAccepted == true && DbFunctions.TruncateTime(b.Trip.LeavingDate) < DbFunctions.TruncateTime(DateTime.Now))) == model.WasTriped)) &&

        //            ((model.AllowDeviates == null) || (t.TripProfile != null && t.TripProfile.IsAllowdedDeviation == model.AllowDeviates)) &&
        //            ((model.AllowDrink == null) || (t.TripProfile != null && t.TripProfile.IsAllowdedDrink == model.AllowDrink)) &&
        //            ((model.AllowEat == null) || (t.TripProfile != null && t.TripProfile.IsAllowdedEat == model.AllowEat)) &&
        //            ((model.AllowMusic == null) || (t.TripProfile != null && t.TripProfile.IsAllowdedMusic == model.AllowMusic)) &&
        //            ((model.AllowPets == null) || (t.TripProfile != null && t.TripProfile.ISAllowdedPets == model.AllowPets)) &&
        //            ((model.AllowSmoke == null) || (t.TripProfile != null && t.TripProfile.IsAllowdedSmoke == model.AllowSmoke)) &&

        //            ((model.Comfort == null) || (t.TripProfile != null && model.Comfort == t.TripProfile.Comfort)) &&
        //            ((model.Baggage == null) || (t.TripProfile != null && model.Baggage == t.TripProfile.Baggage))
        //        ).ToList<Trip>();

        //        if (model.WayPoints != null && model.WayPoints.Count > 0)
        //        {
        //            for (var i = 0; i < trips.Count; i++)
        //            {
        //                if (trips[i].WayPoints != null)
        //                {
        //                    if (trips[i].WayPoints.Count != model.WayPoints.Count)
        //                    {
        //                        trips.RemoveAt(i--);
        //                    }
        //                    else
        //                    {
        //                        var curWp = trips[i].WayPoints.ToList();
        //                        for (var j = 0; j < model.WayPoints.Count; j++)
        //                        {
        //                            if (curWp[j].CityId != model.WayPoints[j].CityId)
        //                            {
        //                                trips.RemoveAt(i--);
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    trips.RemoveAt(i--);
        //                }
        //            }
        //        }

        //        var totalCount = trips.Count();

        //        if (totalCount != 0)
        //        {
        //            result.TotalCount = totalCount;

        //            var resultTrips = trips.Select(t => new FindTripModel
        //            {
        //                TripId = t.TripId,
        //                Pay = t.PayForOne,
        //                LeavingDate = t.LeavingDate,
        //                CreationDate = t.CreationDate,
        //                UserOwner = new UserModel
        //                {
        //                    UserId = t.UserId,
        //                    FirstName = t.User.FirstName,
        //                    LastName = t.User.LastName
        //                },
        //                OriginCity = new CityModel
        //                {
        //                    CityId = t.OriginCityId,
        //                    CityName = t.OriginCity.RuCityName,
        //                    RegionName = t.OriginCity.Region.RuRegionName
        //                },
        //                DestinationCity = new CityModel
        //                {
        //                    CityId = t.DestinationCityId,
        //                    CityName = t.DestinationCity.RuCityName,
        //                    RegionName = t.DestinationCity.Region.RuRegionName
        //                },
        //                TotalPlaces = t.PeopleCount,
        //                FreePlaces = t.PeopleCount - t.Bids.Count(b => b.IsAccepted == true),
        //                UserRating =
        //                    ((double)(t.User.Trips.SelectMany(lk => lk.Likes).Select(n => n.Vote).Sum()) /
        //                    (double)(t.User.Trips.SelectMany(l => l.Likes).Count() * 5)) * 100
        //            })
        //            .OrderBy(model.SortOption.Field + (model.SortOption.Order ? " ascending" : " descending"))
        //            .Skip(model.PageModel.PerPage * (page - 1)).Take(model.PageModel.PerPage)
        //            .ToList<FindTripModel>();

        //            result.FindTrips = resultTrips;
        //        }
        //    }
        //    return result;
        //}

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
