'use strict';
angular.module('InoDrive').controller('privateCabinetController', function (
    $scope,
    $q,
    $upload,
    $http,
    $tooltip,
    $timeout,
    $alert,
    $modal,
    ngAuthSettings,
    usersService,
    customStorageService,
    authService) {

    $scope.car = {};
    $scope.profile = {};

    $scope.newFile = null;
    $scope.newCarFile = null;
    $scope.myImage = '';
    $scope.myCroppedImage = '';

    $scope.avatarsFolder  = "images/avatars/";
    $scope.carsFolder = "images/cars/";
    
    $scope.yearsOfBirth = [];
    var year = new Date().getFullYear() - 14;
    for (var i = 0; i < 100; ++i) {
        year = year - 1;
        $scope.yearsOfBirth.push(year);
    }

    $scope.yearsOfStage = [];
    var year = new Date().getFullYear();
    for (var i = 0; i < 100; ++i) {
        year = year - 1;
        $scope.yearsOfStage.push(year);
    }

    $scope.sexOptions = [
        {
            "value": null,
            "label": "Не указан"
        },
        {
            "value": true,
            "label": "<i class=\"fa fa-mars\"></i>&nbsp;Мужской"
        },
        {
            "value": false,
            "label": "<i class=\"fa fa-venus\"></i>&nbsp;&nbsp;Женский"
        }
    ];

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

    $scope.getUserProfile = function () {

        usersService.getUserProfile({ userId: $scope.authentication.userId }).then(function (profile) {

            $scope.car.car = profile.car;
            $scope.car.carClass = profile.carClass;
            $scope.car.carImage = profile.carImage;
            $scope.car.carImageExtension = profile.carImageExtension;
            $scope.car.userId = profile.userId;

            profile.car = null, profile.carClass = null, profile.carImage = null, profile.carImageExtension = null;

            $scope.profile = profile;

            if ($scope.profile.yearOfBirth == null) {
                $scope.profile.yearOfBirth = 0;
            }

            if ($scope.profile.yearOfStage == null) {
                $scope.profile.yearOfStage = 0;
            }
            
            $scope.profile.oldAvatarImage = profile.avatarImage;
            $scope.car.oldCarImage = $scope.car.carImage;

            $scope.oldProfile = clone($scope.profile);
            $scope.oldCar = clone($scope.car);

            if ($scope.profile.avatarImage) {

                var fullPathToFileOne = ngAuthSettings.apiServiceBaseUri + $scope.avatarsFolder + $scope.profile.avatarImage;

                isImage(fullPathToFileOne).then(function (test) {
                    if (test) {
                        $scope.avatarDataUrl = fullPathToFileOne;
                    }
                });
            }

            if ($scope.car.carImage) {

                var fullPathToFileTwo = ngAuthSettings.apiServiceBaseUri + $scope.carsFolder + $scope.car.carImage;

                isImage(fullPathToFileTwo).then(function (test) {
                    if (test) {
                        $scope.carDataUrl = fullPathToFileTwo;
                    }
                });
            }

        }).catch(function (error) {

            //neederror

        }).finally(function () {

            $scope.wasLoaded = true;

        });
    };

    $scope.formProfileSubmit = function (formEditProfile) {

        if (formEditProfile.$valid) {

            var oldProfile = angular.toJson($scope.oldProfile);
            var newProfile = angular.toJson($scope.profile);

            if (oldProfile != newProfile || $scope.wasFileChanged) {
                
                if ($scope.newFile && $scope.fileExists) {
                    $scope.newFile.name = $scope.rememberName;
                    //$scope.newFile.type = $scope.rememberType;
                } else {
                    $scope.newFile = {};
                }

                $scope.laddaEditProfileFlag = true;

                debugger;

                if ($scope.profile.yearOfStage == "0") {
                    $scope.profile.yearOfStage = null;
                }
                if ($scope.profile.yearOfBirth == "0") {
                    $scope.profile.yearOfBirth = null;
                }
                
                $upload.upload({
                    url: ngAuthSettings.apiServiceBaseUri + "api/user/setUserProfile",
                    method: "POST",
                    data: { profile: $scope.profile },
                    file: $scope.newFile
                }).success(function (data, status, headers, config) {
                                  
                    var newInitials = $scope.profile.firstName + " " + $scope.profile.lastName;
                    $scope.authentication.initials = newInitials;
                    authService.setAuthorizationData("initials", newInitials);

                    $scope.wasFileChanged = false;              
                    $scope.profile.oldAvatarImage = $scope.profile.avatarImage = data.avatarImage;
                    $scope.oldProfile = clone($scope.profile);

                    $scope.showAlert({
                        title: "Поздравляем! Изменения в профиле были успешно сохранены!",
                        content: "",
                        type: "success",
                        show: false,
                        container: ".form-profile-alert"
                        , template: "/app/templates/alert.html"
                    });

                }).error(function (data, status, headers, config) {

                    $scope.showAlert({
                        title: 'При сохранении произошла ошибка!',
                        content: 'Попробуйте снова!',
                        type: 'danger',
                        show: false,
                        container: '.form-profile-alert'
                    });

                }).finally(function (data, status, headers, config) {

                    $scope.laddaEditProfileFlag = false;

                });

            } else {

                $scope.showAlert({
                    title: "Для того чтобы сохранить изменения в профиле, вначале нужно что-то изменить!",
                    content: "",
                    type: "info",
                    show: false,
                    container: ".form-profile-alert",
                    template: "/app/templates/alert.html"
                });
            }

        } else {

            $scope.showAlert({
                title: 'Невозможно выполнить сохранение! Исправьте отмеченные поля!',
                content: '',
                type: 'danger',
                show: false,
                container: '.form-profile-alert',
                template: "/app/templates/alert.html"
            });

            angular.forEach(formEditProfile.$error.required, function (field) {
                field.$setDirty();
            });
        }
    };
    
    $scope.formCarSubmit = function (formCarSubmit) {

        debugger;

        if (formCarSubmit.$valid) {

            var oldCar = angular.toJson($scope.oldCar);
            var newCar = angular.toJson($scope.car);

            if (oldCar != newCar || $scope.wasCarFileChanged) {

                if (!$scope.carFile) {
                    $scope.carFile = {};
                }

                $scope.laddaEditCarFlag = true;

                $upload.upload({
                    url: ngAuthSettings.apiServiceBaseUri + "api/user/setUserCar",
                    method: "POST",
                    data: { profile: $scope.car },
                    file: $scope.carFile
                }).success(function (data, status, headers, config) {

                    $scope.wasCarFileChanged = false;
                    $scope.car.oldCarImage = $scope.car.carImage = data.carImage;
                    $scope.oldCar = clone($scope.car);

                    $scope.showAlert({
                        title: "Поздравляем! Изменения в профиле были успешно сохранены!",
                        content: "",
                        type: "success",
                        show: false,
                        container: ".form-car-alert"
                        , template: "/app/templates/alert.html"
                    });

                }).error(function (data, status, headers, config) {

                    $scope.showAlert({
                        title: 'При сохранении произошла ошибка!',
                        content: 'Попробуйте снова!',
                        type: 'danger',
                        show: false,
                        container: '.form-car-alert'
                    });

                }).finally(function (data, status, headers, config) {

                    $scope.laddaEditCarFlag = false;

                });

            } else {

                $scope.showAlert({
                    title: "Для того чтобы сохранить изменения в профиле, вначале нужно что-то изменить!",
                    content: "",
                    type: "info",
                    show: false,
                    container: ".form-car-alert",
                    template: "/app/templates/alert.html"
                });
            }

        } else {

            $scope.showAlert({
                title: 'Невозможно выполнить сохранение! Исправьте отмеченные поля!',
                content: '',
                type: 'danger',
                show: false,
                container: '.form-car-alert',
                template: "/app/templates/alert.html"
            });

            angular.forEach(formCarSubmit.$error.required, function (field) {
                field.$setDirty();
            });
        }
    };


    $scope.noCarImage = ngAuthSettings.clientAppBaseUri + "content/images/no-car.png";
    $scope.noAvatarImage = ngAuthSettings.clientAppBaseUri + "content/images/no-avatar.jpg";
    $scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);

    $scope.onFileSelect = function (files) {

        var file = files[0];
        $scope.fileErrorMsg = null;

        if (file != null) {
            if ($scope.fileReaderSupported && file.type.indexOf('image') > -1) {

                if (file.size > 2 * 1024 * 1024) {
                    $scope.fileErrorMsg = "Файл должен быть не больше 2MB!";
                } else {
                    $scope.file = file;
                    $scope.rememberName = file.name;//need when convert
                    $scope.rememberType = file.type;//from uri to blob

                    $scope.wasFileChanged = true;

                    var fileReader = new FileReader();
                    fileReader.onload = function (e) {
                        $scope.$apply(function ($scope) {
                            cropAvatarModal.$promise.then(cropAvatarModal.show);
                            $scope.myImage = e.target.result;
                        });
                    }
                    fileReader.readAsDataURL(file);
                }
            } else {
                $scope.fileErrorMsg = "Выберите файл изображения (не GIF)";
            }
        }
    };

    $scope.onCarFileSelect = function (files) {

        var file = files[0];
        $scope.fileCarErrorMsg = null;

        if (file != null) {
            if ($scope.fileReaderSupported && file.type.indexOf('image') > -1 && file.type.indexOf('gif') == -1) {

                if (file.size > 2 * 1024 * 1024) {
                    $scope.fileCarErrorMsg = "Файл должен быть не больше 2MB!";
                } else {
                    $scope.carFile = file;
                    $scope.car.carImageName = file.name;
                    $scope.car.carImageExtension = file.type;

                    $scope.wasCarFileChanged = true;

                    var fileReader = new FileReader();
                    fileReader.onload = function (e) {
                        $timeout(function () {
                            $scope.carDataUrl = e.target.result;
                        });
                    }
                    fileReader.readAsDataURL(file);
                }
            } else {
                $scope.fileCarErrorMsg = "Пожалуйста, выберите файл изображения (не GIF) ";
            }
        }
    };

    $scope.removeAvatarFile = function () {

        $scope.newFile = null;
        $scope.fileExists = false;
        $scope.avatarDataUrl = null;

        $scope.fileErrorMsg = null;
        $scope.wasFileChanged = true;
        $scope.profile.avatarImage = "";    

    };

    $scope.removeCarFile = function () {

        $scope.carFile = null;
        $scope.carDataUrl = null;
        
        $scope.fileCarErrorMsg = null;
        $scope.wasCarFileChanged = true;
        $scope.car.carImage = "";
    };

    $scope.changeCroped = function (uri) {
        $scope.toShowAvatar = uri;
        $scope.newFile = dataURItoBlob(uri);
    };

    var myAlert;

    $scope.showAlert = function (alertData) {

        if (myAlert != null) {
            myAlert.$promise.then(myAlert.destroy);
        }
        myAlert = $alert(alertData);
        myAlert.$promise.then(myAlert.show);
    };

    var cropAvatarModal = $modal({
        scope: $scope,
        template: 'App/Templates/crop_avatar.html',
        show: false,
        animation: "am-fade-and-scale",
        placement: "center",
        backdrop: 'static'
    });

    $scope.ok = function () {
        cropAvatarModal.$promise.then(cropAvatarModal.hide);
        $scope.avatarDataUrl = $scope.toShowAvatar;
        $scope.fileExists = true;
    };

    $scope.cancel = function () {
        cropAvatarModal.$promise.then(cropAvatarModal.hide);
    };

    function dataURItoBlob(dataURI) {

        var byteString;
        if (dataURI.split(',')[0].indexOf('base64') >= 0)
            byteString = atob(dataURI.split(',')[1]);
        else
            byteString = unescape(dataURI.split(',')[1]);

        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

        var ia = new Uint8Array(byteString.length);
        for (var i = 0; i < byteString.length; i++) {
            ia[i] = byteString.charCodeAt(i);
        }

        return new Blob([ia], { type: mimeString });
    }

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

    $scope.getUserProfile();

});