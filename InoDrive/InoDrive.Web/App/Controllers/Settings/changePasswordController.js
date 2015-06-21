"use strict";
angular.module("InoDrive").controller("changePasswordController", function (
    $scope,
    $stateParams,
    $state,
    $alert,
    authService) {

    //$scope.resetPassword = {};

    $scope.formSubmit = function (form) {

        debugger;

        if (form.$valid) {

            $scope.laddaResetPasswordFlag = true;
            $scope.resetPassword.userId = $scope.authentication.userId;

            authService.changePassword($scope.resetPassword).then(function (results) {

                $scope.showAlert({
                    title: "Поздравляем! Пароль был успешно изменен!",
                    content: "",
                    type: "info",
                    show: false,
                    container: ".form-alert"
                    , template: "/app/templates/alert.html"
                });

            }).catch(function (results) {

                $scope.showAlert({
                    title: 'Ошибка! ' + results.data.message,
                    content: '',
                    type: 'danger',
                    show: false,
                    container: '.form-alert'
                    , template: "/app/templates/alert.html"
                });

            }).finally(function () {
                $scope.laddaResetPasswordFlag = false;
            });

        } else {

            $scope.showAlert({
                title: 'Невозможно сменить пароль! Исправьте отмеченные поля!',
                content: '',
                type: 'danger',
                show: false,
                container: '.form-alert'
                , template: "/app/templates/alert.html"
            });

            angular.forEach(form.$error.required, function (field) {
                field.$setDirty();
            });
        }
    };

    var myAlert;

    $scope.showAlert = function (alertData) {

        if (myAlert != null) {
            myAlert.$promise.then(myAlert.destroy);
        }
        myAlert = $alert(alertData);
        myAlert.$promise.then(myAlert.show);
    };

    if (!$scope.authentication.isAuth) {
        $state.go("home", null, { reload: true });
    }
});