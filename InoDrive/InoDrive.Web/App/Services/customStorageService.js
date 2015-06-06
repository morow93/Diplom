'use strict';
angular.module('InoDrive').factory('customStorageService', function () {

    var shareData = {};

    var get = function (key) {
        return shareData[key];
    };

    var set = function (key, value) {
        shareData[key] = value;
    };

    var remove = function (key) {
        shareData[key] = null;
    };
    
    var customStorageServiceFactory = {};

    customStorageServiceFactory.get     = get;
    customStorageServiceFactory.set     = set;
    customStorageServiceFactory.remove  = remove;

    return customStorageServiceFactory;

});