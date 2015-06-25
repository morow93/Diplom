angular.module('InoDrive').controller('sendedBidsController', function ($scope) {

    $scope.endedBidsTooltip =
   {
       title: 'Завершенные поездки окрашены в серый цвет!',
       checked: false
   };

    $scope.loading = false;
    $scope.showOption = true;

    $scope.update = function (val) {
        $scope.page = 1;
        $scope.myBids = null;
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

        bidsService.getMyBids(
        {
            page: $scope.page,
            perPage: $scope.perPage,
            userId: $scope.authentication.userId,
            showEnded: !$scope.showOption

        }).then(function (data) {

            $scope.loading = false;
            if (data.totalCount != 0) {
                $scope.wasLoading = true;
            }

            if (!data.myBids || data.myBids.length == 0) {
                $scope.emptyResult = true;
                $scope.initiatedFind = false;
            } else {
                var nowDate = new Date();
                for (var j = 0; j < data.myBids.length; j++) {
                    if (new Date(data.myBids[j].leavingDate) < nowDate) {
                        data.myBids[j].ended = true;
                    }
                }
            }

            if ($scope.myBids) {
                for (var i = 0; i < data.myBids.length; i++) {
                    $scope.myBids.push(data.myBids[i]);
                }
            } else {
                $scope.myBids = data.myBids;
            }

            $scope.wasRequested = true;

            ++$scope.page;
            $scope.totalPages = Math.ceil(data.totalCount / $scope.perPage);
            if ($scope.page > $scope.totalPages) $scope.emptyResult = true;
            $scope.totalCount = data.totalCount;

        }).catch(function (error) {
            throw error.data;
        }).finally(function () {
            $scope.loading = false;
            $scope.triggerFind = false;
        });
    };

    $scope.watchBid = function (bidId, index) {
        if (!$scope.myBids[index].isWatched) {

            $scope.myBids[index].isWatched = true;

            var params = {
                userOwnerId: $scope.authentication.userId,
                bidId: bidId
            };
            bidsService.watchBid(params).then(function (data) {

                $scope.countOfOwnBids.count -= 1;
                shareService.setData("countOfOwnBids", $scope.countOfOwnBids);

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

    var getUpdatedOwnBids = function () {

        var params = {
            userId: $scope.authentication.userId
        };
        bidsService.getUpdatedOwnBids(params).then(function (data) {

            $scope.countOfOwnBids.count = data.length;
            shareService.setData("countOfOwnBids", $scope.countOfOwnBids);

            for (var i = 0; i < $scope.myBids.length; i++) {
                for (var j = 0; j < data.length; j++) {
                    if ($scope.myBids[i].bidId === data[j].bidId &&
                        !$scope.myBids[i].isAccepted &&
                        !$scope.myBids[i].wasUpdated) {

                        $scope.myBids[i].isAccepted = data[j].isAccepted;
                        $scope.myBids[i].wasUpdated = true;

                        if ($scope.myBids[i].isAccepted && $scope.myBids[i].freePlaces > 0) {
                            $scope.myBids[i].freePlaces--;
                        }
                    }
                }
            }

        }).catch(function (error) {
            throw error.data;
        });
    };

    var promise = $interval(getUpdatedOwnBids, 10000);

    $scope.$on('$destroy', function () {
        if (angular.isDefined(promise)) {
            $interval.cancel(promise);
            promise = undefined;
        }
    });

    //$scope.getPageOfBids();

});