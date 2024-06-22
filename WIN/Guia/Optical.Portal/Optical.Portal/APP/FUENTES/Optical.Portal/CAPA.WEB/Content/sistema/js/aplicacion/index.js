var Aplicacion = Aplicacion || {};
var $fq = $fq || {};

$(document).ready(function () {
    Aplicacion.Inicial();
});

Aplicacion = {
    Inicial: function () {
        Aplicacion.ListaDeAplicaciones();
        Aplicacion.DatosUsuarioSesion();
    },
    AbrirApp: function (rutaApp) {
        let ruta = `${window.location.protocol}//${window.location.hostname}${$fq.port}` + rutaApp;
        window.open(ruta, '_blank');
    },
    HoverApp: function () {
        $(".aplicacion").on({
            mouseenter: function () {
                $(this).find("#ico-ver-aplicacion").removeClass("hidden-tran").addClass("visible-tran");
                $(this).find("#url-ver-aplicacion").removeClass("visible-tran").addClass("hidden-tran");
                $(this).find('#text-aplicacion').removeClass("fadeInUp").addClass('fadeOutDown animated');
                $(this).find("#img-aplicacion").removeClass().addClass("img-aplicacion-hover");
            },
            mouseleave: function () {
                $(this).find("#ico-ver-aplicacion").removeClass("visible-tran").addClass("hidden-tran");
                $(this).find("#url-ver-aplicacion").removeClass("hidden-tran").addClass("visible-tran");
                $(this).find('#text-aplicacion').removeClass("fadeOutDown").addClass('fadeInUp animated');
                $(this).find("#img-aplicacion").removeClass().addClass("img-aplicacion-out");
            }
        });
    },
    DatosUsuarioSesion: function () {
        $fq.Ajax({
            url: `Usuario/DatosUsuarioSesion`,
            data: {},
            type: "POST"
        }, function (rpta) {
                if (rpta.EstadoSolicitud.EstaCorrecto) {
                    //#region Codigo Programable
                    $("#nombre-usuario-head").html(rpta.UsuarioSesion.NombreUsuario);
                    //#endregion
                } else {
                    if (rpta.EstadoSolicitud.TipoNotificacionId === 6) {
                        window.location.href = `${$fq.urlBase}Login/CerrarSesion`;
                    } else {
                        $fq.Notify(rpta.EstadoSolicitud.MensajeRespuesta, rpta.EstadoSolicitud.TipoNotificacionId);
                    }
                }
        }, null);
    },
    ListaDeAplicaciones: function () {
        $('link[rel="shortcut icon"]').attr('href', $fq.RutaUrlLogo);
        $("#logo-header").attr("src", $fq.RutaUrlLogoHeader);
        if ($fq.RutaUrlLogoHeader.includes("win-blanco")) {
            $("#logo-header").css("width", "76px");
        }
        $fq.Ajax({
            url: `Aplicacion/ListaPorNombreUsuario`,
            data: {},
            type: "POST"
        }, function (rpta) {
            if (rpta.EstadoSolicitud.EstaCorrecto) {
                //#region Codigo Programable
                $(".container-app-list").html(rpta.Cadena);
                Aplicacion.HoverApp();
                //#endregion
            } else {
                $fq.Notify(rpta.EstadoSolicitud.MensajeRespuesta, rpta.EstadoSolicitud.TipoNotificacionId);
            }
        }, null);
    }
};