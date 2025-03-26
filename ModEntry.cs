using System;
using HarmonyLib;
using JumpKing.Mods;
using JumpKing.PauseMenu.BT.Actions;
using JumpKing.PauseMenu;
using JumpKing_CelesteDashMod.Menu;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using JumpKing_CelesteDashMod.Model;
using EntityComponent;
using JumpKing.Player;
using JumpKing.PauseMenu.BT;

namespace JumpKing_CelesteDashMod
{
    [JumpKingMod("YutaGoto.JumpKing_CelesteDashMod")]
    public static class ModEntry
	{
        private static readonly string IDENTIFIER = "JumpKing_CelesteDashMod";
        private static readonly string harmonyId = "YutaGoto.JumpKing_CelesteDashMod";
        public static readonly Harmony harmony = new Harmony(harmonyId);
        public const string SETTINGS_FILE = "YutaGoto.CelesteDashMod.Settings.xml";
        private static string AssemblyPath { get; set; }
        internal static Preferences Preferences { get; private set; }
        internal static CustomPadInstance PadInstance { get; private set; }
        internal static Behaviours.CelesteDash Behaviour { get; private set; }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static ITextToggle AddToggleEnabled(object factory, GuiFormat format)
        {
            return new ToggleEnabled();
        }

        [PauseMenuItemSetting]
        [MainMenuItemSetting]
        public static TextButton BindSettings(object factory, GuiFormat format)
        {
            return new TextButton("Bind Keys", MenuOptions.CreateSaveStatesBindControls(factory));
        }

        /// <summary>
        /// Called by Jump King before the level loads
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
#if DEBUG
            Debug.WriteLine("-------");
            Debugger.Launch();
            Harmony.DEBUG = true;
#endif

            AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // try reading config file
            try
            {
                Preferences = XmlSerializerHelper.Deserialize<Preferences>($@"{AssemblyPath}\{SETTINGS_FILE}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[ERROR] [{IDENTIFIER}] {e.Message}");
                Preferences = new Preferences();
            }

            Preferences.PropertyChanged += ToggleCelesteDash;
            Preferences.PropertyChanged += SaveSettingsOnFile;

            PadInstance = new CustomPadInstance(harmony);
            _ = new CelesteJump(harmony);
            Behaviour = new Behaviours.CelesteDash();
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            PlayerEntity player = EntityManager.instance.Find<PlayerEntity>();

            if (player != null)
            {
                player.m_body.RegisterBehaviour(Behaviour);
            }
            
            _ = new CelesteStatsScreen(harmony);
        }

        /// <summary>
        /// Called by Jump King when the Level Ends
        /// </summary>
        [OnLevelEnd]
        public static void OnLevelEnd()
        {
            // Your code here
        }

        private static void ToggleCelesteDash(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Preferences.IsEnabled))
                return;

            if (!bool.TryParse(((Preferences)sender).IsEnabled.ToString(), out bool isEnabled))
                return;

            PlayerEntity player = EntityManager.instance.Find<PlayerEntity>();
            if (player is null)
                return;

            if (isEnabled)
            {
                player.m_body.RegisterBehaviour(Behaviour);
            }
            else
            {
                player.m_body.RemoveBehaviour(Behaviour);
            }
        }

        private static void SaveSettingsOnFile(object sender, PropertyChangedEventArgs args)
        {
            try
            {
                XmlSerializerHelper.Serialize($@"{AssemblyPath}\{SETTINGS_FILE}", Preferences);
                Debug.WriteLine("---Saved---");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[ERROR] [{IDENTIFIER}] {e.Message}");
            }
        }
    }

    public enum EBinding
    {
        Dash,
    }
}
