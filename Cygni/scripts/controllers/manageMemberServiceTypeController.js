'use strict';

angular.module('sbAdminApp')
  .controller('manageMemberServiceTypeController', ['$scope', 'companyService', 'authService',
      'Notification', '$state', '$stateParams',
function ($scope, companyService, authService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.memberId = undefined;
    $scope.member = undefined;
    $scope.companyDetail = undefined;
  
    $scope.serviceTypes = companyService.getDefaultServices();
   
   
    $scope.goBack = function () {
        $state.go('base.companyMember');
    }
    $scope.updateServiceType = function (serviceType) {
        if ($scope.memberId) {


            var selectedServiceTypes = _.filter($scope.serviceTypes, function (item) { return item.ticked; });
            selectedServiceTypes = _.pluck(selectedServiceTypes, 'enumValue');
            companyService.updateMemberServiceTypes($scope.memberId, selectedServiceTypes).then(function (result) {
                Notification.success({ message: "Updated", delay: 2000 });
                init();
            }, function (error) {
                Notification.error({ message: error.message, delay: 2000 });
            });
        }
    }
    var getCompanyDetail = function () {
        companyService.getCurrentCompany().then(function (company) {
            $scope.companyDetail = company;
            angular.forEach($scope.serviceTypes, function (value, key) {
               
                if (company.tradeTypes.indexOf(value.enumValue) > -1) {
                    value.visible = true;
                } else {
                    value.visible = false;
                }
            });

            $scope.memberId = $stateParams.memberId;
            if ($scope.memberId) {
                companyService.getMemberById($scope.memberId).then(function (result) {
                    $scope.member = result;

                    angular.forEach($scope.serviceTypes, function (value, key) {

                        if ($scope.member.allowedTradeTypes.indexOf(value.enumValue) > -1) {//AllowedTradeTypes
                            value.ticked = true;
                        } else {
                            value.ticked = false;
                        }
                    });



                }, function (error) {
                    Notification.error({ message: error, delay: 2000 });
                });
            } 



        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }

    var init = function ()
    {
        getCompanyDetail();
       
    }

    init();

}]);
