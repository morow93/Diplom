angular.module('InoDrive').controller('sendedBidsController', function ($scope, $state, $interval, $timeout, bidsService, localStorageService) {

    $scope.loading = false;
    $scope.showEnded = true;

    $scope.update = function (val) {
        $scope.page = 1;
        $scope.myBids = null;
        $scope.showEnded = val;
        $scope.showTotalCount = false;
        $scope.countExcluded = 0;
        $scope.getPageOfBids();
    };

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

            bidsService.getMyBids(
            {
                page: $scope.page,
                perPage: $scope.perPage,
                userId: $scope.authentication.userId,
                showEnded: $scope.showEnded

            }).then(function (response) {

                if (response.results && response.results.length > 0 && response.totalCount > 0) {

                    $scope.totalCount = response.totalCount;
                    $scope.showTotalCount = true;
                    $scope.firstLoad = true;

                    if ($scope.myBids) {
                        for (var i = 0; i < response.results.length; i++) {
                            $scope.myBids.push(response.results[i]);
                        }
                    } else {
                        $scope.myBids = response.results;
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
                }

            }).catch(function (error) {

                //neederror

            }).finally(function () {
                $scope.loading = false;
                $scope.firstLoad = true;
            });

        }, 1500);

    };

    $scope.watchBid = function (bidId, index) {
        
        //console.log('was on ' + bidId + ' bid');
        
        if (!$scope.myBids[index].isWatched && ($scope.myBids[index].isAccepted != null)) {

            $scope.myBids[index].isWatched = true;

            var params = {
                userOwnerId: $scope.authentication.userId,
                bidId: bidId
            };

            bidsService.watchBid(params).then(function (data) {

                $scope.countOfOwnBids.count -= 1;
                localStorageService.set("countOfOwnBids", $scope.countOfOwnBids);

            }).catch(function (error) {
                $scope.myBids[index].isWatched = false;
                throw error.data;
            });
        }
    };

    function equalsArrays(arr1, arr2) {
        if (arr1.length != arr2.length) return false;
        for (var i = 0; i < arr1.length; i++) {
            if (!angular.equals(arr1[i], arr2[i])) return false;
        }
        return true;
    }
    
    $scope.getTrip = function (tripId) {
        $state.go("user.trip", { tripId: tripId }, { reload: false });
    };

    //var getUpdatedOwnBids = function () {

    //    var params = {
    //        userId: $scope.authentication.userId
    //    };
    //    bidsService.getUpdatedOwnBids(params).then(function (data) {

    //        $scope.countOfOwnBids.count = data.length;
    //        localStorageService.set("countOfOwnBids", $scope.countOfOwnBids);

    //        for (var i = 0; i < $scope.myBids.length; i++) {
    //            for (var j = 0; j < data.length; j++) {

    //                if ($scope.myBids[i].bidId === data[j].bidId &&
    //                    !$scope.myBids[i].isAccepted &&
    //                    !$scope.myBids[i].wasUpdated) {

    //                    $scope.myBids[i].isAccepted = data[j].isAccepted;
    //                    $scope.myBids[i].wasUpdated = true;

    //                    if ($scope.myBids[i].isAccepted && $scope.myBids[i].freePlaces > 0) {
    //                        $scope.myBids[i].freePlaces--;
    //                    }
    //                }

    //            }
    //        }

    //    }).catch(function (error) {
    //        throw error.data;
    //    });
    //};

    //var promise = $interval(getUpdatedOwnBids, 10000);

    //$scope.$on('$destroy', function () {
    //    if (angular.isDefined(promise)) {
    //        $interval.cancel(promise);
    //        promise = undefined;
    //    }
    //});

    $scope.getPageOfBids();
});