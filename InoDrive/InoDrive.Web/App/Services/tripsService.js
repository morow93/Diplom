'use strict';
app.factory('tripsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var tripsServiceFactory = {};
        
    var getTripForEdit = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/getTripForEdit', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var getCar = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/getCar', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var getTrip = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/getTrip', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var findTrips = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/findTrips', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var getAllTrips = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/getAllTrips', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var getDriverTrips = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/getDriverTrips', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var getPassengerTrips = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/getPassengerTrips', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var removeTrip = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/removeTrip', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var recoverTrip = function (params) {
        var promise = $http.post(serviceBase + 'api/trips/recoverTrip', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    tripsServiceFactory.getTripForEdit = getTripForEdit;
    tripsServiceFactory.getCar = getCar;

    tripsServiceFactory.getTrip = getTrip;
    tripsServiceFactory.findTrips = findTrips;
    tripsServiceFactory.getAllTrips = getAllTrips;
    tripsServiceFactory.getDriverTrips = getDriverTrips;
    tripsServiceFactory.getPassengerTrips = getPassengerTrips;

    tripsServiceFactory.removeTrip = removeTrip;
    tripsServiceFactory.recoverTrip = recoverTrip;

    return tripsServiceFactory;

}]);