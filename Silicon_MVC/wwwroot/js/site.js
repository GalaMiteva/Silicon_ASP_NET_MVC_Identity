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

/*********************************************************************************** */


document.addEventListener('DOMContentLoaded', function () {
    const dropdown_button= document.querySelector('.dropdown-btn');
    const courseSelect = document.getElementById('courseSearchInput');
    const courseSearchButton = document.getElementById('courseSearchButton');
    const filterDropdown = document.querySelector('.dropdown-content');

    dropdown_button.addEventListener('click', function () {
        this.nextElementSibling.classList.toggle('show');
    });

    window.DomContentLoaded = function () {
        const urlParams = new URLSearchParams(window.location.search);
        const category = urlParams.get('category');

        if (category !== null) {
            CategorySelect.value = category;
        }
    };

    dropdown_button.addEventListener('change', function () {
        localStorage.setItem('selectedCategory', this.value);
    });

    const savedCategory = localStorage.getItem('selectedCategory');
    if (savedCategory !== null) {
        CategorySelect.value = savedCategory;
    }

    courseSearchButton.addEventListener('click', searchCourses);
    courseSelect.addEventListener('keypress', function (event) {
        if (event.key === 'Enter') {
            searchCourses();
        }
    });

    function searchCourses() {
        const searchTerm = courseSelect.value.trim().toLowerCase();
        const courseCards = document.querySelectorAll('.course-card');

        courseCards.forEach(function (card) {
            const title = card.querySelector('.course-card-content h2').innerText.toLowerCase();
            const author = card.querySelector('.course-card-content p').innerText.toLowerCase();

            if (title.includes(searchTerm) || author.includes(searchTerm)) {
                card.style.display = 'block'; 
            } else {
                card.style.display = 'none'; 
            }
        });
    }



    filterDropdown.addEventListener('click', function (event) {
        const filterOption = event.target.dataset.filter;
        if (filterOption) {
            filterCourses(filterOption);
        }
    });

    function filterCourses(filterOption) {
        const courseCards = document.querySelectorAll('.course-card');
        courseCards.forEach(function (card) {
            if (filterOption === 'bestsellers' && card.querySelector('.bestseller')) {
                card.style.display = 'block';
            } else if (filterOption === 'non-bestsellers' && !card.querySelector('.bestseller')) {
                card.style.display = 'block';
            } else if (filterOption === 'all') {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        });
    }
});