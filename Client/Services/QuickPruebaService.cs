
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace QuickEx.Client
{
    public partial class QuickPruebaService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public QuickPruebaService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/QuickPrueba/");
        }


        public async System.Threading.Tasks.Task ExportAlumnosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/quickprueba/alumnos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/quickprueba/alumnos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAlumnosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/quickprueba/alumnos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/quickprueba/alumnos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAlumnos(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<QuickEx.Server.Models.QuickPrueba.Alumno>> GetAlumnos(Query query)
        {
            return await GetAlumnos(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<QuickEx.Server.Models.QuickPrueba.Alumno>> GetAlumnos(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Alumnos");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAlumnos(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<QuickEx.Server.Models.QuickPrueba.Alumno>>(response);
        }

        partial void OnCreateAlumno(HttpRequestMessage requestMessage);

        public async Task<QuickEx.Server.Models.QuickPrueba.Alumno> CreateAlumno(QuickEx.Server.Models.QuickPrueba.Alumno alumno = default(QuickEx.Server.Models.QuickPrueba.Alumno))
        {
            var uri = new Uri(baseUri, $"Alumnos");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(alumno), Encoding.UTF8, "application/json");

            OnCreateAlumno(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<QuickEx.Server.Models.QuickPrueba.Alumno>(response);
        }

        partial void OnDeleteAlumno(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAlumno(string dni = default(string))
        {
            var uri = new Uri(baseUri, $"Alumnos('{HttpUtility.UrlEncode(dni.Trim().Replace("'", "''").Replace(" ","%20"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAlumno(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAlumnoByDni(HttpRequestMessage requestMessage);

        public async Task<QuickEx.Server.Models.QuickPrueba.Alumno> GetAlumnoByDni(string expand = default(string), string dni = default(string))
        {
            var uri = new Uri(baseUri, $"Alumnos('{HttpUtility.UrlEncode(dni.Trim().Replace("'", "''").Replace(" ","%20"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAlumnoByDni(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<QuickEx.Server.Models.QuickPrueba.Alumno>(response);
        }

        partial void OnUpdateAlumno(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAlumno(string dni = default(string), QuickEx.Server.Models.QuickPrueba.Alumno alumno = default(QuickEx.Server.Models.QuickPrueba.Alumno))
        {
            var uri = new Uri(baseUri, $"Alumnos('{HttpUtility.UrlEncode(dni.Trim().Replace("'", "''").Replace(" ","%20"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(alumno), Encoding.UTF8, "application/json");

            OnUpdateAlumno(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}