angular.module('InoDrive').controller('resetPasswordController', function ($scope, $alert, $state) {

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

            $state.go("signin");
        }
        else {

            $scope.showAlert({
                title: 'Для того чтобы восстановить пароль, пожалуйста, исправьте отмеченные поля!',
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

    $scope.resetPassword = {};

});