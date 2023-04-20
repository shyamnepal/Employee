$(document).ready(function () {
    // Handle change event of file input
    $('#imageFile').on('change', function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#previewImage').attr('src', e.target.result);
        }
        reader.readAsDataURL(file);
    });
});