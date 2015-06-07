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

    var setUserProfile = function (params) {
        var promise = $http.post(serviceBase + 'api/user/setUserProfile', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    userServiceFactory.getUserProfile = getUserProfile;
    userServiceFactory.setUserProfile = setUserProfile;

    return userServiceFactory;

}]);