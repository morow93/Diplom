angular.module('InoDrive').controller('indexController', function ($scope, $document, $state, authService) {

    $scope.isVisibleToTop = false;

    $scope.toTheTop = function () {
        $document.scrollTopAnimated(0, 500);
    }

    $document.on('scroll', function () {    

        $scope.$apply(function () {
            $scope.isVisibleToTop = $document.scrollTop() > 1000;
        });

    });

    $scope.signOut = function () {

        authService.signOut().then(function () {

            $state.go("home", null, { reload: true });

        }).catch(function (e) {

            $state.go("error", null, { reload: true });

        });

    };

    $scope.authentication = authService.authentication;

}).value('duScrollOffset', 30);