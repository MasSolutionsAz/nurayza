$(document).ready(() => {
    const loginForm = $('#loginForm');
    if (loginForm) {
        loginForm.submit((e) => {
            e.preventDefault();
            if ($(e.target).valid()) {
                $.ajax({
                    url: '/account/login',
                    type: 'POST',
                    data: $(e.target).serialize(),
                    success: (response) => {
                        if (response.hasOwnProperty('status')) {
                            if (response.status == 200) {
                                location.reload();
                            }
                            else if (response.status == 400) {
                                if (response.hasOwnProperty('errors')) {
                                    const errors = response.errors
                                    for (var error in errors) {
                                        if (error)
                                            $(`#${error} + span`).text(errors[error]);
                                        else
                                            $('#loginFormValidation').text(errors[error]);
                                    }
                                }
                                else {
                                    alert(response.error);
                                }
                            }
                        }
                    }
                });
            }
        });

        //$(".external-login").click(function (e) {
        //    e.preventDefault();
        //    var win = window.open($(this).attr("href"), 'Popup Window',
        //        'width=600, height=600, top=70, left=100, resizable=1, menubar=yes', true);
        //    win.focus();
        //})
    }

    //function externalLogin(a,e) {
    //    e.preventDefault();
    //    alert(a.getAttribute("href"));
    //}
});