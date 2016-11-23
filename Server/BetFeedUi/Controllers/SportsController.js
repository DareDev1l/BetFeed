(function () { 
    var app = angular.module("BetFeed");

    app.controller("SportsController", ['SportsService', 'EventsService', 'MatchesService', '$routeParams', '$http', function (SportsService, EventsService, MatchesService, $routeParams, $http) {
        var self = this;
        var sportId = $routeParams.id;

        this.sport = {};
        this.event = {};
        this.match = {};

        this.getSport = SportsService.GetSportsEvents(sportId)
                    .then(function success(response) {
                        self.sport = response.data;
                    },
                    function error(err) {
                        console.log("Error: " + JSON.stringify(err));
                    }
                );;

        this.selectEvent = function (eventId) {
            this.event = EventsService.GetEventMatches(eventId)
                            .then(function success(response) {
                                self.event = response.data;
                            },
                            function error(err) {
                                console.log("Error: " + JSON.stringify(err));
                            }
                        );;
        }

        this.selectMatch = function (matchId) {
            this.match = MatchesService.GetMatchBets(matchId)
                            .then(function success(response) {
                                self.match = response.data;
                            },
                            function error(err) {
                                console.log("Error: " + JSON.stringify(err));
                            }
                        );;
        }
    }]);
})();

