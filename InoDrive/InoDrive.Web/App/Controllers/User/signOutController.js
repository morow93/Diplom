﻿angular.module('InoDrive').controller('signOutController', function ($scope, $state, $modal, customStorageService, authService) {

    $scope.message = "Вы действительно хотите выйти?"

    var signOutModal = $modal({
        scope: $scope,
        template: 'App/Templates/modal.html',
        show: false,
        animation: "am-fade-and-scale",
        placement: "center"
    });

    $scope.showModal = function () {
        signOutModal.$promise.then(signOutModal.show);
    };

    $scope.cancelModal = function () {
        signOutModal.$promise.then(signOutModal.hide);
    };

    $scope.ok = function () {

        authService.signOut().then(function () {

            signOutModal.$promise.then(signOutModal.hide);

            customStorageService.set("notifyToShow", {
                message: 'Поздравляем! Вы успешно выполнили выход!',
                type: 'success',
            });

            $state.go("home.greeting", null, { reload: true });

        }).catch(function (e) {
            $state.go("error", null, { reload: true });
        });
    };

}).value('duScrollOffset', 30);