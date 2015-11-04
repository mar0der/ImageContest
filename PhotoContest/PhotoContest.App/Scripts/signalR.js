var signalRConnection = (function () {
    var connection = $.hubConnection();
    connection.url = "http://localhost:17578/signalr";
    var baseHub = connection.createHubProxy('baseHub');
    connection.start().done(function () {
        console.log("gotou");
    });
    return {
        baseHub: baseHub
    }
})();