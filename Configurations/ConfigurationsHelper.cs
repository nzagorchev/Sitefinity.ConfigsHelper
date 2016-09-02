using System.Linq;
using System.Web.UI;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Web.UI.Backend.Elements.Config;
using Telerik.Sitefinity.Web.UI.Backend.Elements.Widgets;
using Telerik.Sitefinity.Web.UI.ContentUI.Config;
using Telerik.Sitefinity.Web.UI.ContentUI.Views.Backend.Master.Config;
using Telerik.Sitefinity.Web.UI.Fields.Config;

namespace SitefinityWebApp.Configurations
{
    internal class ConfigurationsHelper
    {
        internal static T GetContentViewElement<T>(ConfigManager configManager, string contentViewControlName,
    string viewName, out ContentViewConfig config)
            where T : ContentViewDefinitionElement
        {
            config = configManager.GetSection<ContentViewConfig>();
            var contentBackend = config.ContentViewControls[contentViewControlName];
            var view = contentBackend.ViewsConfig.Values.FirstOrDefault(v => v.ViewName == viewName);
            var viewElement = (T)view;

            return viewElement;
        }

        internal static FieldDefinitionElement GetFieldElement(ContentViewDetailElement contentView, string sectionName, string fieldName)
        {
            var sectionElement = contentView.Sections[sectionName];
            var fieldElement = sectionElement.Fields[fieldName];

            return fieldElement;
        }

        internal static WidgetBarSectionElement GetToolbarElement(MasterGridViewElement masterView)
        {
            var toolbar = masterView.ToolbarConfig.Sections.Elements.Where(e => e.Name == "toolbar").First();

            return toolbar;
        }

        internal static ConfigElementList<WidgetElement> GetActionsMenuItems(MasterGridViewElement masterView, string name)
        {
            // Grid or TreeTable
            var gridElement = masterView.ViewModesConfig.Elements.FirstOrDefault(vm => vm.Name == name);

            var gridModelElement = gridElement as GridViewModeElement;
            var actionsLinkColumnList = gridModelElement.ColumnsConfig.Elements.FirstOrDefault(c => c.Name == "ActionsLinkText");
            var actionsMenuElement = actionsLinkColumnList as ActionMenuColumnElement;

            var commandList = actionsMenuElement.MenuItems;
            return commandList;
        }

        internal static void AddExtensionScript(ContentViewMasterElement masterView,
            string scriptLocation, string loadMethodName,
            out bool needSave)
        {
            needSave = false;
            var script = masterView.Scripts.Elements
                .Where(e => e.ScriptLocation == scriptLocation)
                .FirstOrDefault();

            if (script == null)
            {
                var clientScript = new ClientScriptElement(masterView.Scripts);
                clientScript.ScriptLocation = scriptLocation;
                clientScript.LoadMethodName = loadMethodName;
                clientScript.CollectionItemName = "script";
                masterView.Scripts.Add(clientScript);

                needSave = true;
            }
        }

        internal static CommandWidgetElement CreateCommandWidgetElement(ConfigElement parent,
            string commandArgument, string commandName,
            string name, string text)
        {
            var commandWidget = new CommandWidgetElement(parent);
            commandWidget.CommandArgument = commandArgument;
            commandWidget.CommandName = commandName;
            commandWidget.Name = name;
            commandWidget.Text = text;
            commandWidget.WrapperTagKey = HtmlTextWriterTag.Li;
            commandWidget.ButtonType = Telerik.Sitefinity.Web.UI.Backend.Elements.Enums.CommandButtonType.Standard;
            commandWidget.WidgetType = typeof(CommandWidget);

            return commandWidget;
        }
    }
}