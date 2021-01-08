using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Tasks;
using Nop.Services.Localization;
using Nop.Services.Common;
using Nop.Services.Plugins;
using Nop.Services.Tasks;
using Nop.Web.Framework.Menu;
using Microsoft.AspNetCore.Routing;

namespace Nop.Plugin.Misc.ErplyIntegration
{
    class ErplyIntegrationPlugin : BasePlugin, IMiscPlugin, IAdminMenuPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public ErplyIntegrationPlugin(
                ILocalizationService localizationService,
                IScheduleTaskService scheduleTaskService,
                IWebHelper webHelper
            )
        {
            _localizationService = localizationService;
            _scheduleTaskService = scheduleTaskService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/ErplyIntegration/Configure";
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            //install categories import task
            InstallTask(ErplyIntegrationDefaults.CategoriesImportTask, "Erply categories import");

            //install products import task
            InstallTask(ErplyIntegrationDefaults.ProductsImportTask, "Erply products import");

            //locales
            _localizationService.AddPluginLocaleResource(new Dictionary<string, string>
            {
                ["Plugins.Misc.ErplyIntegration.Username"] = "Username",
                ["Plugins.Misc.ErplyIntegration.Password"] = "Password",
                ["Plugins.Misc.ErplyIntegration.ClientCode"] = "Client code",
            });

            base.Install();
        }

        private void InstallTask(string taskType, string taskName)
        {
            if (_scheduleTaskService.GetTaskByType(taskType) == null)
            {
                _scheduleTaskService.InsertTask(new ScheduleTask
                {
                    Enabled = false,
                    Seconds = 3600,
                    Name = taskName,
                    Type = taskType,
                });
            }
        }

        private void DeleteTask(string taskType)
        {
            var task = _scheduleTaskService.GetTaskByType(taskType);
            if (task != null)
                _scheduleTaskService.DeleteTask(task);
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            //delete categories import task
            DeleteTask(ErplyIntegrationDefaults.CategoriesImportTask);

            //delete products import task
            DeleteTask(ErplyIntegrationDefaults.ProductsImportTask);

            //locales
            _localizationService.DeletePluginLocaleResources("Plugins.Misc.ErplyIntegration");

            base.Uninstall();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "ErplyImport",
                Title = "Erply import",
                ControllerName = "ErplyIntegration",
                ActionName = "ManageImport",
                IconClass = "fa-dot-circle-o",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "Area", "Admin" } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Catalog");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }

        #endregion
    }
}
