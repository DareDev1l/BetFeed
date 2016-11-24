(function () {
    var app = angular.module("BetFeed");

    app.factory("MatchesService", ['$http', function ($http) {
        var apiUri = "http://localhost:54540/api/matches";

        function GetMatchBets(matchId) {
            return $http.get(apiUri + "/match/" + matchId);
        };

        function GetNewBets(matchId, afterDate) {
            return $http.get(apiUri + "/NewBets/?matchId=" + matchId + "&after=" + afterDate.substring(0, 19));
        }

        return {
            GetMatchBets: GetMatchBets,
            GetNewBets: GetNewBets
        }
    }]);
})();