'use strict';

angular.module('InoDrive').controller('createTripController', function ($scope, $state, $alert, $document, $timeout, $upload, $q, tripsService, ngAuthSettings, customStorageService) {
    
    $scope.formHeader = "Новая поездка";
    $scope.formButton = "Создать";
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

    $scope.dt = new Date();
    
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

            $scope.wasLoaded = true;

        });
    };

    $scope.formSubmit = function (form, needUp) {

        function showFormAlert(title, type) {

            $scope.showAlert({
                title: title,
                content: "",
                type: type,
                show: false,
                container: ".form-alert"
                , template: "/app/templates/alert.html"
            });

            angular.forEach(form.$error.required, function (field) {
                field.$setDirty();
            });
        };

        if (form.$valid) {

            $scope.trip.userId = $scope.authentication.userId;

            var type = (typeof $scope.trip.leavingDate);
            if (type != "string") {
                $scope.trip.leavingDate = moment($scope.trip.leavingDate).format();
            }

            if ($scope.trip.price == "neg") {
                $scope.trip.pay = null;
            }

            $scope.trip.originPlace = {
            
                placeId:    $scope.trip.rawOriginPlace.details.place_id,
                name:       $scope.trip.rawOriginPlace.details.formatted_address
            };

            $scope.trip.destinationPlace = {

                placeId:    $scope.trip.rawDestinationPlace.details.place_id,
                name:       $scope.trip.rawDestinationPlace.details.formatted_address
            };
            
            if ($scope.trip.wayPoints) {
                $scope.trip.selectedPlaces = [];
                for (var i = 0; i < $scope.trip.wayPoints.length; ++i) {

                    if ($scope.trip.wayPoints[i].details) {
                        $scope.trip.selectedPlaces.push({
                            placeId: $scope.trip.wayPoints[i].details.place_id,
                            name: $scope.trip.wayPoints[i].details.formatted_address
                        });
                    }
                }
            }

            $scope.laddaManageTripFlag = true;
  
            $upload.upload({
                url: ngAuthSettings.apiServiceBaseUri + "api/trips/createTrip",
                method: "POST",
                data: { trip: $scope.trip },
                file: $scope.file
            }).success(function (data, status, headers, config) {

                customStorageService.set("notifyToShow", {
                    message: 'Поздравляем! Поездка была создана.',
                    type: 'success',
                });
                $state.go("user.view", null, { reload: true });

            }).error(function (data, status, headers, config) {

                if (needUp) {
                    $document.scrollTopAnimated(0, 500).then(function () {
                        showFormAlert("Внимание! При сохранении произошла ошибка! Можете попытаться создать поездку опять или связаться в администратором.", "danger");
                    });
                } else {
                    showFormAlert("Внимание! При сохранении произошла ошибка! Можете попытаться создать поездку опять или связаться в администратором.", "danger");
                }

            }).finally(function (data, status, headers, config) {

                $scope.laddaManageTripFlag = false;

            });

        } else {

            if (needUp) {
                $document.scrollTopAnimated(0, 500).then(function () {
                    showFormAlert("Для того чтобы создать поездку, пожалуйста, исправьте отмеченные поля!", "danger");
                });
            } else {
                showFormAlert("Для того чтобы создать поездку, пожалуйста, исправьте отмеченные поля!", "danger");
            }
        }

    };

    $scope.noImage = ngAuthSettings.clientAppBaseUri + "content/images/no-car.png";
    $scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);

    $scope.removeFile = function () {

        $scope.trip.dataUrl = null;
        $scope.trip.carImage = null;
        $scope.trip.carImageExtension = null;
        $scope.file = {};
        $scope.fileErrorMsg = null;
    };

    $scope.onFileSelect = function (files) {

        var file = files[0];
        $scope.fileErrorMsg = null;

        if (file != null) {
            if ($scope.fileReaderSupported && file.type.indexOf('image') > -1 && file.type.indexOf('gif') == -1) {

                if (file.size > 4 * 1024 * 1024) {
                    $scope.fileErrorMsg = "Размер файла должен не превышать 4MB";
                } else {

                    $scope.file = file;
                    $scope.trip.carImageName = file.name;       //need when convert
                    $scope.trip.carImageExtension = file.type;  //from uri to blob

                    var fileReader = new FileReader();
                    fileReader.onload = function (e) {
                        $timeout(function () {
                            $scope.trip.dataUrl = e.target.result;
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

    $scope.getCar();

});
