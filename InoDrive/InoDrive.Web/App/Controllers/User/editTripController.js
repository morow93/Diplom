'use strict';

app.controller('editTripController', function ($scope, $timeout, $upload, $stateParams, $q, $alert, $document, tripsService, ngAuthSettings, customStorageService) {
    
    $scope.formHeader = "Редактирование поездки";
    $scope.formButton = "Сохранить";
    $scope.carsFolder = "images/cars/";
    $scope.file = {};

    $scope.trip = {};




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
        { "value": "A", "label": "<i class=\"fa fa-star\"></i> Класс А (особо малый)" },
        { "value": "B", "label": "<i class=\"fa fa-star\"></i> Класс B (малый)" },
        { "value": "C", "label": '<i class=\"fa fa-star\"></i> Класс C ("гольф"-класс)' },
        { "value": "D", "label": "<i class=\"fa fa-star\"></i> Класс D (средний)" },
        { "value": "E", "label": '<i class=\"fa fa-star\"></i> Класс E ("бизнес"-класс)' },
        { "value": "F", "label": '<i class=\"fa fa-star\"></i> Класс F ("люкс"-класс)' },
        { "value": "G", "label": '<i class=\"fa fa-star\"></i> Купе/кабриолет' },
        { "value": "H", "label": '<i class=\"fa fa-star\"></i> Внедорожник' },
        { "value": "I", "label": '<i class=\"fa fa-star\"></i> Минивэн' },
        { "value": "J", "label": '<i class=\"fa fa-star\"></i> Кроссовер' }
    ];

    var myAlert;

    $scope.showAlert = function (alertData) {

        if (myAlert != null) {
            myAlert.$promise.then(myAlert.destroy);
        }
        myAlert = $alert(alertData);
        myAlert.$promise.then(myAlert.show);
    };

    $scope.getTripForEdit = function () {

        var params = {
            userId: $scope.authentication.userId,
            tripId: $stateParams.tripId
        };
        
        tripsService.getTripForEdit(params).then(function (data) {

            $scope.trip = data;

            if ($scope.trip.pay) {
                $scope.trip.price = "cur";
            } else {
                $scope.trip.price = "neg";
            }

            if (!$scope.trip.wayPoints || $scope.trip.wayPoints.length == 0) {
                $scope.trip.wayPoints = [{}];
            }

            for (var i = 0; i < $scope.trip.wayPoints.length; i++) {
                $scope.trip.wayPoints[i].model = $scope.trip.wayPoints[i].name;
            }

            $scope.trip.oldCarImage = $scope.trip.carImage;
            $scope.trip.rawOriginPlace.model = $scope.trip.rawOriginPlace.name;
            $scope.trip.rawDestinationPlace.model = $scope.trip.rawDestinationPlace.name;

            $scope.oldTrip = clone($scope.trip);

            if (data.carImage != null && data.carImage != "") {

                var fullPathToFile =
                    ngAuthSettings.apiServiceBaseUri + $scope.carsFolder + data.carImage;

                isImage(fullPathToFile).then(function (test) {
                    if (test) {
                        $scope.trip.dataUrl = fullPathToFile;
                    }
                });
            }

        }).catch(function (error) {
            throw error.data;
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

            var oldTrip = angular.toJson($scope.oldTrip);
            var newTrip = angular.toJson($scope.trip);

            if ((oldTrip != newTrip) || $scope.wasFileChanged) {
                
                $scope.trip.userId = $scope.authentication.userId;
                $scope.trip.tripId = $stateParams.tripId;

                var type = (typeof $scope.trip.leavingDate);
                if (type != "string") {
                    $scope.trip.leavingDate = moment($scope.trip.leavingDate).format();
                }

                if ($scope.trip.price == "neg") {
                    $scope.trip.pay = null;
                }

                if ($scope.trip.rawOriginPlace.details) {

                    $scope.trip.originPlace = {
                        placeId:    $scope.trip.rawOriginPlace.details.place_id,
                        name:       $scope.trip.rawOriginPlace.details.formatted_address,
                        lat:        $scope.trip.rawOriginPlace.details.geometry.location.lat(),
                        lng:        $scope.trip.rawOriginPlace.details.geometry.location.lng()
                    };

                } else {

                    $scope.trip.originPlace = {
                        placeId: $scope.trip.rawOriginPlace.placeId,
                        name: $scope.trip.rawOriginPlace.name
                    };
                    
                }

                if ($scope.trip.rawDestinationPlace.details) {

                    $scope.trip.destinationPlace = {
                        placeId:    $scope.trip.rawDestinationPlace.details.place_id,
                        name:       $scope.trip.rawDestinationPlace.details.formatted_address,
                        lat:        $scope.trip.rawDestinationPlace.details.geometry.location.lat(),
                        lng:        $scope.trip.rawDestinationPlace.details.geometry.location.lng()
                    };

                } else {

                    $scope.trip.destinationPlace = {
                        placeId:    $scope.trip.rawDestinationPlace.placeId,
                        name:       $scope.trip.rawDestinationPlace.name
                    };

                }
               
                if ($scope.trip.wayPoints) {
                    $scope.trip.selectedPlaces = [];
                    for (var i = 0; i < $scope.trip.wayPoints.length; ++i) {

                        if ($scope.trip.wayPoints[i].details) {
                            $scope.trip.selectedPlaces.push({
                                placeId:    $scope.trip.wayPoints[i].details.place_id,
                                name:       $scope.trip.wayPoints[i].details.formatted_address,
                                lat:        $scope.trip.wayPoints[i].details.geometry.location.lat(),
                                lng:        $scope.trip.wayPoints[i].details.geometry.location.lng()
                            });
                        } else if ($scope.trip.wayPoints[i].name && $scope.trip.wayPoints[i].placeId) {
                            $scope.trip.selectedPlaces.push({
                                placeId: $scope.trip.wayPoints[i].placeId,
                                name: $scope.trip.wayPoints[i].name
                            });
                        }
                    }
                }

                $scope.laddaManageTripFlag = true;

                $upload.upload({
                    url: ngAuthSettings.apiServiceBaseUri + "/api/trips/editTrip",
                    method: "POST",
                    data: { trip: $scope.trip },
                    file: $scope.file
                }).success(function (data, status, headers, config) {

                    $scope.file = {};
                    $scope.wasFileChanged = false;
                    $scope.trip.oldCarImage = $scope.trip.carImage;
                    $scope.oldTrip = clone($scope.trip);

                    if (needUp) {
                        $document.scrollTopAnimated(0, 500).then(function () {
                            showFormAlert("Поздравляем! Поездка была успешно сохранена!", "success");
                        });
                    } else {
                        showFormAlert("Поздравляем! Поездка была успешно сохранена!", "success");
                    }

                }).error(function (data, status, headers, config) {

                    if (data.exceptionType == "System.NotSupportedException") {
                        $scope.trip.carImage = $scope.trip.oldCarImage = "";
                    }
                    
                    if (needUp) {
                        $document.scrollTopAnimated(0, 500).then(function () {
                            showFormAlert("Внимание! При сохранении произошла ошибка! Можете попытаться изменить поездку опять или связаться в администратором.", "danger");
                        });
                    } else {
                        showFormAlert("Внимание! При сохранении произошла ошибка! Можете попытаться изменить поездку опять или связаться в администратором.", "danger");
                    }


                }).finally(function (data, status, headers, config) {

                    $scope.laddaManageTripFlag = false;

                });

            } else {

                if (needUp) {
                    $document.scrollTopAnimated(0, 500).then(function () {
                        showFormAlert("Для того чтобы сохранить изменения, вначале нужно что-то изменить!", "info");
                    });
                } else {
                    showFormAlert("Для того чтобы сохранить изменения, вначале нужно что-то изменить!", "info");
                }

            }

        } else {

            if (needUp) {
                $document.scrollTopAnimated(0, 500).then(function () {
                    showFormAlert();
                });
            } else {
                showFormAlert();
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
            if ($scope.fileReaderSupported && file.type.indexOf('image') > -1) {

                if (file.size > 4 * 1024 * 1024) {
                    $scope.fileErrorMsg = "Файл должен быть не больше 4MB!";
                } else {
                    $scope.file = file;
                    $scope.wasFileChanged = true;

                    var fileReader = new FileReader();
                    fileReader.readAsDataURL(file);
                    fileReader.onload = function (e) {
                        $timeout(function () {
                            $scope.trip.dataUrl = e.target.result;
                        });
                    }
                }
            } else {
                $scope.fileErrorMsg = "Выберите файл изображения!";
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

    $scope.getTripForEdit();

});