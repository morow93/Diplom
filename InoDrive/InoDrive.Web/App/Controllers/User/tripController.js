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

    $scope.carsFolder = "images/cars/";
    $scope.noCarImage = ngAuthSettings.clientAppBaseUri + "images/no-car.png";
    $scope.fullCarsFolder = ngAuthSettings.apiServiceBaseUri + $scope.carsFolder;

    $scope.needHideBidButton = true;
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

                if ($scope.authentication) {
                    if ($scope.authentication.userId != trip.userId) {
                        $scope.needHideBidButton = false;
                    }
                }

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
                debugger;
                $scope.curPath = [];
                $scope.curPath.push(new WayPoint(trip.originPlace.lat, trip.originPlace.lng, 0));
                if (trip.wayPoints) {
                    for (var i = 0; i < trip.wayPoints.length; i++) {
                        var wp = trip.wayPoints[i];
                        $scope.curPath.push(new WayPoint(wp.lat, wp.lng, i + 1));
                    }
                }
                $scope.curPath.push(new WayPoint(trip.destinationPlace.lat, trip.destinationPlace.lng, i + 1));

                $scope.p =
                {
                    id: 2,
                    path: $scope.curPath,
                    stroke: {
                        color: '#6060FB',
                        weight: 3
                    },
                    editable: false,
                    draggable: false,
                    geodesic: true,
                    visible: true
                };
                $scope.map = {
                    center: {
                        lat: trip.originPlace.lat, lng: trip.originPlace.lng
                    },
                    zoom: 10,
                    bounds: {}
                };

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

    function WayPoint(latitude, longitude, id) {
        this.latitude = latitude;
        this.longitude = longitude;
        this.id = id;
    };

    $scope.getTrip();

});