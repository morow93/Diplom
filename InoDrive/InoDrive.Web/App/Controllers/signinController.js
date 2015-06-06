angular.module('InoDrive').controller('signinController', function ($scope, $alert, $state, authService) {

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

            authService.signIn($scope.signin).then(function (response) {
                debugger;
                $state.go("user", { userName: $scope.signin.userName }, { reload: true });

            }).catch(function (error) {

                $scope.showAlert({
                    title: "Внимание! " + error.error_description,
                    content: '',
                    type: "danger",
                    show: false,
                    container: '.form-alert'
                    ,template: '/app/templates/alert.html'
                });

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

    $scope.signin = {};

});
