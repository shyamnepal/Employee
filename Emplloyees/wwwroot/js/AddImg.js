$(document).ready(function () {
    $('#uploadProfileImageBtn').click(function (e) {
        e.preventDefault();
        $('#imageFile').click(); // Trigger file input dialog
    });
});