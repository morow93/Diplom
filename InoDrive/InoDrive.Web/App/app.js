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
    'cgNotify'
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
        controller: "signinController",
        templateUrl: "/app/views/signin.html",
        resolve: {
            store: function ($ocLazyLoad) {
                return $ocLazyLoad.load(
                    {
                        name: "InoDrive",
                        files: ["app/controllers/signinController.js"]
                    }
                );
            }
        }
    });

    $stateProvider.state("signup", {
        url: "/signup/",
        controller: "signupController",
        templateUrl: "/app/views/signup.html",
        resolve: {
            store: function ($ocLazyLoad) {
                return $ocLazyLoad.load(
                    {
                        name: "InoDrive",
                        files: ["app/controllers/signupController.js"]
                    }
                );
            }
        }
    });
    
    $stateProvider.state("send_code", {
        url: "/send_code/",
        controller: "sendCodeController",
        templateUrl: "/app/views/send_code.html",
        resolve: {
            store: function ($ocLazyLoad) {
                return $ocLazyLoad.load(
                    {
                        name: "InoDrive",
                        files: ["app/controllers/sendCodeController.js"]
                    }
                );
            }
        }
    });

    $stateProvider.state("reset_password", {
        url: "/reset_password/?userId&code",
        controller: "resetPasswordController",
        templateUrl: "/app/views/reset_password.html",
        resolve: {
            store: function ($ocLazyLoad) {
                return $ocLazyLoad.load(
                    {
                        name: "InoDrive",
                        files: ["app/controllers/resetPasswordController.js"]
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
                templateUrl: "/app/views/user/user.view.html"
            })
            .state("user.my_trips", {
                url: "my_trips/",
                templateUrl: "/app/views/user/user.my_trips.html"
            })
            .state("user.create_trip", {
                url: "create_trip/",
                templateUrl: "/app/views/user/user.create_trip.html"
            })
            .state("user.find", {
                url: "find/",
                templateUrl: "/app/views/user/user.find.html"
            })
            .state("user.my_bids", {
                url: "my_bids/",
                templateUrl: "/app/views/user/user.my_bids.html"
            })
            .state("user.settings", {
                url: "settings/",
                templateUrl: "/app/views/user/user.settings.html"
            });

    $urlRouterProvider.otherwise("/greeting/");

});

//other configs must be here
app.config(function ($datepickerProvider) {

    angular.extend($datepickerProvider.defaults, {
        dateFormat: 'dd/MM/yyyy',
        startWeek: 1
    });

});

//run run run
app.run(function ($rootScope, notify) {
        
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

        displayNotificationOnStageChange(notify, "test", "success");     

    });

});

function displayNotificationOnStageChange(notify, message, type) {

    notify.config({
        startTop: 15,
        verticalSpacing: 15,
        maximumOpen: 5,
        duration: 5000
    });
    notify({
        type: type,
        message: message,
        templateUrl: '/app/templates/notify.html'
    });
}