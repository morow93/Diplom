'use strict';
angular.module('InoDrive').controller('confirmEmailController', function ($scope, $stateParams, $location, $state, $interval, authService) {

    $scope.messageT = "Вы будете перенаправлены на главную страницу через 3 секунды";

    if ($stateParams.userId && $stateParams.code) {

        var params = {
            userId: $stateParams.userId,
            code: $stateParams.code
        };

        authService.confirmEmail(params).then(function (response) {

            $scope.message = response.data;
            $scope.success = true;

        }).catch(function (response) {

            $scope.message = response.data;
            $scope.success = false;

            var c = 3;
            var secondsEnd = "";
            var timer = $interval(function () {
                --c;
                if (c === 0 || c === 5) {
                    secondsEnd = " секунд";
                } else if (c === 1) {
                    secondsEnd = " секунду";
                } else {
                    secondsEnd = " секунды";
                }

                if (c === 0) {
                    if (angular.isDefined(timer)) {
                        $interval.cancel(timer);
                        timer = undefined;
                        $state.go("home", null, { reload: true });
                    }
                } else {
                    $scope.messageT = "Вы будете перенаправлены на главную страницу через " + c + secondsEnd;
                }

            }, 1000);
        });

    } else {
        $state.go("home", null, { reload: true });
    }

});