angular.module('InoDrive').controller('userController', function ($scope, $state) {

    $scope.list = [
        {
            text: "Моя страница",
            value: "user.view"
        },
        {
            text: "Создать поездку",
            value: "user.create_trip"
        },
        {
            text: "Искать поездку",
            value: "user.find"
        },
        {
            text: "Мои заявки +999",
            value: "user.my_bids"
        },
        {
            text: "Мои настройки",
            value: "user.settings"
        }
    ];

    $scope.isActiveNav = function (stateName) {

        var isActive = (stateName === $state.current.name);
        return isActive;        

    };

});