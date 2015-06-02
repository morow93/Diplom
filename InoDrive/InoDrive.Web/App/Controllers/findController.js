angular.module('InoDrive').controller('findController', function ($scope, $state, $alert) {

    $scope.rangePrice = {
        minModel: 0,
        maxModel: 100
    };

    $scope.selectCurPrice = function () {
        $scope.rangePrice.minModel = 0;
        $scope.rangePrice.maxModel = 100;
    };

    $scope.find = {};
    $scope.find.wayPoints = [1];

    $scope.autocompleteOptions = {
        types: "(cities)",
        country: "ru"
    };
    
    $scope.addWayPointer = function () {
        debugger;
        if ($scope.find.wayPoints.length <= 4) {
            $scope.find.wayPoints.push(1);
        }
    };

    $scope.removeWayPointer = function () {
        debugger;
        if ($scope.find.wayPoints.length > 1) {
            $scope.find.wayPoints.pop();
        }
    };

    $scope.$watch("find.wayPoints", function (newValue, oldValue) {
        debugger;
        if (oldValue.length <= newValue.length && newValue[newValue.length - 1].details) {
            if (!newValue[newValue.length - 1].flag) {
                $scope.addWayPointer();
            } else {
                $scope.find.wayPoints.flag = true;
            }
        }

    }, true);

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
                title: "Поиск начался!",
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
                title: "Для того чтобы начать поиск, пожалуйста, исправьте отмеченные поля!",
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

    $scope.selectedSortOption = "UserRating";
    $scope.selectedOrderOption = "Desc";

    $scope.sortOptions = [
        {
            "value": "UserRating", 
            "label": "<i class=\"fa fa-star\"></i> Рейтингу пользователя"
        },
        {
            "value": "Pay",
            "label": "<i class=\"fa fa-money\"></i> Сумме с человека"
        },
        {
            "value": "FreePlaces", 
            "label": "<i class=\"fa fa-car\"></i> Числу свободных мест"
        }
    ];

    $scope.orderOptions = [
        {
            "value": "Desc",
            "label": "<i class=\"fa fa-arrow-circle-down\"></i> По убыванию"
        },
        {
            "value": "Asc",
            "label": "<i class=\"fa fa-arrow-circle-up\"></i> По возраcтанию"
        }
    ];
});
