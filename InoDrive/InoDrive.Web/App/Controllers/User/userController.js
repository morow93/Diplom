angular.module('InoDrive').controller('userController', function ($scope, $state, authService, customStorageService) {

    $scope.isActiveNav = function (stateName) {
        var isActive = (stateName === $state.current.name);
        return isActive;       
    };

    $scope.isActiveInherited = function (stateName) {
        var isActive = ($state.current.name.indexOf(stateName) >= 0)
        return isActive;
    };

    $scope.sidebarSignIn = function () {

    };

    $scope.sidebarSignUp = function () {
        debugger;
        customStorageService.set("findParams", $scope.$$ChildScope.find);
        $state.go("home.signup");
    };

});