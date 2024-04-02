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
var imageUrlLight = '/images/silicone-logo-light_theme.svg';
var imageUrlDark = '/images/silicone-logo-dark_theme.svg';

var imageGetCourses = '/images/courses/illustration.svg';
var imageGetCoursesDark = '/images/courses/illustration_dark.png';


const isDarkMode = localStorage.getItem('darkMode') === 'true';

switchMode.checked = isDarkMode;


if (isDarkMode) {
    body.classList.add('dark-mode');
} else {
    body.classList.remove('dark-mode');
}

switchMode.addEventListener('change', toggleDarkMode);

function toggleDarkMode() {
    if (switchMode.checked) {
        body.classList.add('dark-mode');
        localStorage.setItem('darkMode', 'true');
        imgElem.setAttribute("src", imageUrlDark);
        imgGet.setAttribute("src", imageGetCoursesDark);
    } else {
        body.classList.remove('dark-mode');
        localStorage.setItem('darkMode', 'false');
        imgElem.setAttribute("src", imageUrlLight);
        imgGet.setAttribute("src", imageGetCourses);
    }
}

if (switchMode.checked) {
    imgElem.setAttribute("src", imageUrlDark);
    imgGet.setAttribute("src", imageGetCoursesDark);
} else {
    imgElem.setAttribute("src", imageUrlLight);
    imgGet.setAttribute("src", imageGetCourses);
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