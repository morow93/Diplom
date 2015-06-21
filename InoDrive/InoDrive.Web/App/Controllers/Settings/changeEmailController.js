"use strict";
angular.module("InoDrive").controller("changeEmailController", function (
    $scope,
    $stateParams,
    $state,
    $alert,
    authService) {

    //$scope.resetEmail = {};

    $scope.formSubmit = function (form) {

        debugger;

        if (form.$valid) {

            $scope.laddaResetEmailFlag = true;
            $scope.resetEmail.userId = $scope.authentication.userId;

            authService.changeEmail($scope.resetEmail).then(function (results) {

                $scope.showAlert({
                    title: "Поздравляем! Email был успешно изменен!",
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
                $scope.laddaResetEmailFlag = false;
            });

        } else {

            $scope.showAlert({
                title: 'Невозможно сменить email! Исправьте отмеченные поля!',
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