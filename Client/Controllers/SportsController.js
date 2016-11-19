(function(){

var app = angular.module("BetFeed");

app.controller("SportsController", ['SportsService', '$scope', function(SportsService, $scope){
    var id = 1170;    
    this.sport = SportsService.GetSportsEvents(id);
    console.log("peshoo");
    $scope.message = "In sports view";
}]);

})();