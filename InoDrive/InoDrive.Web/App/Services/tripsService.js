'use strict';
app.factory('tripsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var tripsServiceFactory = {};

    var getCar = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/getCar', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    tripsServiceFactory.getCar = getCar;

    return tripsServiceFactory;

}]);