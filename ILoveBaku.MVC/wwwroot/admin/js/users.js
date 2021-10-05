let pageCount = 20;

function createPaginationMenu() {
    let container = document.createElement('div');
    container.classList.add('pagination-container');
    let firstBtn = document.createElement('a');
    firstBtn.innerText = 'First';
    firstBtn.classList.add('first-last-btn')
    container.appendChild(firstBtn)
    let lastBtn = document.createElement('a');
    lastBtn.innerText = 'Last';
    lastBtn.classList.add('first-last-btn')

    let prevBtn = document.createElement('a');
    let nextBtn = document.createElement('a');
    nextBtn.innerText = '>';
    prevBtn.innerText = '<';
    nextBtn.classList.add('pagination-btn');
    prevBtn.classList.add('pagination-btn')
    container.appendChild(prevBtn);
    if (pageCount <= 5) {
        for (let i = 1; i <= pageCount; i++) {
            let pageNum = document.createElement('a');
            pageNum.classList.add('pagination-btn')
            pageNum.innerText = i.toString();
            container.appendChild(pageNum)
        }

    } else if (pageCount > 5) {
        let pageNum1 = document.createElement('a');
        pageNum1.innerText = '1';
        let pageNum2 = document.createElement('a');
        pageNum2.innerText = '2';
        let mid = document.createElement('a');
        mid.innerText = '...';
        let pageNum4 = document.createElement('a');
        pageNum4.innerText = (pageCount - 1).toString();
        let pageNum5 = document.createElement('a');
        pageNum5.innerText = pageCount.toString();
        pageNum1.classList.add('pagination-btn');
        pageNum2.classList.add('pagination-btn')
        mid.classList.add('pagination-btn')
        pageNum4.classList.add('pagination-btn')
        pageNum5.classList.add('pagination-btn')
        container.append(pageNum1, pageNum2, mid, pageNum4, pageNum5)

    }

    container.appendChild(nextBtn);
    container.appendChild(lastBtn)
    document.querySelector('.main-body').appendChild(container);
    let links = document.querySelectorAll('.pagination-container a');

    for (let j = 0; j < links.length; j++) {
        links[j].href = '#'
    }
}

if(document.querySelector('.news-wrapper') || document.querySelector('.users-table')){
    //createPaginationMenu()
}


//function handleLeftMenu() {
//    let leftMenu = document.querySelector('.left-menu-collapsed');
//    let btn = document.querySelector('.burger-button');
//    let logo = document.querySelector('.left-menu-logo');
//    let logoWrapper = document.querySelector('.left-menu-logo-wrapper');
//    let icons = document.querySelectorAll('.left-menu-item-icon')
//    let titles = document.querySelectorAll('.left-menu-item-title')
//    let submenus = document.querySelectorAll('.submenu');
//    let menuItems = document.querySelectorAll('.left-menu-item');
//    let menuItemsWthSub = document.querySelectorAll('.contains-submenu');
//    let expandBtns = document.querySelectorAll('.submenu-expand-btn');
//    btn.addEventListener('click', () => {
//        if (!document.querySelector('.left-menu-collapsed').classList.contains('left-menu-expanded')) {
//            leftMenu.classList.add('left-menu-expanded');
//            logo.src = '/admin/img/logo.png'
//            logoWrapper.style.width = '95px';
//            for (let i = 0; i < icons.length; i++) {
//                icons[i].style.margin = '0px 20px 0px 0px';
//                titles[i].style.display = 'inline-block';
//                menuItems[i].style.marginRight = '5px'
//            }

//            for (let j = 0; j < submenus.length; j++) {
//                // submenus[j].style.display = 'block';
//                menuItemsWthSub[j].classList.remove('contains-submenu')
//                expandBtns[j].style.display = 'block'


//            }
//            document.querySelector('.left-menu-items-wrapper').style.alignItems = 'flex-start';
//            document.querySelector('.left-menu-items-wrapper').style.overflowY = 'scroll'
//            document.querySelector('.logout-txt').style.display = 'inline-block';


//        } else {
//            leftMenu.classList.remove('left-menu-expanded');
//            logo.src = '/admin/img/logo-small.png';
//            logoWrapper.style.width = "40px";
//            for (let k = 0; k < icons.length; k++) {
//                icons[k].style.margin = '0px 20px';
//                titles[k].style.display = 'none';
//                menuItems[k].style.marginRight = '0px';

//            }

//            for (let j = 0; j < submenus.length; j++) {
//                submenus[j].classList.add('submenu-invisible')
//                menuItemsWthSub[j].classList.add('contains-submenu');
//                expandBtns[j].style.display = 'none'
//                expandBtns[j].classList.remove('expand-btn-up')
//                expandBtns[j].classList.remove('expand-btn-down')

//            }
//            document.querySelector('.left-menu-items-wrapper').style.alignItems = 'flex-start';
//            document.querySelector('.left-menu-items-wrapper').style.overflowY = 'visible'

//            document.querySelector('.logout-txt').style.display = 'none';
//        }

//    })


//}


//handleLeftMenu()

function handleSubmenuExpand() {
    if (document.querySelector('.with-submenu')) {
    let buttons = document.querySelectorAll('.with-submenu');

        buttons.forEach(item => {
            item.addEventListener('click', e => {
                e.currentTarget.classList.toggle('with-submenu-rotated');
                e.currentTarget.children[1].classList.toggle('submenu-invisible')
            })
        })
    }

    
    //buttons.forEach(item=>{
    //item.addEventListener('click', (event)=>{
    //    event.preventDefault()
    //    if (event.currentTarget.parentElement.parentElement.children[1].classList.contains('submenu-invisible')){
    //        event.currentTarget.parentElement.parentElement.children[1].classList.remove('submenu-invisible');
    //        event.currentTarget.classList.add('expand-btn-up');
    //        event.currentTarget.classList.remove('expand-btn-down')
    //    }
    //    else{
    //        event.currentTarget.parentElement.parentElement.children[1].classList.add('submenu-invisible');
    //        event.currentTarget.classList.remove('expand-btn-up')
    //        event.currentTarget.classList.add('expand-btn-down')

    //    }

    //})
    //})
}

handleSubmenuExpand()





