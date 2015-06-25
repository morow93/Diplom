'use strict';
angular.module('InoDrive').controller('confirmEmailController', function ($scope, $stateParams, $state, $timeout, authService) {
    debugger;
    if ($stateParams.userId && $stateParams.code) {

        var params = {
            userId: $stateParams.userId,
            code: $stateParams.code
        };

        authService.confirmEmail(params).then(function (response) {

            $scope.success = response.data;
            $timeout(function () { $state.go("home.signin"); }, 3500);

        }).catch(function (error) {

            $scope.failure = error;
            $timeout(function () { $state.go("home.greeting"); }, 3500);

        }).finally(function () {

            $scope.wasLoaded = true;

        });

    } else {
        $state.go("home.greeting");
    }
});