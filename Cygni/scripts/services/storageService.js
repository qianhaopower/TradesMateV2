'use strict';
app.factory('storageService', ['$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var storageServiceFactory = {};

    var _uploadFile = function (imageFile, entityId, type) {

        //return $http.put(serviceBase + 'api/companies/ModifyCompany' , companyInfo).then(function (response) {
        //    return response;
        //});
      
        let uploadUrl = serviceBase + 'api/storage/PostBlobUpload?entityId=' + entityId + '&type=' + type;

        let formData = new FormData();
        //Take the first selected file
        formData.append("file", imageFile);
      
        var deferred = $q.defer();
        $http.post(uploadUrl, formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }
        ).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };


    var _deleteFile = function (entityId, entityType, attachmentId) {

        let deleteUrl = serviceBase + 'api/storage/DeleteBlob?entityId=' + entityId + '&entityType=' + entityType + '&attachmentId=' + attachmentId;
        var deferred = $q.defer();

        $http.delete(deleteUrl).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _downloadFile = function ( entityId, type, attachmentId) {

        let downUrl = serviceBase + 'api/storage/GetBlobDownload?entityId=' + entityId + '&type=' + type + '&attachmentId=' + attachmentId;
        window.open(downUrl);//fire the download
    };

    var _downloadAttachmentForEntity = function (entityId, entityType) {

        let getUrl = serviceBase + 'api/storage/GetBlobModels?entityId=' + entityId + '&entityType=' + entityType ;
        var deferred = $q.defer();

        $http.get(getUrl).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
  
    storageServiceFactory.uploadFile = _uploadFile;
    storageServiceFactory.downloadFile = _downloadFile;
    storageServiceFactory.downloadAttachmentForEntity = _downloadAttachmentForEntity;
    storageServiceFactory.deleteFile = _deleteFile;

    return storageServiceFactory;
}]);