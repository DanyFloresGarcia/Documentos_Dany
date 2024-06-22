var app = app || {};
app.error = app.error || {};

$(document).ready(function () {
    app.error.resize(".errorHtml");
});

//#region "REDIMENSIONAR VENTANA"
app.error.resize = function (className) {
    let altoPagina = $(window).height() - 10;
    $(className).css("height", altoPagina + "px");

    $(window).resize(function () {
        let altoPagina = $(window).height() - 10;
        $(className).css("height", altoPagina + "px");
    });
};
//#endregion