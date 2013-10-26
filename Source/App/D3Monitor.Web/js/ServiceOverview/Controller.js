
d3MonitorApp.controller('ServiceOverviewController', function ServiceOverviewController($scope, subscriptionService) {
    
    $scope.applications = [];
    
    var svg = d3.select("#drawingArea").append("svg")
            .attr("width", 500)
            .attr("height", 500);

    svg.append("svg:rect").attr("width", 200).attr("height", 80).style("fill", "#eeeeee").style("stroke", "#FFF");
    
    var loadApplications = function() {
        subscriptionService.getApplications()
            .done(function (response) {
                $scope.applications = response;
                $scope.$apply();
            }).fail(function (error) {
                alert(error);
            });
    }

    subscriptionService.onConnected(function () {
        loadApplications();
    });
});