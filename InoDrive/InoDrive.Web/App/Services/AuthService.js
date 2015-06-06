"use strict";
app.factory("authService", [
    "$q",
    "$injector",
    "localStorageService",
    "ngAuthSettings",
    function (
        $q,
        $injector,
        localStorageService,
        ngAuthSettings) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var authServiceFactory = {};
        var $http;

        var authentication = {
            isAuth: false,
            userName: "",
            useRefreshTokens: false
        };

        var signOut = function () {

            function removeAuthorizationData() {

                localStorageService.remove("authorizationData");
                authentication.isAuth = false;
                authentication.userName = "";
                authentication.useRefreshTokens = false;
            }

            var deferred = $q.defer();

            var authData = localStorageService.get("authorizationData");
            if (authData) {

                if (authData.useRefreshTokens) {

                    var params = {
                        userName: authData.userName,
                        refreshToken: authData.refreshToken
                    };

                    $http = $http || $injector.get("$http");
                    $http.post(serviceBase + "api/account/removeRefreshToken", params).then(function (response) {

                        removeAuthorizationData();
                        deferred.resolve();

                    }).catch(function (error) {

                        deferred.reject(error);

                    });

                } else {

                    removeAuthorizationData();
                    deferred.resolve();
                }

            } else {
                deferred.resolve();
            }
            return deferred.promise;
        };

        var signUp = function (params) {

            var deferred = $q.defer();

            signOut().then(function () {

                $http = $http || $injector.get("$http");
                $http.post(serviceBase + "api/account/register", params).then(function (response) {

                    deferred.resolve(response);

                }).catch(function (error) {

                    deferred.reject(error);
                });

            }).catch(function (error) {

                deferred.reject(error);
            });

            return deferred.promise;
        };

        var signIn = function (params) {

            var deferred = $q.defer();

            signOut().then(function () {

                var data =
                "grant_type=password" +
                "&username=" + params.userName +
                "&password=" + params.password;

                if (params.useRefreshTokens) {
                    data = data + "&client_id=" + ngAuthSettings.clientId;
                }

                $http = $http || $injector.get("$http");
                $http.post(serviceBase + "token", data, { headers: { "Content-Type": "application/x-www-form-urlencoded" } }).success(function (response) {

                    if (params.useRefreshTokens) {
                        localStorageService.set("authorizationData", {
                            token: response.access_token,
                            userName: params.userName,
                            refreshToken: response.refresh_token,
                            useRefreshTokens: true
                        });
                    }
                    else {
                        localStorageService.set("authorizationData", {
                            token: response.access_token,
                            userName: params.userName,
                            refreshToken: "",
                            useRefreshTokens: false
                        });
                    }

                    authentication.isAuth = true;
                    authentication.userName = params.userName;
                    authentication.useRefreshTokens = params.useRefreshTokens;

                    deferred.resolve(response);

                }).catch(function (error) {

                    deferred.reject(error.data);

                });

            }).catch(function (error) {

                deferred.reject(error);

            });

            return deferred.promise;
        };

        var fillAuthData = function () {

            var authData = localStorageService.get("authorizationData");
            if (authData) {
                authentication.isAuth = true;
                authentication.userName = authData.userName;
                authentication.useRefreshTokens = authData.useRefreshTokens;
            }
        };

        var refreshToken = function () {

            var deferred = $q.defer();

            var authData = localStorageService.get("authorizationData");

            if (authData && authData.useRefreshTokens) {

                var data =
                    "grant_type=refresh_token" +
                    "&refresh_token=" + authData.refreshToken +
                    "&client_id=" + ngAuthSettings.clientId;

                localStorageService.remove("authorizationData");

                $http = $http || $injector.get("$http");
                $http.post(serviceBase + "token", data, { headers: { "Content-Type": "application/x-www-form-urlencoded" } }).success(function (response) {

                    localStorageService.set("authorizationData", {
                        token: response.access_token,
                        userName: response.userName,
                        refreshToken: response.refresh_token,
                        useRefreshTokens: true
                    });

                    deferred.resolve(response);

                }).error(function (err) {

                    signOut().then(function () {

                        deferred.reject(err);
                    }).catch(function (errorIn) {

                        deferred.reject(errorIn);
                    });

                });

            } else {
                deferred.reject();
            }

            return deferred.promise;
        };

        var sendConfirmEmailCode = function (userData) {

            $http = $http || $injector.get('$http');
            return $http.post(serviceBase + 'api/Account/SendConfirmEmailCode', userData).then(function (response) {
                return response;
            });

        };

        var confirmEmail = function (params) {

            $http = $http || $injector.get('$http');
            return $http.post(serviceBase + 'api/account/ConfirmEmail', params).then(function (results) {
                return results;
            });
        };

        var sendResetPasswordCode = function (userData) {

            $http = $http || $injector.get('$http');
            return $http.post(serviceBase + 'api/Account/SendResetPasswordCode', userData).then(function (response) {
                return response;
            });

        };

        var resetPassword = function (params) {

            $http = $http || $injector.get('$http');
            return $http.post(serviceBase + 'api/account/ResetPassword', params).then(function (results) {
                return results;
            });
        };

        authServiceFactory.signUp                   = signUp;
        authServiceFactory.signIn                   = signIn;
        authServiceFactory.signOut                  = signOut;
        authServiceFactory.fillAuthData             = fillAuthData;
        authServiceFactory.refreshToken             = refreshToken;
        authServiceFactory.sendConfirmEmailCode     = sendConfirmEmailCode;
        authServiceFactory.confirmEmail             = confirmEmail;
        authServiceFactory.sendResetPasswordCode    = sendResetPasswordCode;
        authServiceFactory.resetPassword            = resetPassword;

        authServiceFactory.authentication = authentication;

        return authServiceFactory;
    }]);