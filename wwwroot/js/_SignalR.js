


const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();

hubConnection.serverTimeoutInMilliseconds = 1000 * 60 * 10; // 1 second * 60 * 10 = 10 minutes.

let userName = "123";
let message = { name: userName };

// get new message from server
hubConnection.on("Receive", function (message) {

    message.dialogid = message.dialogid;

    let newMessage = document.createElement("div");
  
    if (message.senderType == "in") {

        newMessage.className = "chat first";
    }
    else {

        newMessage.className = "chat second";
    }
    newMessage.appendChild(document.createTextNode('Name:' + message.name));

    // creates <p> element for user`s message
    let messageText = document.createElement("p");

        if (message.textTupe == "json") {

            jsontext = JSON.parse(message.text);

            messageText.appendChild(document.createTextNode(jsontext.text));
            newMessage.appendChild(messageText);

            if (jsontext.buttoncount > 0) {

                for (let i = 0; i < jsontext.textbutton.length; i++) {

                    var button = document.createElement("button");

                    button.innerHTML = jsontext.textbutton[i];
                    button.classList.add("btn-interract");
                    button.classList.add("btn");
                    button.classList.add("btn-primary");

                    newMessage.appendChild(button);
                    messageText.appendChild(button);
                    button.onclick = UserButtonInterracts;

                    newMessage.appendChild(document.createElement("br"));
                } 

            }
            var str = "              <span class=\"time\"><\/span>";
            messageText.insertAdjacentHTML('beforeend', str);
        }
        else
        {
            messageText.appendChild(document.createTextNode(message.text));
            var str = "              <span class=\"time\"><\/span>";
            messageText.insertAdjacentHTML('beforeend', str);
            newMessage.appendChild(messageText);
        }

 

    var firstElem = document.getElementById("chat-messages").firstChild;
    document.getElementById("chat-messages").insertBefore(newMessage, firstElem);
});

hubConnection.start();
