angular.module('InoDrive').controller('sendCodeController', function ($scope, $alert, $state) {

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

            $state.go("reset_password");
        }
        else {

            $scope.showAlert({
                title: 'Пожалуйста, укажите email!',
                content: '',
                type: 'danger',
                show: false,
                container: '.form-alert'
            });

            angular.forEach(form.$error.required, function (field) {
                field.$setDirty();
            });

        }

    };

    $scope.sendCode = {};

});