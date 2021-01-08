using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.ErplyIntegration.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Plugin.Misc.ErplyIntegration.Services;

namespace Nop.Plugin.Misc.ErplyIntegration.Controllers
{
    public class ErplyIntegrationController : BasePluginController
    {

        #region Fields

        private readonly ErplyIntegrationSettings _erplyIntegrationSettings;
        private readonly IErplyImportManager _erplyImportManager;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public ErplyIntegrationController(ErplyIntegrationSettings erplyIntegrationSettings,
            IErplyImportManager erplyImportManager,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService)
        {
            _erplyIntegrationSettings = erplyIntegrationSettings;
            _erplyImportManager = erplyImportManager;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                Username = _erplyIntegrationSettings.Username,
                Password = _erplyIntegrationSettings.Password,
                ClientCode = _erplyIntegrationSettings.ClientCode
            };

            return View("~/Plugins/Misc.ErplyIntegration/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            // save settings
            _erplyIntegrationSettings.Username = model.Username;
            _erplyIntegrationSettings.Password = model.Password;
            _erplyIntegrationSettings.ClientCode = model.ClientCode;
            _settingService.SaveSetting(_erplyIntegrationSettings);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult ManageImport()
        {
            return View("~/Plugins/Misc.ErplyIntegration/Areas/Admin/Views/ManageImport.cshtml");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult ManageImport(string importType)
        {
            switch (importType)
            {
                case "products":
                    ImportProducts();
                    break;
                case "categories":
                    ImportCategories();
                    break;
            }
            return View("~/Plugins/Misc.ErplyIntegration/Areas/Admin/Views/ManageImport.cshtml");
        }

        private async void ImportCategories()
        {
            try
            {
                await _erplyImportManager.ImportCategories();
                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Imported"));
            } catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc);
            }
            
        }

        private async void ImportProducts()
        {
            try
            {
                await _erplyImportManager.ImportProducts();
                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Imported"));
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc);
            }
        }

        #endregion
    }
}
