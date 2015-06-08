﻿'use strict';

var app = angular.module('InoDrive',
[
    'ngSanitize',
    'ngAnimate',
    'ngCookies',
    'ngResource',
    'ui.router',
    'ui.sortable',
    'uiGmapgoogle-maps',
    'ngAutocomplete',
    'mgcrea.ngStrap',
    'oc.lazyLoad',
    'duScroll',
    'cgNotify',
    'ui-rangeSlider',
    'LocalStorageModule',
    'angular-ladda',
    'angularFileUpload',
    'angular-loading-bar',
    'ngImgCrop',
    'ui.utils.masks'
]);

//states config
app.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider.state("home", {
        url: "/greeting/",
        controller: "homeController",
        templateUrl: "/app/views/home.html",
        resolve: {
            store: function ($ocLazyLoad) {
                return $ocLazyLoad.load(
                    {
                        name: "InoDrive",
                        files: ["app/controllers/homeController.js"]
                    }
                );
            }
        }
    });

    $stateProvider.state("signin", {
        url: "/signin/",
        controller: "signInController",
        templateUrl: "/app/views/auth/signin.html",
        resolve: {
            store: function ($ocLazyLoad) {
                return $ocLazyLoad.load(
                    {
                        name: "InoDrive",
                        files: ["app/controllers/auth/signInController.js"]
                    }
                );
            }
        }
    });

    $stateProvider.state("signup", {
        url: "/signup/",
        controller: "signUpController",
        templateUrl: "/app/views/auth/signup.html",
        resolve: {
            store: function ($ocLazyLoad) {
                return $ocLazyLoad.load(
                    {
                        name: "InoDrive",
                        files: ["app/controllers/auth/signUpController.js"]
                    }
                );
            }
        }
    });

    $stateProvider.state("confirm_email", {
        templateUrl: "/app/views/auth/confirm_email.html",
        controller: "confirmEmailController",
        url: "/confirm_email?userId&code",
        resolve: {
            loadCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {

                return $ocLazyLoad.load({
                    name: 'InoDrive',
                    files: ['app/controllers/auth/confirmEmailController.js']
                });

            }]
        }
    });
    
    $stateProvider.state("send_code", {
        url: "/send_code/",
        controller: "sendCodeController",
        templateUrl: "/app/views/auth/send_code.html",
        resolve: {
            store: function ($ocLazyLoad) {
                return $ocLazyLoad.load(
                    {
                        name: "InoDrive",
                        files: ["app/controllers/auth/sendCodeController.js"]
                    }
                );
            }
        }
    });

    $stateProvider.state("reset_password", {
        url: "/reset_password?userId&code",
        controller: "resetPasswordController",
        templateUrl: "/app/views/auth/reset_password.html",
        resolve: {
            store: function ($ocLazyLoad) {
                return $ocLazyLoad.load(
                    {
                        name: "InoDrive",
                        files: ["app/controllers/auth/resetPasswordController.js"]
                    }
                );
            }
        }
    });

    $stateProvider
        .state("user", {
            url: "/",
            templateUrl: "/app/views/user/user.html",
            abstract: true,
            controller: "userController",
            resolve: {
                store: function ($ocLazyLoad) {
                    return $ocLazyLoad.load(
                        {
                            name: "InoDrive",
                            files: ["app/controllers/userController.js"]
                        }
                    );
                }
            }
        })
            .state("user.view", {
                url: "",
                controller: "userViewController",
                templateUrl: "/app/views/user/user.view.html",
                resolve: {
                    store: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/userViewController.js"]
                            }
                        );
                    }
                }
            })
            .state("user.my_trips", {
                url: "my_trips/",
                templateUrl: "/app/views/user/user.my_trips.html"
            })
            .state("user.create_trip", {
                url: "create_trip/",
                controller: "createTripController",
                templateUrl: "/app/views/user/user.manage_trip.html",
                resolve: {
                    store: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/trips/createTripController.js"]
                            }
                        );
                    }
                }
            })
            .state("user.find", {
                url: "find/",
                controller: "findController",
                templateUrl: "/app/views/user/user.find.html",
                resolve: {
                    store: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/findController.js"]
                            }
                        );
                    }
                }
            })
            .state("user.my_bids", {
                url: "my_bids/",
                templateUrl: "/app/views/user/user.my_bids.html"
            })
            .state("user.settings", {
                url: "settings/",
                templateUrl: "/app/views/settings/user.settings.html",
                controller: "settingsController",
                abstract: true,
                resolve: {
                    store: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/settings/settingsController.js"]
                            }
                        );
                    }
                }
            })
                .state("user.settings.private_cabinet", {
                    url: "edit/",
                    templateUrl: "/app/views/settings/user.settings.private_cabinet.html",
                    controller: "privateCabinetController",
                    resolve: {
                        store: function ($ocLazyLoad) {
                            return $ocLazyLoad.load(
                                {
                                    name: "InoDrive",
                                    files: ["app/controllers/settings/privateCabinetController.js"]
                                }
                            );
                        }
                    }
                })
                .state("user.settings.change_email", {
                    url: "change_email/",
                    templateUrl: "/app/views/settings/user.settings.change_email.html"
                })
                .state("user.settings.change_password", {
                    url: "change_password/",
                    templateUrl: "/app/views/settings/user.settings.change_password.html"
                });

    $urlRouterProvider.otherwise("/greeting/");

});

//other configs must be here
app.config(function ($datepickerProvider, laddaProvider, cfpLoadingBarProvider) {

    angular.extend($datepickerProvider.defaults, {
        dateFormat: 'dd/MM/yyyy',
        startWeek: 1
    });

    laddaProvider.setOption({
        style: 'slide-right'
    });

    cfpLoadingBarProvider.includeSpinner = false;
    cfpLoadingBarProvider.latencyThreshold = 300;

});

//constants
app.constant('ngAuthSettings', {
    apiServiceBaseUri: 'http://localhost:60947/',
    clientAppBaseUri: 'http://localhost:56023/',
    clientId: 'InoDriveAngularApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

//run run run
app.run(function ($rootScope, $state, notify, authService, customStorageService) {

    authService.fillAuthorizationData();

    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

        var notifyToShow = customStorageService.get("notifyToShow");
        if (notifyToShow) {
            displayNotificationOnStageChange(notify, notifyToShow.message, notifyToShow.type);
            customStorageService.remove("notifyToShow");
        }
        
    });

});

function displayNotificationOnStageChange(notify, message, type) {

    notify.config({
        startTop: 15,
        verticalSpacing: 15,
        maximumOpen: 5,
        duration: 10000
    });
    notify({
        type: type,
        message: message,
        templateUrl: '/app/templates/notify.html'
    });
}