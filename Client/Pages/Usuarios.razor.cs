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
    public partial class Usuarios
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

        protected IEnumerable<QuickEx.Server.Models.QuickPrueba.Usuario> usuarios;

        protected RadzenDataGrid<QuickEx.Server.Models.QuickPrueba.Usuario> grid0;
        protected int count;
        protected bool isEdit = true;

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await QuickPruebaService.GetUsuarios(filter: $"{args.Filter}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                usuarios = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Usuarios" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            usuario = new QuickEx.Server.Models.QuickPrueba.Usuario();
            await grid0.Reload();
        }

        protected async Task EditRow(QuickEx.Server.Models.QuickPrueba.Usuario args)
        {
            isEdit = true;
            usuario = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, QuickEx.Server.Models.QuickPrueba.Usuario usuario)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await QuickPruebaService.DeleteUsuario(idUsuario:usuario.IdUsuario);

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
                    Detail = $"Unable to delete Usuario" 
                });
            }
        }
        protected bool errorVisible;
        protected QuickEx.Server.Models.QuickPrueba.Usuario usuario;

        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await QuickPruebaService.UpdateUsuario(idUsuario:usuario.IdUsuario, usuario) : await QuickPruebaService.CreateUsuario(usuario);
                usuario = new QuickEx.Server.Models.QuickPrueba.Usuario();

            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            await grid0.Reload();
        }
    }
}