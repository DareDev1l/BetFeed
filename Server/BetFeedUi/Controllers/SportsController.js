(function () {
    var app = angular.module("BetFeed");

    app.controller("SportsController", ['SportsService', 'EventsService', '$routeParams', '$http', function (SportsService, EventsService, $routeParams, $http) {
        var self = this;
        var id = $routeParams.id;
        this.sport = {};
        this.event = {};

        this.getSport = SportsService.GetSportsEvents(id)
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
    }]);
})();

