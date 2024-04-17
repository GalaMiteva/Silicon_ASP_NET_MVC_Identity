const toggleMenu = () => {
    document.getElementById('menu').classList.toggle('hide');
    document.getElementById('account-buttons').classList.toggle('hide');
}

const checkScreenSize = () => {
    if (window.innerWidth >= 1200) {
        document.getElementById('menu').classList.remove('hide');
        document.getElementById('account-buttons').classList.remove('hide');
    }
    else {
        if (!document.getElementById('menu').classList.contains('hide'))
            document.getElementById('menu').classList.add('hide');

        if (!document.getElementById('account-buttons').classList.contains('hide'))
            document.getElementById('account-buttons').classList.add('hide');
    }
}

window.addEventListener('resize', checkScreenSize);
checkScreenSize();











const switchMode = document.getElementById('switch-mode');
const imgElem = document.getElementById('logo');
const imgGet = document.getElementById('illustration');


const body = document.body;
var logoUrlLight = '/images/silicone-logo-light_theme.svg';
var logoUrlDark = '/images/silicone-logo-dark_theme.svg';

var imageGetCourses = '/images/courses/illustration.svg';
var imageGetCoursesDark = '/images/courses/illustration_dark.png';


const isDarkMode = localStorage.getItem('darkMode') === 'true';

switchMode.checked = isDarkMode;


if (isDarkMode) {
    body.classList.add('dark-mode');
    imgElem.setAttribute("src", logoUrlDark);
} else {
    body.classList.remove('dark-mode');
    imgElem.setAttribute("src", logoUrlLight);
}

switchMode.addEventListener('change', toggleDarkMode);

function toggleDarkMode() {
    if (switchMode.checked) {
        body.classList.add('dark-mode');
        localStorage.setItem('darkMode', 'true');
        imgElem.setAttribute("src", logoUrlDark);
        imgGet.setAttribute("src", imageGetCoursesDark);
    } else {
        body.classList.remove('dark-mode');
        localStorage.setItem('darkMode', 'false');
        imgElem.setAttribute("src", logoUrlLight);
        imgGet.setAttribute("src", imageGetCourses);
    }
}




/*********************************  Cookie - Dark Mode  **********************************************' */
//document.addEventListener("DOMContentLoaded", function () {
//    let sw = document.querySelector('#switch-mode')

//    sw.addEventListener('change', function () {
//        let theme = this.checked ? "dark-mode" : "light-mode"

//        fetch(`/sitesettings/changetheme?mode=${theme}`)
//            .then(res => {
//                if (res.ok)
//                    window.location.reload()
//                else
//                console.log('something')
//            })
//    })
//})

/*********************************************************************************** */


document.addEventListener('DOMContentLoaded', function () {
    select();
    searchQuery();
});

function select() {
    let selectContainer = document.querySelector('.select');
    let selected = selectContainer.querySelector('.selected');
    let selectOptions = selectContainer.querySelector('.select-options');

    try {
        selectContainer.addEventListener('click', function () {
            selectOptions.style.display = (selectOptions.style.display === 'block' ? 'none' : 'block');
        });

        let options = selectOptions.querySelectorAll('.option');
        options.forEach(function (option) {
            option.addEventListener('click', function () {
                selected.innerHTML = this.textContent;
                selectOptions.style.display = 'none';
                let category = this.getAttribute('data-value');
                selected.setAttribute('data-value', category);
                upddateCourseByFilters();
            });
        });

    } catch (error) {
        console.error(error);
    }
}

function searchQuery() {
    try {
        document.querySelector('#searchQuery').addEventListener('keyup', function () {
            upddateCourseByFilters()
        })

    }
    catch
    {
        console.error(error);
    }
}

function upddateCourseByFilters() {

    const category = document.querySelector('.select .selected').getAttribute('data-value') || 'all';
    const searchQuery = document.querySelector('#searchQuery').value

    const url = (`/courses/index?category=${encodeURIComponent(category)}&searchQuery=${encodeURIComponent(searchQuery)}`);


    fetch(url)
        .then(res => res.text())
        .then(data => {
            const parser = new DOMParser();
            const dom = parser.parseFromString(data, 'text/html');
            document.querySelector('.grid').innerHTML = dom.querySelector('.grid').innerHTML;

            const pagination = dom.querySelector('.pagination') ? dom.querySelector('.pagination').innerHTML : ''
            document.querySelector('.pagination').innerHTML = pagination;
        })
        .catch(err => console.error(err));
}

document.addEventListener('DOMContentLoaded', function () {
    handleProfileImageUpload();
})

function handleProfileImageUpload() {
    try {
        let fileUploader = document.querySelector('#fileUploader');
        if (fileUploader != undefined) {
            fileUploader.addEventListener('change', function () {
                if (this.files.length > 0) {
                    this.form.submit();
                }
            })
        }

    }
    catch (error) {
        console.error(error);
    }
}


const myTimeout = setTimeout(alert, 5000);


function alert() {
    const alertsSucces = document.querySelector('.alert-success')
    const alertsDanger = document.querySelector('.alert-danger')
    const alertsWarning = document.querySelector('.alert-warning')
    const alert = document.querySelector('.alert')
    const message = document.getElementById('statusMessage')
    alert.classList.add('hide')
    alertsSucces.classList.add('hide')
    alertsDanger.classList.add('hide')
    alertsWarning.classList.add('hide')
    message.classList.add('hide')
}