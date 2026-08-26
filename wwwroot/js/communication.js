connection.on("ReceiveMessage", function (message) {
    // console.log("=================================");
    // console.log("Response received from server");
    // console.log("=================================");
    // console.log("Message Type :", message.messageType);
    // console.log("Request ID   :", message.requestId);
    // console.log("Source       :", message.source);
    // console.log("Timestamp    :", message.timestamp);
    // console.log("Data         :", message.data);
    console.log("Response received from server", message);
});

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
