(function () {
    var app = angular.module("BetFeed");
    
    app.factory("EventsService", ['$http', function ($http) {
        var apiUri = "http://localhost:54540/api/Events";

        function GetEventMatches(eventId) {
            return $http.get(apiUri + "/event/" + eventId);
        };
        
        function GetNewMatches(eventId, afterDate) {
            return $http.get(apiUri + "/NewMatches/?eventId=" + eventId + "&after=" + afterDate.substring(0,19));
        }

        return {
            GetEventMatches: GetEventMatches,
            GetNewMatches: GetNewMatches
        }
    }]);
})();