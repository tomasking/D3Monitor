
d3MonitorApp.controller('ServiceOverviewController', function ServiceOverviewController($scope, subscriptionService) {
    
    $scope.applications = [];
    $scope.links = [];
    $scope.selectedApplication = undefined;

    var draw = function () {

        for (var app = 0; app < $scope.applications.length; app++) {
            $scope.applications[app].StartLinks = [];
            $scope.applications[app].EndLinks = [];
        }

        var svg = d3.select("#drawingArea").append("svg")
            .attr("width", "100%")
            .attr("height", "100%");
        
        for (var l = 0; l < $scope.links.length; l++) {
            var lineForLink = svg.append("line").attr("stroke", "black");
            for (var a = 0; a < $scope.applications.length; a++) {
                if ($scope.links[l].StartApplicationName == $scope.applications[a].ApplicationName) {
                    lineForLink.attr("x1", $scope.applications[a].X + 150).attr("y1", $scope.applications[a].Y + 40).attr("stroke-width", 1);
                    $scope.applications[a].StartLinks.push(lineForLink);
                }
                if ($scope.links[l].EndApplicationName == $scope.applications[a].ApplicationName) {
                    lineForLink.attr("x2", $scope.applications[a].X + 150).attr("y2", $scope.applications[a].Y + 40).attr("stroke-width", 1);
                    $scope.applications[a].EndLinks.push(lineForLink);
                }
            }
        }

        var application = svg.selectAll("g")
            .data($scope.applications)
            .enter()
            .append("g")
            .classed("application-group", true)
            
            .call(d3.behavior.drag()
                .origin(function () {
                    var t = d3.select(this);
                    return {
                        x: t.attr("x"), y: t.attr("y")
                    };
                })
                .on("drag", move)
                .on("dragend", savePosition)
            );

        application.append("rect")
            .classed("application-box", true)
            .attr("width", 300)
            .attr("height", 80)
            .attr("x", function(d) { return d.X; })
            .attr("y", function (d) { return d.Y; })
            .transition()
             .styleTween("stroke", function(d) {
                 if (d.ServiceDown) {
                     return d3.interpolate("green", "red");
                 } else {
                     return null;
                 }
             });
            
        
        application.append("text")
            .classed("application-title", true)
            .attr("x", function(d) { return d.X + 10; })
            .attr("y", function(d) { return d.Y + 18; })
            .text(function (d) { return d.ApplicationName; })
            .on("click", function (d) {
                $scope.selectedApplication = d;
                $scope.$apply();
            })
            .on("mouseover", function () {
                d3.select(this).style("fill", "blue");
            })
           .on("mouseout", function() {
                d3.select(this).style("fill", "black");
           });

        application
            .selectAll("p")
            .data(function(d) {
                var serviceBuses = [];
                for (var i = 0; i < d.ServiceBuses.length; i++) {
                    serviceBuses.push(
                        {
                            X: d.X,
                            Y: d.Y,
                            EndpointUri: d.ServiceBuses[i].EndpointUri,
                            MessageCount: d.ServiceBuses[i].MessageCount,
                            Index: i
                        });
                }
                return serviceBuses;
            })
            .enter()
            .append("text")
            .attr("x", function(d) { return d.X + 20; })
            .attr("y", function(d) { return 40 + d.Y + d.Index * 20; })
            .on("mouseover", function() {
                d3.select(this).style("fill", "blue");
            })
            .on("mouseout", function() { d3.select(this).style("fill", "black"); })
            .text(function (d) { return d.EndpointUri + ' (' + d.MessageCount + ')'; });
    };

    function savePosition(app) {
        var t = d3.select(this);
        var x = Number(t.attr("x")) + app.X;
        var y = Number(t.attr("y")) + app.Y;
        subscriptionService.saveApplicationPosition(app.ApplicationName, x, y)
        .fail(function (error) {
            alert(error);
        });;
    }
    
    function move(app) {
        var dx = d3.event.x;
        var dy = d3.event.y;
        d3.select(this)
            .attr("transform", "translate(" + dx + "," + dy + ")");
        d3.select(this).attr("x", d3.event.x);
        d3.select(this).attr("y", d3.event.y);
        for (var s = 0; s < app.StartLinks.length; s++) {
            app.StartLinks[s]
                .attr("x1", d3.event.x + app.X + 150)
                .attr("y1", d3.event.y + app.Y + 40);
        }
        for (var e = 0; e < app.EndLinks.length; e++) {
            app.EndLinks[e]
                .attr("x2", d3.event.x + app.X + 150)
                .attr("y2", d3.event.y + app.Y + 40);
        }
    }



    var loadApplications = function() {
        subscriptionService.getApplications()
            .done(function(response) {
                $scope.applications = response.Applications;
                $scope.links = response.Links;
                $scope.$apply();
                draw();
            }).fail(function(error) {
                alert(error);
            });
    };

    subscriptionService.onApplicationsChanged(function (applications) {
        $scope.applications = applications; //TODO: update individual ones rather than whole array
        $scope.$apply();
        draw();
    });
    
    subscriptionService.onConnected(function () {
        loadApplications();
    });
});