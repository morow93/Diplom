'use strict';
app.factory('mapService', ['$http', function ($http) {

    var mapServiceFactory = {};

    var getCities = function () {

        var promise = $http.get('sources/cities.json').then(function (response) {            
            return response.data;
        });
        return promise;

    };
    
    mapServiceFactory.getCities = getCities;

    return mapServiceFactory;

}]);