using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using PRYANIK_Plugin.Services;


namespace PRYANIK_Plugin
{
    [Plugin("PRYANIK_Plugin", "хPRYANIKх", DisplayName = "PRYANIK_Plugin")]
    [RibbonLayout("PRYANIK_Plugin.xaml")]

    [RibbonTab("PluginTab", DisplayName = "PRYANIK_Plugin")]
    [Command("ID_Button_A", DisplayName = "Автоматический комментарий", Icon = "Source\\comment_small.png", LargeIcon = "Source\\comment_big.png", ToolTip = "Создание текстового комментария. Для создания комментария в автоматическом режиме необходимо выделить два эллемента и нажать клавишу Q", CanToggle = true)]
    public class Main : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string name, params string[] parameters)
        {
            switch (name)
            {
                case "ID_Button_A":
                    {
                        if (Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems.Count >= 1 && Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems.Count <= 20)
                        {
                            AddComment.GettingDataForAComment();
                            AddComment.CreateViewpoint();
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return 0;
        }
    }
}

namespace PRYANIK_Plugin_inputPlugin
{
    [Plugin("PRYANIK_inputPlugin", "хPRYANIKх")]
    public class Main : InputPlugin
    {
        public override bool KeyUp(View view, KeyModifiers modifier, ushort key, double timeOffset)
        {
            f (Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems.Count >= 1 && Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems.Count <= 20 && key == 69)
            {
                AddComment.GettingDataForAComment();
                AddComment.CreateViewpoint();
                return true;
            }

            return base.KeyUp(view, modifier, key, timeOffset);
        }
    }
}