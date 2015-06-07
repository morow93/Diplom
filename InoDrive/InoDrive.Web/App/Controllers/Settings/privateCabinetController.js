'use strict';

app.controller('privateCabinetController', function (
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
    authService) {

    $scope.myImage = '';
    $scope.myCroppedImage = '';
    $scope.avatarsFolder = "images/avatars/";
    
    $scope.getUserProfile = function () {

        usersService.getUserProfile({ userId: $scope.authentication.userId }).then(function (profile) {

            $scope.profile = profile;
            $scope.profile.oldAvatarImage = profile.avatarImage;
            $scope.oldProfile = clone($scope.profile);

            if (profile.avatarImage != null && profile.avatarImage != "") {

                var fullPathToFile = ngAuthSettings.apiServiceBaseUri + $scope.avatarsFolder + profile.avatarImage;

                isImage(fullPathToFile).then(function (test) {
                    if (test) {
                        $scope.dataUrl = fullPathToFile;
                    }
                });
            }

        }).catch(function (error) {
            throw error.data;
        });
    };

    //initialize

    $scope.getUserProfile();

    //edit profile

    $scope.editProfile = function (formEditProfile) {

        if (formEditProfile.$valid) {

            var oldProfile = angular.toJson($scope.oldProfile);
            var newProfile = angular.toJson($scope.profile);
            var wasInputsChanged = oldProfile != newProfile;

            if (wasInputsChanged || $scope.wasFileChanged) {

                if ($scope.newFile == null) {
                    $scope.newFile = {};
                } else {
                    $scope.newFile.type = $scope.rememberType;
                    $scope.newFile.name = $scope.rememberName;
                }
                $scope.disableSubmit = true;

                $upload.upload({
                    url: ngAuthSettings.apiServiceBaseUri + "api/user/editUserProfile",
                    method: "POST",
                    data: { profile: $scope.profile },
                    file: $scope.newFile
                }).success(function (data, status, headers, config) {
                    endUpSuccessfully(data);
                }).error(function (data, status, headers, config) {
                    $scope.showAlert({
                        title: 'При сохранении произошла ошибка!',
                        content: 'Попробуйте снова!',
                        type: 'danger',
                        show: false,
                        container: '.form-alert'
                    });
                }).finally(function (data, status, headers, config) {
                    $scope.disableSubmit = false;
                });

            } else {

                $scope.showAlert({
                    title: 'Внимание!',
                    content: 'Для того чтобы сохранить изменения, вначале нужно что-то изменить!',
                    type: 'info',
                    show: false,
                    container: '.form-alert'
                });
            }

        } else {

            $scope.showAlert({
                title: 'Невозможно выполнить сохранение!',
                content: 'Исправьте отмеченные поля!',
                type: 'danger',
                show: false,
                container: '.form-alert'
            });
            //set all required inputs dirty
            angular.forEach(formEditProfile.$error.required, function (field) {
                field.$setDirty();
            });
        }
    };

    //files

    $scope.noImage = ngAuthSettings.clientAppBaseUri + "content/images/no-avatar.jpg";

    $scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);

    $scope.onFileSelect = function (files) {

        var file = files[0];
        $scope.fileErrorMsg = null;

        if (file != null) {
            if ($scope.fileReaderSupported && file.type.indexOf('image') > -1) {

                if (file.size > 4 * 1024 * 1024) {
                    $scope.fileErrorMsg = "Размер файла должен не превышать 4MB!";
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
                $scope.fileErrorMsg = "Выберите файл изображения!";
            }
        }
    };

    $scope.removeFile = function () {

        $scope.newFile = null;
        $scope.dataUrl = null;
        $scope.fileErrorMsg = null;
        $scope.wasFileChanged = true;
        $scope.profile.avatarImage = "";
    };

    $scope.changeCroped = function (uri) {

        $scope.toShowAvatar = uri;
        $scope.newFile = dataURItoBlob(uri);

    };

    //alerts

    var myAlert;

    $scope.showAlert = function (alertData) {

        if (myAlert != null) {
            myAlert.$promise.then(myAlert.destroy);
        }
        myAlert = $alert(alertData);
        myAlert.$promise.then(myAlert.show);
    };

    //modals
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
        $scope.dataUrl = $scope.toShowAvatar;
    };

    $scope.cancel = function () {
        cropAvatarModal.$promise.then(cropAvatarModal.hide);
    };

    //not scope functions

    function endUpSuccessfully(profile) {

        var newInitials = $scope.profile.firstName + " " + $scope.profile.lastName;

        //change user  
        $scope.authentication.initials = newInitials;
        authService.changeInitials(newInitials);

        //for next changing profile
        $scope.wasFileChanged = false;

        $scope.profile = profile;
        $scope.profile.oldAvatarImage = profile.avatarImage;
        $scope.oldProfile = clone($scope.profile);

        //notify
        $scope.showAlert({
            title: 'Поздравляем!',
            content: 'Изменения были успешно сохранены!',
            type: 'success',
            show: false,
            container: '.form-alert'
        });
    }

    function dataURItoBlob(dataURI) {
        // convert base64/URLEncoded data component to raw binary data held in a string
        var byteString;
        if (dataURI.split(',')[0].indexOf('base64') >= 0)
            byteString = atob(dataURI.split(',')[1]);
        else
            byteString = unescape(dataURI.split(',')[1]);

        // separate out the mime component
        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

        // write the bytes of the string to a typed array
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
        if (obj == null || typeof (obj) != 'object')
            return obj;

        var temp = obj.constructor();

        for (var key in obj) {
            if (obj.hasOwnProperty(key)) {
                temp[key] = clone(obj[key]);
            }
        }
        return temp;
    }
});