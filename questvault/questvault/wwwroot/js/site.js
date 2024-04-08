// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// avatar size limit
$(document).ready(function () {
    $('#avatarForm').submit(function (e) {
        var fileSize = $('#avatarInput')[0].files[0].size; // Get file size
        var maxSize = 5 * 1024 * 1024; // 5MB in bytes

        if (fileSize > maxSize) {
            alert('File size exceeds the maximum limit of 5MB.');
            e.preventDefault(); // Prevent form submission
        }
    });
});

document.getElementById("avatarInput").onchange = function () {
  document.getElementById("fileName").innerText = this.files[0].name;
};