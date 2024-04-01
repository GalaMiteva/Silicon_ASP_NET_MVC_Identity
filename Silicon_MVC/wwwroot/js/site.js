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




//const toggleMenu = () => {
//    document.getElementById('menu').classList.toggle('hide');
//    document.getElementById('account-buttons').classList.toggle('hide');
//}

//const checkScreenSize = () => {
//    if (window.innerWidth >= 1200) {
//        document.getElementById('menu').classList.remove('hide');
//        document.getElementById('account-buttons').classList.remove('hide');
//    }
//    else {
//        if (!document.getElementById('menu').classList.contains('hide'))
//            document.getElementById('menu').classList.add('hide');

//        if (!document.getElementById('account-buttons').classList.contains('hide'))
//            document.getElementById('account-buttons').classList.add('hide');
//    }
//}

//window.addEventListener('resize', checkScreenSize);
//checkScreenSize();

//const switchMode = document.getElementById('switch-mode');
//const body = document.body;

//const isDarkMode = localStorage.getItem('darkMode') === 'true';

//switchMode.checked = isDarkMode;

//if (isDarkMode) {
//    body.classList.add('dark-mode');
//} else {
//    body.classList.remove('dark-mode');
//}

//switchMode.addEventListener('change', toggleDarkMode);


//-----------2var
//document.addEventListener('DOMContentLoaded', function () {
//    var menuButton = document.getElementById('menuToggle');
//    var mobileMenu = document.getElementById('mobileMenu');

//    if (menuButton && mobileMenu) {
//        menuButton.addEventListener('click', function () {
//            console.log('Succe');
//            mobileMenu.classList.toggle('open');
//            var ariaExpanded = mobileMenu.getAttribute('aria-expanded');
//            mobileMenu.setAttribute('aria-expanded', ariaExpanded === 'true' ? 'false' : 'true');
//        });
//    } else {
//        console.error('Error.');
//    }
//});






const switchMode = document.getElementById('switch-mode');
const imgElem = document.getElementById('logo');
const imgGet = document.getElementById('illustration');


//const overview = document.getElementById('overview');
const body = document.body;
var imageUrlLight = '/images/Silicon-Logotype -Light-Mode.svg';
var imageUrlDark = '/images/silicone-logo-dark_theme.svg';

var imageGetCourses = '/images/courses/illustration.svg';
var imageGetCoursesDark = '/images/courses/illustration_dark.png';


const isDarkMode = localStorage.getItem('darkMode') === 'true';

switchMode.checked = isDarkMode;


if (isDarkMode) {
    body.classList.add('dark-mode');
    //overview.classList.add('dark-mode');
} else {
    body.classList.remove('dark-mode');
    //overview.classList.remove('dark-mode');
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
