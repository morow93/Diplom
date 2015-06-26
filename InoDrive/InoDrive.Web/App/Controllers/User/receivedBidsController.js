angular.module('InoDrive').controller('receivedBidsController', function ($scope,
    $rootScope,
    $state,
    $timeout,
    $interval,
    bidsService,
    localStorageService) {
    
    $scope.loading = false;
    $scope.fromId = null;
    $scope.countExcluded = 0;
    $scope.countNewBids = 0;

    $scope.getPageOfBids = function () {

        if ($scope.loading) {
            return;
        }
        if ($scope.perPage == undefined) {
            $scope.perPage = 10;
        }
        if ($scope.page == undefined) {
            $scope.page = 1;
        }

        $scope.loading = true;//true when loading items
        $scope.emptyResult = false;//if no more items to load

        $timeout(function () {

           bidsService.getBidsForMyTrips(
           {
               page: $scope.page,
               perPage: $scope.perPage,
               countExcluded: $scope.countExcluded,
               userId: $scope.authentication.userId,
               fromId: $scope.fromId

           }).then(function (data) {

               if (data.results && data.results.length > 0 && data.totalCount > 0) {

                   if ($scope.page == 1) {
                       $scope.fromId = data.results[0].bidId;
                   }

                   $scope.totalCount = data.totalCount;
                   $scope.showTotalCount = true;
                   $scope.firstLoad = true;

                   if ($scope.bidsForMyTrips) {
                       for (var i = 0; i < data.results.length; i++) {
                           $scope.bidsForMyTrips.push(data.results[i]);
                       }
                   } else {
                       $scope.bidsForMyTrips = data.results;
                   }

                   var totalPages = Math.ceil(($scope.totalCount + $scope.countExcluded) / $scope.perPage);
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
                   $scope.totalCount = 0;
               }

           }).catch(function (error) {

               throw error.data;

           }).finally(function () {
               $scope.loading = false;
               $scope.firstLoad = true;
           });

        }, 1500);        
       
    };

    $scope.acceptBid = function (bidId, tripId, userId, index) {

        $scope.countExcluded++;

        var params = {
            userClaimedId: userId,
            userOwnerId: $scope.authentication.userId,
            bidId: bidId,
            tripId: tripId
        };

        bidsService.acceptBid(params).then(function (data) {

            $scope.bidsForMyTrips[index].makedDesign = 1;

            var tripId = $scope.bidsForMyTrips[index].tripId;
            for (var i = 0; i < $scope.bidsForMyTrips.length; i++) {
                if ($scope.bidsForMyTrips[i].tripId == tripId) {
                    $scope.bidsForMyTrips[i].freePlaces--;
                }
            }
            --$scope.countOfAssignedBids.count;
            localStorageService.set("countOfAssignedBids", $scope.countOfAssignedBids);

        }).catch(function (error) {
            throw error.data;
        });
    };

    $scope.rejectBid = function (bidId, tripId, userId, index) {

        $scope.countExcluded++;

        var params = {
            userClaimedId: userId,
            userOwnerId: $scope.authentication.userId,
            bidId: bidId,
            tripId: tripId
        };

        bidsService.rejectBid(params).then(function (data) {

            $scope.bidsForMyTrips[index].makedDesign = 2;

            --$scope.countOfAssignedBids.count;
            localStorageService.set("countOfAssignedBids", $scope.countOfAssignedBids);

        }).catch(function (error) {
            throw error.data;
        });
    };
    
    $scope.loadNewBids = function () {
        
        if ($scope.newBids) {
            if ($scope.bidsForMyTrips && $scope.bidsForMyTrips.length != 0) {
                for (var i = $scope.newBids.length - 1; i >= 0; i--) {
                    $scope.bidsForMyTrips.unshift($scope.newBids[i]);
                }
            } else {
                $scope.bidsForMyTrips = $scope.newBids;
            }

            $scope.newBids = [];
            $scope.countNewBids = 0;
        }
    };

    $scope.getTrip = function (tripId) {
        $state.go("user.trip", { tripId: tripId }, { reload: false });
    };

    //var getUpdatedAssignedBids = function () {

    //    if ($scope.newBids && $scope.newBids.length != 0) {
    //        $scope.newFromId = $scope.newBids[0].bidId;
    //    } else if ($scope.bidsForMyTrips && $scope.bidsForMyTrips.length != 0) {

    //        var existUnchecked = false;
    //        for (var i = 0; i < $scope.bidsForMyTrips.length; i++) {
    //            if (!$scope.bidsForMyTrips[i].makedDesign) {
    //                $scope.newFromId = $scope.bidsForMyTrips[i].bidId;
    //                existUnchecked = true;
    //                break;
    //            }
    //        }
    //        if (!existUnchecked) $scope.newFromId = 0;

    //    } else {
    //        $scope.newFromId = 0;
    //    }

    //    var params = {
    //        userId: $scope.authentication.userId,
    //        fromId: $scope.newFromId
    //    };

    //    bidsService.getUpdatedAssignedBids(params).then(function (data) {

    //        if (data && data.length != 0) {

    //            $scope.countNewBids += data.length;
    //            $scope.countOfAssignedBids.count += data.length;
    //            localStorageService.setData("countOfAssignedBids", $scope.countOfAssignedBids);

    //            if ($scope.newBids && $scope.newBids.length != 0) {

    //                for (var i = 0; i < data.length; i++) {
    //                    $scope.newBids.unshift(data[i]);
    //                }

    //            } else {
    //                $scope.newBids = data;
    //            }
    //        }
    //    }).catch(function (error) {
    //        throw error.data;
    //    });
    //};

    //var promise = $interval(getUpdatedAssignedBids, 10000);

    //$scope.$on('$destroy', function () {
    //    if (angular.isDefined(promise)) {
    //        $interval.cancel(promise);
    //        promise = undefined;
    //    }
    //});

    $scope.getPageOfBids();

});