'use strict';

angular.module('sbAdminApp')
  .controller('clientController', ['$scope', 'clientDataService', 'Notification', '$state', 'ModalService',
function ($scope, clientDataService, Notification, $state, ModalService) {

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

    $scope.openClientDetail = function (client, readonly) {
        if (readonly)
            $state.go('base.viewClient', { param: client.id });
        else
            $state.go('base.editClient', { param: client.id });

    };
    $scope.addNewClient = function () {
        $state.go('base.createClient');
    }
    //$scope.addNewClient = function () {
    //    $state.go('login');
    //}


    $scope.deleteClient = function (client) {
        var clientRef = client;
        ModalService.showModal({
            templateUrl: 'deleteClientmodal.html',
            controller: "deleteClientModalController"
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result) {
                    $scope.proceedDelete(clientRef.id);
                }
            });
        });
    }

    

    $scope.proceedDelete = function (clientId) {
        clientDataService.deleteClient(clientId).then(function (result) {
            Notification.success({ message: 'Deleted', delay: 2000 });
            init();
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };

    $scope.viewClientProperties = function (client) {
        $state.go('base.clientProperties', { param: client.id });

    }


    var init = function () {
        clientDataService.getAllClients().then(function (result) {
            $scope.clientlist = result;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    $scope.inputValid = false;

    $scope.$watch('selected', function (newVal, oldVal) {
        if (newVal)
            $scope.inputValid = $scope.selected.clientId || (typeof ($scope.selected) == 'string' && /\S+@\S+\.\S+/.test($scope.selected));

    });
   

    $scope.searchClient = function (search) {
       return companyService.searchClientForJoinCompany(search).then(function (clients) {
           for(var i = 0; i< clients.length; i++){
               clients[i].label = clients[i].fullName + ' (' + clients[i].email + ')';
           }
            return clients;
            //$scope.searchResult = clients;
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
   
    $scope.proceedAddClient = function () {
        if ($scope.selected.clientId) {
            $scope.sendingRequestStatus = 'existedClient';
            //we are adding existing client
           
        } else {
            // we are adding via a email

            $scope.newClient = {
                userName: undefined,
                firstName: undefined,
                lastName: undefined,
                email: $scope.selected,
                isContractor : true,
                userType:1,//trade
                

            };


            $scope.sendingRequestStatus = 'newClient';
            // we are adding new client
        }
    }
     
    $scope.sendExistedClient = function () {
        addExistingClientToCompany();
       
    }
    $scope.sendNewClient = function () {
        addNewClientToCompany();
    }


    $scope.cancel = function () {
        $scope.newClient = {};
        $scope.sendingRequestStatus = undefined;
    }
    

    var addExistingClientToCompany = function () {
        if($scope.selected.clientId){
            var data = {clientId:$scope.selected.clientId, text:$scope.inviteText};
            companyService.addExistingClientToCompany(data).then(function () {
                $scope.sendingRequestStatus = undefined;
                Notification.success({ message: 'Request sent', delay: 2000 });
                //getClientsInCompany();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });

        }else{
            Notification.error({ message: 'Please select a client to proceed', delay: 2000 }); 
        }

    }
    var addNewClientToCompany = function () {
        if ($scope.selected
            && typeof ($scope.selected == 'string')
            && /\S+@\S+\.\S+/.test($scope.selected)) {
            companyService.addNewClientToCompany($scope.newClient).then(function () {
                $scope.sendingRequestStatus = undefined;
                Notification.success({ message: 'Client created', delay: 2000 });
                getClientsInCompany();
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
    init();

}]);



angular.module('sbAdminApp').controller('deleteClientModalController', function ($scope, close) {

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };
});