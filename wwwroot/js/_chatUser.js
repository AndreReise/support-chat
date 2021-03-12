"use strict";
window.onload = Init();
var user = new User(2);

function UserButtonInterracts(elem) {
    //console.dir(elem);
    //либо тут  добавлять
    hubConnection.invoke("Send", { "Text": this.innerHTML, "Name": userName, "SenderType": "in" });
}

// отправка сообщения от простого пользователя
document.getElementById("chat-submit").addEventListener("click", function (e) {
    let text = document.getElementById("chat-input").value;
    let mes = {};
    mes.name = message.name;
    mes.text = text;
    hubConnection.invoke("Send", mes);
    document.getElementById("chat-input").value = "";
});


function Init() {
    $(".minimizeChat").click(ToggleChat);
    var span = document.getElementsByClassName("closeChat")[0];
    span.onclick = function () {
        CloseChat();
    };

    RegisterModal();
    RegisterInput();

    // установка имени пользователя
    document.getElementById("loginBtn").addEventListener("click", function (e) {
        var width = $("#user-chat-container").width();
        userName = document.getElementById("userName").value;
        document.getElementById("header").innerHTML = "<h3>Welcome " + userName + "</h3>";
        message.name = userName;
        $("#userNameBlock").hide();
        $("#user-chat-container").width = width;
    });
    $(".enterName").click({ msg: "Profile" },UserProfileData);
    $(".enterEmail").click({ msg: "Profile" },UserProfileData);

}

function UserProfileData(e) {
    if (e.data.msg ==="Profile") {
        $("#modal-name").val(userName);
        $(".modal-body").children()[0].style.display = "none";
        $(".modal-header").children()[0].innerHTML = e.data.msg;
        $("#modalNo").hide();
    }
    $("#Modal").show();
}

function ModalSetDefault(){
    $(".modal-body").children()[0].style.display = "block";
    $(".modal-header").children()[0].innerHTML = "Do you want to complete this dialog ?";
    $("#modalNo").show();
    userName = $("#modal-name").val();
    document.getElementById("header").innerHTML = "<h3>Welcome " + userName + "</h3>";
    message.name = userName;
    $("#modal-name").val(userName);
        ToggleChat();
}


function ToggleChat(){
    $("#user-chat-container").toggle();
    $("#ChatOpenButton").toggle();
}

function CloseChat() {
    $("#Modal").toggle();
    $('.reason-buttons').find('[data-reason="' + user.Reasone + '"]')[0].focus(); //.addClass('active')
}

function RegisterModal() {
    var modal = document.getElementById("Modal");
    var btn = document.getElementById("modalYes");
   

    btn.addEventListener("click", function () {
        $("#Modal").toggle();
        ToggleChat();
        ModalSetDefault();
         //OnDisconnect();
    }, false);

    btn = document.getElementById("modalNo");
    btn.addEventListener("click", function () { modal.style.display = "none"; }, false);
    var span = document.getElementsByClassName("closeModal")[0];
    span.onclick = function () {
        modal.style.display = "none";
    };
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    };
}

function RegisterInput() {
    $("#chat-submit").click(function (e) {
        e.preventDefault();
        var msg = $("#chat-input").val();
        if (msg.trim() == '') {
            return false;
        }

    });
    $("#chat-input").keyup("keyup", function (event) {
        // Number 13 is the "Enter" key on the keyboard
        if (event.keyCode === 13) {
            // Cancel the default action, if needed
            event.preventDefault();
            // Trigger the button element with a click
            document.getElementById("chat-submit").click();
        }
    });

}

function SwapChatPanel() {
    $(".chat-input").toggle();
    $(".chat-button-container").toggle();
}


function SetReasone(elem) {
    //console.dir(elem.dataset.reason);
    user.Reasone = elem.dataset.reason; 
    //console.dir(user.Reasone );

}

function UserButtonInterract(elem) {
    //console.dir(elem);
    SwapChatPanel();
    $(".chat-button-row").empty();
}
