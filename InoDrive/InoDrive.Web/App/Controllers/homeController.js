angular.module('InoDrive').controller('homeController', function ($scope, $interval, $alert, $state, mapService) {

    var myAlert;

    $scope.showAlert = function (alertData) {
        if (myAlert != null) {
            myAlert.$promise.then(myAlert.destroy);
        }
        myAlert = $alert(alertData);
        myAlert.$promise.then(myAlert.show);
    };

    $scope.formSubmit = function (form) {

        if (form.$valid) {

            $state.go("find");
        }
        else {

            $scope.showAlert({
                title: 'Для того чтобы искать поездку, пожалуйста, исправьте отмеченные поля!',
                content: '',
                type: 'danger',
                show: false,
                container: '.form-alert',
                template: '/app/templates/alert.html'
            });
            
            angular.forEach(form.$error.required, function (field) {
                field.$setDirty();
            });

        }

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
    
    $scope.autocompleteOptions = {
        types: "(cities)",
        country: "ru"
    };
    $scope.home = {};
    $scope.home.originCity = null;
    $scope.home.destinationCity = null;
    $scope.minDate = new Date(); 
    $scope.minDate.setDate($scope.minDate.getDate() - 1);

    $scope.updateMapUrl();
    $interval($scope.updateMapUrl, 15000);

});

function getRandomArbitrary(min, max) {
    return Math.random() * (max - min) + min;
}