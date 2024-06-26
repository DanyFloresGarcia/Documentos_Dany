﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace CAPA.CONSUMO.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class ResultadoApi
    {
        /// <summary>
        /// Initializes a new instance of the ResultadoApi class.
        /// </summary>
        public ResultadoApi() { }

        /// <summary>
        /// Initializes a new instance of the ResultadoApi class.
        /// </summary>
        public ResultadoApi(Empresa empresaDefecto = default(Empresa), EstadoSolicitud estadoSolicitud = default(EstadoSolicitud), IList<Aplicacion> listaDeAplicacion = default(IList<Aplicacion>), Usuario usuario = default(Usuario))
        {
            EmpresaDefecto = empresaDefecto;
            EstadoSolicitud = estadoSolicitud;
            ListaDeAplicacion = listaDeAplicacion;
            Usuario = usuario;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "EmpresaDefecto")]
        public Empresa EmpresaDefecto { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "EstadoSolicitud")]
        public EstadoSolicitud EstadoSolicitud { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ListaDeAplicacion")]
        public IList<Aplicacion> ListaDeAplicacion { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Usuario")]
        public Usuario Usuario { get; set; }

    }
}
