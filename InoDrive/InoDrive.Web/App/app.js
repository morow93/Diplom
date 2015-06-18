'use strict';

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
    'ui.utils.masks',
    'infinite-scroll'
]);

//states config
app.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider
        .state("home", {
            url: "/home/",
            controller: "homeController",
            templateUrl: "/app/views/home/home.html",
            abstract: true,
            resolve: {
                loadCtrl: function ($ocLazyLoad) {
                    return $ocLazyLoad.load(
                        {
                            name: "InoDrive",
                            files: ["app/controllers/home/homeController.js"]
                        }
                    );
                }
            }
        })
            .state("home.greeting", {
                url: "",
                controller: "homeController",
                templateUrl: "/app/views/home/home.greeting.html",
                resolve: {
                    loadCtrl: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/home/greetingController.js"]
                            }
                        );
                    }
                }
            })
            .state("home.signin", {
                url: "signin/",
                controller: "signInController",
                templateUrl: "/app/views/home/home.signin.html",
                resolve: {
                    loadCtrl: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/home/signInController.js"]
                            }
                        );
                    }
                }
            })
            .state("home.signup", {
                url: "signup/",
                controller: "signUpController",
                templateUrl: "/app/views/home/home.signup.html",
                resolve: {
                    loadCtrl: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/home/signUpController.js"]
                            }
                        );
                    }
                }
            })
            .state("home.reset_password", {
                url: "reset_password?userId&code",
                controller: "resetPasswordController",
                templateUrl: "/app/views/home/home.reset_password.html",
                resolve: {
                    loadCtrl: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/home/resetPasswordController.js"]
                            }
                        );
                    }
                }
            })
            .state("home.confirm_email", {
                templateUrl: "/app/views/home/home.confirm_email.html",
                controller: "confirmEmailController",
                url: "confirm_email?userId&code",
                resolve: {
                    loadCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {

                        return $ocLazyLoad.load({
                            name: 'InoDrive',
                            files: ['app/controllers/home/confirmEmailController.js']
                        });

                    }]
                }
            })    
            .state("home.send_code", {
                url: "send_code/",
                controller: "sendCodeController",
                templateUrl: "/app/views/home/home.send_code.html",
                resolve: {
                    loadCtrl: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/home/sendCodeController.js"]
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
                loadCtrl: function ($ocLazyLoad) {
                    return $ocLazyLoad.load(
                        {
                            name: "InoDrive",
                            files: ["app/controllers/user/userController.js"]
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
                    loadCtrl: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/user/userViewController.js"]
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
                    loadCtrl: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/user/createTripController.js"]
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
                    loadCtrl: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/user/findController.js"]
                            }
                        );
                    }
                }
            })
            .state("user.my_bids", {
                url: "my_bids/",
                templateUrl: "/app/views/user/user.my_bids.html",
                controller: "bidsController",
                resolve: {
                    loadCtrl: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                            {
                                name: "InoDrive",
                                files: ["app/controllers/user/bidsController.js"]
                            }
                        );
                    }
                },
                abstract: true
            })
                .state("user.my_bids.sended", {
                    url: "",
                    templateUrl: "/app/views/user/user.my_bids.sended.html",
                    controller: "sendedBidsController",
                    resolve: {
                        loadCtrl: function ($ocLazyLoad) {
                            return $ocLazyLoad.load(
                                {
                                    name: "InoDrive",
                                    files: ["app/controllers/user/sendedBidsController.js"]
                                }
                            );
                        }
                    }
                })
                .state("user.my_bids.received", {
                    url: "received/",
                    templateUrl: "/app/views/user/user.my_bids.received.html",
                    controller: "receivedBidsController",
                    resolve: {
                        loadCtrl: function ($ocLazyLoad) {
                            return $ocLazyLoad.load(
                                {
                                    name: "InoDrive",
                                    files: ["app/controllers/user/receivedBidsController.js"]
                                }
                            );
                        }
                    }
                })
            .state("user.settings", {
                url: "settings/",
                templateUrl: "/app/views/settings/user.settings.html",
                controller: "settingsController",
                abstract: true,
                resolve: {
                    loadCtrl: function ($ocLazyLoad) {
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
                        loadCtrl: function ($ocLazyLoad) {
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

    $urlRouterProvider.otherwise("/home/");

});

//other configs must be here
app.config(function ($httpProvider, $datepickerProvider, laddaProvider, cfpLoadingBarProvider) {

    $httpProvider.defaults.timeout = 500;

    angular.extend($datepickerProvider.defaults, {
        //dateFormat: 'dd/MM/yyyy',
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
app.run(function ($rootScope, $state, notify, authService, customStorageService, localStorageService) {

    authService.fillAuthorizationData();

    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

        var notifyToShow = customStorageService.get("notifyToShow");
        if (notifyToShow) {
            displayNotificationOnStageChange(notify, notifyToShow.message, notifyToShow.type);
            customStorageService.remove("notifyToShow");
        }
        
        if (toState.name.indexOf('home') >= 0) {
            var authorizationData = localStorageService.get("authorizationData");
            if (authorizationData) {
                debugger;
                event.preventDefault();
                $state.go('user.view', null, { reload: true });
            }
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