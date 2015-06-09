angular.module("InoDrive").controller("sendCodeController", function ($scope, $alert, $state, authService, customStorageService) {

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
            
            $scope.laddaSendCodeFlag = true;

            authService.sendResetPasswordCode($scope.sendCode).then(function (response) {
                
                customStorageService.set("notifyToShow", {
                    message: 'На указанную почту было отправлено письмо для сброса пароля!',
                    type: 'success',
                });

                $state.go("home.greeting", null, { reload: true });

            }).catch(function (err) {

                $scope.showAlert({
                    title: 'Внимание!',
                    content: err.data,
                    type: 'danger',
                    show: false,
                    container: '.reset-rassword-alert'
                });

            }).finally(function () {

                $scope.laddaSendCodeFlag = false;
            });
        }
        else {

            $scope.showAlert({
                title: "Пожалуйста, укажите email, чтобы мы могли отправить Вам письмо для восстановления пароля!",
                content: "",
                type: "info",
                show: false,
                container: ".form-alert",
                template: "/app/templates/alert.html"
            });

            angular.forEach(form.$error.required, function (field) {
                field.$setDirty();
            });

        }

    };

    $scope.sendCode = {};

});