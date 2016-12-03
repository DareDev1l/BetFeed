(function () { 
    var app = angular.module("BetFeed");

    app.controller("SportsController", ['SportsService', 'EventsService', 'MatchesService', '$routeParams', '$http', '$interval',
                                        function (SportsService, EventsService, MatchesService, $routeParams, $http, $interval) {
        var self = this;
        var updateIntervalInSeconds = 60;
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
                        );

        this.selectEvent = function (eventId) {
            if (self.event.Id == eventId) {
                return;
            }

            this.event = EventsService.GetEventMatches(eventId)
                            .then(function success(response) {
                                console.log("EventId : " + response.data.Id)
                                self.event = response.data;
                            },
                            function error(err) {
                                console.log("Error: " + JSON.stringify(err));
                            }
                        );
        };

        this.selectMatch = function (matchId) {
            if (self.match.Id == matchId) {
                return;
            }

            this.match = MatchesService.GetMatchBets(matchId)
                            .then(function success(response) {
                                console.log("MatchId : " + response.data.Id)
                                self.match = response.data;
                            },
                            function error(err) {
                                console.log("Error: " + JSON.stringify(err));
                            }
                        );
        }

        var updateMatches = function(){
            var curScope = this;
            this.newMatches = {};

            if (!self.event.RequestDate) {
                return;
            }

            this.getNewMatches = EventsService.GetNewMatches(self.event.Id, self.event.RequestDate)
                                    .then(function success(response) {
                                        curScope.newMatches = response.data.Matches;
                                        curScope.RequestDate = response.data.RequestDate;

                                        if (curScope.newMatches.length > 0) {
                                            self.event.Matches.push.apply(self.event.Matches, curScope.newMatches);
                                            self.event.RequestDate = curScope.RequestDate;
                                        }
                                    },
                                    function error(err) {
                                        console.log("Error: " + JSON.stringify(err));
                                    });
        };

        var updateBets = function () {
            var curScope = this;
            this.newBets = {};

            if (!self.match.RequestDate) {
                return;
            }

            this.getNewBets = MatchesService.GetNewBets(self.match.Id, self.match.RequestDate)
                                    .then(function success(response) {
                                        curScope.newBets = response.data.Bets;
                                        curScope.RequestDate = response.data.RequestDate;

                                        if (curScope.newBets.length > 0) {
                                            self.match.Bets.push.apply(self.match.Bets, curScope.newBets);
                                            self.match.RequestDate = curScope.RequestDate;
                                        }
                                    },
                                    function error(err) {
                                        console.log("Error: " + JSON.stringify(err));
                                    });
        };

        $interval(updateMatches, updateIntervalInSeconds * 1000);
        $interval(updateBets, updateIntervalInSeconds * 1000);
    }]);
})();

