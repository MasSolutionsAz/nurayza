// show password Btn Click

function handleShowPassClick() {
    let passInput = document.querySelector('.password-input');
    if (passInput.type === 'password'){
        passInput.type = 'text';
    }
    else passInput.type = 'password'
}

let showPassBtn = document.querySelector('.show-password-btn');
showPassBtn.addEventListener('click', handleShowPassClick);
