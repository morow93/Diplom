﻿using InoDrive.Domain.Contexts;
using InoDrive.Domain.Helpers;
using InoDrive.Domain.Models;
using InoDrive.Domain.Models.OutputModels;
using InoDrive.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Repositories.Concrete
{
    public class UsersRepository : IUsersRepository
    {
        public UsersRepository(InoDriveContext dataContext)
        {
            _ctx = dataContext;
        }

        private InoDriveContext _ctx;
        
        public ProfileModel GetUserProfile(ShortUserModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                var result = new ProfileModel
                {
                    UserId = model.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    YearOfBirth = user.YearOfBirth,
                    About = user.About,
                    Phone = user.Phone,
                    AvatarImage = user.AvatarImage,
                    AvatarImageExtension = user.AvatarImageExtension,
                    YearOfStage = user.YearOfStage,
                    Sex = user.Sex,
                    Car = user.Car,
                    CarClass = user.CarClass,
                    CarImage = user.CarImage,
                    CarImageExtension = user.CarImageExtension
                };
                
                return result;
            }
            else
            {
                throw new Exception(AppConstants.USER_NOT_FOUND);
            }   
        }

        public OutputUserSummaryModel GetUserSummary(ShortUserModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                var driverTrips =
                    user.
                    Trips.
                    Where(t => !t.IsDeleted);

                var passengerTrips =
                    user.
                    Bids.
                    Where(b => b.IsAccepted == true).
                    Select(b => b.Trip).
                    Where(t => !t.IsDeleted);

                var rating = 
                    (double)(user.Trips.SelectMany(lk => lk.Commnents).Select(n => n.Vote).Sum()) / 
                    (double)(user.Trips.SelectMany(l => l.Commnents).Count() * 5) * 100;

                var result = new OutputUserSummaryModel
                {
                    Age = user.YearOfBirth == null ? null : DateTimeOffset.Now.Year - user.YearOfBirth,
                    Stage = user.YearOfStage == null ? null : DateTimeOffset.Now.Year - user.YearOfStage,
                    Initials = user.FirstName + " " + user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Sex = user.Sex,
                    DriverTripsCount = driverTrips.Count(),
                    PassengerTripsCount = passengerTrips.Count(),
                    AllTripsCount = driverTrips.Count() + passengerTrips.Count(),
                    Rating = rating,
                    About = user.About,
                    UserId = user.Id
                };

                return result;
            }
            else
            {
                throw new Exception(AppConstants.USER_NOT_FOUND);
            }          
        }

        public void SetUserProfile(ProfileModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                try
                {
                    user.About = model.About;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.About = model.About;
                    user.Phone = model.Phone;
                    user.YearOfBirth = model.YearOfBirth;
                    user.AvatarImage = model.AvatarImage;
                    user.AvatarImageExtension = model.AvatarImageExtension;
                    user.YearOfStage = model.YearOfStage;
                    user.Sex = model.Sex;

                    _ctx.SaveChanges();

                }
                catch
                {
                    throw new Exception(AppConstants.PROFILE_EDIT_ERROR);
                }
            }
            else
            {
                throw new Exception(AppConstants.USER_NOT_FOUND);
            }
        }

        public void SetUserCar(CarModel model)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user != null)
            {
                try
                {
                    user.Car = model.Car;
                    user.CarImage = model.CarImage;
                    user.CarImageExtension = model.CarImageExtension;
                    user.CarClass = model.CarClass;

                    _ctx.SaveChanges();

                }
                catch
                {
                    throw new Exception(AppConstants.PROFILE_EDIT_ERROR);
                }
            }
            else
            {
                throw new Exception(AppConstants.USER_NOT_FOUND);
            }
        }
        
        //public ExtendProfileModel GetUserInfo(WatchingProfileModel model)
        //{
        //    var user = _ctx.Users.FirstOrDefault(U => U.Id == model.ObservedUserId);
        //    if (user != null)
        //    {
        //        var result = new ExtendProfileModel
        //        {
        //            UserId = user.Id,
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            AllowVote = false,
        //            BiddedTrip = false
        //        };

        //        //get count user trips
        //        var countTrips = user.Trips.Count(
        //            t => !t.IsDeleted && 
        //            t.LeavingDate.Date < DateTime.Now.Date && 
        //            t.Bids.Any(bb => bb.IsAccepted == true));
        //        var countBids = user.Bids.Count(b => b.IsAccepted == true && b.Trip.LeavingDate.Date < DateTime.Now.Date);
        //        result.CountTrips = countBids + countTrips;

        //        if (!String.IsNullOrEmpty(model.LoggedUserId)){

        //            var loggedUser = _ctx.Users.FirstOrDefault(u => u.Id == model.LoggedUserId);
        //            if (loggedUser != null)
        //            {
        //                var bid = loggedUser.Bids.FirstOrDefault(b => !b.Trip.IsDeleted && b.TripId == model.ObservedTripId && b.IsAccepted == true);
        //                if (bid != null)
        //                {
        //                    result.BiddedTrip = true;//then display star bar
        //                    if (bid.Trip.LeavingDate.Date < DateTime.Now.Date)
        //                    {
        //                        result.AllowVote = true;//then user has right to vote
        //                    }
        //                }
        //                var like = loggedUser.Likes.FirstOrDefault(l => l.TripId == model.ObservedTripId);
        //                if (like != null)
        //                {
        //                    result.ExposedRating = like.Vote;//get current vote for observed user
        //                }                  

        //            }else
        //            {
        //                throw new Exception("Такого пользователя не существует!");
        //            }
        //        }

        //        //get total rating and total count of votes
        //        var totalLikes = user.Trips.SelectMany(t => t.Likes).ToList<Like>();                    
        //        if (totalLikes.Count != 0)
        //        {
        //            result.TotalRating = totalLikes.Count * 5;
        //            result.TotalVotes = totalLikes.Select(v => v.Vote).Sum();
        //        }

        //        var userProfile = user.UserProfile;
        //        if (userProfile != null)
        //        {
        //            result.PublicEmail = userProfile.PublicEmail;
        //            result.Phone = userProfile.Phone;
        //            result.Info = userProfile.About;
        //            result.Age = userProfile.Age;
        //            result.AvatarImage = userProfile.AvatarImage;
        //        }
        //        return result;
        //    }
        //    else
        //    {
        //        throw new Exception("Такого пользователя не существует!");
        //    }
        //}

    }
}
