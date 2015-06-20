angular.module('InoDrive').controller('greetingController', function ($scope, $interval, $alert, $state, mapService) {

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

            $state.go("find", null, { reload: true });

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

    $scope.dt = new Date();
    $scope.dt.setDate($scope.dt.getDate() - 1);

    $scope.autocompleteOptions = {
        types: "(cities)",
        country: "ru"
    };
    $scope.home = {};
    $scope.home.originCity = null;
    $scope.home.destinationCity = null;

});

function getRandomArbitrary(min, max) {
    return Math.random() * (max - min) + min;
}