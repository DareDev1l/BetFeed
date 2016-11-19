(function() {
    var app = angular.module("BetFeed", ['ngRoute']);


    app.config(['$routeProvider', '$locationProvider', function($routeProvider, $locationProvider){
    $locationProvider.html5Mode(true);

    $routeProvider
        .when('/sport', {
            templateUrl: 'Partials/sport-details.html',
            controller: 'SportsController'
        });
    }]);
})();