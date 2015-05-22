angular.module('InoDrive').controller('userController', function ($scope, $state) {

    $scope.list = [
        {
            text: "Моя страница",
            value: "user.view"
        },
        {
            text: "Мои поездки",
            value: "user.my_trips"
        },
        {
            text: "Мои заявки",
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