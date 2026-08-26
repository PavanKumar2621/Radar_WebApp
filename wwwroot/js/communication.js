connection.on("ReceiveMessage", function (message) {

    console.log("=================================");
    console.log("Response received from server");
    console.log("=================================");
    console.log("Message Type :", message.messageType);
    console.log("Request ID   :", message.requestId);
    console.log("Source       :", message.source);
    console.log("Timestamp    :", message.timestamp);
    console.log("Data         :", message.data);
});

async function requestRadarData() {

    if (!connection) {
        console.error("SignalR connection does not exist");
        return;
    }

    if (connection.state !== "Connected") {
        console.warn("SignalR not connected. Current state:", connection.state);
        return;
    }

    try {
        await connection.invoke("SendRadarData");
        console.log("Radar data request sent");
    }
    catch (error) {
        console.error("Radar request failed:", error);
    }
}

async function sendMessage(message) {
    try {
        await connection.invoke("SendMessage", message);
        console.log("=================================");
        console.log("Message sent:");
        console.log(message);
        console.log("=================================");
    }
    catch (error) {
        console.error("Failed to send message:",error);
    }
}