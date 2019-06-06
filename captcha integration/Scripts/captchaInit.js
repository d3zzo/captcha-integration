$(function () {
    
    function loadCaptcha() {
        return $('#captcha').visualCaptcha({
            imgPath: '/Content/img/',
            autoRefresh: false,
            captcha: {
                numberOfImages: 5,
                routes: { start: "/Home/Start/", image: "/Home/Image/", audio: "/Home/Audio/" },
            }

        }).data('captcha');
    }

    function captchaSubmit(captcha) {
        $('#submitBtn').click(function (event) {
            if (captcha.getCaptchaData().valid) {
                var dataToSend = captcha.getCaptchaData().value;
                //var request_method = $(this).attr("method"); //get form GET/POST method
                //var form_data = $(this).serialize(); //Encode form elements for submission
                $.ajax({
                    url: '/Home/Try/',
                    type: 'POST',
                    data: { "captcha" : dataToSend },
                    async: false,
                    success: function (data, status) {
                        if (data.Valid != null && data.Valid == false) {
                            event.preventDefault();
                            captcha.refresh();
                            $("#captcha").addClass('input-validation-error');
                            //loadCaptcha();
                            //reloadCaptcha();
                        }
                        else if (data.RedirectUrl != null) {

                        }
                    },
                });
            }
        });

        //$("#formToSubmit").submit(function (event) {
        //    debugger;
        //    //event.preventDefault(); //prevent default action 

        //});
    }

    //var cap = loadCaptcha();
    captchaSubmit(loadCaptcha());
});
            