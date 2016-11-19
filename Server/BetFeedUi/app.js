/// <reference path="C:\Users\tedko\Desktop\BetFeedNew\Server\BetFeedUi\Scripts/angular.js" />
(function () {
    var app = angular.module("BetFeed", ['ngRoute']);

    app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/sport/:id', {
                templateUrl: 'Views/sport-details.html',
                controller: 'SportsController',
                controllerAs: 'sportCtrl'
            })
            .otherwise({
                redirectTo : '/'
            });
    }]);

    app.factory("SportsService", ['$http', function ($http) {
        var apiUri = "http://localhost:54540/api/Sports/";

        function GetSportsEvents(id) {
            return $http.get(apiUri + id);
        };

        return {
            GetSportsEvents: GetSportsEvents
        }
    }]);

    app.controller("SportsController", ['SportsService', '$routeParams', '$http', function (SportsService, $routeParams, $http) {
        var self = this;
        var id = $routeParams.id;

        this.sport = SportsService.GetSportsEvents(id)
                    .then(function success(response) {
                            console.log("in callback" + response.data);
                            self.sport = response.data;
                            },
                            function error(err) {
                                console.log("Error: " + JSON.stringify(err));
                            }
                        );;
    }]);

    app.controller("MenuController", ['SportsService' , function (SportsService) {
    }]);
})();