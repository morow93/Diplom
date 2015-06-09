angular.module('InoDrive').controller('signUpController', function ($scope, $alert, $state, $timeout, authService, customStorageService) {

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

            $scope.laddaSignUpFlag = true;

            authService.signUp($scope.signup).then(function (response) {
                
                customStorageService.set("notifyToShow", {
                    message:
                        'Вы успешно зарегистрирвоались! Но чтобы войти, Вы должны подтвердить свой аккаунт! ' +
                        '(на указанную почту было выслано письмо для подтверждения).',
                    type: 'success',
                });
                $state.go("home.greeting", null, { reload: true });                

            }).catch(function (response) {

                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }
                errors = errors.join(" ");

                if (errors.indexOf("is already taken") >= 0) {
                    errors = "Уже зарегистрирвоан пользователь с такой почтой!";
                }

                $scope.showAlert({
                    title: "Внимание! " + errors,
                    content: "",
                    type: "danger",
                    show: false,
                    container: '.form-alert',
                    template: '/app/templates/alert.html'
                });

            }).finally(function () {

                $scope.laddaSignUpFlag = false;

            });
        }
        else {

            $scope.showAlert({
                title: 'Для того чтобы зарегистрироваться, пожалуйста, исправьте отмеченные поля!',
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

    $scope.signup = {};

});
