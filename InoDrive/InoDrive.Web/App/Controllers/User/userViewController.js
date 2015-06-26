angular.module('InoDrive').controller('userViewController', function ($scope, $state, $timeout, $q, tripsService, usersService, ngAuthSettings, customStorageService) {

    $scope.page = 1;
    $scope.perPage = 10;
    $scope.showEnded = true;
    $scope.tripsType = 1;//all
    $scope.totalCount = 0;
    $scope.firstLoad = false;
    $scope.countExcluded = 0;

    $scope.avatarsFolder = "images/avatars/";
    $scope.noAvatarImage = ngAuthSettings.clientAppBaseUri + "content/images/no-avatar.jpg";

    $scope.selectAllTrips = function () {

        $scope.tripsType = 1;//all
        $scope.getUserTrips = $scope.getAllTrips;
        $scope.firstLoad = false;
        $scope.showEnded = true;

        $scope.reloadUserTrips();
    };

    $scope.selectDriverTrips = function () {

        $scope.tripsType = 2;//driver
        $scope.getUserTrips = $scope.getDriverTrips;
        $scope.firstLoad = false;
        $scope.showEnded = true;

        $scope.reloadUserTrips();
    };

    $scope.selectPassengerTrips = function () {

        $scope.tripsType = 3;//passenger
        $scope.getUserTrips = $scope.getPassengerTrips;
        $scope.firstLoad = false;
        $scope.showEnded = true;

        $scope.reloadUserTrips();
    };    

    $scope.reloadUserTrips = function () {

        $scope.page = 1;
        $scope.showTotalCount = false;
        $scope.trips = [];
        $scope.countExcluded = 0;

        $scope.templateGetUserTrips();
    };
    
    $scope.templateGetUserTrips = function () {

        var templateSearch;

        switch ($scope.tripsType)
        {            
            case 3:
                templateSearch = tripsService.getPassengerTrips;
                break;
            case 2:
                templateSearch = tripsService.getDriverTrips;
                break;
            case 1:
            default:
                templateSearch = tripsService.getAllTrips;
                break;
        }
        
        $scope.totalEmptyResults = false;

        if ($scope.loading) {
            return;
        } else {
            $scope.loading = true;
        }

        var params = {
            userId: $scope.authentication.userId,
            page: $scope.page,
            perPage: $scope.perPage,
            showEnded: $scope.showEnded,
            isOwner: true,
            countExcluded: $scope.countExcluded
        };

        $timeout(function () {

            templateSearch(params).then(function (response) {

                if (response.results && response.results.length > 0 && response.totalCount > 0) {

                    $scope.totalCount = response.totalCount;
                    $scope.showTotalCount = true;
                    $scope.firstLoad = true;

                    if ($scope.trips) {
                        for (var i = 0; i < response.results.length; i++) {
                            $scope.trips.push(response.results[i]);
                        }
                    } else {
                        $scope.trips = response.results;
                    }

                    var totalPages = Math.ceil(($scope.totalCount + $scope.countExcluded)/$scope.perPage);
                    if (++$scope.page > totalPages) {
                        $scope.emptyResults = true;
                    } else {
                        $scope.emptyResults = false;
                    }

                } else {
                    $scope.emptyResults = true;
                    if ($scope.page == 1) {
                        $scope.totalEmptyResults = true;
                    }
                }

            }).catch(function (e) {

                //neederror

            }).finally(function () {

                $scope.loading = false;

            });

        }, 1500);

    };

    $scope.removeTrip = function (tripId) {
  
        var params =
        {
            "userId": $scope.authentication.userId,
            "tripId": tripId
        };

        tripsService.removeTrip(params).then(function () {

            for (var i = 0; i < $scope.trips.length; i++) {
                if ($scope.trips[i].tripId == tripId) {
                    $scope.trips[i].isDeleted = true;
                    break;
                }
            }
            ++$scope.countExcluded;
            --$scope.totalCount;           

        }).catch(function (error) {

            customStorageService.set("notifyToShow", {
                message: 'Внимание! Произошла ошибка при удалении поездки!',
                type: 'danger',
            });

        });

    };

    $scope.recoverTrip = function (tripId) {

        var params =
        {
            "userId": $scope.authentication.userId,
            "tripId": tripId
        };

        tripsService.recoverTrip(params).then(function () {

            for (var i = 0; i < $scope.trips.length; i++) {
                if ($scope.trips[i].tripId == tripId) {
                    $scope.trips[i].isDeleted = false;
                    break;
                }
            }
            --$scope.countExcluded;
            ++$scope.totalCount;            

        }).catch(function (error) {

            customStorageService.set("notifyToShow", {
                message: 'Внимание! Произошла ошибка при восстановлении поездки!',
                type: 'danger',
            });

        });
    };

    $scope.editTrip = function (tripId) {
        $state.go("user.edit_trip", { tripId: tripId }, { reload: false });
    };

    $scope.getTrip = function (tripId) {
        $state.go("user.trip", { tripId: tripId }, { reload: true });
    };

    $scope.getUserSummary = function () {

        usersService.getUserSummary({ userId: $scope.authentication.userId }).then(function (data) {

            if (data.rating == "NaN") {
                data.rating = 0.0;
            }

            if (data.stage == null) {
                data.stage = "Не указан";
            } else if (data.stage == 0) {
                data.stage = "Меньше года";
            } else if (data.stage == 1) {
                data.stage = "1 год";
            } else if (data.stage >= 5 && data.stage <= 20) {
                data.stage = data.stage + " лет";
            } else {
                var indexStage = data.stage.toString()[(data.stage.toString().length - 1)];
                if (indexStage == "2" || indexStage == "3" || indexStage == "4") {
                    data.stage = data.stage + " года";
                } else if (indexStage == 1) {
                    data.stage = data.stage + " год";
                } else {
                    data.stage = data.stage + " лет";
                }
            }

            if (data.age == null) {
                data.age = "Не указан";
            } else if (data.age >= 5 && data.age <= 20) {
                data.age = data.age + " лет";
            } else {     
                var indexStage = data.age.toString()[(data.age.toString().length - 1)];
                if (indexStage == "2" || indexStage == "3" || indexStage == "4") {
                    data.age = data.age + " года";
                } else if (indexStage == "1") {
                    data.age = data.age + " год";
                } else {
                    data.age = data.age + " лет";
                }
            }

            $scope.userInfo = data;

            if ($scope.userInfo.avatarImage) {

                var fullPathToFileOne = ngAuthSettings.apiServiceBaseUri + $scope.avatarsFolder + $scope.userInfo.avatarImage;

                isImage(fullPathToFileOne).then(function (test) {
                    if (test) {
                        $scope.avatarDataUrl = fullPathToFileOne;
                    }
                });
            }

        }).catch(function () {

            //neederror

        }).finally(function () {

            $scope.wasUserLoaded = true;

        });

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
    
    $scope.getUserSummary();
});