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

    app.factory("SportsService", ['$http', function ($http) {
        var apiUri = "http://localhost:54540/api/Sports/sport/";

        function GetSportsEvents(sportId) {
            return $http.get(apiUri + sportId);
        };

        return {
            GetSportsEvents: GetSportsEvents
        }
    }]);

    app.factory("EventsService", ['$http', function ($http) {
        var apiUri = "http://localhost:54540/api/Events/event/";

        function GetEventMatches(eventId) {
            return $http.get(apiUri + eventId);
        };

        return {
            GetEventMatches: GetEventMatches
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