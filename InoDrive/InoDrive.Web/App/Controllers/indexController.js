angular.module('InoDrive').controller('indexController', function ($scope, $document, $state, $modal, $timeout, authService, localStorageService) {

    $scope.isVisibleToTop = false;

    $scope.toTheTop = function () {
        $document.scrollTopAnimated(0, 500);
    }

    $document.on('scroll', function () {    

        $scope.$apply(function () {
            $scope.isVisibleToTop = $document.scrollTop() > 1000;
        });

    });
    
    $scope.signOut = function () {

        authService.signOut().then(function () {

            $state.go("home.greeting", null, { reload: true });

        }).catch(function (e) {

            $state.go("error", null, { reload: true });

        });

    };

    $scope.authentication = authService.authentication;
    
    $scope.countOfAssignedBids = localStorageService.get("countOfAssignedBids");
    $scope.countOfOwnBids = localStorageService.get("countOfOwnBids");
    if (!$scope.countOfAssignedBids) {
        $scope.countOfAssignedBids = { count: 0 };
    }
    if (!$scope.countOfOwnBids) {
        $scope.countOfOwnBids = { count: 0 };
    }

    $scope.updateCounters = function () {

        if (false && $scope.authentication.isAuth && !$scope.loading) {

            $scope.loading = true;

            if ($state.current.name != 'my_trips.my_bids') {

                bidsService.getCountOfAssignedBids({ userId: $scope.authentication.userId }).then(function (data) {

                    $scope.countOfAssignedBids = { count: data.count };
                    localStorageService.set("countOfAssignedBids", $scope.countOfAssignedBids);

                }).finally(function () {

                    updateSecondCounter();

                });

            } else {

                updateSecondCounter();
            }
        } else {

            $timeout(function () {

                $scope.updateCounters();

            }, 10000);
        }
    };
    
    function updateSecondCounter() {

        $timeout(function () {

            if ($state.current.name != 'my_bids') {

                bidsService.getCountOfOwnBids({ userId: $scope.authentication.userId }).then(function (data) {

                    $scope.countOfOwnBids = { count: data.count };
                    localStorageService.setData("countOfOwnBids", $scope.countOfOwnBids);

                }).finally(function () {

                    $scope.loading = false;
                    $timeout(function () {

                        $scope.updateCounters();

                    }, 10000);

                });

            } else {

                $scope.loading = false;
                $timeout(function () {

                    $scope.updateCounters();

                }, 10000);

            }

        }, 3000);



    };

    $scope.updateCounters();

}).value('duScrollOffset', 30);