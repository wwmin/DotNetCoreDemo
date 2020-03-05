// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
var server = "ws://localhost:5001";//如果开启了https则这里是wss
var web_socket = new WebSocket(server + "/ws");
web_socket.onopen = function (event) {
    console.log("Connection open ...");
    $("#msgList").val("websocket connection opened.");
};

web_socket.onmessage = function (event) {
    console.log("received message: " + event.data);
    if (event.data) {
        var content = $("#msgList").val();
        content = content + "\r\n" + event.data;

        $("#msgList").val(content);
    }
};

web_socket.onclose = function (event) {
    console.log("Connection closed.");
};

$("#btnJoin").on("click", function () {
    var roomNo = $("#txtRoomNo").val();
    var nick = $("#txtNickName").val();
    if (roomNo) {
        var msg = {
            action: "join",
            msg: roomNo,
            nick: nick
        };
        web_socket.send(JSON.stringify(msg));
    }
});

$("#btnSend").on("click", function () {
    var message = $("#txtMsg").val();
    var nick = $("#txtNickName").val();
    if (message) {
        web_socket.send(JSON.stringify({
            action: "send_to_room",
            msg: message,
            nick: nick
        }));
    }
});

$("#btnLeave").on("click", function () {
    var nick = $("#txtNickName").val();
    var msg = {
        action: "leave",
        msg: "",
        nick: nick
    };
    web_socket.send(JSON.stringify(msg));
});