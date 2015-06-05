angular.module('InoDrive').controller('signupController', function ($scope, $alert, $state) {

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

                showAlert({
                    title: "Завершен первый этап регистрации! На указанную почту было выслано письмо для подтверждения аккаунта.",
                    content: "",
                    type: "success",
                    show: false,
                    container: '.form-alert',
                    template: '/app/templates/alert.html'
                });


            }).catch(function (response) {

                //var errors = [];
                //for (var key in response.data.modelState) {
                //    for (var i = 0; i < response.data.modelState[key].length; i++) {
                //        errors.push(response.data.modelState[key][i]);
                //    }
                //}

                showAlert({
                    title: "Error occurred!",
                    content: "Failed to sign up due to some reasons!",
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
