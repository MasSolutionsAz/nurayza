$(document).ready(() => {
    const registerForm = $('#registerForm');
    if (registerForm) {
        registerForm.submit((e) => {
            e.preventDefault();
            if ($(e.target).valid()) {
                $.ajax({
                    url: '/account/register',
                    type: 'POST',
                    data: $(e.target).serialize(),
                    success: (response) => {
                        if (response.hasOwnProperty('status')) {
                            if (response.status == 200) {
                                location.reload();
                            }
                            else if (response.status == 400) {
                                const errors = response.errors
                                for (var error in errors) {
                                    if (error)
                                        $(`#${error} + span`).text(errors[error]);
                                    else
                                        $('#registerFormValidation').text(errors[error]);
                                }
                            }
                        }
                    }
                });
            }
        });

        if (false) {
            const monthNames =
                [
                    'Yanvar', 'Fevral', 'Mart', 'Aprel',
                    'May', 'İyun', 'İyul', 'Avqust',
                    'Sentyabr', 'Oktyabr', 'Noyabr', 'Dekabr'
                ];

            var yearCount = 50;
            let birthDay = $('#birthDay');
            let birthMonth = $('#birthMonth');
            let birthYear = $('#birthYear');
            let currentYear = new Date().getFullYear() - 14;//minimum yas heddi

            for (let m = 0; m < 12; m++) {
                let option = document.createElement('option');
                option.value = m + 1;
                option.textContent = monthNames[m];
                birthMonth.append(option);
            }

            for (let y = 0; y < yearCount; y++) {
                var option = document.createElement("option");
                option.value = currentYear;
                option.textContent = currentYear;
                birthYear.append(option);
                currentYear--;
            }

            //birthMonth.val(new Date().getMonth());
            birthMonth.on('change', RefreshDays);
            //birthYear.val(new Date().getFullYear() - 14);
            birthYear.on('change', RefreshDays);

            RefreshDays();

            function RefreshDays() {
                var year = birthYear.val();
                var month = parseInt(birthMonth.val());
                birthDay.html('<option value="0" selected>Day</option>');

                //get the last day, so the number of days in that month
                var days = new Date(year, month, 0).getDate();

                //lets create the days of that month
                for (var d = 1; d <= days; d++) {
                    let option = document.createElement('option');
                    option.value = d;
                    option.textContent = d;
                    birthDay.append(option);
                }
            }
        }
    }
});