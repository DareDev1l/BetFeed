/// <reference path="C:\Users\tedko\Desktop\BetFeedNew\Server\BetFeedUi\Scripts/angular.js" />
(function () {
    var app = angular.module("BetFeed", ['ngRoute']);

    app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/sport/:id', {
                templateUrl: 'Views/sport-details.html',
                controller: 'SportsController'//,
                //controllerAs: 'sportCtrl'
            })
            .when('/sport/:sportId/:eventId', {
                templateUrl: 'Views/sport-details.html',
                controller: 'SportsController',
                controllerAs: 'sportCtrl'
            })
            .otherwise({
                redirectTo : '/'
            });
    }]);
})();