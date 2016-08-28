'use strict';

angular.module('sbAdminApp')
  .controller('propertyDetailController', ['$scope', 'propertyDataService', 'clientDataService',
      'Notification', '$state', '$stateParams',
function ($scope, propertyDataService, clientDataService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.propertyId = undefined;
    $scope.property = undefined;
    $scope.clientId = undefined;
    $scope.client = undefined;

    $scope.readOnly = $state.current.name == 'base.viewProperty';

    

    $scope.save = function () {
        $scope.property.address = undefined;

        if ($scope.property.isNew) {
            var propertyCopy = angular.copy($scope.property);
            propertyCopy.clientId = $stateParams.clientId;
            propertyDataService.createProperty(propertyCopy).then(function (result) {
                Notification.success({ message: 'Created', delay: 2000 });
                $scope.goToPropertyIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else {
            propertyDataService.editProperty($scope.property).then(function (result) {
                Notification.success({ message: 'Saved', delay: 2000 });
                $scope.goToPropertyIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }
    
    }
    $scope.goToPropertyIndex = function () {
        if ($scope.property.clientId)
            $state.go('base.clientProperties', { param: $scope.property.clientId });
        else
            $state.go('base.properties');
    };


    $scope.myInterval = 500000;
    var slides = $scope.slides = [];
    $scope.addSlide = function () {
        var newWidth = 600 + slides.length + 1;
        slides.push({
            image: 'http://placekitten.com/' + newWidth + '/300',
            text: ['More', 'Extra', 'Lots of', 'Surplus'][slides.length % 4] + ' ' +
              ['Cats', 'Kittys', 'Felines', 'Cutes'][slides.length % 4]
        });
    };
    for (var i = 0; i < 2; i++) {
       // $scope.addSlide();
    }
    //$scope.previewFile = function() {
    //    var preview = document.querySelector('img');
    //    var file = document.querySelector('input[type=file]').files[0];
    //    var reader = new FileReader();

    //    reader.addEventListener("load", function () {
           
    //        $scope.slides.push({ image: reader.result });
    //        $scope.$apply();
    //    }, false);

    //    if (file) {
    //        reader.readAsDataURL(file);
    //    }
    //}

    $scope.uploadFile = function () {
        var file = event.target.files[0];
      
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            $scope.slides.push({ image: reader.result });
            $scope.$apply();
        }, false);

        if (file) {
            reader.readAsDataURL(file);
        }
    };


    var init = function () {

        if ($state.current.name == 'base.createProperty') {
            $scope.clientId = $stateParams.clientId;
        } else {
            $scope.propertyId = $stateParams.propertyId;
        }
       
        if ($state.current.name == 'base.editProperty' || $state.current.name == 'base.viewProperty') {
            propertyDataService.getPropertyById($scope.propertyId).then(function (result) {
                $scope.property = result;
                $scope.clientId = result.clientId;

                if ($scope.clientId) {
                    clientDataService.getClientById($scope.clientId).then(function (result) {
                        $scope.client = result

                    }, function (error) { Notification.error({ message: error, delay: 2000 }); });
                }

            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else if ($state.current.name == 'base.createProperty') {
            //create new property
            $scope.property = {
                condition:'Normal',//default
                isNew: true,
            }
        }
        //propertyDataService.getAllPropertys().then(function (result) {
        //    $scope.propertylist = result;
        //}, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);
