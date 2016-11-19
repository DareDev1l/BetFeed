(function(){

var app = angular.module("BetFeed");

app.factory("SportsService", ['$http', function($http){
    var apiUri = "http://localhost:54540/api/Sports/";

    function GetSportsEvents(id){
        $http.get(apiUri + id)
            .then(function success(data){
                console.log(data.Name);
                return data;
            },
            function error(err){
                console.log("Error: " + err);
            });
    };

    return {
        GetSportsEvents : GetSportsEvents
    }
}]);

})();