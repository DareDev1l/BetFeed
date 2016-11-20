(function () {
    var app = angular.module("BetFeed");

    app.factory("SportsService", ['$http', function ($http) {
        var apiUri = "http://localhost:54540/api/Sports/sport/";

        function GetSportsEvents(sportId) {
            return $http.get(apiUri + sportId);
        };

        return {
            GetSportsEvents: GetSportsEvents
        }
    }]);
})();

