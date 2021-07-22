"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/BlogHub").build();

connection.on("RefreshBlogsPage", function (user, message) {
    $("#myToast").toast({
        delay: 5000
    })

    $("#myToast").toast('show');

    setTimeout(function () {
        location.reload();
    }, 5000);
});

connection.start().then(function () {
    console.log("Connected to Blog Hub");
}).catch(function (err) {
    return console.error(err.toString());
});
