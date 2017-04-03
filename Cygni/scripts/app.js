'use strict';
/**
 * @ngdoc overview
 * @name sbAdminApp
 * @description
 * # sbAdminApp
 *
 * Main module of the application.
 */
angular
  .module('sbAdminApp', [
    'oc.lazyLoad',
    'ui.router',
    'ui.bootstrap',
    'angular-loading-bar',
    'ui-notification',
    'ngMessages',
    'angularModalService',
    'angular-carousel',
    'LocalStorageModule',
    'isteven-multi-select',
    'pageslide-directive',
 
  ])
  .constant('domain', 'http://' + window.location.hostname)
  .constant('api', '/DataService/odata')
  .constant("constants", {
      "userType": {client:0, trade:1},
        
    })
  .service('urls', function (domain, api) { this.apiUrl = domain + api; })

  .config(['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', 'NotificationProvider','$httpProvider',
      function ($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, NotificationProvider,$httpProvider) {
          //$httpProvider.defaults.headers.common = {};
          //$httpProvider.defaults.headers.post = {};
          //$httpProvider.defaults.headers.put = {};
          //$httpProvider.defaults.headers.patch = {};
          //$httpProvider.defaults.headers.get = {};
          //$httpProvider.defaults.headers.common['Access-Control-Allow-Headers'] = '*';
          //$httpProvider.defaults.useXDomain = true;
          //delete $httpProvider.defaults.headers.common['X-Requested-With'];

          $ocLazyLoadProvider.config({
              debug: false,
              events: true,
          });

          $urlRouterProvider.otherwise('/base/home');

          $stateProvider
            .state('base', {
                url: '/base',
                templateUrl: 'views/base/main.html',
                controller: 'baseController',
                resolve: {
                    loadMyDirectives: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                        {
                            name: 'sbAdminApp',
                            files: [
                            'scripts/directives/header/header.js',
                            'scripts/directives/header/header-notification/header-notification.js',
                            'scripts/directives/sidebar/sidebar.js',
                            'scripts/directives/sidebar/sidebar-search/sidebar-search.js'
                            ]
                        }),
                        $ocLazyLoad.load(
                        {
                            name: 'toggle-switch',
                            files: ["bower_components/angular-toggle-switch/angular-toggle-switch.min.js",
                                   "bower_components/angular-toggle-switch/angular-toggle-switch.css"
                            ]
                        }),
                        $ocLazyLoad.load(
                        {
                            name: 'ngAnimate',
                            files: ['bower_components/angular-animate/angular-animate.js']
                        })
                        $ocLazyLoad.load(
                        {
                            name: 'ngCookies',
                            files: ['bower_components/angular-cookies/angular-cookies.js']
                        })
                        $ocLazyLoad.load(
                        {
                            name: 'ngResource',
                            files: ['bower_components/angular-resource/angular-resource.js']
                        })
                        $ocLazyLoad.load(
                        {
                            name: 'ngSanitize',
                            files: ['bower_components/angular-sanitize/angular-sanitize.js']
                        })
                        $ocLazyLoad.load(
                        {
                            name: 'ngTouch',
                            files: ['bower_components/angular-touch/angular-touch.js']
                        })
                    }
                }
            })
            .state('base.home', {
                url: '/home',
                controller: 'dashboardController',
                templateUrl: 'views/base/home.html',
                resolve: {
                    loadMyFiles: function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                            //controllers
                            'scripts/controllers/dashboardController.js',
                            'scripts/controllers/clientController.js',
                            'scripts/controllers/propertyController.js',
                            'scripts/controllers/propertySectionController.js',
                            'scripts/controllers/workItemController.js',

                             //services
                            'scripts/services/clientDataService.js',


                            //directives
                            'scripts/directives/timeline/timeline.js',
                            'scripts/directives/notifications/notifications.js',
                            'scripts/directives/chat/chat.js',
                            'scripts/directives/base/stats/stats.js'


                            ]
                        })
                    }
                }
            })
            //.state('base.form', {
            //    templateUrl: 'views/pages/form.html',
            //    url: '/form'
            //})
            //.state('base.blank', {
            //    templateUrl: 'views/pages/blank.html',
            //    url: '/blank'
            //})
            //.state('login', {
            //    templateUrl: 'views/pages/login.html',
            //    url: '/login'
            //})
            .state('base.chart', {
                templateUrl: 'views/chart.html',
                url: '/chart',
                controller: 'ChartCtrl',
                resolve: {
                    loadMyFile: function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'chart.js',
                            files: [
                              'bower_components/angular-chart.js/dist/angular-chart.min.js',
                              'bower_components/angular-chart.js/dist/angular-chart.css'
                            ]
                        }),
                        $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: ['scripts/controllers/chartContoller.js']
                        })
                    }
                }
            })
            .state('base.table', {
                templateUrl: 'views/table.html',
                url: '/table'
            })
            .state('base.panels-wells', {
                templateUrl: 'views/ui-elements/panels-wells.html',
                url: '/panels-wells'
            })
            .state('base.buttons', {
                templateUrl: 'views/ui-elements/buttons.html',
                url: '/buttons'
            })
            .state('base.notifications', {
                templateUrl: 'views/ui-elements/notifications.html',
                url: '/notifications'
            })
            .state('base.typography', {
                templateUrl: 'views/ui-elements/typography.html',
                url: '/typography'
            })
            .state('base.icons', {
                templateUrl: 'views/ui-elements/icons.html',
                url: '/icons'
            })
            .state('base.grid', {
                templateUrl: 'views/ui-elements/grid.html',
                url: '/grid'
            })

           //authetication
          //$routeProvider.when("/home", {
          //    controller: "homeController",
          //    templateUrl: "/views/home.html"
          //});

          .state('noAccess', {
              controller: "noAccessController",
              templateUrl: "views/authentication/noAccess.html",
              url: '/noAccess'
          })

          .state("login", {
              controller: "loginController",
              templateUrl: "views/authentication/login.html",
              url: '/login',
          })

          .state("signup", {
              controller: "signupController",
              templateUrl: "views/authentication/signup.html",
              url: '/signup'
          })

          //.state("orders", {
          //    controller: "ordersController",
          //    templateUrl: "views/orders.html",
          //    url: '/orders'
          //})

          .state("refresh", {
              controller: "refreshController",
              templateUrl: "views/authentication/refresh.html",
              url: '/refresh'
          })

          .state("tokens", {
              controller: "tokensManagerController",
              templateUrl: "views/authentication/tokens.html",
              url: '/tokens'
          })

          .state("associate", {
              controller: "associateController",
              templateUrl: "views/authentication/associate.html",
              url: '/associate'
          })
          



         //business logic start from here


         

              //client
            .state('base.clients', {
                templateUrl: 'views/clients/clients.html',
                controller: 'clientController',
                url: '/clients',
                //resolve: {
                //    loadMyFiles: function ($ocLazyLoad) {
                //        return $ocLazyLoad.load({
                //            name: 'sbAdminApp',
                //            files: [
                //            'scripts/controllers/clientController.js',
                //            'scripts/services/clientDataService.js',
                //            ]
                //        })
                //    }
                //}
            })
               .state('base.editClient', {
                   templateUrl: 'views/clients/clientDetail.html',
                   controller: 'clientDetailController',
                   url: '/client/edit/:param',
               })
               .state('base.createClient', {
                   templateUrl: 'views/clients/clientDetail.html',
                   controller: 'clientDetailController',
                   url: '/client/create',
               })
               .state('base.viewClient', {
                   templateUrl: 'views/clients/clientDetail.html',
                   controller: 'clientDetailController',
                   url: '/client/view/:param',
               })



              //property
             .state('base.properties', {
                 templateUrl: 'views/properties/properties.html',
                 controller: 'propertyController',
                 url: '/properties'
             })
              .state('base.createProperty', {
                  templateUrl: 'views/properties/propertyDetail.html',
                  controller: 'propertyDetailController',
                  url: '/client/:clientId/property/create',
              })
               .state('base.editProperty', {
                   templateUrl: 'views/properties/propertyDetail.html',
                   controller: 'propertyDetailController',
                   url: '/property/edit/:propertyId',
               })
               .state('base.viewProperty', {
                   templateUrl: 'views/properties/propertyDetail.html',
                   controller: 'propertyDetailController',
                   url: '/property/view/:propertyId',
               })
             .state('base.clientProperties', {
                  templateUrl: 'views/properties/properties.html',
                  controller: 'propertyController',
                  url: '/client/:param/properties'
             })
         
                 .state('base.propertyCompanies', {
                     templateUrl: 'views/properties/propertyCompanies.html',
                     controller: 'propertyCompaniesController',
                     url: '/property/:propertyId/propertycompanies'
                 })


              //propertySection
             .state('base.propertySections', {
                 templateUrl: 'views/sections/sections.html',
                 controller: 'propertySectionController',
                 url: '/property/:propertyId/property-sections'//property section List for a property
                                  
             })

              .state('base.createPropertySection', {
                  templateUrl: 'views/sections/sectionDetail.html',
                  controller: 'propertySectionDetailController',
                  url: '/property/:propertyId/property-section/create'
              })

                .state('base.editPropertySection', {
                    templateUrl: 'views/sections/sectionDetail.html',
                    controller: 'propertySectionDetailController',
                    url: '/property/:propertyId/property-section/edit/:sectionId'
                })
               .state('base.viewPropertySection', {
                   templateUrl: 'views/sections/sectionDetail.html',
                   controller: 'propertySectionDetailController',
                   url: '/property/:propertyId/property-section/view/:sectionId'
              })

              //property  Attachments
              .state('base.propertyAttachments', {
                  templateUrl: 'views/attachment/attachments.html',
                  controller: 'attachmentController',
                  url: '/property/:propertyId/attachments'//property attachment
              })

              //workItems
            

          .state('base.workItems', {
              templateUrl: 'views/workItems/workItems.html',
              controller: 'workItemController',
              url: '/property/:propertyId/section/:sectionId/workItems'                    
          })

            .state('base.createWorkItem', {
                templateUrl: 'views/workItems/workItemDetail.html',
                controller: 'workItemDetailController',
                url: '/property/:propertyId/section/:sectionId/workItems/create'
            })

          .state('base.editWorkItem', {
              templateUrl: 'views/workItems/workItemDetail.html',
              controller: 'workItemDetailController',
              url: '/property/:propertyId/section/:sectionId/workItems/edit/:workItemId'
          })
         .state('base.viewWorkItem', {
             templateUrl: 'views/workItems/workItemDetail.html',
             controller: 'workItemDetailController',
             url: '/property/:propertyId/section/:sectionId/workItems/view/:workItemId'
         })

          //profile
          .state(
          'base.profile', {
              templateUrl: 'views/profile/userProfile.html',
              controller: 'userProfileController',
              url: '/profile/'
          })
          .state(
          'base.personalSetting', {
              templateUrl: 'views/profile/personalSettings.html',
              controller: 'peronalSettingController',
              url: '/profile/settings'
          })
           //manage company
          .state('base.manageCompany', {
                  templateUrl: 'views/company/manageCompany.html',
                  controller: 'manageCompanyController',
                  url: '/managecompany'
          })

          //manage company member
          .state('base.companyMember', {
                  templateUrl: 'views/company/companyMember.html',
                  controller: 'companyMemberController',
                  url: '/companymember'
          })
           .state('base.editCompanyMember', {
               templateUrl: 'views/company/companyMemberDetail.html',
               controller: 'companyMemberDetailController',
               url: '/companymember/edit/:memberId',
           })
              .state('base.createCompanyMember', {
                  templateUrl: 'views/company/companyMemberDetail.html',
                  controller: 'companyMemberDetailController',
                  url: '/companymember/create/:memberId',
          })
          .state('base.viewCompanyMember', {
              templateUrl: 'views/company/companyMemberDetail.html',
              controller: 'companyMemberDetailController',
              url: '/companymember/view/:memberId',
          })
          
          .state('base.allocateProperty', {
              templateUrl: 'views/company/allocateProperty.html',
              controller: 'allocatePropertyController',
              url: '/companymember/allocate/:memberId',
          })
       

          //message
            .state('base.message', {
                templateUrl: 'views/message/message.html',
                controller: 'messageController',
                url: '/messages',
            })

           .state('base.messageDetail', {
               templateUrl: 'views/message/messageDetail.html',
               controller: 'messageDetailController',
               url: '/message/:messageId',
           }) 
          //work request
           .state('base.workRequest', {
               templateUrl: 'views/workRequest/workRequest.html',
               controller: 'workRequestController',
               url: '/workRequests',
           })
          


          //NotificationProvider.setOptions({
          //    delay: 10000,
          //    startTop: 20,
          //    startRight: 10,
          //    verticalSpacing: 20,
          //    horizontalSpacing: 20,
          //    positionX: 'left',
          //    positionY: 'bottom'
          //});
      }]);



//var app = angular.module('AngularAuthApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);
var app = angular.module('sbAdminApp');
var serviceBase = 'http://localhost/DataService/'; //base for authentication
//var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

// In the run phase of your Angular application  
app.run(function ($rootScope, authService, $state) {


    $rootScope.$on('$stateChangeStart', function (event, toState, toStateParams) {
              if (authService.authentication && authService.authentication.isAuth) {
                  //already loggedin
                  if (toState.name == 'login') {
                      //already loggedin but try to load the login page. 
                      //redirect to home page. 
                      //if the user what to login again, he/she need logout first
                      event.preventDefault();
                      $state.go('base.home');
                  }

              } else {
                  //not loggedin 
                  if (toState.name == 'signup') {
                      //allow go to sign up when not signed in 
                  }
                  else if (toState.name == 'associate') {
                      //allow go to associate, no auth info yet
                  }
                  else if (toState.name != 'login') {
                     
                      event.preventDefault();
                      $state.go('login');
                  }
                  
              }
          });

    // Listen to '$locationChangeSuccess', not '$stateChangeStart'
    //$rootScope.$on('$locationChangeSuccess', function (event, url, oldUrl, state, oldState) {
    //    if (authService.authentication && authService.authentication.isAuth) {
    //        //already loggedin


    //    } else {
    //        //not loggedin 
    //        $state.go('login')
    //    }
    //});
})


//global scripts outside angular, like polyfill
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}