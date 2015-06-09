angular.module('InoDrive').controller('findController', function ($scope, $state, $alert, $document) {

    var titles = [
        "Обычный поиск (?)",
        "Расширенный поиск (?)"
    ];
    $scope.findTitle = titles[0];

    $scope.rangePrice = {
        minModel: 0,
        maxModel: 100
    };

    $scope.selectCurPrice = function () {
        $scope.rangePrice.minModel = 0;
        $scope.rangePrice.maxModel = 100;
    };

    $scope.find = {};
    $scope.find.wayPoints = [{}];

    $scope.autocompleteOptions = {
        types: "(cities)",
        country: "ru"
    };
    
    $scope.addWayPointer = function () {
        if ($scope.find.wayPoints.length <= 4) {
            $scope.find.wayPoints.push({});
        }
    };

    $scope.removeWayPointer = function () {
        if ($scope.find.wayPoints.length > 1) {
            $scope.find.wayPoints.pop();
        } else if ($scope.find.wayPoints.length === 1) {
            $scope.find.wayPoints[0] = {};
        }
    };

    var myAlert;

    $scope.showAlert = function (alertData) {
        if (myAlert != null) {
            myAlert.$promise.then(myAlert.destroy);
        }
        myAlert = $alert(alertData);
        myAlert.$promise.then(myAlert.show);
    };

    $scope.formSubmit = function (form, needUp) {

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
        } else {
            
            function showErrorFormAlert() {
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
            };

            if (needUp) {
                $document.scrollTopAnimated(0, 500).then(function() {
                    showErrorFormAlert();
                });
            } else {
                showErrorFormAlert();
            }
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
            "label": "<i class=\"fa fa-arrow-circle-down\"></i> В порядке убывания"
        },
        {
            "value": "Asc",
            "label": "<i class=\"fa fa-arrow-circle-up\"></i> В порядке возрастания"
        }
    ];

    $scope.triggerFind = function() {
      
        $scope.extendFind = !$scope.extendFind;
        var index = titles.indexOf($scope.findTitle);

        switch(index) {
            case 0:
                $scope.findTitle = titles[1];
                break;
            case 1:
            default:
                $scope.findTitle = titles[0];
                break;
        }

    };
});
