const toggle = evt => {
    const course = evt.currentTarget;
    const buttonlist = course.nextElementSibling;

    buttonlist.classList.toggle("student-dropdown");

    evt.preventDefault();
};

document.addEventListener("DOMContentLoaded", () => {
    const dropdown = document.querySelectorAll("button.button-main");

    for (let course of dropdown) {
        course.addEventListener("click", toggle);
    }
})