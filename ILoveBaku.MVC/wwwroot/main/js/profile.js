$(document).ready(() => {
    $('#changePasswordForm').submit((e) => {
        e.preventDefault();
        $('#changePasswordForm input[name=Password] + span').text('');
        $.ajax({
            url: '/account/changePassword',
            type: 'PUT',
            data: $(e.target).serialize(),
            success: (response) => {
                if (response.hasOwnProperty('status')) {
                    switch (response.status) {
                        case 200:
                            $('#changePasswordForm input[name=Password]').val('');
                            alert('Password changed.');
                            break;
                        case 400:
                            alert(response.error); break;
                        case 404:
                            alert(response.error); break;
                        case 401:
                            $('#changePasswordForm input[name=Password] + span').text(response.error); break;
                    }
                }
            }
        });
    });
});