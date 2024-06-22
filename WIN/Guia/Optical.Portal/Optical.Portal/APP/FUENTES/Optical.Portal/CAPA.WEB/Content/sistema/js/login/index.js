var Login = Login || {};
Login.ShiftPressed = false;

$(document).ready(function () {
    //Titulo Página
    document.title = $fq.nombreAplicacion + " - Inicio Sesión";
    //Configuración inicial
    Login.Configuration.Init();
    //CRUD
    $("#btn-ingresar").on("click", Login.Autenticar);
    $("#formIngreso").ExecuteFunctionEnter(Login.Autenticar);
    $("#solicitar-ayuda").on("click", Login.SolicitarAyuda);
    $("#a-no-ini-sesion").on("click", Login.BorrarDatosSolicitud);
    //Iniciar detección de teclas
    $(".ico-mayus").ElementVisible(false);
    document.querySelector("#Contrasena").addEventListener('keyup', Login.CheckCapsLock);
    document.querySelector("#Contrasena").addEventListener('mousedown', Login.CheckCapsLock);
});

Login.CheckCapsLock = function (e) {
    var caps_lock_on = e.getModifierState('CapsLock');

    if (caps_lock_on == true) {
        $(".ico-mayus").ElementVisible(true);
    }
    else {
        $(".ico-mayus").ElementVisible(false);
    }
};

//Mostrar Empresa Por Servidor
Login.ServidorImagen = function () {
    $("#login-logo-empresa").attr("src", $fq.RutaUrlLogo);
    $("#img-inicio").attr("src", $fq.RutaUrlFondoLogin);
    $("#img-inicio").css("height", $(".hide-panel-rigth").css("height"));
    $('link[rel="shortcut icon"]').attr('href', $fq.RutaUrlLogo);
};

//a-no-ini-sesion
Login.BorrarDatosSolicitud = function () {
    $("#Solicitud").EraseForm();
    $("#CorreoReceptor").val("mesadeayuda@optical.pe");
    $("#ArchivoAdjunto").parent().children('.md-label-file').show();
    $("#ArchivoAdjunto").parent().children('.md-form-file').val("");
    $("#ArchivoAdjunto").val(null);
};

Login.SolicitarAyuda = function () {
    let upFileLength = document.getElementById("ArchivoAdjunto").files.length;
    let formData = new FormData();

    for (var i = 0; i < upFileLength; i++) {
        var file = document.getElementById("ArchivoAdjunto").files[i];
        formData.append("ListaDeArchivoAdjunto", file);
    }
    formData.append("CorreoRemitente", $("#CorreoRemitente").val());
    formData.append("MensajeRemitente", $("#MensajeRemitente").val());

    $.ajax({
        beforeSend: function () {
            $("#Solicitud").LoadingAnimation(true);
        },
        complete: function () {
            $("#Solicitud").LoadingAnimation(false);
        },
        type: "POST",
        url: `${window.location.protocol}//${window.location.hostname}/Portal/Correo/SolicitarAyuda`,
        data: formData,
        contentType: false,
        processData: false,
        success: function (rpta) {
            rpta = JSON.parse(rpta);
            if (rpta.EstadoSolicitud.EstaCorrecto === true) {
                document.getElementById('solicitar-ayuda2').click();
            }
            $fq.Notify(rpta.EstadoSolicitud.MensajeRespuesta, rpta.EstadoSolicitud.TipoNotificacionId);
        },
        error: function (error) {
            //ajaxStop();
            $fq.Notify(error.statusText, 4);
        }
    });
};

Login.Configuration = {
    UpFile: function () {
        $("input:file").change(function (e, v) {
            var pathArray = $(this).val().split('\\');
            var img_name = pathArray[pathArray.length - 1];
            $(this).parent().children('.md-form-file').val(img_name);
            if (img_name)
                $(this).parent().children('.md-label-file').hide();
            else
                $(this).parent().children('.md-label-file').show();
        });
    },
    Init: function () {
        $(".label-version-sistema").text($fq.versionSistema);
        Login.Configuration.ColorBackground();
        Login.Configuration.Window.Resize();
        Login.Configuration.AnimatePassword();
        Login.Configuration.StayConnected();
        Login.Configuration.ShowButtonLogin();
        Login.Configuration.UpFile();
        Login.ServidorImagen();
    },
    ColorBackground: function () {
        $(".container-fluid").css("height", "auto");
        $(".container-fluid").css("background-color", "transparent");
    },
    Window: {
        //Redimensionar
        Resize: function () {
            $(window).resize(function () {
                $(".container-fluid").css("height", "auto");
                $(".container-fluid").css("background-color", "transparent");
            });
        }
    },
    ShowPassword: function () {
        if ($("#Contrasena").hasClass("password")) {
            $("#Contrasena").removeClass("password");
            $(".ico-password").addClass("fa-eye").removeClass("fa-eye-slash");
        } else {
            $("#Contrasena").addClass("password");
            $(".ico-password").removeClass("fa-eye").addClass("fa-eye-slash");
        }
    },
    AnimatePassword: function () {
        $(".ico-password").ElementVisible(false);

        $("#Contrasena").blur(function () {
            if ($('.ico-password').is(':hover')) {
                $(".ico-password").ElementVisible(true);
            } else {
                $(".ico-password").ElementVisible(false);
                $("#Contrasena").addClass("password");
            }
        }).focus(function () {
            $(".ico-password").ElementVisible(true);
        });
    },
    StayConnected: function () {
        $(".DebePermancerConectado").on("click", function () {
            if ($(".DebePermancerConectado").is(":checked")) {
                $(".label-recordar").css("color", "black");
            } else {
                $(".label-recordar").css("color", "#818181");
            }
        });
    },
    ShowButtonLogin: function () {
        $("#NombreUsuario").on("keyup", function () {
            if ($(this).val().length > 0 && $("#Contrasena").val().length) {
                $("#btn-ingresar").prop("disabled", false);
                $("#btn-ingresar").removeClass("btn-ingresar-disabled").addClass("btn-ingresar-active");
            } else {
                $("#btn-ingresar").prop("disabled", true);
                $("#btn-ingresar").removeClass("btn-ingresar-active").addClass("btn-ingresar-disabled");
            }
        });

        $("#Contrasena").on("keyup", function () {
            if ($(this).val().length > 0 && $("#NombreUsuario").val().length) {
                $("#btn-ingresar").prop("disabled", false);
                $("#btn-ingresar").removeClass("btn-ingresar-disabled").addClass("btn-ingresar-active");
            } else {
                $("#btn-ingresar").prop("disabled", true);
                $("#btn-ingresar").removeClass("btn-ingresar-active").addClass("btn-ingresar-disabled");
            }
        });
    }
};

Login.Autenticar = function () {
    let form = $("#formIngreso");
    let data = form.SerializeForm();

    $("#btn-ingresar").prop("disabled", true);

    $fq.Ajax({
        url: `Login/Autenticar`,
        data: data,
        type: "POST",
        animationId: $(".login-card"),
        animationSize: 20
    }, function (rpta) {
        if (rpta.EstadoSolicitud.EstaCorrecto) {
            //#region Codigo Programable
            if (rpta.EstadoSolicitud.TipoNotificacionId === 1) {
                $fq.Notify(rpta.EstadoSolicitud.MensajeRespuesta, rpta.EstadoSolicitud.TipoNotificacionId);
                window.location.href = `${$fq.urlBase}Aplicacion`;
            } else {
                window.location.href = `${window.location.protocol}//${window.location.hostname}${rpta.RutaAplicativo}`;
            }
            //#endregion
        } else {
            if (rpta.EstadoSolicitud.TipoNotificacionId === 7) {
                $fq.Notify(rpta.EstadoSolicitud.MensajeRespuesta, 3);
            } else {
                $("#btn-ingresar").prop("disabled", false);
                form.ShowError(rpta);
            }
        }
    }, null);
};


