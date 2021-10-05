function handleCategoryCollapse() {
    let expandAllBtn = document.querySelector('.expand-all-btn')
let plusBtn = document.querySelectorAll('.collapse-btn span');
let categories = document.querySelectorAll('.clothes-category-main .collapse-btn');
let subitems = document.querySelectorAll('.category-subitems');
for(let i = 0;i<plusBtn.length;i++){

    categories[i].addEventListener('click' , (event)=>{
        event.preventDefault()
        if (event.currentTarget.parentElement.parentElement.children[1].classList.contains('invisible')) {
            event.currentTarget.parentElement.parentElement.children[1].classList.remove('invisible');
            event.currentTarget.children[0].innerText = '-';
            event.currentTarget.children[0].classList.add('collapse-minus')

        }
        else {
            event.currentTarget.parentElement.parentElement.children[1].classList.add('invisible');
            event.currentTarget.children[0].innerText = '+';
            event.currentTarget.children[0].classList.remove('collapse-minus')

        }
    })

}
expandAllBtn.addEventListener('click', (e)=>{
    e.preventDefault()

    if (expandAllBtn.classList.contains('expand-all-clicked')){
        expandAllBtn.innerText = 'Expand All'

        expandAllBtn.classList.remove('expand-all-clicked')
        for (let j=0;j<plusBtn.length;j++){
            subitems[j].classList.add('invisible');
            plusBtn[j].classList.remove('collapse-minus');
            plusBtn[j].innerText = '+';
        }

    }
    else {
        expandAllBtn.classList.add('expand-all-clicked')
        expandAllBtn.innerText = 'Shrink All'
        for (let k=0;k<plusBtn.length;k++){
            subitems[k].classList.remove('invisible');
            plusBtn[k].classList.add('collapse-minus');
            plusBtn[k].innerText = '-';
        }
    }
})
}

handleCategoryCollapse()