'use strict';
app.factory('addressService', ['$http', '$q', function ($http, $q) {

    //https://maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&key=YOUR_API_KEY

    var addressServiceFactory = {};
    var geocoder = new google.maps.Geocoder();
    var address = 'London, UK';


    var _findAddress = function (address) {
        var deferred = $q.defer();
        var encoded = encodeURIComponent(address);
        if (geocoder) {
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    console.log(results[0].geometry.location);
                    deferred.resolve(results);
                }
                else {
                    console.log("Geocoding failed: " + status);
                    deferred.reject(status);
                }
            });
        }
        
        return deferred.promise;



        ////to do, need save the key in server. or make server call this.
        //var request = 'https://maps.googleapis.com/maps/api/geocode/json?address=' + encoded + '&key=' + 'AIzaSyCawP-E904PjDMbT0J1MCHFizmndQ8WbrA';
        ////var headers = {
        ////    headers: {
        ////        'Access-Control-Allow-Origin': '*',
        ////        'Access-Control-Allow-Credentials': 'true',
        ////        'Access-Control-Allow-Methods': 'GET,HEAD,OPTIONS,POST,PUT',
        ////        'Access-Control-Allow-Headers': 'Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers',

        ////    }
        ////}
        //var req = {
        //    method: 'GET',
        //    url: request,
        //    headers: {
        //        'Access-Control-Allow-Origin': '*',
        //    },
            
        //}
        //var config = {
        //    headers: {
        //        'Access-Control-Allow-Origin': '*',
        //        //'Accept': 'application/json;odata=verbose',
        //        //"X-Testing": "testing"
        //    }
        //};
        //return $http(req).then(function (results) {
        //    return results;
        //});

        ////$http.get('request', {
        ////    headers: {
        ////        'Access-Control-Allow-Origin': '*',
        ////        'Access-Control-Allow-Credentials': 'true',
        ////        'Access-Control-Allow-Methods': 'GET,HEAD,OPTIONS,POST,PUT',
        ////        'Access-Control-Allow-Headers': 'Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers',
               
        ////    }
        ////});
    };

    addressServiceFactory.findAddress = _findAddress;

    return addressServiceFactory;

}]);