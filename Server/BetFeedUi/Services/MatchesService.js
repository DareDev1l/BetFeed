(function () {
    var app = angular.module("BetFeed");

    app.factory("MatchesService", ['$http', function ($http) {
        var apiUri = "http://localhost:54540/api/matches/match/";

        function GetMatchBets(matchId) {
            return $http.get(apiUri + matchId);
        };

        return {
            GetMatchBets: GetMatchBets
        }
    }]);
})();