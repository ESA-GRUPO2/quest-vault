// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const inputs = document.querySelectorAll(".code-container-2fa input");

inputs.forEach((input, index) => {
    input.dataset.index = index;
    input.addEventListener("paste", handleOtppaste);
    input.addEventListener("keyup", handleOtp);
});

function handleOtppaste(e) {
    const data = e.clipboardData.getData("text");
    const value = data.split(" ");
    if (value.length === input.length) {
        inputs.forEach((input, index) => (input.value = value[index]));
        submit();
    }
}

function handleOtp(e) {
    const input = e.target;
    let value = input.value;
    input.value = " ";
    input.value = value ? value[0] : " ";

    let fieldIndex = input.dataset.index;
    if (e.key>=0 && e.key<=9) {
        input.nextElementSibling.focus();
    }

    if (e.key === "Backspace" && fieldIndex > 0) {
        input.previousElementSibling.focus();
    }

    if ( fieldIndex == input.length - 1) {
        submit();
    }
}

function submit() {
    console.log("Submitting....!");
    let otp = "";
    inputs.forEach((input) => {
        otp += input.value;

    });
    console.log(otp);
}

