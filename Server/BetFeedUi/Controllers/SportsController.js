(function () { 
    var app = angular.module("BetFeed");

    app.controller("SportsController", ['SportsService', 'EventsService', 'MatchesService', '$routeParams', '$http', '$interval', function (SportsService, EventsService, MatchesService, $routeParams, $http, $interval) {
        var self = this;
        var updateIntervalInSeconds = 20;
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
            this.event = EventsService.GetEventMatches(eventId)
                            .then(function success(response) {
                                console.log(response.data.Id)
                                self.event = response.data;
                            },
                            function error(err) {
                                console.log("Error: " + JSON.stringify(err));
                            }
                        );
        };

        this.selectMatch = function (matchId) {
            this.match = MatchesService.GetMatchBets(matchId)
                            .then(function success(response) {
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

                                        console.log(curScope.newMatches);

                                        if (curScope.newMatches.length > 0) {
                                            self.event.Matches.push.apply(self.event.Matches, curScope.newMatches);
                                            self.event.RequestDate = curScope.RequestDate;
                                            console.log(self.event.Matches);
                                        }
                                    },
                                    function error(err) {
                                        console.log("Error: " + JSON.stringify(err));
                                    });
        };

        $interval(updateMatches, updateIntervalInSeconds * 1000);
    }]);
})();

