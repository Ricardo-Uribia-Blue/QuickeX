using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace QuickEx.Server.Controllers.QuickPrueba
{
    [Route("odata/QuickPrueba/Alumnos")]
    public partial class AlumnosController : ODataController
    {
        private QuickEx.Server.Data.QuickPruebaContext context;

        public AlumnosController(QuickEx.Server.Data.QuickPruebaContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<QuickEx.Server.Models.QuickPrueba.Alumno> GetAlumnos()
        {
            var items = this.context.Alumnos.AsQueryable<QuickEx.Server.Models.QuickPrueba.Alumno>();
            this.OnAlumnosRead(ref items);

            return items;
        }

        partial void OnAlumnosRead(ref IQueryable<QuickEx.Server.Models.QuickPrueba.Alumno> items);

        partial void OnAlumnoGet(ref SingleResult<QuickEx.Server.Models.QuickPrueba.Alumno> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/QuickPrueba/Alumnos(dni={dni})")]
        public SingleResult<QuickEx.Server.Models.QuickPrueba.Alumno> GetAlumno(string key)
        {
            var items = this.context.Alumnos.Where(i => i.dni == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnAlumnoGet(ref result);

            return result;
        }
        partial void OnAlumnoDeleted(QuickEx.Server.Models.QuickPrueba.Alumno item);
        partial void OnAfterAlumnoDeleted(QuickEx.Server.Models.QuickPrueba.Alumno item);

        [HttpDelete("/odata/QuickPrueba/Alumnos(dni={dni})")]
        public IActionResult DeleteAlumno(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Alumnos
                    .Where(i => i.dni == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnAlumnoDeleted(item);
                this.context.Alumnos.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAlumnoDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAlumnoUpdated(QuickEx.Server.Models.QuickPrueba.Alumno item);
        partial void OnAfterAlumnoUpdated(QuickEx.Server.Models.QuickPrueba.Alumno item);

        [HttpPut("/odata/QuickPrueba/Alumnos(dni={dni})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAlumno(string key, [FromBody]QuickEx.Server.Models.QuickPrueba.Alumno item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.dni != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnAlumnoUpdated(item);
                this.context.Alumnos.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Alumnos.Where(i => i.dni == Uri.UnescapeDataString(key));
                ;
                this.OnAfterAlumnoUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/QuickPrueba/Alumnos(dni={dni})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAlumno(string key, [FromBody]Delta<QuickEx.Server.Models.QuickPrueba.Alumno> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Alumnos.Where(i => i.dni == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnAlumnoUpdated(item);
                this.context.Alumnos.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Alumnos.Where(i => i.dni == Uri.UnescapeDataString(key));
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAlumnoCreated(QuickEx.Server.Models.QuickPrueba.Alumno item);
        partial void OnAfterAlumnoCreated(QuickEx.Server.Models.QuickPrueba.Alumno item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] QuickEx.Server.Models.QuickPrueba.Alumno item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnAlumnoCreated(item);
                this.context.Alumnos.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Alumnos.Where(i => i.dni == item.dni);

                ;

                this.OnAfterAlumnoCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
