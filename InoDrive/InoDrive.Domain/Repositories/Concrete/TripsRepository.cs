using InoDrive.Domain.Contexts;
using InoDrive.Domain.Entities;
using InoDrive.Domain.Models.InputModels;
using InoDrive.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Repositories.Concrete
{
    public class TripRepository : ITripsRepository
    {
        public TripRepository(DataContext dataContext)
        {
            _ctx = dataContext;
        }
        private DataContext _ctx;

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
        /// Get info about last added trip
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public LastTripInfo GetLastTripInfo(ShortUserModel model)
        //{
        //    var result = new LastTripInfo();

        //    var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
        //    if (user != null)
        //    {
        //        var lastUserTrip = user.Trips.Where(t => !t.IsDeleted).OrderByDescending(t => t.CreationDate).FirstOrDefault();
        //        if (lastUserTrip != null)
        //        {
        //            var lastUserTripProfile = lastUserTrip.TripProfile;
        //            if (lastUserTripProfile != null)
        //            {
        //                result.CarImage = lastUserTripProfile.CarImage;
        //                result.CarDescription = lastUserTripProfile.CarDescription;
        //                result.Baggage = lastUserTripProfile.Baggage;
        //                result.Comfort = lastUserTripProfile.Comfort;
        //                result.Stage = lastUserTripProfile.Stage;
        //            }
        //        }
        //        return result;
        //    }
        //    else
        //    {
        //        throw new RedirectException("Такого пользователя не существует!");
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
        /// Get trips of current user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public ResultMyTripsModel GetMyTrips(MyTripsPagedOrderModel model)
        //{
        //    var result = new ResultMyTripsModel();

        //    var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
        //    if (user != null)
        //    {
        //        var page = Math.Max(model.PageModel.Page, 1);
        //        var countExcluded = Math.Max(model.CountExcluded, 0);

        //        var trips = user.Trips.Where(t => !t.IsDeleted).AsQueryable();
        //        var totalCount = trips.Count();

        //        if (totalCount != 0)
        //        {
        //            result.TotalCount = totalCount;

        //            var resultTrips = trips.Select(t => new MyTripModel
        //            {
        //                TripId = t.TripId,
        //                LeavingDate = t.LeavingDate,
        //                CreationDate = t.CreationDate,
        //                IsDeleted = false,
        //                Pay = t.PayForOne,
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
        //                UserOwner = new UserModel
        //                {
        //                    UserId = model.UserId,
        //                    FirstName = user.FirstName,
        //                    LastName = user.LastName
        //                },
        //                TotalPlaces = t.PeopleCount,
        //                FreePlaces = t.PeopleCount - t.Bids.Count(b => b.IsAccepted == true)

        //            })
        //            .OrderBy(model.SortOption.Field + (model.SortOption.Order ? " ascending" : " descending"))
        //            .Skip(model.PageModel.PerPage * (page - 1) - countExcluded).Take(model.PageModel.PerPage)
        //            .ToList<MyTripModel>();

        //            result.MyTrips = resultTrips;
        //        }
        //        return result;
        //    }
        //    else
        //    {
        //        throw new RedirectException("Нет такого пользователя!");
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

        #endregion

        #region Modifications

        //public void EditTrip(EditTripModel model)
        //{
        //    var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
        //    if (user != null)
        //    {
        //        var trip = user.Trips.FirstOrDefault(t => t.TripId == model.TripId);
        //        if (trip != null)
        //        {
        //            trip.OriginCityId = model.OriginCity.CityId;
        //            trip.DestinationCityId = model.DestinationCity.CityId;
        //            trip.LeavingDate = model.Date;
        //            trip.PeopleCount = model.Places;
        //            trip.PayForOne = model.Pay;

        //            if (trip.WayPoints.Count != 0)
        //            {
        //                foreach (var wayPoint in trip.WayPoints.ToList())
        //                    _ctx.WayPoint.Remove(wayPoint);
        //            }
        //            for (int i = 0; i < model.SelectedWayPoints.Count; i++)
        //            {
        //                trip.WayPoints.Add(new WayPoint
        //                {
        //                    TripId = model.TripId,
        //                    CityId = model.SelectedWayPoints[i].CityId,
        //                    IndexNumber = (i + 1)
        //                });
        //            }

        //            var tripProfile = trip.TripProfile;
        //            if (tripProfile == null)
        //            {
        //                throw new RedirectException("У поездки нет своих настроек (задаются при создании)!");
        //            }
        //            else
        //            {
        //                tripProfile.CarDescription = model.CarDescription;
        //                tripProfile.CarImage = model.CarImage;
        //                tripProfile.Stage = model.Stage;
        //                tripProfile.Comfort = model.Comfort ?? 0;
        //                tripProfile.Baggage = model.Baggage ?? 0;
        //                tripProfile.IsAllowdedDeviation = model.AllowDeviates;
        //                tripProfile.ISAllowdedPets = model.AllowPets;
        //                tripProfile.IsAllowdedMusic = model.AllowMusic;
        //                tripProfile.IsAllowdedEat = model.AllowEat;
        //                tripProfile.IsAllowdedDrink = model.AllowDrink;
        //                tripProfile.IsAllowdedSmoke = model.AllowSmoke;
        //                tripProfile.MoreInfo = model.Info;
        //            }
        //            try
        //            {
        //                _ctx.SaveChanges();
        //            }
        //            catch
        //            {
        //                throw new AlertException("Ошибка при редактировании поездки!");
        //            }
        //        }
        //        else
        //        {
        //            throw new RedirectException("У данного пользователя нет такой поездки!");
        //        }
        //    }
        //    else
        //    {
        //        throw new RedirectException("Такого пользователя не существует!");
        //    }
        //}

        public void CreateTrip(InputCreateTripModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                List<WayPoint> wayPointsToAdd = null;
                if (model.SelectedPlaces != null)
                {
                    wayPointsToAdd = new List<WayPoint>();
                    var i = 0;
                    foreach (var wp in model.SelectedPlaces)
                    {
                        var wayPoint = new WayPoint
                        {
                            //CityId = wp.CityId,
                            //IndexNumber = (++i)//from one
                        };
                        wayPointsToAdd.Add(wayPoint);
                    }
                }
                //var tripProfile = new TripProfile
                //{
                //    IsAllowdedDeviation = model.AllowDeviates,
                //    ISAllowdedPets = model.AllowPets,
                //    IsAllowdedMusic = model.AllowMusic,
                //    IsAllowdedDrink = model.AllowDrink,
                //    IsAllowdedEat = model.AllowEat,
                //    IsAllowdedSmoke = model.AllowSmoke,
                //    MoreInfo = model.Info,
                //    CarDescription = model.CarDescription,
                //    CarImage = model.CarImage,
                //    CarImageExtension = model.CarImageExtension,
                //    Baggage = model.Baggage ?? 0,
                //    Comfort = model.Comfort ?? 0,
                //    Stage = model.Stage
                //};
                //var trip = new Trip
                //{
                //    UserId = model.UserId,
                //    OriginCityId = model.OriginCity.CityId,
                //    DestinationCityId = model.DestinationCity.CityId,
                //    CreationDate = DateTime.Now,
                //    LeavingDate = model.Date,
                //    PeopleCount = model.Places,
                //    PayForOne = model.Pay,
                //    WayPoints = wayPointsToAdd,
                //    TripProfile = tripProfile
                //};

                //try
                //{
                //    _ctx.Trips.Add(trip);
                //    _ctx.SaveChanges();
                //}
                //catch
                //{
                //    throw new RedirectException("Ошибка при создании поездки!");
                //}
            }
            else
            {
                //throw new RedirectException("Такого пользователя не существует!");
            }
        }

        //public void DeleteTrip(ManageTripModel model)
        //{
        //    var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
        //    if (user != null)
        //    {
        //        var trip = user.Trips.FirstOrDefault(t => t.TripId == model.TripId);
        //        if (trip != null)
        //        {
        //            try
        //            {
        //                trip.IsDeleted = true;
        //                _ctx.SaveChanges();
        //            }
        //            catch
        //            {
        //                throw new AlertException("Ошибка при записи в базу!");
        //            }
        //        }
        //        else
        //        {
        //            throw new RedirectException("У данного пользователя нет такой поездки!");
        //        }
        //    }
        //    else
        //    {
        //        throw new RedirectException("Такого пользователя не существует!");
        //    }
        //}

        //public void RecoverTrip(ManageTripModel model)
        //{
        //    var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
        //    if (user != null)
        //    {
        //        var trip = user.Trips.FirstOrDefault(t => t.TripId == model.TripId);
        //        if (trip != null)
        //        {
        //            try
        //            {
        //                trip.IsDeleted = false;
        //                _ctx.SaveChanges();
        //            }
        //            catch
        //            {
        //                throw new AlertException("Ошибка при записи в базу!");
        //            }
        //        }
        //        else
        //        {
        //            throw new RedirectException("У данного пользователя нет такой поездки!");
        //        }
        //    }
        //    else
        //    {
        //        throw new RedirectException("Такого пользователя не существует!");
        //    }
        //}

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
