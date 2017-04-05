'use strict';
app.factory('storageService', ['$http', '$q', 'ngAuthSettings', 'localStorageService', function ($http, $q, ngAuthSettings, localStorageService) {

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
        ).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };


    var _deleteFile = function (entityId, entityType, attachmentId) {

        let deleteUrl = serviceBase + 'api/storage/DeleteBlob?entityId=' + entityId + '&entityType=' + entityType + '&attachmentId=' + attachmentId;
        var deferred = $q.defer();

        $http.delete(deleteUrl).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var downLoadFromUrl = function (url) {

        var authData = localStorageService.get('authorizationData');

        var headers = {};
        headers.Authorization = 'Bearer ' + authData.token;
        var deferred = $q.defer();

        $http.get(url, { responseType: 'arraybuffer', headers: headers })
            .then(function (data) {
                var file = new Blob([data.data], { type: data.headers()['content-type'] });

                var disposition = data.headers()['content-disposition'];
                var fileName = undefined;
                if (disposition && disposition.indexOf('attachment') !== -1) {
                    var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                    var matches = filenameRegex.exec(disposition);
                    if (matches != null && matches[1]) {
                        fileName = matches[1].replace(/['"]/g, '');
                    }
                }
                if (fileName)
                saveAs(file, fileName);
            },function (data) {
                console.log("Request failed with status: " + data.status);
            });;
        return deferred.promise;
    }

    var getFileNameFromContentDisposition = function (contentDisposition) {
        return "test";

    }


    var _downloadFile = function ( entityId, type, attachmentId) {

        let downUrl = serviceBase + 'api/storage/GetBlobDownload?entityId=' + entityId + '&type=' + type + '&attachmentId=' + attachmentId;

        downLoadFromUrl(downUrl);
      
    };

    var _downloadAttachmentForEntity = function (entityId, entityType) {

        let getUrl = serviceBase + 'api/storage/GetBlobModels?entityId=' + entityId + '&entityType=' + entityType ;
        var deferred = $q.defer();

        $http.get(getUrl).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };



   

    //var getPDF2 = function (apiUrl) {
    //    var promise = $http({
    //        method: 'GET',
    //        url: hostApiUrl + apiUrl,
    //        headers: { 'Authorization': 'Bearer ' + sessionStorage.tokenKey },
    //        responseType: 'arraybuffer'
    //    });
    //    promise.then(function (data) {
    //        return data;
    //    },function (data, status) {
    //        console.log("Request failed with status: " + status);
    //    });
    //    return promise;
    //}
  
    storageServiceFactory.uploadFile = _uploadFile;
    storageServiceFactory.downloadFile = _downloadFile;
    storageServiceFactory.downloadAttachmentForEntity = _downloadAttachmentForEntity;
    storageServiceFactory.deleteFile = _deleteFile;

    return storageServiceFactory;
}]);