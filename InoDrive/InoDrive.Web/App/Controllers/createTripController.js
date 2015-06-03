angular.module('InoDrive').controller('createTripController', function ($scope, $state, $alert) {

    $scope.rangePrice = {
        minModel: 0,
        maxModel: 100
    };

    $scope.selectNegPrice = function () {
        $scope.rangePrice.maxModel = 100;
    };

    $scope.trip = {};
    $scope.trip.wayPoints = [{}];

    $scope.autocompleteOptions = {
        types: "(cities)",
        country: "ru"
    };
    
    $scope.addWayPointer = function () {
        if ($scope.trip.wayPoints.length <= 4) {
            $scope.trip.wayPoints.push({});
        }
    };

    $scope.removeWayPointer = function () {
        if ($scope.trip.wayPoints.length > 1) {
            $scope.trip.wayPoints.pop();
        } else if ($scope.trip.wayPoints.length === 1) {
            $scope.trip.wayPoints[0] = {};
        }
    };
    
    $scope.classes = [
        { "value": "A",     "label": "<i class=\"fa fa-star\"></i> Класс А (особо малый)" },
        { "value": "B",     "label": "<i class=\"fa fa-star\"></i> Класс B (малый)" },
        { "value": "C",     "label": '<i class=\"fa fa-star\"></i> Класс C ("гольф"-класс)' },
        { "value": "D",     "label": "<i class=\"fa fa-star\"></i> Класс D (средний)" },
        { "value": "E",     "label": '<i class=\"fa fa-star\"></i> Класс E ("бизнес"-класс)' },
        { "value": "F",     "label": '<i class=\"fa fa-star\"></i> Класс F ("люкс"-класс)' },
        { "value": "G",     "label": '<i class=\"fa fa-star\"></i> Купе/кабриолет' },
        { "value": "H",     "label": '<i class=\"fa fa-star\"></i> Внедорожник' },
        { "value": "I",     "label": '<i class=\"fa fa-star\"></i> Минивэн' },
        { "value": "J",     "label": '<i class=\"fa fa-star\"></i> Кроссовер' }
    ];  

    var myAlert;

    $scope.showAlert = function (alertData) {
        if (myAlert != null) {
            myAlert.$promise.then(myAlert.destroy);
        }
        myAlert = $alert(alertData);
        myAlert.$promise.then(myAlert.show);
    };

    $scope.formSubmit = function (form) {

        if (form.$valid) {

            $scope.showAlert({
                title: "Поездка была добавлена!",
                content: "",
                type: "success",
                show: false,
                container: ".form-alert"
               , template: "/app/templates/alert.html"
            });
            //$state.go("user.view");
        }
        else {

            $scope.showAlert({
                title: "Для того чтобы создать поездку, пожалуйста, исправьте отмеченные поля!",
                content: "",
                type: "danger",
                show: false,
                container: ".form-alert"
                , template: "/app/templates/alert.html"
            });

            angular.forEach(form.$error.required, function (field) {
                field.$setDirty();
            });

        }

    };

});
