
console.log("Script loaded");
function fadeAwayAlert() {
    $("#FlashcardsUpdatedAlert").addClass("out");
}

window.setTimeout(function () {
    fadeAwayAlert();
}, 3000);