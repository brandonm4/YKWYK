using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

using YouKeepWhatYouKill.Models;

namespace Utility
{
    public static class MessageHelper
    {
        public static void ShowMessage(string msg, Color? color = null, bool showAlways = false)
        {
            if (showAlways || Settings.Instance.DebugMode)
            {
                if (color == null)
                {
                    color = Color.White;
                }
                try
                {
                    InformationManager.DisplayMessage(new InformationMessage(msg, (Color)color));
                }
                catch { }
            }
        }

        public static void ShowNotification(TextObject message, int priority = 0, BasicCharacterObject charObj = null,
                                             string soundEventPath = "")
        {
            MBInformationManager.AddQuickInformation(message, 0, charObj, soundEventPath);
        }

        public static void ShowDebugMessage(string msg)
        {
            if (Settings.Instance.DebugMode)
            {
                ShowMessage(msg, Colors.Red);
            }
        }
    }
}
