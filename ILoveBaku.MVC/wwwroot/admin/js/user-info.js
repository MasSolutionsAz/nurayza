function handleTabsChange() {
    let tabs = document.querySelectorAll('.info-tab');
    let contents = document.querySelectorAll('.info-content');
    tabs.forEach(item => {
        item.addEventListener('click', () => {
            tabs.forEach(e => {
                e.classList.remove('info-tab-active');
            });

            var dataId = item.getAttribute("data-name");
            item.classList.add('info-tab-active');
            console.log(dataId);

            contents.forEach(e => {
                e.classList.remove("active");
            });

            var content = document.querySelector(`.info-content[data-name='${dataId}']`);
            content.classList.add("active");
        });
    });
}
if (document.querySelector('.info-content')) {
    handleTabsChange();
}

let messageCloseBtn = document.querySelectorAll('.popup-close-btn');
messageCloseBtn.forEach(item => {
    item.addEventListener('click', event => {
        event.currentTarget.parentElement.parentElement.style.display = 'none'
    })
})