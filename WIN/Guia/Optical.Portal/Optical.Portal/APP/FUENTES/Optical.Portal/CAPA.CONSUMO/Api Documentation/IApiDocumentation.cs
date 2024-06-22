﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace CAPA.CONSUMO
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Models;

    /// <summary>
    /// Api Documentation
    /// </summary>
    public partial interface IApiDocumentation : IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets json serialization settings.
        /// </summary>
        JsonSerializerSettings SerializationSettings { get; }

        /// <summary>
        /// Gets or sets json deserialization settings.
        /// </summary>
        JsonSerializerSettings DeserializationSettings { get; }

        /// <summary>
        /// Subscription credentials which uniquely identify client
        /// subscription.
        /// </summary>
        ServiceClientCredentials Credentials { get; }


            /// <summary>
        /// ListaPorNombreUsuario
        /// </summary>
        /// <param name='apellidoMaterno'>
        /// </param>
        /// <param name='apellidoPaterno'>
        /// </param>
        /// <param name='nombreUsuario'>
        /// </param>
        /// <param name='nombres'>
        /// </param>
        /// <param name='usuarioId'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<ResultadoApi>> ListaPorNombreUsuarioUsingPOSTWithHttpMessagesAsync(string apellidoMaterno = default(string), string apellidoPaterno = default(string), string nombreUsuario = default(string), string nombres = default(string), int? usuarioId = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// DefectoPorNombreUsuario
        /// </summary>
        /// <param name='apellidoMaterno'>
        /// </param>
        /// <param name='apellidoPaterno'>
        /// </param>
        /// <param name='nombreUsuario'>
        /// </param>
        /// <param name='nombres'>
        /// </param>
        /// <param name='usuarioId'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<ResultadoApi>> DefectoPorNombreUsuarioUsingPOSTWithHttpMessagesAsync(string apellidoMaterno = default(string), string apellidoPaterno = default(string), string nombreUsuario = default(string), string nombres = default(string), int? usuarioId = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// DatosPorNombreUsuario
        /// </summary>
        /// <param name='apellidoMaterno'>
        /// </param>
        /// <param name='apellidoPaterno'>
        /// </param>
        /// <param name='nombreUsuario'>
        /// </param>
        /// <param name='nombres'>
        /// </param>
        /// <param name='usuarioId'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<ResultadoApi>> DatosPorNombreUsuarioUsingPOSTWithHttpMessagesAsync(string apellidoMaterno = default(string), string apellidoPaterno = default(string), string nombreUsuario = default(string), string nombres = default(string), int? usuarioId = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

    }
}