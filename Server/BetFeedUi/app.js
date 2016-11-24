/// <reference path="C:\Users\tedko\Desktop\BetFeedNew\Server\BetFeedUi\Scripts/angular.js" />
(function () {
    var app = angular.module("BetFeed", ['ngRoute']);

    app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/', {
                templateUrl: 'Views/home.html',
            })
            .when('/sport/:id', {
                templateUrl: 'Views/sport-details.html',
                controller: 'SportsController'
            })
            .otherwise({
                redirectTo : '/'
            });
    }]);
})();