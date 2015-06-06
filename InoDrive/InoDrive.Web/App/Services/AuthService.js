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
            userId: "",
            userName: "",
            initials: "",
            useRefreshTokens: false
        }; 

        var signOut = function () {

            var deferred = $q.defer();

            var authorizationData = localStorageService.get("authorizationData");
            if (authorizationData) {

                if (authorizationData.useRefreshTokens) {

                    var params = {
                        userName: authorizationData.userName,
                        refreshToken: authorizationData.refreshToken
                    };

                    $http = $http || $injector.get("$http");
                    $http.post(serviceBase + "api/account/removeRefreshToken", params).then(function (response) {

                        removeAuthorizationData();

                    }).finally(function (error) {

                        deferred.resolve();
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
                            useRefreshTokens: true,
                            userId: response.userId,
                            initials: response.initials
                        });
                    }
                    else {
                        localStorageService.set("authorizationData", {
                            token: response.access_token,
                            userName: params.userName,
                            refreshToken: "",
                            useRefreshTokens: false,
                            userId: response.userId,
                            initials: response.initials
                        });
                    }
        
                    authentication.isAuth = true;
                    authentication.userName = params.userName;
                    authentication.useRefreshTokens = params.useRefreshTokens;
                    authentication.userId = response.userId;
                    authentication.initials = response.initials;
                    
                    deferred.resolve(response);

                }).catch(function (error) {

                    deferred.reject(error.data);

                });

            }).catch(function (error) {

                deferred.reject(error);

            });

            return deferred.promise;
        };

        var fillAuthorizationData = function () {

            var authorizationData = localStorageService.get('authorizationData');

            if (authorizationData) {
                authentication.isAuth               = true;
                authentication.userName             = authorizationData.userName;
                authentication.useRefreshTokens     = authorizationData.useRefreshTokens;
                authentication.initials             = authorizationData.initials;
                authentication.userId               = authorizationData.userId;
            }
        };

        var refreshToken = function () {

            var deferred = $q.defer();

            var authorizationData = localStorageService.get("authorizationData");

            if (authorizationData && authorizationData.useRefreshTokens) {

                var data =
                    "grant_type=refresh_token" +
                    "&refresh_token=" + authorizationData.refreshToken +
                    "&client_id=" + ngAuthSettings.clientId;

                localStorageService.remove("authorizationData");

                $http = $http || $injector.get("$http");
                $http.post(serviceBase + "token", data, { headers: { "Content-Type": "application/x-www-form-urlencoded" } }).success(function (response) {

                    localStorageService.set("authorizationData", {
                        token: response.access_token,
                        userName: response.userName,
                        refreshToken: response.refresh_token,
                        useRefreshTokens: true,
                        userId: response.userId,
                        initials: response.initials
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

        var changePassword = function (params) {

            $http = $http || $injector.get('$http');
            return $http.post(serviceBase + 'api/account/ChangePassword', params).then(function (results) {
                return results;
            });
        };

        var changeEmail = function (params) {

            $http = $http || $injector.get('$http');
            return $http.post(serviceBase + 'api/account/ChangeEmail', params).then(function (results) {
                return results;
            });
        };

        var setAuthorizationData = function (key, value) {

            var authorizationData = localStorageService.get('authorizationData');
            authorizationData[key] = value;

            localStorageService.remove('authorizationData');
            localStorageService.set('authorizationData', authorizationData);
        };

        var getAuthorizationData = function () {

            return localStorageService.get('authorizationData');
        };

        var removeAuthorizationData = function () {

            localStorageService.remove("authorizationData");

            authentication.isAuth = false;
            authentication.userId = "";
            authentication.userName = "";
            authentication.initials = "";
            authentication.useRefreshTokens = false;
        };
                
        authServiceFactory.signUp                   = signUp;
        authServiceFactory.signIn                   = signIn;
        authServiceFactory.signOut                  = signOut;
        authServiceFactory.fillAuthorizationData    = fillAuthorizationData;
        authServiceFactory.refreshToken             = refreshToken;
        authServiceFactory.sendConfirmEmailCode     = sendConfirmEmailCode;
        authServiceFactory.confirmEmail             = confirmEmail;
        authServiceFactory.sendResetPasswordCode    = sendResetPasswordCode;
        authServiceFactory.resetPassword            = resetPassword;
        authServiceFactory.changePassword           = changePassword;
        authServiceFactory.changeEmail              = changeEmail;
        authServiceFactory.setAuthorizationData     = setAuthorizationData;
        authServiceFactory.getAuthorizationData     = getAuthorizationData;
        authServiceFactory.removeAuthorizationData  = removeAuthorizationData;

        authServiceFactory.authentication           = authentication;

        return authServiceFactory;

    }]);