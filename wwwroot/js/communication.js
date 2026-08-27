connection.on("ReceiveMessage", function (message) {
    console.log("Response received from server", message);
});

async function sendMessage(message) {
    try {
        await connection.invoke("SendMessage", message);
    }
    catch (error) {
        console.error("Failed to send message:",error);
    }
}
