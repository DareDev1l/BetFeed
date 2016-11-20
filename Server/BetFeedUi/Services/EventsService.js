(function () {
    var app = angular.module("BetFeed");
    
    app.factory("EventsService", ['$http', function ($http) {
        var apiUri = "http://localhost:54540/api/Events/event/";

        function GetEventMatches(eventId) {
            return $http.get(apiUri + eventId);
        };

        return {
            GetEventMatches: GetEventMatches
        }
    }]);
})();