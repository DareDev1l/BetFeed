(function () {
    var app = angular.module("BetFeed");

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
})();

