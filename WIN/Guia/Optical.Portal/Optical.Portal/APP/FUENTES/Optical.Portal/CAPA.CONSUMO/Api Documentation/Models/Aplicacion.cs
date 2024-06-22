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

    public partial class Aplicacion
    {
        /// <summary>
        /// Initializes a new instance of the Aplicacion class.
        /// </summary>
        public Aplicacion() { }

        /// <summary>
        /// Initializes a new instance of the Aplicacion class.
        /// </summary>
        public Aplicacion(int? aplicacionId = default(int?), string color = default(string), string descripcion = default(string), string icono = default(string), string nombre = default(string), string ruta = default(string))
        {
            AplicacionId = aplicacionId;
            Color = color;
            Descripcion = descripcion;
            Icono = icono;
            Nombre = nombre;
            Ruta = ruta;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AplicacionId")]
        public int? AplicacionId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Color")]
        public string Color { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Icono")]
        public string Icono { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Nombre")]
        public string Nombre { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Ruta")]
        public string Ruta { get; set; }

    }
}