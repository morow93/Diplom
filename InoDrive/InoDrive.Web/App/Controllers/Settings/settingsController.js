angular.module('InoDrive').controller('settingsController', function ($scope, $state) {

    $scope.settingsType = "cabinet";

    $scope.selectPrivateCabinet = function () {
        $scope.settingsType = "cabinet";
    };

    $scope.selectChangeEmail = function () {
        $scope.settingsType = "email";
    };

    $scope.selectChangePassword = function () {
        $scope.settingsType = "password";
    };

});