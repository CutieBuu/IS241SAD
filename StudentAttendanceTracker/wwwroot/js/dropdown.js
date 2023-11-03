"use strict";
var $ = function (id) { return document.getElementById(id); };

// the event handler for the click event of each button
var toggle = function () {
    var button = this;                    // clicked button     
    var div = button.nextElementSibling;  // button's sibling div tag
    var arr = document.querySelectorAll("button.button-dropdown");

    div.classList.toggle("student-dropdown");

    // allows for only one div to be displayed at a time
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] !== button) {
            arr[i].nextElementSibling.setAttribute("class", "student-dropdown");
        }
    }
};

window.onload = function () {
    var cmain = $("cmain");
    var buttons = document.getElementsByClassName("button-dropdown");

    // attach event handler for each button    
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].onclick = toggle;
    }
};