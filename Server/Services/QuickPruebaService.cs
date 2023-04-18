using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using QuickEx.Server.Data;

namespace QuickEx.Server
{
    public partial class QuickPruebaService
    {
        QuickPruebaContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly QuickPruebaContext context;
        private readonly NavigationManager navigationManager;

        public QuickPruebaService(QuickPruebaContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportAlumnosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/quickprueba/alumnos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/quickprueba/alumnos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAlumnosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/quickprueba/alumnos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/quickprueba/alumnos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAlumnosRead(ref IQueryable<QuickEx.Server.Models.QuickPrueba.Alumno> items);

        public async Task<IQueryable<QuickEx.Server.Models.QuickPrueba.Alumno>> GetAlumnos(Query query = null)
        {
            var items = Context.Alumnos.OrderBy(i => i.dni).AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnAlumnosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAlumnoGet(QuickEx.Server.Models.QuickPrueba.Alumno item);

        public async Task<QuickEx.Server.Models.QuickPrueba.Alumno> GetAlumnoByDni(string dni)
        {
            var items = Context.Alumnos
                              .AsNoTracking()
                              .Where(i => i.dni == dni);

  
            var itemToReturn = items.FirstOrDefault();

            OnAlumnoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAlumnoCreated(QuickEx.Server.Models.QuickPrueba.Alumno item);
        partial void OnAfterAlumnoCreated(QuickEx.Server.Models.QuickPrueba.Alumno item);

        public async Task<QuickEx.Server.Models.QuickPrueba.Alumno> CreateAlumno(QuickEx.Server.Models.QuickPrueba.Alumno alumno)
        {
            OnAlumnoCreated(alumno);

            var existingItem = Context.Alumnos
                              .Where(i => i.dni == alumno.dni)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Alumnos.Add(alumno);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(alumno).State = EntityState.Detached;
                throw;
            }

            OnAfterAlumnoCreated(alumno);

            return alumno;
        }

        public async Task<QuickEx.Server.Models.QuickPrueba.Alumno> CancelAlumnoChanges(QuickEx.Server.Models.QuickPrueba.Alumno item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAlumnoUpdated(QuickEx.Server.Models.QuickPrueba.Alumno item);
        partial void OnAfterAlumnoUpdated(QuickEx.Server.Models.QuickPrueba.Alumno item);

        public async Task<QuickEx.Server.Models.QuickPrueba.Alumno> UpdateAlumno(string dni, QuickEx.Server.Models.QuickPrueba.Alumno alumno)
        {
            OnAlumnoUpdated(alumno);

            var itemToUpdate = Context.Alumnos
                              .Where(i => i.dni == alumno.dni)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(alumno);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAlumnoUpdated(alumno);

            return alumno;
        }

        partial void OnAlumnoDeleted(QuickEx.Server.Models.QuickPrueba.Alumno item);
        partial void OnAfterAlumnoDeleted(QuickEx.Server.Models.QuickPrueba.Alumno item);

        public async Task<QuickEx.Server.Models.QuickPrueba.Alumno> DeleteAlumno(string dni)
        {
            var itemToDelete = Context.Alumnos
                              .Where(i => i.dni == dni)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAlumnoDeleted(itemToDelete);


            Context.Alumnos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAlumnoDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}