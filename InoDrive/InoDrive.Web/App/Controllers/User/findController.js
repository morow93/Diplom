angular.module('InoDrive').controller('findController', function ($scope, $state, $alert, $document, $timeout, tripsService) {

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

    $scope.currentFind = null;
    $scope.find = {};
    $scope.page = 1;
    $scope.perPage = 10;
    $scope.find.wayPoints = [{}];
    $scope.emptyResults = true;

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

    $scope.selectedSortOption = "Rating";
    $scope.selectedOrderOption = "descending";

    $scope.sortOptions = [
        {
            "value": "Rating",
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
            "value": "descending",
            "label": "<i class=\"fa fa-arrow-circle-down\"></i> В порядке убывания"
        },
        {
            "value": "ascending",
            "label": "<i class=\"fa fa-arrow-circle-up\"></i> В порядке возрастания"
        }
    ];

    $scope.formSubmit = function (form, needUp) {
        
        if (form.$valid) {
           
            $scope.page = 1;
            $scope.perPage = 10;

            $scope.trips = [];
            $scope.totalEmptyResults = false;
            $scope.savedWayPoints = $scope.find.wayPoints;

            $scope.currentFind = clone($scope.find);
            $scope.getPageOfTrips();

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

    $scope.getPageOfTrips = function () {
  
        if ($scope.loading) {
            return;
        } else {
            $scope.loading = true;
        }

        if ($scope.currentFind && !angular.equals({}, $scope.currentFind)) {

            if ($scope.page == 1)
                $scope.laddaFind = true;

            var type = (typeof $scope.currentFind.leavingDate);
            if (type != "string") {
                $scope.currentFind.leavingDate = moment($scope.currentFind.leavingDate).format();
            }
       
            var tmpWayPoints = [];
            for (var i = 0; i < $scope.savedWayPoints.length; i++) {
                if ($scope.savedWayPoints[i].details && $scope.savedWayPoints[i].details.place_id) {
                    tmpWayPoints.push($scope.savedWayPoints[i].details.place_id);
                }
            }
            $scope.currentFind.wayPoints = tmpWayPoints;

            $scope.currentFind.originPlaceId = $scope.currentFind.originCity.details.place_id;
            $scope.currentFind.destinationPlaceId = $scope.currentFind.destinationCity.details.place_id;

            $scope.currentFind.page = $scope.page;
            $scope.currentFind.perPage = $scope.perPage;
            
            $scope.currentFind.priceTop = $scope.rangePrice.maxModel;
            $scope.currentFind.priceBottom = $scope.rangePrice.minModel;
            $scope.currentFind.price = $scope.find.price;

            $scope.currentFind.sortField = $scope.selectedSortOption;
            $scope.currentFind.sortOrder = $scope.selectedOrderOption;

            $timeout(function () {

                tripsService.findTrips($scope.currentFind).then(function (response) {

                    if (response.results && response.results.length > 0 && response.totalCount > 0) {

                        $scope.totalCount = response.totalCount;
                        $scope.showTotalCount = true;

                        for (var i = 0; i < response.results.length; i++) {
                            if (response.results[i].userRating === "NaN") {
                                response.results[i].userRating = 0;
                            }
                        }

                        if ($scope.trips && $scope.page != 1) {
                            for (var i = 0; i < response.results.length; i++) {
                                $scope.trips.push(response.results[i]);
                            }
                        } else {
                            $scope.trips = response.results;
                        }

                        var totalPages = Math.ceil(($scope.totalCount + $scope.countExcluded) / $scope.perPage);
                        if (++$scope.page > totalPages) {
                            $scope.emptyResults = true;
                        } else {
                            $scope.emptyResults = false;
                        }

                    } else {

                        $scope.emptyResults = true;
                        if ($scope.page == 1) {
                            $scope.totalEmptyResults = true;
                        }
                    }

                }).catch(function () {

                    //neederror

                }).finally(function () {

                    $scope.loading = false;
                    $scope.laddaFind = false;

                });

            }, 1500);

        }
    };

    $scope.triggerFind = function() {
      
        $scope.extendFind = !$scope.extendFind;
        var index = titles.indexOf($scope.findTitle);

        switch(index) {
            case 0:

                $scope.findTitle = titles[1];//"Расширенный поиск (?)"
                break;

            case 1:
            default:

                $scope.find.IsAllowdedChildren = null;
                $scope.find.IsAllowdedDeviation = null;
                $scope.find.IsAllowdedDrink = null;
                $scope.find.IsAllowdedEat = null;
                $scope.find.IsAllowdedMusic = null;
                $scope.find.IsAllowdedPets = null;
                $scope.find.IsAllowdedSmoke = null;
                $scope.find.wayPoints = [{}];

                $scope.findTitle = titles[0];//"Обычный поиск (?)"
                break;
        }

    };

    function clone(obj) {
        var copy;

        // Handle the 3 simple types, and null or undefined
        if (null == obj || "object" != typeof obj) return obj;

        // Handle Date
        if (obj instanceof Date) {
            copy = new Date();
            copy.setTime(obj.getTime());
            return copy;
        }

        // Handle Array
        if (obj instanceof Array) {
            copy = [];
            for (var i = 0, len = obj.length; i < len; i++) {
                copy[i] = clone(obj[i]);
            }
            return copy;
        }

        // Handle Object
        if (obj instanceof Object) {
            copy = {};
            for (var attr in obj) {
                if (obj.hasOwnProperty(attr)) copy[attr] = clone(obj[attr]);
            }
            return copy;
        }

        throw new Error("Unable to copy obj! Its type isn't supported.");
    }
});
