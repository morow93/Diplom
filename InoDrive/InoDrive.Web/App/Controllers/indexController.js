angular.module('InoDrive').controller('indexController', function ($scope, $document) {

    $scope.isVisibleToTop = false;

    $scope.toTheTop = function () {
        $document.scrollTopAnimated(0, 500);//.then(function () { });
    }

    $document.on('scroll', function () {    

        $scope.$apply(function () {
            $scope.isVisibleToTop = $document.scrollTop() > 1000;
        });

    });

}).value('duScrollOffset', 30);