$(document).ready(() => {
    document.querySelector('.phone-number').addEventListener('input', (e) => {
        e.currentTarget.isDelete = e.currentTarget.value < e.currentTarget.oldValue;
        if (operatorValidate(e.currentTarget)) {
            e.currentTarget.nextElementSibling.innerText = "";
            e.currentTarget.oldValue = e.currentTarget.value;
        }
        else {
            e.currentTarget.value = (e.currentTarget.hasOwnProperty('oldValue')) ?
                e.currentTarget.oldValue : e.currentTarget.defaultValue;
        }
    });

    $('#contactSendForm').submit((e) => {
        e.preventDefault();
        $.ajax({
            url: '/contact/send',
            type: 'POST',
            data: $(e.target).serialize(),
            success: (response) => {
                if (response.hasOwnProperty('status')) {
                    switch (response.status) {
                        case 200:
                            alert('Sorğunuz göndərildi.'); break;
                        case 400:
                            alert(response.error); break;
                    }
                }
            }
        });
    });
});