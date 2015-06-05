"use strict";
app.factory("authInterceptorService", [
    "$q",
    "$injector",
    "localStorageService",
    function (
        $q,
        $injector,
        localStorageService) {

        var authInterceptorServiceFactory = {};

        var $http;
        var $state;
        var authService;

        var request = function (config) {

            config.headers = config.headers || {};

            var authData = localStorageService.get("authorizationData");
            if (authData) {
                config.headers.Authorization = "Bearer " + authData.token;
            }

            return config;
        }

        var retryHttpRequest = function (config, deferred) {

            $http = $http || $injector.get("$http");
            $http(config).then(function (response) {
                deferred.resolve(response);
            }, function (response) {
                deferred.reject(response);
            });
            //if you will use not http provider in services then need change logic
        }

        var responseError = function (rejection) {
            var deferred = $q.defer();
            if (rejection.status === 401) {
                authService = authService || $injector.get("authService");
                authService.refreshToken().then(function (response) {
                    retryHttpRequest(rejection.config, deferred);
                }, function () {
                    debugger;
                    authService.signOut().then(function () {

                        $state = $state || $injector.get("$state");
                        $state.go("signin", null, { reload: true });
                        deferred.resolve(rejection);

                    }).catch(function () {
                        deferred.reject(rejection);
                    });

                });
            } else {
                deferred.reject(rejection);
            }
            return deferred.promise;
        }

        authInterceptorServiceFactory.request = request;
        authInterceptorServiceFactory.responseError = responseError;

        return authInterceptorServiceFactory;
    }]);