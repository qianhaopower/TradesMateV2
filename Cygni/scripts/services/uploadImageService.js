'use strict';
app.factory('uploadImageService', ['$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var uploadIamgeServiceFactory = {};

    var _uploadImage = function (imageFile, entityId, type) {

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

    var _downloadFile = function ( entityId, type, attachmentId) {

        let downUrl = serviceBase + 'api/storage/GetBlobDownload?entityId=' + entityId + '&type=' + type + '&attachmentId=' + attachmentId;
        window.open(downUrl);//fire the download
  

        //var deferred = $q.defer();
        //$http.get(downUrl).success(function (response) {
        //    deferred.resolve(response);
        //}).error(function (err, status) {
        //    deferred.reject(err);
        //});
        //return deferred.promise;

    };
  
    uploadIamgeServiceFactory.uploadImage = _uploadImage;
    uploadIamgeServiceFactory.downloadFile = _downloadFile;

    return uploadIamgeServiceFactory;
}]);