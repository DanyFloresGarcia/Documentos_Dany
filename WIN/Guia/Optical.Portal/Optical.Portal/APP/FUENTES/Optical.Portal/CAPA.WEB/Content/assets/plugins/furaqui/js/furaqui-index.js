var $fq = $fq || {};

$(document).ready(function () {
    $fq.Init();
});

$fq = {
    //Inicio de configuraciones
    Init: function () {
        $fq.FnFunction();
        $fq.Configuration();
        $fq.Variables();
    },
    //Variables
    Variables: function () {
        $fq.urlBase = "";
        $fq.navLevel = "";
        $fq.treeView = "";
        $fq.ambienteProgramcion = "";
        $fq.derechosAutor = "";
        $fq.versionSistema = "";
        $fq.nombreAplicacion = "";

        $fq.port = "";
        if (window.location.port !== "") {
            $fq.port = `:${window.location.port}`;
        }

        if (`${window.location.protocol}//${window.location.hostname}`.includes("10.1.2.105")) {
            window.location.href = "http://app.optical.pe/Portal/";
        }

        if (`${window.location.protocol}//${window.location.hostname}`.includes("10.1.2.183")) {
            window.location.href = "http://app2.optical.pe/Portal/";
        }

        $fq.urlBase = `${window.location.protocol}//${window.location.hostname}/Portal/`;

        $fq.Ajax({
            url: `App/Variables`,
            data: {
                NombreDominio: `${window.location.protocol}//${window.location.hostname}`
            },
            type: "POST",
            animation: false,
            async: false
        }, function (rpta) {
            if (rpta.EstadoSolicitud.EstaCorrecto) {
                $fq.ambienteProgramcion = rpta.ListaDeVariableConfiguracion.find(x => x.Nombre === "AmbienteProgramacion").Valor;
                $fq.derechosAutor = rpta.ListaDeVariableConfiguracion.find(x => x.Nombre === "DerechosAutor").Valor;
                $fq.versionSistema = rpta.ListaDeVariableConfiguracion.find(x => x.Nombre === "VersionSistema").Valor;
                $fq.nombreAplicacion = rpta.ListaDeVariableConfiguracion.find(x => x.Nombre === "NombreAplicacion").Valor;
                $fq.servidor = rpta.ListaDeVariableConfiguracion.find(x => x.Nombre === "Servidor").Valor;
                /* Nuevas Variables */
                $fq.RutaUrlLogo = rpta.ListaDeVariableConfiguracion.find(x => x.Nombre === "RutaUrlLogo").Valor;
                $fq.RutaUrlFondoLogin = rpta.ListaDeVariableConfiguracion.find(x => x.Nombre === "RutaUrlFondoLogin").Valor;
                $fq.RutaUrlLogoHeader = rpta.ListaDeVariableConfiguracion.find(x => x.Nombre === "RutaUrlLogoHeader").Valor;

                document.title = $fq.nombreAplicacion;
            } else {
                $fq.Notify(rpta.EstadoSolicitud.MensajeRespuesta, rpta.EstadoSolicitud.TipoNotificacionId);
            }
        }, null);
    },
    //Show loading animation
    Loading: {
        Start: function () {
            $("body").LoadingAnimation(true);
        },
        Finish: function () {
            $("body").LoadingAnimation(false);
        }
    },
    //Notifications
    Notify: function (msgNotificacion, typeNotification) {
        let pnotify_center_number = ($(window).width() / 2) - (Number(PNotify.prototype.options.width.replace(/\D/g, '')) / 2);
        let icon = "";
        let type = "";

        //#region position notification
        let stack_topleft = { "dir1": "down", "dir2": "right", firstpos1: 25, firstpos2: 25 };
        let stack_topright = { "dir1": "down", "dir2": "left", firstpos1: 25, firstpos2: 25 };
        let stack_topcenter = { "dir1": "down", "dir2": "right", firstpos1: 25, firstpos2: pnotify_center_number };
        let stack_bottomright = { "dir1": "up", "dir2": "left", "firstpos1": 25, "firstpos2": 25 };
        let stack_bottomleft = { "dir1": "up", "dir2": "right", "firstpos1": 25, "firstpos2": 25 };
        let stack_bottomcenter = { "dir1": "up", "dir2": "right", "firstpos1": 25, "firstpos2": pnotify_center_number };
        //#endregion

        PNotify.removeAll();

        switch (typeNotification) {
            case 4:
            case 6:
                icon = "icofont-close-circled";
                type = "error";
                break;
            case 3:
                icon = "icofont-warning-alt";
                type = "notice";
                break;
            case 1:
                icon = "icofont-check-circled";
                type = "success";
                break;
            case 2:
                icon = "icofont-info-circle";
                type = "info";
                break;
            default:
                break;
        }

        $(window).resize(function () {
            pnotify_center_number = ($(window).width() / 2) - (Number(PNotify.prototype.options.width.replace(/\D/g, '')) / 2);

            setTimeout(function () {
                $(".ui-pnotify").css({ left: pnotify_center_number, transition: "all 1000ms ease" });
            }, 300);
        });

        let parameters = {
            title: `<span class="${icon}"></span>`,
            text: `<span data-notify="message">${msgNotificacion}</span>`,
            addclass: "stack_bottomcenter",
            stack: stack_bottomcenter,
            type: type,
            icon: false,
            cornerclass: "border20"
        };

        new PNotify(parameters);
    },
    //Ajax submits
    Ajax: function (parameter, success, error) {
        PNotify.removeAll();
        var urlAjax = $fq.urlBase + parameter.url;

        if (parameter.animation === null || parameter.animation === undefined) {
            parameter.animation = true;
        }

        if (parameter.async === null || parameter.async === undefined) {
            parameter.async = true;
        }

        if (parameter.data === null) {
            parameter.data = {};
        }

        $.ajax({
            url: `${urlAjax}`,
            type: parameter.type,
            async: parameter.async,
            data: parameter.data,
            dataType: parameter.dataType,
            beforeSend: function () {
                if (parameter.animation) {
                    if (parameter.animationId === null || parameter.animationId === undefined) {
                        $fq.Loading.Start();
                    } else {
                        parameter.animationId.LoadingAnimation(true, parameter.animationSize);
                    }
                }
            },
            success: function (rpta) {
                rpta = JSON.parse(rpta);
                if (success !== null) {
                    success(rpta);
                }
            },
            error: function (rpta) {
                let errorRpta = {
                    status: rpta.status,
                    statusText: rpta.statusText,
                    responseText: rpta.responseText
                };

                let msgNotification = `${errorRpta.statusText} ${errorRpta.status}`;

                $fq.Notify(msgNotification, "error");

                if (error !== null) {
                    error(rpta);
                }
            },
            complete: function () {
                if (parameter.animation) {
                    if (parameter.animationId === null || parameter.animationId === undefined) {
                        setTimeout(function () {
                            $fq.Loading.Finish();
                        }, 500);
                    } else {
                        setTimeout(function () {
                            parameter.animationId.LoadingAnimation(false, parameter.animationSize);
                        }, 500);
                        
                    }
                }
            }
        });
    },
    //Functions automatics
    FnFunction: function () {
        //SiderToggle
        $.fn.SliderToggle = function () {
            this.slideToggle();
        };
        //Loading overlay
        $.fn.LoadingAnimation = function (state, size) {
            let customElement = $("<div>", {
                "class": "loader-animated"
            });

            if (state === true) {
                if (size === null || size === undefined) {
                    size = 50;
                }

                this.LoadingOverlay("show", {
                    //image: '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1000 1000"><circle r="80" cx="500" cy="90"/><circle r="80" cx="500" cy="910"/><circle r="80" cx="90" cy="500"/><circle r="80" cx="910" cy="500"/><circle r="80" cx="212" cy="212"/><circle r="80" cx="788" cy="212"/><circle r="80" cx="212" cy="788"/><circle r="80" cx="788" cy="788"/></svg>',
                    //imageColor: "#777",
                    //imageClass: "lo-ov-image",
                    //text: "Cargando",
                    //textColor: "#777",
                    //textClass: "lo-ov-text",
                    //size: size,
                    background: "#ffffff",
                    image: "",
                    custom: customElement
                });
            } else {
                this.LoadingOverlay("hide", true);
            }
        };
        //Execute function when press enter
        $.fn.ExecuteFunctionEnter = function (executeFuncion) {
            let _this = this;
            this.on('keypress', function (e) {
                if (e.which === 13) {
                    executeFuncion(_this);
                }
            });
        };
        //visibility or hidden element
        $.fn.ElementVisible = function (state) {
            if (state === true) {
                this.css("visibility", "visible");
            } else {
                this.css("visibility", "hidden");
            }
        };
        //Show or hide Div
        $.fn.ShowDiv = function (state) {
            if (state === true) {
                this.removeClass("cuerpo-ocultar").addClass("cuerpo-mostrar");
            } else {
                this.removeClass("cuerpo-mostrar").addClass("cuerpo-ocultar");
            }
        };
        //Alter Div Principal and Detail
        $.fn.ShowPrincipal = function (state) {
            if (state === true) {
                $("#cuerpo-principal").ShowDiv(true);
                $("#cuerpo-detalle").ShowDiv(false);

                setTimeout(function () {
                    $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                }, 300);
            } else {
                $("#cuerpo-principal").ShowDiv(false);
                $("#cuerpo-detalle").ShowDiv(true);
            }
        };
        //Show Error
        $.fn.ShowError = function (rpta) {
            $fq.Loading.Finish();
            let dividform = "#" + this.prop("id");
            let partForm = dividform.split("-")[1];

            if (partForm !== undefined) {
                partForm = "-" + partForm;
            } else {
                partForm = "";
            }

            if (rpta.ListaErrorValidarModelo !== null && rpta.ListaErrorValidarModelo !== undefined && rpta.ListaErrorValidarModelo.length > 0) {
                $fq.Notify(rpta.EstadoSolicitud.MensajeRespuesta, rpta.EstadoSolicitud.TipoNotificacionId);
                rpta.ListaErrorValidarModelo.forEach(function (valor, indice, array) {
                    let rpta = valor;
                    let idElemento = dividform + " #" + rpta.NombreCampo + partForm;
                    let ctrl = $("[name=" + rpta.NombreCampo + partForm + "]", dividform);

                    switch (ctrl.prop("type")) {
                        case "select-one":
                        case "select-multiple":
                            //Hacer algo
                            break;
                        case "radio":
                            //Hacer algo
                            break;
                        case "number":
                            //Hacer algo
                            break;
                        case undefined:
                            //Hacer algo
                            break;
                        case "text":
                            setTimeout(function () {
                                $(idElemento).data("placeholder", $(idElemento).attr('placeholder'));
                                $(idElemento).attr("placeholder", rpta.MensajeError);
                                $(idElemento).addClass("error-placeholder").removeClass("default-placeholder");
                                $(idElemento).parent().children(".input-group-prepend").children(".input-group-text").children("i").css("color", "#a9646f").removeClass("icofont-check").addClass("icofont-close");
                            }, 400);
                            break;
                        default:
                            if (ctrl.prop("class").indexOf("date-timepicker-format") >= 0) {
                                //Hacer algo
                            } else {
                                if (ctrl.prop("class").indexOf("mode-decimal") >= 0) {
                                    //Hacer algo
                                } else {
                                    //Hacer algo
                                }
                            }
                            break;
                    }
                });
            } else {
                $fq.Notify(rpta.EstadoSolicitud.MensajeRespuesta, rpta.EstadoSolicitud.TipoNotificacionId);

                if (rpta.EstadoSolicitud.TipoNotificacionId === 6) {
                    setTimeout(function () {
                        window.location.href = `${window.location.protocol}//${window.location.hostname}:${window.location.port}/Login/CerrarSesion`;
                    }, 1000);
                }
            }
        };
        //Reset placeholder input
        $.fn.InputText = function () {
            return this.each(function () {
                $(this).keydown(function (e) {
                    $(this).removeClass("error-placeholder").addClass("default-placeholder");
                    $(this).attr("placeholder", $(this).data("placeholder"));
                    $(this).parent().children(".input-group-prepend").children(".input-group-text").children("i").css("color", "#777").removeClass("icofont-close").addClass("icofont-check");
                });
            });
        };
        //Only Integers
        $.fn.ForceNumericOnly = function () {
            return this.each(function () {
                $(this).keydown(function (e) {
                    -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault();
                });
            });
        };
        //Only Decimals
        $.fn.ForceDecimalOnly = function () {
            return this.each(function () {
                $(this).keydown(function (e) {
                    let position = this.selectionEnd;
                    if ($(this).val().indexOf(".") > -1 && e.keyCode === 190) {
                        e.preventDefault();
                    } else {
                        if ($(this).val().indexOf("-") > -1 && (e.keyCode === 189 || e.keyCode === 109)) {
                            e.preventDefault();
                        } else {
                            if (position > 0 && (e.keyCode === 189 || e.keyCode === 109)) {
                                e.preventDefault();
                            } else {
                                if (position === 0 && e.keyCode === 190) {
                                    e.preventDefault();
                                } else {
                                    -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 189, 190, 109]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault();
                                }
                            }
                        }
                    }
                });
                $(this).keypress(function (e) {
                    let position = this.selectionEnd;
                    let charStr = String.fromCharCode(e.keyCode);
                    if (position > 0 && e.keyCode === 45) {
                        e.preventDefault();
                    } else {
                        if (position === 0 && e.keyCode === 46) {
                            e.preventDefault();
                        } else {
                            if (!(/\d/.test(charStr)) && e.keyCode !== 45 && e.keyCode !== 46) {
                                e.preventDefault();
                            }
                        }
                    }
                });
            });
        };
        //Only Letters(With Space)
        $.fn.ForceLettersSpaceOnly = function () {
            return this.each(function () {
                $(this).keydown(function (e) {
                    var key = e.keyCode;
                    if (!(key === 8 || key === 9 || e.shiftKey || e.ctrlKey || key === 32 || key === 46 || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || key === 192)) {
                        e.preventDefault();
                    }
                });
            });
        };
        //Only Letters(Without Space)
        $.fn.ForceLettersOnly = function () {
            return this.each(function () {
                $(this).keydown(function (e) {
                    var key = e.keyCode;
                    if (!(key === 8 || key === 9 || e.shiftKey || e.ctrlKey || key === 46 || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || key === 192)) {
                        e.preventDefault();
                    }
                });
            });
        };
        //Convert form in params object[]
        $.fn.ParamsObject = function () {
            let deshabilitar = this.find(':input:disabled').removeAttr('disabled');
            let data = this.SerializeObject();
            let ob = [];

            $.each(data, function (key, value) {
                ob.push([key, value]);
            });

            deshabilitar.attr('disabled', 'disabled');

            return ob;
        };
        //Serializ Object
        $.fn.SerializeObject = function () {
            var o = {};
            let listObject = this.serializeArray();
            let idForm = "#" + this.prop("id");

            $.each(listObject, function (key, value) {
                let paramEntity = value.name.split("-")[0];
                let element = "[name=" + value.name + "]";
                let ctrl = $("[name=" + value.name + "]", idForm);

                switch (ctrl.prop("type")) {
                    case "select-one":
                        o[paramEntity] = this.value() || '';
                        break;
                    case "select-multiple":
                        $(element).selectpicker("val", $fq.ArrayMultiSelect(value));
                        break;
                    case "radio":
                        //Si el radio desencadena un evento al cambiar como limpiar un input text, el radio va antes que 
                        //el text en la clase
                        $("input[name='" + key + "'][value='" + value + "']").prop("checked", true).change();
                        break;
                    case "text":
                        o[paramEntity] = $(element).val().trim() || '';
                        break;
                    case "checkbox":
                        o[paramEntity] = $(element).is(":checked");
                        break;
                    case undefined:
                        o[paramEntity] = this.value.toString().trim() || '';
                        break;
                    default:
                        if (ctrl.prop("class").indexOf("date-timepicker-format") >= 0) {
                            o[paramEntity] = this.value.toString().trim() || '';
                        } else {
                            if (ctrl.prop("class").indexOf("mode-decimal") >= 0) {
                                o[paramEntity] = Number(this.value) || 0;
                            } else {
                                if (!isNaN(this.value)) { //check if value is convertible to number
                                    o[paramEntity] = Number(this.value) || 0;
                                } else {
                                    o[paramEntity] = this.value.toString().trim() || '';
                                }
                            }
                        }
                        break;
                }
            });

            return o;
        };
        //Serializ Form
        $.fn.SerializeForm = function () {
            let deshabilitar = this.find(':input:disabled').removeAttr('disabled');
            let data = this.SerializeObject();
            deshabilitar.attr('disabled', 'disabled');

            return data;
        };
        //Clear form
        $.fn.EraseForm = function () {
            let deshabilitar = this.find(':input:disabled').removeAttr('disabled');
            let data = this.SerializeObject();
            let idForm = "#" + this.prop("id");
            let listObject = this.serializeArray();
            let checkBoxesList = $(idForm + ' input:checkbox').map(function () {
                return { name: this.name, value: this.checked ? this.value : "false" };
            });

            $.each(listObject, function (key, value) {
                let nombre = value.name;

                if (nombre !== "") {
                    let element = idForm + " #" + nombre;
                    let ctrl = $("[name=" + nombre + "]", idForm);

                    $(element).removeClass("error-placeholder").addClass("default-placeholder");
                    $(element).attr("placeholder", $(this).data("placeholder"));
                    $(element).parent().children(".input-group-prepend").children(".input-group-text").children("i").css("color", "#777").removeClass("icofont-close").addClass("icofont-check");

                    switch (ctrl.prop("type")) {
                        case "select-one":
                        case "select-multiple":
                            if ($(idForm).prop("class").indexOf("filtro") >= 0) {
                                $(element).selectpicker('selectAll');
                            } else {
                                $(element).selectpicker('deselectAll');
                            }
                            break;
                        case "radio":
                            //Si el radio desencadena un evento al cambiar como limpiar un input text, el radio va antes que 
                            //el text en la clase
                            $("input[name='" + nombre + "'][value='" + value + "']").prop("checked", false).change();
                            break;
                        case "checkbox":
                            break;
                        case "number":
                            $(element).val("0");
                            break;
                        case undefined:
                            break;
                        default:
                            if (ctrl.prop("class").indexOf("date-timepicker-format") >= 0) {
                                $(element).val($fq.FormatDate(fechaActual));
                            } else {
                                if (ctrl.prop("class").indexOf("mode-decimal") >= 0) {
                                    $(element).val($fq.FormatDecimal(0, 2));
                                } else {
                                    $(element).val("");
                                }
                            }
                            break;
                    }
                }
            });

            $.each(checkBoxesList, function (key, value) {
                let element = "[name=" + value.name + "]";

                $(element).prop("checked", false).change();
            });

            deshabilitar.attr('disabled', 'disabled');

            return data;
        };
        //Fill data in Form
        $.fn.FillForm = function (rpta) {
            let idForm = "#" + this.prop("id");
            let listObject = this.serializeArray();
            let checkBoxesList = $(idForm + ' input:checkbox').map(function () {
                return { name: this.name, value: this.checked ? this.value : "false" };
            });

            $.each(listObject, function (key, value) {
                let element = "[name=" + value.name + "]";
                let ctrl = $("[name=" + value.name + "]", idForm);
                let entity = rpta.Entidad;
                let attrEntity = value.name.split("-")[0];
                let valEntity = entity[attrEntity];

                switch (ctrl.prop("type")) {
                    case "select-one":
                        $(element).val(valEntity).change();
                        break;
                    case "select-multiple":
                        $(element).selectpicker("val", $fq.ArrayMultiSelect(valEntity));
                        break;
                    case "radio":
                        //Si el radio desencadena un evento al cambiar como limpiar un input text, el radio va antes que 
                        //el text en la clase
                        $("input[name='" + key + "'][value='" + valEntity + "']").prop("checked", true).change();
                        break;
                    case "checkbox":
                        break;
                    case undefined:
                        $("#" + key).val(valEntity);
                        break;
                    default:
                        if (ctrl.prop("class").indexOf("date-timepicker-format") >= 0) {
                            $(element).val($fq.FormatDate(valEntity));
                        } else {
                            if (ctrl.prop("class").indexOf("mode-decimal") >= 0) {
                                $(element).val($fq.FormatDecimal(valEntity, 2));
                            } else {
                                $(element).val(valEntity);
                            }
                        }
                        break;
                }
            });

            $.each(checkBoxesList, function (key, value) {
                let element = "[name=" + value.name + "]";
                let entity = rpta.Entidad;
                let attrEntity = value.name.split("-")[0];
                let valEntity = entity[attrEntity];

                $(element).prop("checked", valEntity).change();
            });
        };
        //Disable all in form
        $.fn.DisableAllForm = function (state) {
            let idForm = "#" + this.prop("id");
            if (state) {
                $(idForm + " input").prop("disabled", true);
                $(idForm + " button").prop("disabled", true);
            } else {
                $(idForm + " input").prop("disabled", false);
                $(idForm + " button").prop("disabled", false);
            }
        };
        //Datatable
        $.fn.DatatableCustom = function (parameter) {
            let idForm = "#" + this.prop("id");
            let urlAjax = $fq.urlBase + parameter.url;

            let highTable = $(window).height() - 210 + "px";
            let thisdom = "";
            let thisdomButton = "";

            $(idForm).css("visibility", "hidden");

            if ($("#content").width() >= 576) {
                thisdom = '<"card-header card-header-table"<"row"<"btn-group-table-buttons col-md-3"l>';
            } else {
                thisdom = '<"card-header card-header-table"<"row"<"btn-group-table-buttons col-9"<"form-group"l>>';
            }

            if (parameter.domButtons === true) {
                if ($("#content").width() >= 576) {
                    thisdomButton = '<"btn-group-table-length col-md-1"<"form-group"B>>';
                } else {
                    thisdomButton = '<"btn-group-table-length col-3"B>';
                }
                thisdom = thisdom + thisdomButton;
            }

            if ($("#content").width() >= 576) {
                thisdom = thisdom + '<"btn-group-table-filter col-md-4">>>tip';
            } else {
                thisdom = thisdom + '>><"row"<"btn-group-table-filter col-12"<"form-group"f>>>tip';
            }

            if (parameter.lengthMenu === null || parameter.lengthMenu === undefined) {
                parameter.lengthMenu = [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Todo"]];
            }

            if (parameter.animationId === null || parameter.animationId === undefined) {
                $fq.Loading.Start();
            } else {
                parameter.animationId.LoadingAnimation(true, parameter.animationSize);
            }

            parameter.filtroId.DisableAllForm(true);

            let tbDatatable = $(idForm).DataTable({
                dom: thisdom,
                ajax: {
                    url: urlAjax,
                    type: "POST",
                    datatype: "json",
                    data: parameter.data,
                    dataFilter: function (rpta) {
                        var json = jQuery.parseJSON(rpta);
                        json = JSON.parse(json);
                        if (json.EstadoSolicitud.EstaCorrecto) {
                            json.recordsTotal = json.TotalRegistros;
                            json.recordsFiltered = json.TotalRegistros;
                            json.data = json.ListaEntidad;
                        } else {
                            $.fn.ShowError(json);
                            json.recordsTotal = 0;
                            json.recordsFiltered = 0;
                            json.data = [];
                        }

                        return JSON.stringify(json);
                    }
                },
                autoWidth: false,
                responsive: false,
                lengthMenu: parameter.lengthMenu,
                destroy: true,
                scrollX: "100%",
                scrollY: highTable,
                scrollCollapse: true,
                columns: parameter.columns,
                initComplete: parameter.initComplete,
                footerCallback: parameter.footerCallback,
                processing: false,
                serverSide: true,
                filter: false,
                orderMulti: false,
                pageLength: parameter.lengthMenu[0][0],
                columnDefs: parameter.columnDefs,
                order: parameter.order,
                oLanguage: {
                    sInfo: "Mostrando de _START_ hasta _END_ - _TOTAL_ resultados",
                    sSearch: "Filtrar:",
                    oPaginate: {
                        sFirst: "Primero",
                        sLast: "Último",
                        sNext: "Siguiente",
                        sPrevious: "Anterior"
                    },
                    sLengthMenu: "Mostrar _MENU_ resultados",
                    sLoadingRecords: "",
                    sProcessing: "Procesando...",
                    sEmptyTable: "No se encontraron resultados",
                    sZeroRecords: "No se encontraron registros coincidentes",
                    sInfoEmpty: "Mostrando 0 to 0 of 0 resultados",
                    sInfoFiltered: "(filtrado de _MAX_ resultados)"
                },
                buttons: [
                    {
                        extend: 'collection',
                        text: '<i class="icon icofont icofont-download"></i>',
                        titleAttr: 'DESCARGAR',
                        buttons: [
                            {
                                extend: "pdf",
                                text: '<i class="icofont-file-pdf"></i> PDF',
                                titleAttr: 'PDF'
                            },
                            {
                                extend: "csv",
                                text: '<i class="icofont-file-text"></i> CSV',
                                titleAttr: 'CSV'
                            },
                            {
                                extend: "excel",
                                text: '<i class="icofont-file-excel"></i> Excel',
                                titleAttr: 'EXCEL'
                            },
                            {
                                extend: "print",
                                text: '<i class="icofont-printer"></i> Imprimir',
                                titleAttr: 'IMPRIMIR'
                            }
                        ]
                    }
                ]
            });

            tbDatatable.off('xhr').off('processing').off('preXhr');

            tbDatatable
                .on('preXhr', function () {
                    $(idForm).css("visibility", "hidden");
                    if (parameter.animationId === null || parameter.animationId === undefined) {
                        $fq.Loading.Start();
                    } else {
                        parameter.animationId.LoadingAnimation(true, parameter.animationSize);
                    }
                })
                .on('xhr', function () {
                    $("#TablaEmpresa").css("visibility", "inherit");
                    $(".table").css("visibility", "inherit");

                    let rpta = tbDatatable.ajax.json();
                    if (rpta === undefined) {
                        $fq.Notify("No se cargaron los datos correctamente", 4);
                    } else {
                        if (parameter.xhr !== null && parameter.xhr !== undefined) {
                            parameter.xhr();
                        }
                    }

                    parameter.filtroId.DisableAllForm(false);

                    if (parameter.animationId === null || parameter.animationId === undefined) {
                        setTimeout(function () {
                            $fq.Loading.Finish();
                        }, 500);
                    } else {
                        setTimeout(function () {
                            parameter.animationId.LoadingAnimation(false, parameter.animationSize);
                        }, 500);

                    }

                    setTimeout(function () {
                        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                    }, 1000);
                });
        };
    },
    Configuration: function () {
        $fq.ColorBackGround();
        $fq.DateTimePicker();
        $fq.SidebarToggle();
        $fq.ResizeWindow();
        $fq.TextTrasform();
        $fq.Selectpicker();
        $fq.NoAutomaticEnterForm();
        $fq.ValidateFormatInput();
        $fq.ChangeInputDecimal();
        $fq.WavesEfect();
        $fq.WindowPortlets();
        $fq.IcoOptions();
        $fq.FabButtom();
    },
    ColorBackGround: function () {
        let altoTotal = $(window).height();
        let altoPaginaFondo = altoTotal - 50;
        $(".container-fluid").height(altoPaginaFondo);
    },
    DateTimePicker: function () {
        let todayDate = moment(new Date());
        //De una sola fecha
        $('.date-timepicker-format').datetimepicker({
            locale: 'es',
            format: 'DD/MM/YYYY',
            icons:
            {
                next: 'icofont-rounded-right',
                previous: 'icofont-rounded-left'
            },
            defaultDate: todayDate
        });

        $.validator.addMethod('date',
            function (value, element) {
                if (this.optional(element)) {
                    return true;
                }

                var ok = true;
                try {
                    $.datepicker.parseDate('dd/mm/yy', value);
                }
                catch (err) {
                    ok = false;
                }
                return ok;
            });
        //Con rangos
        let startDate = moment().startOf('month');
        let endDate = moment().endOf('month');
        $('.date-timepicker-range-format').daterangepicker({
            startDate: startDate,
            endDate: endDate,
            ranges: {
                'Hoy': [moment(), moment()],
                'Ayer': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Último 7 Días': [moment().subtract(6, 'days'), moment()],
                'Último 30 Días': [moment().subtract(29, 'days'), moment()],
                'Este Mes': [moment().startOf('month'), moment().endOf('month')],
                'Último Mes': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'Todo el Año': [moment().startOf('year'), moment().endOf('year')]
            },
            locale: {
                format: "DD/MM/YYYY",
                applyLabel: 'Aceptar',
                cancelLabel: 'Cancelar',
                fromLabel: 'Desde',
                toLabel: 'Hasta',
                customRangeLabel: 'Personalizado',
                daysOfWeek: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                monthNames: ["Enero", "Febrero", "Marzo", "Abril",
                    "Mayo", "Junio", "Julio", "Agosto",
                    "Setiembre", "Octubre", "Noviembre", "Diciembre"],
                firstDay: 1
            }
        });
    },
    //Adjust Datatable Click Sidebar Toggle
    SidebarToggle: function () {
        $(".sidebar-toggle").click(function () {
            let widthWindow = $(window).width();

            if (widthWindow > 767) {
                if ($("body").hasClass("sidebar-collapse")) {
                    $(".main-header-top .navbar .sidebar-toggle").addClass("sidebar-toggle-active");
                    $(".treeview-menu").css("display", "none");
                } else {
                    $(".main-header-top .navbar .sidebar-toggle").removeClass("sidebar-toggle-active");
                }
            } else {
                if ($("body").hasClass("sidebar-open")) {
                    $(".main-header-top .navbar .sidebar-toggle").removeClass("sidebar-toggle-active");
                } else {
                    $(".main-header-top .navbar .sidebar-toggle").addClass("sidebar-toggle-active");
                }
            }


            setTimeout(function () {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            }, 300);
        });
    },
    //Resize Window
    ResizeWindow: function () {
        //Maximizar página
        $(".full-screen").on("click", function () {
            toggleFullScreen(this);
        });
        //Al cargar la pagina
        let widthWindow = $(window).width();

        $(".card-footer").children("div.row").children(".group-actions").toArray().forEach(function (valor, indice, array) {
            let htmlFooter = $(valor).html();
            let htmlHeader = htmlFooter + $(valor).parent("div.row").parent("div.card-footer").parent("div.card-block").parent("div.card").children("div.card-header.card-header-cuerpo").children("div.row").children(".group-actions").html();
            $(valor).parent("div.row").parent("div.card-footer").parent("div.card-block").parent("div.card").children("div.card-header.card-header-cuerpo").children("div.row").children(".group-actions").html(htmlHeader);
        });
        if (widthWindow < 767) {
            $(".main-header-top .navbar .sidebar-toggle").addClass("sidebar-toggle-active");
            $(".card-footer").children("div.row").children(".group-actions").css("display", "block");
        } else {
            $(".card-footer").children("div.row").children(".group-actions").css("display", "none");
        }
        //Al redimensionar la pagina
        $(window).resize(function () {
            let heightWindow = $(window).height();
            let widthWindow = $(window).width();
            let heightContent = heightWindow - 210 + "px";
            $(".dataTables_scrollBody").css("max-height", heightContent);

            let altoTotal = $(window).height();
            let altoPaginaFondo = altoTotal - 50;
            $(".container-fluid").height(altoPaginaFondo);
            $(".container-fluid").css("background-color", "#ecf0f5");
            $('.tooltip-custom').each(function () {
                $(this).popover('hide');
            });

            if (widthWindow < 767) {
                $(".main-header-top .navbar .sidebar-toggle").addClass("sidebar-toggle-active");
                $(".card-footer").children("div.row").children(".group-actions").css("display", "block");
            } else {
                $(".main-header-top .navbar .sidebar-toggle").removeClass("sidebar-toggle-active");
                $(".card-footer").children("div.row").children(".group-actions").css("display", "none");
            }

            setTimeout(function () {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            }, 1000);
        });
    },
    TextTrasform: function () {
        //uppercase
        $(document).on('blur', ".text-uppercase", function () {
            $(this).val(function (_, val) {
                if (val !== null) {
                    return val.toUpperCase();
                } else {
                    return val;
                }
            });
        });

        //lowercase
        $(document).on('blur', ".text-lowercase", function () {
            $(this).val(function (_, val) {
                if (val !== null) {
                    return val.toLowerCase();
                } else {
                    return val;
                }
            });
        });
    },
    Selectpicker: function () {
        $(".selectpicker").selectpicker({
            noneSelectedText: 'SELECCIONAR',
            selectAllText: "TODOS",
            deselectAllText: "NINGUNO",
            countSelectedText: function (numSelected, numTotal) {
                return (numSelected === 1) ? "{0} seleccionado" : "{0} seleccionados";
            }
        });

        $(".selectpicker").selectpicker("val", null).change();
    },
    NoAutomaticEnterForm: function () {
        $("form").submit(function () {
            return false;
        });
    },
    ValidateFormatInput: function () {
        $("input").InputText();
        $('.mode-numeric').ForceNumericOnly();
        $('.mode-decimal').ForceDecimalOnly();
        $('.mode-letters').ForceLettersOnly();
        $('.mode-letters-space').ForceLettersSpaceOnly();
    },
    FormatDateTime: function (parametro) {
        let date = new Date(parametro).toLocaleDateString("es-US", options = { day: '2-digit', month: '2-digit', year: 'numeric' });
        let time = new Date(parametro).toLocaleDateString("es-US", options = { day: '2-digit', month: '2-digit', year: 'numeric' });
        return date + " " + time;
    },
    FormatDate: function (parametro) {
        return new Date(parametro).toLocaleDateString("es-US", options = { day: '2-digit', month: '2-digit', year: 'numeric' });
    },
    FormatTime: function (parametro) {
        return new Date(parametro).toLocaleTimeString("es-US");
    },
    FormatDecimal: function (amount, numberDecimals) {
        if (amount !== null && amount !== undefined) {
            var decimals = numberDecimals;
            amount += '';
            amount = parseFloat(amount.replace(/-[^0-9\.]/g, ''));
            decimals = decimals || 0;
            // si no es un numero o es igual a cero retorno el mismo cero
            if (isNaN(amount) || amount === 0)
                return parseFloat(0).toFixed(decimals);
            // si es mayor o menor que cero retorno el valor formateado como numero
            amount = '' + amount.toFixed(decimals);
            var amount_parts = amount.split('.'),
                regexp = /(\d+)(\d{3})/;
            while (regexp.test(amount_parts[0]))
                amount_parts[0] = amount_parts[0].replace(regexp, '$1' + ',' + '$2');
            return amount_parts.join('.');
        } else {
            return amount;
        }
    },
    IsNaN: function (parametro) {
        if (isNaN(parametro)) {
            return 0;
        } else {
            return parametro;
        }
    },
    FormatMultiSelect: function (idSelect, separador) {
        let targets = [];
        $.each($(idSelect + " option:selected"), function () {
            targets.push($(this).val());
        });
        let rolSeleccionados = targets.join(separador);

        return rolSeleccionados;
    },
    ArrayMultiSelect: function (parametro) {
        if (parametro === undefined) {
            return null;
        } else {
            return JSON.parse("[" + parametro + "]");
        }
    },
    ChangeInputDecimal: function () {
        $(".mode-decimal").ExecuteFunctionEnter(function (_this) {
            let id = "#" + _this.prop("id");
            if ($(id).val() === "") {
                $(id).val($fq.FormatDecimal(0, 2));
            } else {
                $(id).val($fq.FormatDecimal($(id).val(), 2));
            }
        });

        $(".mode-decimal").change(function () {
            if ($(this).val() === "") {
                $(this).val($fq.FormatDecimal(0, 2));
            } else {
                $(this).val($fq.FormatDecimal($(this).val(), 2));
            }
        });
    },
    WavesEfect: function () {
        Waves.init();
        Waves.attach('.flat-buttons', ['waves-button']);
        Waves.attach('.float-buttons', ['waves-button', 'waves-float']);
        Waves.attach('.float-button-light', ['waves-button', 'waves-float', 'waves-light']);
        Waves.attach('.flat-buttons', ['waves-button', 'waves-float', 'waves-light', 'flat-buttons']);
        Waves.attach('.btn');
        Waves.attach('.dt-button');
    },
    WindowPortlets: function () {
        //Minimize
        $(".minimize-card-block").off('click').on('click', function (e) {
            e.preventDefault();
            if ($(this).children('i').hasClass("icofont-caret-up")) {
                $(this).html(``);
                $(this).html(`<i class="icofont-caret-down"></i> Mostrar`);
            } else {
                $(this).html(``);
                $(this).html(`<i class="icofont-caret-up"></i> Ocultar`);
            }
            $(this).parents('.card').children('.card-block').slideToggle();
        });
        //Close
        $(".close-cuerpo-detalle").on('click', function (e) {
            $(this).parents('#cuerpo-detalle').ShowDiv(false);
        });
    },
    IcoOptions: function () {
        $('[data-toggle="tooltip"]').tooltip({
            trigger: 'hover'
        });
        $('[data-toggle="tooltip"]').tooltip({
            trigger: 'active'
        });
        $("a").on("click", function () {
            $(this).tooltip("hide");
        });
        $(".a-ico").on("click", function () {
            $(this).css("transform", "scale(1.0)");
        });
        $(".a-ico").hover(function () {
            $(this).css("transform", "scale(1.4)");
        });
        $(".a-ico").mouseleave(function () {
            $(this).css("transform", "scale(1.0)");
        });
    },
    MobileOptions: function () {
        var check = false;
        (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
        return check;
    },
    FabButtom: function () {
        var links = [
            {
                "bgcolor": "#7cb342",
                "icon": "<i class='icofont icofont-plus'></i>"
            },
            {
                "bgcolor": "#f1c40f",
                "color": "fffff",
                "icon": "<i class='icofont icofont-pencil-alt-2'></i>",
                "target": "_blank"
            },
            {
                "bgcolor": "#2ecc71",
                "color": "#fffff",
                "icon": "<i class='icofont icofont-speech-comments'></i>"
            },
            {
                "url": "http://www.google.com.pe",
                "bgcolor": "#e74c3c",
                "color": "#fffff",
                "icon": "A"
            }
        ];
        var options = {
            rotate: true
        };
        $('#vertical-fab').jqueryFab(links, options);
    }
};