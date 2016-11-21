(function () {
    var app = angular.module("BetFeed");

    app.factory("SportsService", ['$http', function ($http) {
        var apiUri = "http://localhost:54540/api/Sports/";

        function GetSportsEvents(sportId) {
            return $http.get(apiUri + "sport/" + sportId);
        };

        function GetSportsNamesAndIds() {
            return $http.get(apiUri + "NamesAndIds");
        };

        return {
            GetSportsEvents: GetSportsEvents,
            GetSportsNamesAndIds: GetSportsNamesAndIds
        }
    }]);
})();

