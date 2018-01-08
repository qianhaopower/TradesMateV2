'use strict';
app.factory('workItemTemplateService', ['$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var templateServiceFactory = {};
    
    var _deleteTemplate = function (templateId) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/workitemtemplates/' + templateId).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    
    var _getAllTemplates = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/workitemtemplates').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }


    var _createTemplate = function (templateModel) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/workitemtemplates', templateModel).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    var _editTemplate = function (templateId, templateModel) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/workitemtemplates', templateModel).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    }
    var _getTemplateById = function (templateId) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/WorkItemTemplates/' + templateId).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }


    templateServiceFactory.deleteTemplate = _deleteTemplate;
    
    templateServiceFactory.getAllTemplates = _getAllTemplates;
    
    templateServiceFactory.createTemplate = _createTemplate;
    
    templateServiceFactory.editTemplate = _editTemplate;
    
    templateServiceFactory.getTemplateById = _getTemplateById;


    return templateServiceFactory;
}]);