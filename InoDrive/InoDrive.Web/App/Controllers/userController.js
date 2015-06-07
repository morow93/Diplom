angular.module('InoDrive').controller('userController', function ($scope, $state) {

    $scope.isActiveNav = function (stateName) {
        var isActive = (stateName === $state.current.name);
        return isActive;       
    };

    $scope.isActiveInherited = function (stateName) {
        var isActive = ($state.current.name.indexOf(stateName) >= 0)
        return isActive;
    };

});