using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace QuickEx.Client.Pages
{
    public partial class Alumnos
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public QuickPruebaService QuickPruebaService { get; set; }

        protected IEnumerable<QuickEx.Server.Models.QuickPrueba.Alumno> alumnos;

        protected RadzenDataGrid<QuickEx.Server.Models.QuickPrueba.Alumno> grid0;
        protected int count;
        protected bool isEdit = true;

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await QuickPruebaService.GetAlumnos(filter: $"{args.Filter}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                alumnos = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Alumnos" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            alumno = new QuickEx.Server.Models.QuickPrueba.Alumno();
        }

        protected async Task EditRow(QuickEx.Server.Models.QuickPrueba.Alumno args)
        {
            isEdit = true;
            alumno = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, QuickEx.Server.Models.QuickPrueba.Alumno alumno)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await QuickPruebaService.DeleteAlumno(dni:alumno.dni);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Alumno" 
                });
            }
        }
        protected bool errorVisible;
        protected QuickEx.Server.Models.QuickPrueba.Alumno alumno;

        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await QuickPruebaService.UpdateAlumno(dni:alumno.dni, alumno) : await QuickPruebaService.CreateAlumno(alumno);

            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {

        }
    }
}