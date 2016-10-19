'use strict';


angular.module('sbAdminApp').controller('companyMemberController', ['$scope', '$location', '$timeout', '$state', 'Notification', 'companyService',
    function ($scope, $location, $timeout, $state, Notification, companyService) {

        $scope.editMode = false;

      


        $scope.filterTextModel = {
            searchText: undefined,
        };

        $scope.search = function (item) {
            if (!$scope.filterTextModel.searchText
                || (item.firstName && (item.firstName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
                || (item.lastName && (item.lastName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
                ) {
                return true;
            }
            return false;
        };

        $scope.memberList = [];
    
        $scope.companyInfo = {
            companyName: "Company",
            description: undefined,
            creditCard: undefined,
           // companyId: undefined,

        };
        $scope.openMemberDetail = function (member) {
            $state.go("base.viewCompanyMember", {
                memberId: member.memberId
            });
           
        }


        $scope.addNewMember = function () {
            
            $state.go("base.createCompanyMember");

        }

        

        $scope.deleteMember = function (member) {

            companyService.deleteMemberById(member.memberId).then(function () {
                Notification.success({ message: 'Deleted', delay: 2000 });
                getMembersInCompany();



            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }



        //$scope.editMemberDetail = function (member) {

        //    $state.go("base.editCompanyMember", {
        //        memberId: member.memberId
        //    });

        //}
   

    var getMembersInCompany = function () {
        companyService.getCompanyMembers().then(function (members) {

            $scope.memberList = members;
        },function (error) { Notification.error({ message: error, delay: 2000 }); });
    }
    var getCompanyDetail = function () {
        companyService.getCurrentCompany().then(function (company) {
            $scope.companyInfo.companyName = company.companyName;
            $scope.companyInfo.description = company.description;
            $scope.companyInfo.creditCard = company.creditCard;
            //$scope.companyInfo.companyId = company.companyId;

        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    // get the company info for display
    getCompanyDetail();
        
    //get member list for the current company
    getMembersInCompany();


    }

]);