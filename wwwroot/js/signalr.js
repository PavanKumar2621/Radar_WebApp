const connection = new signalR.HubConnectionBuilder()
    .withUrl("/communicationHub")
    .withAutomaticReconnect()
    .build();

async function startSignalR() {
    try {
        await connection.start();
        console.log("SignalR connected");
        updateConnectionStatus("Connected");
    }
    catch (error) {
        console.error("SignalR connection failed:", error);
        updateConnectionStatus("Disconnected");
        setTimeout(startSignalR, 5000);
    }
}

connection.onreconnecting(function () {
    console.log("SignalR reconnecting...");
    updateConnectionStatus("Reconnecting");
});

connection.onreconnected(function () {
    console.log("SignalR reconnected");
    updateConnectionStatus("Connected");
});

connection.onclose(function () {
    console.log("SignalR connection closed");
    updateConnectionStatus("Disconnected");
});

function updateConnectionStatus(status) {
    const element = document.getElementById("connectionStatus");
    if (element) {
        element.textContent = status;
    }
}

startSignalR();