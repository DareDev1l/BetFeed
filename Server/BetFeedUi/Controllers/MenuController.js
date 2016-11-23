(function () {
    var app = angular.module("BetFeed");

    app.controller("MenuController", ['SportsService', function (SportsService) {
        var self = this;
        self.sportNamesAndIds = {};

        this.getSportNamesAndIds = SportsService.GetSportsNamesAndIds()
                    .then(function success(response) {
                        console.log(response.data);
                        self.sportNamesAndIds = response.data;
                    },
                    function error(err) {
                        console.log("Error: " + JSON.stringify(err));
                    }
                );;
    }]);
})();