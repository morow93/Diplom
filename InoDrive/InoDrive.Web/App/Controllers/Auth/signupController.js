angular.module('InoDrive').controller('signUpController', function ($scope, $alert, $state, $timeout, authService) {

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

            authService.signUp($scope.signup).then(function (response) {

                $scope.showAlert({
                    title:
                        "На указанную почту было выслано письмо для подтверждения аккаунта! " +
                        "А сейчас Вы будете перенаправлены на главную страницу сайта.",
                    content: "",
                    type: "success",
                    show: false,
                    container: '.form-alert',
                    template: '/app/templates/alert.html'
                });

                $timeout(function () { $state.go("home", null, { reload: true }); }, 3000);

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
