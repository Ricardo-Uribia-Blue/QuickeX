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
    [Route("odata/QuickPrueba/Usuarios")]
    public partial class UsuariosController : ODataController
    {
        private QuickEx.Server.Data.QuickPruebaContext context;

        public UsuariosController(QuickEx.Server.Data.QuickPruebaContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<QuickEx.Server.Models.QuickPrueba.Usuario> GetUsuarios()
        {
            var items = this.context.Usuarios.AsQueryable<QuickEx.Server.Models.QuickPrueba.Usuario>();
            this.OnUsuariosRead(ref items);

            return items;
        }

        partial void OnUsuariosRead(ref IQueryable<QuickEx.Server.Models.QuickPrueba.Usuario> items);

        partial void OnUsuarioGet(ref SingleResult<QuickEx.Server.Models.QuickPrueba.Usuario> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/QuickPrueba/Usuarios(IdUsuario={IdUsuario})")]
        public SingleResult<QuickEx.Server.Models.QuickPrueba.Usuario> GetUsuario(long key)
        {
            var items = this.context.Usuarios.Where(i => i.IdUsuario == key);
            var result = SingleResult.Create(items);

            OnUsuarioGet(ref result);

            return result;
        }
        partial void OnUsuarioDeleted(QuickEx.Server.Models.QuickPrueba.Usuario item);
        partial void OnAfterUsuarioDeleted(QuickEx.Server.Models.QuickPrueba.Usuario item);

        [HttpDelete("/odata/QuickPrueba/Usuarios(IdUsuario={IdUsuario})")]
        public IActionResult DeleteUsuario(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Usuarios
                    .Where(i => i.IdUsuario == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<QuickEx.Server.Models.QuickPrueba.Usuario>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnUsuarioDeleted(item);
                this.context.Usuarios.Remove(item);
                this.context.SaveChanges();
                this.OnAfterUsuarioDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUsuarioUpdated(QuickEx.Server.Models.QuickPrueba.Usuario item);
        partial void OnAfterUsuarioUpdated(QuickEx.Server.Models.QuickPrueba.Usuario item);

        [HttpPut("/odata/QuickPrueba/Usuarios(IdUsuario={IdUsuario})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutUsuario(long key, [FromBody]QuickEx.Server.Models.QuickPrueba.Usuario item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Usuarios
                    .Where(i => i.IdUsuario == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<QuickEx.Server.Models.QuickPrueba.Usuario>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnUsuarioUpdated(item);
                this.context.Usuarios.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Usuarios.Where(i => i.IdUsuario == key);
                ;
                this.OnAfterUsuarioUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/QuickPrueba/Usuarios(IdUsuario={IdUsuario})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchUsuario(long key, [FromBody]Delta<QuickEx.Server.Models.QuickPrueba.Usuario> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Usuarios
                    .Where(i => i.IdUsuario == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<QuickEx.Server.Models.QuickPrueba.Usuario>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnUsuarioUpdated(item);
                this.context.Usuarios.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Usuarios.Where(i => i.IdUsuario == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUsuarioCreated(QuickEx.Server.Models.QuickPrueba.Usuario item);
        partial void OnAfterUsuarioCreated(QuickEx.Server.Models.QuickPrueba.Usuario item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] QuickEx.Server.Models.QuickPrueba.Usuario item)
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

                this.OnUsuarioCreated(item);
                this.context.Usuarios.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Usuarios.Where(i => i.IdUsuario == item.IdUsuario);

                ;

                this.OnAfterUsuarioCreated(item);

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
