using System;
using System.Linq;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Modules.Events;
using Telerik.Sitefinity.Modules.News;
using Telerik.Sitefinity.Web.UI.Backend.Elements.Config;
using Telerik.Sitefinity.Web.UI.Backend.Elements.Widgets;
using Telerik.Sitefinity.Web.UI.ContentUI.Config;
using Telerik.Sitefinity.Web.UI.ContentUI.Views.Backend.Master.Config;

namespace SitefinityWebApp.Configurations
{
    public partial class ConfigurationsSamples : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          //  InstallNewsExtensionScript();
            ModifyPressReleasesListViewToolbarCreateButton();
            //    InstallPagesCommandWidgets();
            ModifyRecurrencyFieldCssClass();
        }

        public virtual void InstallNewsExtensionScript()
        {
            var configManager = ConfigManager.GetManager();
            using (new ElevatedModeRegion(configManager))
            {
                ContentViewConfig config;
                string contentView = NewsDefinitions.BackendDefinitionName;
                string viewName = NewsDefinitions.BackendListViewName;

                var masterView = ConfigurationsHelper.GetContentViewElement<ContentViewMasterElement>(configManager,
                    contentView, viewName,
                    out config);

                bool needSave;
                ConfigurationsHelper.AddExtensionScript(masterView, "~/News/NewsExtensionScript.js", "newsMasterViewLoadedExtended", out needSave);
                if (needSave)
                {
                    configManager.SaveSection(config, true);
                }
            }
        }

        public static void ModifyPressReleasesListViewToolbarCreateButton()
        {
            // 'Press releases' dynamic module, 'Press release' content type
            string contentViewControlName = "Telerik.Sitefinity.DynamicTypes.Model.Pressreleases.PressReleaseBackendDefinition";
            string viewName = "Press releaseBackendList";

            var configManager = ConfigManager.GetManager();
            using (new ElevatedModeRegion(configManager))
            {
                ContentViewConfig config;
                var masterView = ConfigurationsHelper.GetContentViewElement<MasterGridViewElement>(configManager, contentViewControlName, viewName, out config);

                var toolbar = masterView.ToolbarConfig.Sections.Elements.Where(e => e.Name == "toolbar").First();

                CommandWidgetElement createItem = toolbar.Items.Elements.Where(i => i.Name == "CreateItemWidget").First() as CommandWidgetElement;
                createItem.Text = "CREATE PRESS RELEASES";

                //configManager.SaveSection(config, true);
                configManager.SaveSection(createItem.Section, true);
            }
            //configManager.Dispose();
        }

        public virtual void InstallPagesCommandWidgets()
        {
            var configManager = ConfigManager.GetManager();
            using (new ElevatedModeRegion(configManager))
            {
                ContentViewConfig config;
                var masterView = ConfigurationsHelper.GetContentViewElement<MasterGridViewElement>(configManager,
                   "FrontendPages", "FrontendPagesListView",
                   out config);

                // TreeTable
                var actionsMenuElements = ConfigurationsHelper.GetActionsMenuItems(masterView, "TreeTable");

                bool needSaveChanges = false;
                string commandName = "unlockPageCustom";

                var command = actionsMenuElements.Elements
                    .Where(e => e.WidgetType == typeof(CommandWidget)
                        && ((CommandWidgetElement)e).CommandName == commandName)
                    .FirstOrDefault();

                if (command == null)
                {
                    var commandWidget = ConfigurationsHelper.CreateCommandWidgetElement(actionsMenuElements, string.Empty,
                        commandName, "UnlockPage", "Unlock page");
                    actionsMenuElements.Add(commandWidget);
                    needSaveChanges = true;
                }

                // ListView
                var actionsMenuItemsElementList = ConfigurationsHelper.GetActionsMenuItems(masterView, "Grid");

                var commandList = actionsMenuItemsElementList.Elements
                    .Where(e => e.WidgetType == typeof(CommandWidget)
                        && ((CommandWidgetElement)e).CommandName == commandName)
                    .FirstOrDefault();

                if (commandList == null)
                {
                    var commandWidget = ConfigurationsHelper.CreateCommandWidgetElement(actionsMenuItemsElementList, string.Empty,
                        commandName, "UnlockPage", "Unlock page");
                    actionsMenuItemsElementList.Add(commandWidget);
                    needSaveChanges = true;
                }

                if (needSaveChanges)
                {
                    configManager.SaveSection(config);
                }
            }
        }

        public static void ModifyRecurrencyFieldCssClass()
        {
            string contentViewControlName = EventsDefinitions.BackendDefinitionName; // "EventsBackend" 
            string viewName = EventsDefinitions.BackendEditDetailsViewName; // "EventsBackendEdit"
            string sectionName = "MainSection";
            string fieldName = "RecurrencyField";

            var configManager = ConfigManager.GetManager();
            using (new ElevatedModeRegion(configManager))
            {
                ContentViewConfig config;
                var detailView = ConfigurationsHelper.GetContentViewElement<DetailFormViewElement>(configManager, contentViewControlName, viewName, out config);

                var fieldElement = ConfigurationsHelper.GetFieldElement(detailView, sectionName, fieldName);

                // Set as display none
                fieldElement.CssClass += " sfDisplayNoneImportant dasdasd123123123131232131";

                configManager.SaveSection(fieldElement.Section, true);
            }
        }
    }
}