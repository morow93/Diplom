angular.module('InoDrive').controller('createTripController', function ($scope, $state) {

    $scope.rangePrice = {
        minModel: 0,
        maxModel: 100
    };

    $scope.trip = {};
    $scope.trip.price = "neg";

    $scope.selectNegPrice = function () {
        $scope.rangePrice.maxModel = 100;
    };

});
