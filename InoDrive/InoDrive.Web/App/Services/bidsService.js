'use strict';
app.factory('bidsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var bidsServiceFactory = {};

    //Section of requests for updating counters, bids

    var _getCountOfOwnBids = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/GetCountOfOwnBids', params, { ignoreLoadingBar: true }).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var _getCountOfAssignedBids = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/GetCountOfAssignedBids', params, { ignoreLoadingBar: true }).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var _getUpdatedOwnBids = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/GetUpdatedOwnBids', params, { ignoreLoadingBar: true }).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var _getUpdatedAssignedBids = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/GetUpdatedAssignedBids', params, { ignoreLoadingBar: true }).then(function (response) {
            return response.data;
        });
        return promise;
    };

    //Section of main requests for select bids

    var _getMyBids = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/GetMyBids', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var _getBidsForMyTrips = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/GetBidsForMyTrips', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    //Add or update some bids entities

    var _addBid = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/AddBid', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var _acceptBid = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/AcceptBid', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var _rejectBid = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/RejectBid', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    var _watchBid = function (params) {

        var promise = $http.post(serviceBase + 'api/Bids/WatchBid', params).then(function (response) {
            return response.data;
        });
        return promise;
    };

    //initialize and return

    bidsServiceFactory.getCountOfOwnBids = _getCountOfOwnBids;
    bidsServiceFactory.getCountOfAssignedBids = _getCountOfAssignedBids;
    bidsServiceFactory.getUpdatedOwnBids = _getUpdatedOwnBids;
    bidsServiceFactory.getUpdatedAssignedBids = _getUpdatedAssignedBids;

    bidsServiceFactory.getMyBids = _getMyBids;
    bidsServiceFactory.getBidsForMyTrips = _getBidsForMyTrips;

    bidsServiceFactory.addBid = _addBid
    bidsServiceFactory.acceptBid = _acceptBid;
    bidsServiceFactory.rejectBid = _rejectBid;
    bidsServiceFactory.watchBid = _watchBid;

    return bidsServiceFactory;

}]);