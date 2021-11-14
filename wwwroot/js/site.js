$(document).ready(() => {
    postForm = form => {
        try {
            $.ajax({
                type: "POST",
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    // if (res.isValid) {
                    //     $('#viewAll').html(res.html)
                    //     $('#form-modal').modal('hide');
                    // }
                },
                error: function (err) {
                    console.log(err);
                }
            });

            return false;
        }
        catch (ex) {
            console.log(ex);
        }
    };
});