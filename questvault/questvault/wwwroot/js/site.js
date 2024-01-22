// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const codes = document.querySelectorAll('.code-2fa')

codes[0].focus()

codes.forEach((code, idx) => {
    code.addEventListener('keydown', (e) => {
        if (e.key >= 0 && e.key <= 9) {
            codes[idx].value = ''
            setTimeout(() => codes[idx + 1].focus(), 10)
            document.getElementById("code-input").value = codes[idx].value().toString();
            console.log(codes[0].ToString);
        } else if (e.key === 'Backspace') {
            setTimeout(() => codes[idx - 1].focus(), 10)
        }
        
    })
    code.addEventListener("paste", (e) => {
        const data = e.clipboardData.getData("text");
        const value = data.split("");
        
        if (value.length === codes.length) {
            codes.forEach((code, idx) => (code.value=value[idx]));
        }
    })
})


