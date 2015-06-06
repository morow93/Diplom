'use strict';
app.factory('tripsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var tripsServiceFactory = {};

    var test = function (params) {

        var promise = $http.post(serviceBase + 'api/trips/test', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    tripsServiceFactory.test = test;

    return tripsServiceFactory;

}]);