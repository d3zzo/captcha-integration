$(function () {
    var el = $('#captcha').visualCaptcha({
        imgPath: '/Content/img/',
        captcha: {
            numberOfImages: 5,
            routes: { start: "/Home/Start/", image: "/Home/Image/", audio: "/Home/Audio/" },
            callbacks: {
                loading: function (captcha) {
                    console.log('I am loading.', captcha);
                },
                loaded: function (captcha) {
                    console.log('I am loaded.', captcha);
                }
            }
        },

    });

    // use next code for getting captcha object
    var captcha = el.data('captcha');
    captchaSubmit(captcha)
});

function captchaSubmit(captcha) {
    $('#submitBtn').click(function () {
        //var capTest = captcha.getCaptchaData();
        //debugger;

        //console.log('I am clicked.', captcha);
        //alert('asd');
        //$.ajax({
        //    url: '/Home/Try/',
        //    type: 'POST',
        //    data: capTest,
        //    contentType: 'application/json',
        //    success: function (response) {
        //        if (response != null) {
        //            alert("Response :" + response.verified);
        //        } else {
        //            alert("Something went wrong");
        //        }
        //    },


        //});


        //$("#formToSubmit").submit(function (event) {
        //    event.preventDefault(); //prevent default action 
        //    var post_url = $(this).attr("action"); //get form action url
        //    var request_method = $(this).attr("method"); //get form GET/POST method
        //    var form_data = $(this).serialize(); //Encode form elements for submission
        //    $.ajax({
        //        url: '/Home/Try/',
        //        type: request_method,
        //        data: form_data,
        //        success: function (data, status) {
        //            debugger;
        //            $.each(data.Errors, function (key, value) {
        //                if (value != null) {
        //                    $("#Err_" + key).html(value[value.length - 1].ErrorMessage);
        //                }
        //            });
        //            alert('test3');
        //        },
        //        complete: function (result, status) {
        //            debugger;
        //            alert('test1');
        //        }
        //    });
        //});



        $("#formToSubmit").submit(function (event) {
            //event.preventDefault(); //prevent default action 
            debugger;
            var post_url = $(this).attr("action"); //get form action url
            var request_method = $(this).attr("method"); //get form GET/POST method
            var form_data = $(this).serialize(); //Encode form elements for submission
            $.ajax({
                url: '/Home/Try/',
                type: request_method,
                data: form_data,
                success: function (data, status) {
                    debugger;
                    if (data.isRedirect != null) {
                        window.location.href = data.redirectUrl;
                    }
                    if (data.Valid != null && data.Valid == false) {

                    }
                    else {
                        event.preventDefault();
                    }
                },
            });
        });



    })
}