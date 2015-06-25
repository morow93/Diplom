'use strict';
angular.module('InoDrive').controller('tripController', function (
    $scope,
    $rootScope,
    $q,
    $location,
    $window,
    $stateParams,
    $alert,
    $modal,
    $timeout,
    authService,
    tripsService,
    //bidsService,
    ngAuthSettings) {

    $scope.comment = { title: "", vote: 0 };
    $scope.carsFolder = "images/cars/";
    $scope.avatarsFolder = "images/avatars/";
    $scope.noCarImage = ngAuthSettings.clientAppBaseUri + "content/images/no-car.png";
    $scope.noAvatarImage = ngAuthSettings.clientAppBaseUri + "content/images/no-avatar.jpg";
    $scope.fullCarsFolder = ngAuthSettings.apiServiceBaseUri + $scope.carsFolder;
    $scope.fullAvatarsFolder = ngAuthSettings.apiServiceBaseUri + $scope.avatarsFolder;

    $scope.addBidFlag = false;

    $scope.getPercentageRating = function () {
        if ($scope.total == 0) {
            return -1;
        }
        var percentageRating = ($scope.total / ($scope.max * $scope.count)) * 100;
        percentageRating = Math.ceil(percentageRating);
        return percentageRating;
    };

    $scope.voteForTrip = function (rating) {

        debugger;
        var params = {
            vote: rating,
            userId: $scope.authentication.userId,
            tripId: $stateParams.tripId
        };

        tripsService.voteForTrip(params).then(function () {
            //success
        }).catch(function (err) {
            throw err.data;
        });
    };

    $scope.getTrip = function () {

        var params = {
            userId: $scope.authentication.userId,
            tripId: $stateParams.tripId
        };

        $timeout(function () {

            tripsService.getTrip(params).then(function (trip) {

                if (trip.carImage) {

                    var fullPathToFile = ngAuthSettings.apiServiceBaseUri + $scope.carsFolder + trip.carImage;
                    isImage(fullPathToFile).then(function (test) {
                        if (test) {
                            $scope.dataCarUrl = fullPathToFile;
                        }
                    });
                }

                for (var i = 0; i < trip.userIndicators; i++) {
                    var current = trip.userIndicators[i];
                    var fullPathToImg = ngAuthSettings.apiServiceBaseUri + $scope.avatarsFolder + current.avatarImage;
                    isImage(fullPathToFile).then(function (test) {
                        if (test) {
                            current.dataIndicatorUrl = fullPathToImg;
                        }
                    });
                }

                $scope.trip = trip;

                var waypts = [];
                for (var i = 0; i < trip.wayPoints.length; i++) {
                    waypts.push({
                        location: new google.maps.LatLng(trip.wayPoints[i].lat, trip.wayPoints[i].lng),
                        stopover: true
                    });
                }

                var directionsDisplay = new google.maps.DirectionsRenderer({ draggable: true });
                var directionsService = new google.maps.DirectionsService();
                var map;

                var myOptions = {
                    zoom: 10,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    center: new google.maps.LatLng(trip.originPlace.lat, trip.originPlace.lng)
                };

                map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

                var request = {
                    origin: new google.maps.LatLng(trip.originPlace.lat, trip.originPlace.lng),
                    destination: new google.maps.LatLng(trip.destinationPlace.lat, trip.destinationPlace.lng),
                    travelMode: google.maps.TravelMode["DRIVING"]
                };
                if (waypts.length > 0) request.waypoints = waypts;

                directionsService.route(request, function (response, status) {
                    if (status == google.maps.DirectionsStatus.OK) {

                        directionsDisplay.setMap(map);
                        directionsDisplay.setPanel(document.getElementById("directions"));
                        directionsDisplay.setDirections(response);

                    }
                });
      

            }).catch(function (error) {
                //neederror
            }).finally(function () {
                $scope.wasLoaded = true;
            });

        }, 1500);

    };
    
    $scope.addBid = function () {

        if ($scope.authentication.isAuth && !$scope.trip.isBidded) {
            $scope.addBidFlag = true;
            var params = {
                userId: $scope.authentication.userId,
                tripId: $stateParams.tripId
            };

            //bidsService.addBid(params).then(function (data) {
            //    $scope.trip.isBidded = true;
            //    $scope.addBidFlag = false;
            //}).catch(function (error) {
            //    throw error.data;
            //}).finally(function () {
            //    $scope.addBidFlag = false;
            //});
        }
    };

    function isImage(src) {

        var deferred = $q.defer();

        var image = new Image();
        image.onerror = function () {
            deferred.resolve(false);
        };
        image.onload = function () {
            deferred.resolve(true);
        };
        image.src = src;

        return deferred.promise;
    };

    $scope.getTrip();

});