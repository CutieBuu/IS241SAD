const modal = document.querySelector("dialog");

const open = document.querySelector(".button-open");

const close = document.querySelector(".button-close");


open.addEventListener("click", () => {
  modal.showModal();
});

close.addEventListener("click", () => {
  modal.close();
});


