angular.module('InoDrive').controller('signInController', function ($scope, $alert, $state, customStorageService, authService) {

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

            $scope.laddaSignInFlag = true;

            authService.signIn($scope.signin).then(function (response) {

                customStorageService.set("notifyToShow", {
                    message: 'Поздравляем! Вы успешно выполнили вход!',
                    type: 'success',
                });
                
                $state.go("user.view", null, { reload: true });

            }).catch(function (error) {

                if (error.error === "not_confirmed_email") {
                    $scope.sendCodeFlag = true;
                    $scope.userId = error.error_uri;
                }

                $scope.showAlert({
                    title: "Внимание! " + error.error_description,
                    content: '',
                    type: "danger",
                    show: false,
                    container: '.form-alert'
                    ,template: '/app/templates/alert.html'
                });

            }).finally(function () {

                $scope.laddaSignInFlag = false;

            });

        }
        else {

            $scope.showAlert({
                title: 'Для того чтобы войти, пожалуйста, исправьте отмеченные поля!',
                content: '',
                type: 'danger',
                show: false,
                container: '.form-alert'
                ,template: '/app/templates/alert.html'
            });

            angular.forEach(form.$error.required, function (field) {
                field.$setDirty();
            });

        }

    };

    $scope.sendEmailCode = function () {

        if ($scope.sendCodeFlag && $scope.userId) {

            $scope.laddaSendCodeFlag = true;

            authService.sendConfirmEmailCode({ userId: $scope.userId }).then(function (response) {

                $scope.showAlert({
                    title: 'Письмо для подтверждения аккаунта было отправлено на указанный адрес!',
                    content: '',
                    type: 'success',
                    show: false,
                    container: '.form-alert'
                    , template: '/app/templates/alert.html'
                });

                $scope.sendCodeFlag = false;

            }).catch(function (err) {

                $scope.showAlert({
                    title: 'Произошла ошибка при отправлении письма! Попробуйте немного позже.',
                    content: '',
                    type: 'danger',
                    show: false,
                    container: '.form-alert'
                    , template: '/app/templates/alert.html'
                });

            }).finally(function () {

                $scope.laddaSendCodeFlag = false;

            });
        }

    };

    $scope.signin = {};

});
