$(".confirmDelete").on("click", function (e) {
    e.preventDefault();

    if (confirm("Are you sure?")) {
        window.location.href = $(this).attr("href");
    }
})

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#uploadPhotoPreview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#uploadPhoto").change(function () {
    readURL(this);
});

function completedChangingPhoto(a) {
    $("#changedAvatar").slideDown(200);

    setTimeout(function () {
        $("#changedAvatar").slideUp(200);
    },2000)
}
