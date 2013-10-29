<%@ Page Language="C#"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>D3 Monitor</title>
    
    <link href="cs/styles.css" rel="stylesheet" />
    
    <script src="Scripts/jquery-1.6.4.min.js"></script>
    <script src="Scripts/jquery.signalR-2.0.0.min.js"></script>
    <script src="Scripts/angular.min.js"></script>
    <script src="Scripts/d3.v3.js"></script>

    <script src="js/ServiceOverview/Directive.js"></script>
    <script src="js/ServiceOverview/App.js"></script>
    <script src="js/ServiceOverview/SubscriptionService.js"></script>
    <script src="js/ServiceOverview/Controller.js"></script>
    
    <script src='<%: ResolveClientUrl("~/signalr/hubs") %>'></script>
</head>
<body data-ng-app="d3MonitorApp">
  
    <data-service-overview></data-service-overview>

</body>
</html>
