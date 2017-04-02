'use strict';
app.factory('uploadImageService', ['$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var uploadIamgeServiceFactory = {};

    var _uploadImage = function (imageFile, entityId) {

        //return $http.put(serviceBase + 'api/companies/ModifyCompany' , companyInfo).then(function (response) {
        //    return response;
        //});
        var type = 'Property';
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
  
    uploadIamgeServiceFactory.uploadImage = _uploadImage;

    return uploadIamgeServiceFactory;
}]);