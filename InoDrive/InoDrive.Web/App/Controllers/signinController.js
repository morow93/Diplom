angular.module('InoDrive').controller('signinController', function ($scope, $interval, $alert, $state) {

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

            $state.go("user.view");
        }
        else {

            $scope.showAlert({
                title: 'Исправьте отмеченные поля!',
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

    $scope.signin = {};

});
