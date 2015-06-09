angular.module('InoDrive').controller('createTripController', function ($scope, $state, $alert, $document, $timeout, $upload, $q, tripsService, ngAuthSettings, customStorageService) {
    
    $scope.carsFolder = "images/cars/";
    $scope.file = {};

    $scope.trip = {};
    $scope.trip.pay = 100;
    $scope.trip.wayPoints = [{}];
    $scope.trip.price = "neg";
    
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

    //modal begin

    var myAlert;

    $scope.showAlert = function (alertData) {
        if (myAlert != null) {
            myAlert.$promise.then(myAlert.destroy);
        }
        myAlert = $alert(alertData);
        myAlert.$promise.then(myAlert.show);
    };
    
    //modal end

    $scope.getCar = function () {

        tripsService.getCar({ userId: $scope.authentication.userId }).then(function (car) {
            
            angular.forEach(car, function (value, key) {
                $scope.trip[key] = value;
            });

            if (car.carImage) {

                var fullPathToFile = ngAuthSettings.apiServiceBaseUri + $scope.carsFolder + car.carImage;

                isImage(fullPathToFile).then(function (test) {
                    if (test) {
                        $scope.trip.dataUrl = fullPathToFile;                        
                    }
                });
            }

        }).catch(function (err) {

            $scope.showAlert({
                title: "Внимание! Произошла ошибка при загрузке информации о машине.",
                content: "",
                type: "danger",
                show: false,
                container: ".form-alert"
                ,template: "/app/templates/alert.html"
            });

        }).finally(function () {

            $scope.wasLoaded = true;//DON'T REMOVE THIS

        });
    };

    $scope.formSubmit = function (form, needUp) {

        if (form.$valid) {

            var trip = $scope.trip;
            trip.userId = $scope.authentication.userId;

            var type = (typeof trip.leavingDate);
            if (type != "string") {
                trip.leavingDate = moment(trip.leavingDate).format();
            }

            if ($scope.trip.price == "neg") {
                $scope.trip.pay = null;
            }

            //fill places begin

            $scope.trip.originPlace = {
            
                placeId:    $scope.trip.rawOriginPlace.details.place_id,
                name:       $scope.trip.rawOriginPlace.details.formatted_address,
                latitude:   $scope.trip.rawOriginPlace.details.geometry.location.A,
                longitude:  $scope.trip.rawOriginPlace.details.geometry.location.F
            };

            $scope.trip.destinationPlace = {

                placeId:    $scope.trip.rawDestinationPlace.details.place_id,
                name:       $scope.trip.rawDestinationPlace.details.formatted_address,
                latitude:   $scope.trip.rawDestinationPlace.details.geometry.location.A,
                longitude:  $scope.trip.rawDestinationPlace.details.geometry.location.F
            };
            
            if ($scope.trip.wayPoints) {
                $scope.trip.selectedPlaces = [];
                for (var i = 0; i < $scope.trip.wayPoints.length; ++i) {

                    if ($scope.trip.wayPoints[i].details) {
                        $scope.trip.selectedPlaces.push({
                            placeId: $scope.trip.wayPoints[i].details.place_id,
                            name: $scope.trip.wayPoints[i].details.formatted_address,
                            latitude: $scope.trip.wayPoints[i].details.geometry.location.A,
                            longitude: $scope.trip.wayPoints[i].details.geometry.location.F
                        });
                    }
                }
            }

            //fill places end

            $scope.laddaManageTripFlag = true;

            $upload.upload({
                url: ngAuthSettings.apiServiceBaseUri + "api/trips/createTrip",
                method: "POST",
                data: { trip: trip },
                file: $scope.file
            }).success(function (data, status, headers, config) {

                customStorageService.set("notifyToShow", {
                    message: 'Поздравляем! Поездка была создана.',
                    type: 'success',
                });
                $state.go("user.view", null, { reload: true });

            }).error(function (data, status, headers, config) {

                $scope.showAlert({
                    title: "Внимание! При сохранении произошла ошибка! Можете попытаться создать поездку опять или связаться в администратором.",
                    content: "",
                    type: "danger",
                    show: false,
                    container: ".form-alert"
                    ,template: "/app/templates/alert.html"
                });

            }).finally(function (data, status, headers, config) {

                $scope.laddaManageTripFlag = false;

            });

        } else {

            function showErrorFormAlert() {
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
            };

            if (needUp) {
                $document.scrollTopAnimated(0, 500).then(function () {
                    showErrorFormAlert();
                });
            } else {
                showErrorFormAlert();
            }
        }

    };

    //work with file begin

    $scope.noImage = ngAuthSettings.clientAppBaseUri + "content/images/no-car.png";
    $scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);

    $scope.removeFile = function () {
        debugger;
        $scope.fileNature = 1;
        $scope.trip.dataUrl = null;
        $scope.trip.carImage = null;
        $scope.trip.carImageExtension = null;
        $scope.file = {};
        $scope.fileErrorMsg = null;
    };

    $scope.onFileSelect = function (files) {
        debugger;
        var file = files[0];
        $scope.fileErrorMsg = null;

        if (file != null) {
            if ($scope.fileReaderSupported && file.type.indexOf('image') > -1 && file.type.indexOf('gif') == -1) {

                if (file.size > 4 * 1024 * 1024) {
                    $scope.fileErrorMsg = "Размер файла должен не превышать 4MB";
                } else {

                    $scope.file = file;
                    $scope.trip.carImageName = file.name;//need when convert
                    $scope.trip.carImageExtension = file.type;//from uri to blob

                    var fileReader = new FileReader();
                    fileReader.onload = function (e) {
                        $timeout(function () {
                            $scope.trip.dataUrl = e.target.result;
                            $scope.fileNature = 3;// img from form input
                        });
                    }
                    fileReader.readAsDataURL(file);
                }
            } else {
                $scope.fileErrorMsg = "Пожалуйста, выберите файл изображения (не GIF) ";
            }
        }
    };

    function isImage(src) {

        var deferred = $q.defer();

        var image = new Image();
        image.onerror = function () {
            deferred.resolve(false);
        };
        image.onload = function () {
            deferred.resolve(true);
        };
        image.src = src;

        return deferred.promise;
    };

    //work with file end

    //run run run

    $scope.getCar();

});
