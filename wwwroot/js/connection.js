
var connection = new signalR.HubConnectionBuilder()
.withUrl("/statusHub")
.build();

async function createConnection(group) {

    connection.start().then(function () {
      connection.invoke("JoinGroup", group);
    });
    connection.on("ReceiveStatus", function (totalClick, totalView) {
      document.querySelector("#totalClick").innerHTML = totalClick;
      document.querySelector("#totalView").innerHTML = totalView;
    });

}




