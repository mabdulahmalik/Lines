const toggle = (button, inputId) =>  {
    const passwordElement = document.getElementById(inputId);
    if (passwordElement.type === "password") {
        passwordElement.type = "text";
    } else if(passwordElement.type === "text") {
        passwordElement.type = "password";
    }
    button.children.item(0).classList.toggle("hidden");
    button.children.item(1).classList.toggle("hidden");
}