angular.module('InoDrive').controller('createTripController', function ($scope, $state) {

    $scope.rangePrice = {
        minModel: 0,
        maxModel: 100
    };

    $scope.trip = {};

    $scope.selectNegPrice = function () {
        $scope.rangePrice.maxModel = 100;
    };

    $scope.trip = {};
    $scope.trip.wayPoints = [];
    
    $scope.autocompleteOptions = {
        types: "(cities)",
        country: "ru"
    };
    $scope.trip.wayPoints = [{}];
    
    $scope.addWayPointer = function () {

        if ($scope.trip.wayPoints.length <= 4) {
            $scope.trip.wayPoints.push({});
        }

    };

    $scope.removeWayPointer = function (arg) {
        if ($scope.trip.wayPoints.length > 1) {
            $scope.trip.wayPoints.pop();
        }
    };

    $scope.$watch('trip.wayPoints', function (newValue, oldValue) {
        
        if (oldValue.length <= newValue.length && newValue[newValue.length - 1].details) {
            if (!newValue[newValue.length - 1].flag){
                $scope.addWayPointer();
            } else {
                $scope.trip.wayPoints.flag = true;
            }               
        }

    }, true);

});
