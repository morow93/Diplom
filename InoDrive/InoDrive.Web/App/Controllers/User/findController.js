angular.module('InoDrive').controller('findController', function ($scope, $state, $alert, $document) {
    
//    public String OriginPlaceId { get; set; }
//    public String DestinationPlaceId { get; set; }
//    public DateTimeOffset LeavingDate { get; set; }
//    public Int32 Places { get; set; }
//    public Int32? PriceTop { get; set; }
//    public Int32? PriceBottom { get; set; }

//    public List<String> WayPoints { get; set; }

//    public Boolean? IsAllowdedDeviation { get; set; }
//    public Boolean? IsAllowdedChildren { get; set; }
//    public Boolean? IsAllowdedPets { get; set; }
//    public Boolean? IsAllowdedMusic { get; set; }
//    public Boolean? IsAllowdedDrink { get; set; }
//    public Boolean? IsAllowdedEat { get; set; }
//    public Boolean? IsAllowdedSmoke { get; set; }

//    public Int32 Page { get; set; }
//        public Int32 PerPage { get; set; }
//public Boolean SortOrder { get; set; }
//public String  SortField { get; set; }

    debugger;

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

    $scope.selectedSortOption = "Rating";
    $scope.selectedOrderOption = "Desc";

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
            "value": "Desc",
            "label": "<i class=\"fa fa-arrow-circle-down\"></i> В порядке убывания"
        },
        {
            "value": "Asc",
            "label": "<i class=\"fa fa-arrow-circle-up\"></i> В порядке возрастания"
        }
    ];

    $scope.formSubmit = function (form, needUp) {
        debugger;
        if (form.$valid) {

            $scope.find.pageModel.perPage = 10;
            $scope.find.pageModel.page = 1;

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

        if ($scope.currentFind != null && !angular.equals({}, $scope.currentFind)) {

            if ($scope.currentFind.pageModel == undefined) {
                $scope.currentFind.pageModel = {};
                $scope.currentFind.pageModel.perPage = 10;
                $scope.currentFind.pageModel.page = 1;
            }

            $scope.currentFind.sortOption = {
                field: $scope.selectedOpt.field,
                order: $scope.selectedOpt.order
            };

            var params = {
                userId: $scope.authentication.userId,
                page: $scope.page,
                perPage: $scope.perPage
            };

            debugger;
            return;

            tripsService.findTrips($scope.currentFind).then(function (response) {

                if (response.results && response.results.length > 0 && response.totalCount > 0) {

                    $scope.totalCount = response.totalCount;
                    $scope.showTotalCount = true;

                    for (var i = 0; i < response.results.length; i++) {
                        if (response.results[i].userRating === "NaN") {
                            response.results[i].userRating = 0;
                        }
                    }

                    if ($scope.trips) {
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

            });

        }
    };

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
