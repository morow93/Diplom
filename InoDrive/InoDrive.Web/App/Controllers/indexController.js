angular.module('InoDrive').controller('indexController', function ($scope, $document) {

    $scope.isVisibleToTop = false;

    $scope.toTheTop = function () {
        $document.scrollTopAnimated(0, 500);//.then(function () { });
    }

    $document.on('scroll', function () {    

        $scope.$apply(function () {
            $scope.isVisibleToTop = $document.scrollTop() > 10;
        });

    });



    /**
     * Initialize index
     * @type {number}
     */
    var index = 0;

    /**
     * Boolean to show error if new notification is invalid
     * @type {boolean}
     */
    $scope.invalidNotification = false;

    /**
     * Placeholder for notifications
     *
     * We use a hash with auto incrementing key
     * so we can use "track by" in ng-repeat
     *
     * @type {{}}
     */
    $scope.notifications = {};

    /**
     * Add a notification
     *
     * @param notification
     */
    $scope.add = function (notification) {

        var i;

        if (!notification) {
            $scope.invalidNotification = true;
            return;
        }

        i = index++;
        $scope.invalidNotification = false;
        $scope.notifications[i] = notification;
    };


}).value('duScrollOffset', 30);