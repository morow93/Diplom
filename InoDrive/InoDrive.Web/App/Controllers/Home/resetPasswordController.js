angular.module('InoDrive').controller('resetPasswordController', function ($scope, $alert, $state, $stateParams, authService, customStorageService) {

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

            if ($stateParams.userId && $stateParams.code) {

                var params = {
                    userId: $stateParams.userId,
                    code: $stateParams.code,
                    newPassword: $scope.resetPassword.password
                };

                $scope.laddaResetPasswordFlag = true;

                authService.resetPassword(params).then(function (results) {

                    customStorageService.set("notifyToShow", {
                        message: 'Поздравляем! Вы успешно выполнили смену пароля! Теперь можете выполнить вход.',
                        type: 'success',
                    });
                    $state.go("signin", null, { reload: true });

                }).catch(function (results) {

                    $scope.showAlert({
                        title: "Произошла ошибка при смене пароля! Попробуйте еще раз.",
                        content: '',
                        type: "danger",
                        show: false,
                        container: '.form-alert'
                        ,template: '/app/templates/alert.html'
                    });

                }).finally(function () {
                    $scope.laddaResetPasswordFlag = false;
                });

            } else {

                $scope.showAlert({
                    title: "Не задан идентификатор пользователя и/или токен!",
                    content: '',
                    type: "danger",
                    show: false,
                    container: '.form-alert'
                    ,template: '/app/templates/alert.html'
                });
            }
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