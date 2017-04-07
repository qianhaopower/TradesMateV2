'use strict';
app.factory('workItemTemplateService', ['$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var templateServiceFactory = {};
    
    var _deleteTemplate = function (templateId) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/WorkItemTemplates/DeleteWorkItemTemplate?templateId=' + templateId).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    
    var _getAllTemplates = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/WorkItemTemplates/GetWorkItemTemplates').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }


    var _createTemplate = function (templateModel) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/WorkItemTemplates/CreateWorkItemTemplate', templateModel).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }
    var _editTemplate = function (templateId, templateModel) {
        var deferred = $q.defer();
        $http.patch(serviceBase + 'api/WorkItemTemplates/UpdateWorkItemTemplate?templateId=' + templateId, templateModel).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    }
    var _getTemplateById = function (templateId) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/WorkItemTemplates/GetWorkItemTemplateById?templateId=' + templateId).then(function (response) {
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