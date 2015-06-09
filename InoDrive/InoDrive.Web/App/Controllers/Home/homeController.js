angular.module('InoDrive').controller('homeController', function ($scope, $interval, $alert, $state, mapService) {

    $scope.isActiveNav = function (stateName) {
        var isActive = (stateName === $state.current.name);
        return isActive;
    };

    $scope.isActiveInherited = function (stateName) {
        var isActive = ($state.current.name.indexOf(stateName) >= 0)
        return isActive;
    };

    $scope.updateMapUrl = function () {

        mapService.getCities().then(function (data) {

            var max = data.cities.length - 1;
            var index = getRandomArbitrary(0, max).toFixed();

            var currentCity = data.cities[index];
            var zoom = parseInt(getRandomArbitrary(10, 15).toFixed());

            $scope.map = {
                center: {
                    latitude: currentCity.latitude,
                    longitude: currentCity.longitude
                },
                zoom: zoom,
                options: {
                    disableDefaultUI: true
                },
                dragging: false
            };

        });
    };

    $scope.updateMapUrl();
    $interval($scope.updateMapUrl, 15000);

});

function getRandomArbitrary(min, max) {
    return Math.random() * (max - min) + min;
}