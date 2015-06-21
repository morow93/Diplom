'use strict';
app.factory('usersService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var userServiceFactory = {};

    var getUserProfile = function (params) {
        var promise = $http.post(serviceBase + 'api/user/getUserProfile', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var getUserSummary = function (params) {
        var promise = $http.post(serviceBase + 'api/user/getUserSummary', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var setUserProfile = function (params) {
        var promise = $http.post(serviceBase + 'api/user/setUserProfile', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var setUserCar = function (params) {
        var promise = $http.post(serviceBase + 'api/user/setUserCar', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    userServiceFactory.getUserSummary = getUserSummary;
    userServiceFactory.getUserProfile = getUserProfile;
    userServiceFactory.setUserProfile = setUserProfile;
    userServiceFactory.setUserCar = setUserCar;

    return userServiceFactory;

}]);