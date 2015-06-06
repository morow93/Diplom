angular.module('InoDrive').controller('userViewController', function ($scope, $state, tripsService) {

    $scope.userRating = 4;
    $scope.tripsType = "all";

    $scope.selectAllTrips = function () {
        $scope.tripsType = "all";
    };

    $scope.selectDriverTrips = function () {
        $scope.tripsType = "driver";
    };

    $scope.selectPassengerTrips = function () {
        $scope.tripsType = "passenger";
    };
    
    $scope.showEnded = true;

    tripsService.test().then(function () {

        console.log("was");

    });
});