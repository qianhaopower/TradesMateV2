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
 
  ])
  .constant('domain', 'http://' + window.location.hostname)
  .constant('api', '/DataService/odata')
  .service('urls', function (domain, api) { this.apiUrl = domain + api; })
  .config(['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', 'NotificationProvider',
      function ($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, NotificationProvider) {

          $ocLazyLoadProvider.config({
              debug: false,
              events: true,
          });

          $urlRouterProvider.otherwise('/dashboard.home');

          $stateProvider
            .state('dashboard', {
                url: '/dashboard',
                templateUrl: 'views/dashboard/main.html',
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
            .state('dashboard.home', {
                url: '/home',
                controller: 'MainCtrl',
                templateUrl: 'views/dashboard/home.html',
                resolve: {
                    loadMyFiles: function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                            //controllers
                            'scripts/controllers/main.js',
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
                            'scripts/directives/dashboard/stats/stats.js'


                            ]
                        })
                    }
                }
            })
            .state('dashboard.form', {
                templateUrl: 'views/form.html',
                url: '/form'
            })
            .state('dashboard.blank', {
                templateUrl: 'views/pages/blank.html',
                url: '/blank'
            })
            //.state('login', {
            //    templateUrl: 'views/pages/login.html',
            //    url: '/login'
            //})
            .state('dashboard.chart', {
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
            .state('dashboard.table', {
                templateUrl: 'views/table.html',
                url: '/table'
            })
            .state('dashboard.panels-wells', {
                templateUrl: 'views/ui-elements/panels-wells.html',
                url: '/panels-wells'
            })
            .state('dashboard.buttons', {
                templateUrl: 'views/ui-elements/buttons.html',
                url: '/buttons'
            })
            .state('dashboard.notifications', {
                templateUrl: 'views/ui-elements/notifications.html',
                url: '/notifications'
            })
            .state('dashboard.typography', {
                templateUrl: 'views/ui-elements/typography.html',
                url: '/typography'
            })
            .state('dashboard.icons', {
                templateUrl: 'views/ui-elements/icons.html',
                url: '/icons'
            })
            .state('dashboard.grid', {
                templateUrl: 'views/ui-elements/grid.html',
                url: '/grid'
            })

           //authetication
          //$routeProvider.when("/home", {
          //    controller: "homeController",
          //    templateUrl: "/views/home.html"
          //});

          .state('home', {
              controller: "homeController",
              templateUrl: "views/home.html",
              url: '/home'
          })

          .state("login", {
              controller: "loginController",
              templateUrl: "views/login.html",
              url: '/login',
          })

          .state("signup", {
              controller: "signupController",
              templateUrl: "views/signup.html",
              url: '/signup'
          })

          .state("orders", {
              controller: "ordersController",
              templateUrl: "views/ orders.html",
              url: '/orders'
          })

          .state("refresh", {
              controller: "refreshController",
              templateUrl: "views/refresh.html",
              url: '/refresh'
          })

          .state("tokens", {
              controller: "tokensManagerController",
              templateUrl: "views/tokens.html",
              url: '/tokens'
          })

          .state("associate", {
              controller: "associateController",
              templateUrl: "views/associate.html",
              url: '/associate'
          })

         //business logic start from here

              //client
            .state('dashboard.clients', {
                templateUrl: 'views/clients.html',
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
               .state('dashboard.editClient', {
                   templateUrl: 'views/clientDetail.html',
                   controller: 'clientDetailController',
                   url: '/client/edit/:param',
               })
               .state('dashboard.createClient', {
                   templateUrl: 'views/clientDetail.html',
                   controller: 'clientDetailController',
                   url: '/client/create',
               })
               .state('dashboard.viewClient', {
                   templateUrl: 'views/clientDetail.html',
                   controller: 'clientDetailController',
                   url: '/client/view/:param',
               })



              //property
             .state('dashboard.properties', {
                 templateUrl: 'views/properties.html',
                 controller: 'propertyController',
                 url: '/properties'
             })
              .state('dashboard.createProperty', {
                  templateUrl: 'views/propertyDetail.html',
                  controller: 'propertyDetailController',
                  url: '/client/:clientId/property/create',
              })
               .state('dashboard.editProperty', {
                   templateUrl: 'views/propertyDetail.html',
                   controller: 'propertyDetailController',
                   url: '/property/edit/:propertyId',
               })
               .state('dashboard.viewProperty', {
                   templateUrl: 'views/propertyDetail.html',
                   controller: 'propertyDetailController',
                   url: '/property/view/:propertyId',
               })
             .state('dashboard.clientProperties', {
                  templateUrl: 'views/properties.html',
                  controller: 'propertyController',
                  url: '/client/:param/properties'
             })



              //propertySection
             .state('dashboard.propertySections', {
                 templateUrl: 'views/sections.html',
                 controller: 'propertySectionController',
                 url: '/property/:propertyId/property-sections'//property section List for a property
                                  
             })

              .state('dashboard.createPropertySection', {
                  templateUrl: 'views/sectionDetail.html',
                  controller: 'propertySectionDetailController',
                  url: '/property/:propertyId/property-section/create'
              })

                .state('dashboard.editPropertySection', {
                    templateUrl: 'views/sectionDetail.html',
                    controller: 'propertySectionDetailController',
                    url: '/property/:propertyId/property-section/edit/:sectionId'
                })
               .state('dashboard.viewPropertySection', {
                   templateUrl: 'views/sectionDetail.html',
                   controller: 'propertySectionDetailController',
                   url: '/property/:propertyId/property-section/view/:sectionId'
               })

              //workItems
            

          .state('dashboard.workItems', {
              templateUrl: 'views/workitems.html',
              controller: 'workItemController',
              url: '/property/:propertyId/section/:sectionId/workItems'                    
          })

            .state('dashboard.createWorkItem', {
                templateUrl: 'views/workItemDetail.html',
                controller: 'workItemDetailController',
                url: '/property/:propertyId/section/:sectionId/workItems/create'
            })

          .state('dashboard.editWorkItem', {
              templateUrl: 'views/workItemDetail.html',
              controller: 'workItemDetailController',
              url: '/property/:propertyId/section/:sectionId/workItems/edit/:workItemId'
          })
         .state('dashboard.viewWorkItem', {
             templateUrl: 'views/workItemDetail.html',
             controller: 'workItemDetailController',
             url: '/property/:propertyId/section/:sectionId/workItems/view/:workItemId'
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
var serviceBase = 'http://localhost/authenticationservice/'; //base for authentication
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
