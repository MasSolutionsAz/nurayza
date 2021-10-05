let imgUploadContainer = document.querySelector('.main-img-upload');
let uploadMainImgInput = document.querySelector('#upload-img-main');
let events = ['dragover', 'dragenter', 'dragleave', 'drop'];
let droppedFiles;
let rotateLeft = document.querySelector('.rotate-left-btn');
let rotateRight = document.querySelector('.rotate-right-btn');
function overrideDefaultDrop(event) {
    event.preventDefault();
    event.stopPropagation()
}
var array = [];
var deleteArray = [];
events.forEach(eventName => {
    imgUploadContainer.addEventListener(eventName, overrideDefaultDrop);
});

imgUploadContainer.addEventListener('dragover', fileHover);
imgUploadContainer.addEventListener('dragenter', fileHover);
imgUploadContainer.addEventListener('dragleave', endHover);
imgUploadContainer.addEventListener('drop', endHover);
imgUploadContainer.addEventListener('drop', evt => {
    addFiles(evt, '.main-img-upload');

});
function rotateMainImg(left, right) {
    let rotated = 0;
    left.addEventListener('click', ev => {
        rotated += 90;
        ev.currentTarget.parentElement.parentElement.parentElement.style.transform = `rotate(${rotated}deg)`;
        ev.currentTarget.parentElement.parentElement.style.transform = `rotate(${-rotated}deg)`;

    })
    right.addEventListener('click', event => {
        rotated -= 90;
        event.currentTarget.parentElement.parentElement.parentElement.style.transform = `rotate(${rotated}deg)`;
        event.currentTarget.parentElement.parentElement.style.transform = `rotate(${-rotated}deg)`;
    })
}
rotateMainImg(rotateLeft, rotateRight);
function setDetails(name, size) {
    let details = document.querySelector('.main-upload-details-uploading');
    let detName = document.querySelector('.upload-details-file-name');
    let detSize = document.querySelector('.file-size');
    detName.innerText = name;
    detSize.innerText = `${size}MB of ${size}MB`;
    details.style.display = 'inline-block';
}

function fileHover(event) {
    event.currentTarget.classList.add('file-hover');
}

function endHover(event) {
    event.currentTarget.classList.remove('file-hover');
}

function clearContainer(className) {
    document.querySelector(className).classList.add('img-dropped');

}

function addFiles(event, container) {
    droppedFiles = event.dataTransfer.files;
    handleFiles(droppedFiles, container);
}

function previewFileMain(file, containerClass) {
    let reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onloadend = function () {
        let container = document.querySelector(containerClass);
        container.style.backgroundImage = `url(${reader.result})`
    }

}

function handleFiles(files, container) {
    files = [...files];
    let isImg = false;
    files.forEach(file => {
        isImg = file.type.includes('image');
    });
    if (isImg) {
        files.forEach(item => {
            previewFileMain(item, container);
            setDetails(item.name, Number(item.size / 1000000).toFixed(3));
        });
        files.forEach(gedImgSizeDrop);
        clearContainer(container);

    }
}

function gedImgSizeDrop(file) {
    let size = Number(file.size / 1000000).toFixed(2);

}

uploadMainImgInput.addEventListener('change', () => {
    imgUploadContainer.classList.add('img-dropped');

});
uploadMainImgInput.addEventListener('change', readURL);

function readURL(input) {
    if (input.currentTarget.files && input.currentTarget.files[0]) {
        AddToArray(input.target.files[0], true);
        let reader = new FileReader();
        reader.readAsDataURL(input.target.files[0]);

        reader.onload = function (event) {
            let container = document.querySelector('.main-img-upload');
            container.style.backgroundImage = `url(${event.target.result})`;

        }
        $(".main-img-upload-label span").text(input.target.files[0].name);
        $(".main-img-upload-label span").attr("data-old", "false");
        setDetails(input.currentTarget.files[0].name, Number(input.currentTarget.files[0].size / 1000000).toFixed(3))
    }
}

function getImgSizeClick(input) {
    let size = Number(input.target.files[0].size / 1000000).toFixed(2);
}

// OTHER IMAGES UPLOAD


let otherImagesDropzone = document.querySelector('.other-images-upload');
let uploadImgOtherInput = document.querySelector('#upload-img-other');
let imagesContainer = document.querySelector('.other-uploaded-images-container')
let otherDroppedFiles;
events.forEach(eventName => {
    otherImagesDropzone.addEventListener(eventName, overrideDefaultDrop);
});

otherImagesDropzone.addEventListener('dragover', fileHover);
otherImagesDropzone.addEventListener('dragenter', fileHover);
otherImagesDropzone.addEventListener('dragleave', endHover);
otherImagesDropzone.addEventListener('drop', endHover);
otherImagesDropzone.addEventListener('drop', addFilesOther);


function addFilesOther(event) {
    otherDroppedFiles = event.dataTransfer.files;
    handleOtherFiles(otherDroppedFiles, '.other-images-upload');
    event.currentTarget.classList.add('other-images-upload-after-drop')
}

function createUploadItem(imgSrc, name, size) {
    let itemWrapper = document.createElement('div');
    itemWrapper.classList.add('uploaded-item-wrapper');
    let item = document.createElement('div');
    item.classList.add('uploaded-item');
    let itemImgWrapper = document.createElement('div');
    itemImgWrapper.classList.add('uploaded-item-img-wrapper');
    itemImgWrapper.style.backgroundImage = `url(${imgSrc})`;

    let hoverBg = document.createElement('div');
    hoverBg.classList.add('main-img-hover');
    hoverBg.classList.add('visible');
    let editUploadBtn = document.createElement('img');
    editUploadBtn.src = '/admin/img/delete-upload.png';
    editUploadBtn.classList.add('edit-upload-btn');

    let rotateBtnWrapper = document.createElement('div');
    rotateBtnWrapper.classList.add('rotate-buttons');
    let rotateLeft = document.createElement('img');
    rotateLeft.src = '/admin/img/upload-rotate-left.png';
    rotateLeft.classList.add('rotate-left-btn');
    let rotateRight = document.createElement('img');
    rotateRight.classList.add('rotate-right-btn');
    rotateRight.src = '/admin/img/upload-rotate-right.png';
    rotateBtnWrapper.append(rotateLeft, rotateRight);
    hoverBg.append(editUploadBtn, rotateBtnWrapper);
    itemImgWrapper.append(hoverBg);

    let itemDetails = document.createElement('div');
    itemDetails.classList.add('main-upload-details-uploading');
    itemDetails.classList.add('other-upload-details-uploading');
    let itemName = document.createElement('p');
    itemName.classList.add('upload-details-file-name');
    itemName.setAttribute("data-old", "false");
    itemName.innerText = name;
    let fileSizeWrapper = document.createElement('div');
    fileSizeWrapper.classList.add('file-size-wrapper');
    let fileSize = document.createElement('p');
    fileSize.classList.add('file-size');
    fileSize.innerText = `${size}MB of ${size}MB`;
    let cancelBtn = document.createElement('button');
    cancelBtn.classList.add('cancel-upload-btn');
    let cancelBtnTxt = document.createElement('p');
    cancelBtnTxt.innerText = '+';
    cancelBtn.append(cancelBtnTxt);
    let deleteBtn = document.createElement('img');
    deleteBtn.classList.add('delete-upload-btn');
    deleteBtn.src = '/admin/img/delete-upload.png';
    fileSizeWrapper.append(fileSize, cancelBtn, deleteBtn);
    let progressBar = document.createElement('progress');
    progressBar.value = 100;
    progressBar.max = 100;
    progressBar.classList.add('upload-progress');
    itemDetails.append(itemName, fileSizeWrapper, progressBar);
    item.append(itemImgWrapper, itemDetails);
    itemWrapper.append(item);
    imagesContainer.append(itemWrapper);
    itemImgWrapper.addEventListener('mouseover', (e) => {
        e.currentTarget.children[0].style.display = 'flex'
    })
    itemImgWrapper.addEventListener('mouseleave', (e) => {
        e.currentTarget.children[0].style.display = 'none'
    })
    editUploadBtn.addEventListener('click', ev => {
        ev.currentTarget.parentElement.parentElement.parentElement.parentElement.remove(ev.currentTarget.parentElement.parentElement.parentElement)
    })

    rotateMainImg(rotateLeft, rotateRight);
}

function previewFileOthers(file) {
    let reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onloadend = function () {
        createUploadItem(reader.result, file.name, Number(file.size / 1000000).toFixed(3))
    }
}

function handleOtherFiles(files) {
    files = [...files];
    let isImg = false;
    files.forEach(file => {
        isImg = file.type.includes('image');
    })
    if (isImg) {
        files.forEach(item => {
            previewFileOthers(item)
        });
        files.forEach(gedImgSizeDrop);
    }
}

uploadImgOtherInput.addEventListener('change', (event) => {
});
uploadImgOtherInput.addEventListener('change', e => {
    otherImagesDropzone.classList.add('other-images-upload-after-drop');
});
uploadImgOtherInput.addEventListener('change', readURLOther);

function readURLOther(input) {
    if (input.currentTarget.files && input.currentTarget.files[0]) {
        for (let i = 0; i < input.currentTarget.files.length; i++) {
            AddToArray(input.currentTarget.files[i], false);
            let reader = new FileReader();
            let myInput = input.currentTarget.files[i];


            reader.readAsDataURL(input.currentTarget.files[i]);
            reader.onload = function (event) {
                createUploadItem(reader.result, myInput.name, Number(myInput.size / 1000000).toFixed(3));
            }
        }
    }
}

function AddToArray(file, isMain) {
    array.push(new PhotoObject(isMain, file));
}

function RemoveFromArray(name) {
    for (var i = 0; i < array.length; i++) {
        if (array[i].file.name == name) {
            array.splice(i, 1);
        }
    }

}
function AddToDeleteArray(name) {
    deleteArray.push(name);
}
function PhotoObject(IsMain, File) {
    return { isMain: IsMain, file: File }
}




function CreateFormData(e) {
    e.preventDefault();

    var formData = new FormData();
    var counter = 0;
    for (var arr of array) {
        for (var a in arr) {
            formData.append(`photos[${counter}].${a}`, arr[a]);
        }
        counter++;
    }

    return formData;
}
function ErrorModal(text) {
    $(".error-msg-wrapper").css("display", "flex");
    $(".success-msg-wrapper").css("display", "none");
    $(".error-msg-wrapper").find(".msg-details").html(text);
}
function SuccessModal() {
    $(".error-msg-wrapper").css("display", "none");
    $(".success-msg-wrapper").css("display", "flex");
}
function toFormData(obj, form, namespace) {
    let fd = form || new FormData();
    let formKey;
    for (let property in obj) {
        if (obj.hasOwnProperty(property) && obj[property]) {
            if (namespace && Array.isArray(obj)) {
                formKey = namespace + '[' + property + ']';
            }
            else if (namespace) {
                formKey = namespace + '.' + property;
            }
            else {
                formKey = property;
            }

            // if the property is an object, but not a File, use recursivity.
            if (obj[property] instanceof Date) {
                fd.append(formKey, obj[property].toISOString());
            }
            else if (typeof obj[property] === 'object' && !(obj[property] instanceof File)) {
                toFormData(obj[property], fd, formKey);
            } else { // if it's a string or a File object
                fd.append(formKey, obj[property]);
            }
        }
    }

    return fd;
}


$(".other-uploaded-images-container").on("mouseover", ".uploaded-item-img-wrapper", function (e) {
    e.currentTarget.children[0].style.display = 'flex'

});

$(".other-uploaded-images-container").on("mouseleave", ".uploaded-item-img-wrapper", function (e) {
    e.currentTarget.children[0].style.display = 'none'
});

$(document).on("click", ".uploaded-item-wrapper .edit-upload-btn", function (ev) {
    if ($(this).parents(".uploaded-item").find(".upload-details-file-name").attr("data-old") == "true") {
        AddToDeleteArray($(this).parents(".uploaded-item").find(".upload-details-file-name").text());
    }
    else {
        RemoveFromArray($(this).parents(".uploaded-item").find(".upload-details-file-name").text());
    }
    ev.currentTarget.parentElement.parentElement.parentElement.parentElement.remove(ev.currentTarget.parentElement.parentElement.parentElement)
});

$(".main-img-upload-wrapper .edit-upload-btn").click(function () {
    $(".main-img-upload").css("background-image", "none");
    $(".main-img-upload").removeClass("img-dropped");
    if ($(".main-img-upload-label span").attr("data-old") == "true") {
        AddToDeleteArray($(".main-img-upload-label span").text());
    }
    else {
        RemoveFromArray($(".main-img-upload-label span").text());
    }
});