
d3MonitorApp.factory('subscriptionService', function DemoService() {

    var subscriptionHub = $.connection.subscriptionHub;
    var connected = false;
    var onConnectedCallBacks = [];
    var onApplicationsChangedCallbacks = [];
    
    subscriptionHub.client.applicationsChanged = function (applications) {
        for (var c = 0; c < onApplicationsChangedCallbacks.length; c++) {
            onApplicationsChangedCallbacks[c](applications);
        }
    };
    
    $.connection.hub.start().done(function() {
        connected = true;
        for (var c = 0; c < onConnectedCallBacks.length; c++) {
            onConnectedCallBacks[c]();
        }
    }).fail(function() {
        alert("Could not Connect!");
    });

    return {
        onConnected: function(callback) {
            if (connected) {
                callback();
            } else {
                onConnectedCallBacks.push(callback);
            }
        },
        onApplicationsChanged: function(callback) {
            onApplicationsChangedCallbacks.push(callback);
        },
        getApplications: function() {
            return subscriptionHub.server.getAllApplications();
        },
        saveApplicationPosition: function(appName, x, y) {
            return subscriptionHub.server.saveApplicationPosition(appName, x, y);
        }
    };
});