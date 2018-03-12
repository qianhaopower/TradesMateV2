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
        $scope.goBack = function () {
            $state.go('base.manageCompany');
        }
        $scope.roleOptions = ['Default', 'Contractor'];

        $scope.addNewMember = function () {
            
            $state.go("base.createCompanyMember");

        }
        $scope.manageMemberServiceType = function (member) {

            $state.go("base.manageMemberServiceType", {
                memberId: member.memberId
            });
        }


        $scope.allocate = function (member) {

            $state.go("base.allocateProperty",{
            memberId: member.memberId
        });
        }
        $scope.fireUpdateRole = function (member) {
            companyService.updateMemberRole(member.memberId, member.memberRole).then(function (reply) {
                if (reply) {
                    Notification.success({ message: reply, delay: 4000 });
                } else {
                    Notification.success({ message: 'Role updated', delay: 2000 });
                }
                
                getMembersInCompany();
            }, function (error) {
                Notification.error({ message: error.exceptionMessage, delay: 4000 });
                getMembersInCompany();
            });
        }
        

        $scope.deleteMember = function (member) {

            companyService.deleteMemberById(member.memberId).then(function () {
                Notification.success({ message: 'Deleted', delay: 2000 });
                getMembersInCompany();

            }, function (error) {
                Notification.error({ message: error.exceptionMessage ? error.exceptionMessage: error, delay: 2000 });
            });
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

    $scope.searchText = undefined;
    $scope.inputValid = false;

    $scope.$watch('selected', function (newVal, oldVal) {
        if (newVal)
            $scope.inputValid = $scope.selected.memberId || (typeof ($scope.selected) == 'string' && /\S+@\S+\.\S+/.test($scope.selected));

    });
   

    $scope.searchMember = function (search) {
       return companyService.searchMemberForJoinCompany(search).then(function (members) {
           for(var i = 0; i< members.length; i++){
               members[i].label = members[i].fullName + ' (' + members[i].email + ')';
           }
            return members;
            //$scope.searchResult = members;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }

    var getCompanyDetail = function () {
        companyService.getCurrentCompany().then(function (company) {
            $scope.companyInfo.companyName = company.companyName;
            $scope.companyInfo.description = company.description;
            $scope.companyInfo.creditCard = company.creditCard;
            //$scope.companyInfo.companyId = company.companyId;

        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    $scope.sendingRequestStatus = undefined;
   
    $scope.proceedAddMember = function () {
        if ($scope.selected.memberId) {
            $scope.sendingRequestStatus = 'existedMember';
            //we are adding existing member
           
        } else {
            // we are adding via a email

            $scope.newMember = {
                userName: undefined,
                firstName: undefined,
                lastName: undefined,
                email: $scope.selected,
                isContractor : true,
                userType:1,//trade
                

            };


            $scope.sendingRequestStatus = 'newMember';
            // we are adding new member
        }
    }
     
    $scope.sendExistedMember = function () {
        addExistingMemberToCompany();
       
    }
    $scope.sendNewMember = function () {
        addNewMemberToCompany();
    }


    $scope.cancel = function () {
        $scope.newMember = {};
        $scope.sendingRequestStatus = undefined;
    }
    

    var addExistingMemberToCompany = function () {
        if($scope.selected.memberId){
            var data = {memberId:$scope.selected.memberId, text:$scope.inviteText};
            companyService.addExistingMemberToCompany(data).then(function () {
                $scope.sendingRequestStatus = undefined;
                Notification.success({ message: 'Request sent', delay: 2000 });
                //getMembersInCompany();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });

        }else{
            Notification.error({ message: 'Please select a member to proceed', delay: 2000 }); 
        }

    }
    var addNewMemberToCompany = function () {
        if ($scope.selected
            && typeof ($scope.selected == 'string')
            && /\S+@\S+\.\S+/.test($scope.selected)) {
            companyService.addNewMemberToCompany($scope.newMember).then(function () {
                $scope.sendingRequestStatus = undefined;
                Notification.success({ message: 'Member created', delay: 2000 });
                getMembersInCompany();
            },
            function (response) {
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }
                let errorMessage =  errors.join(' ');
                Notification.error({ message: errorMessage, delay: 2000 });
            });

        } else {
            Notification.error({ message: 'Please input a valid email to proceed', delay: 2000 });
        }

    }

    // get the company info for display
    getCompanyDetail();
        
    //get member list for the current company
    getMembersInCompany();


    }

]);