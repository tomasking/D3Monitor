var directives = angular.module('directives', []);

directives.directive('serviceOverview', function () {
    return {
        templateUrl: 'js/ServiceOverview/service-overview.html',
        replace: true,
        transclude: false,
        restrict: 'E', //E: use as an element <my-directive />, A: use as attribute <div my-directive />, C: use as a class <div class='my-directive' />
        scope: { //by putting this on means the parent scope isn't passed through
            
        },
        controller: 'ServiceOverviewController'
    };
});