using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using QuickEx.Server.Data;

namespace QuickEx.Server.Controllers
{
    public partial class ExportQuickPruebaController : ExportController
    {
        private readonly QuickPruebaContext context;
        private readonly QuickPruebaService service;

        public ExportQuickPruebaController(QuickPruebaContext context, QuickPruebaService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/QuickPrueba/alumnos/csv")]
        [HttpGet("/export/QuickPrueba/alumnos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAlumnosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAlumnos(), Request.Query), fileName);
        }

        [HttpGet("/export/QuickPrueba/alumnos/excel")]
        [HttpGet("/export/QuickPrueba/alumnos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAlumnosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAlumnos(), Request.Query), fileName);
        }
    }
}
