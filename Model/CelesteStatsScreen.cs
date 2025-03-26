using HarmonyLib;
using JumpKing;
using JumpKing.GameManager;
using JumpKing.Util;
using Microsoft.Xna.Framework;

namespace JumpKing_CelesteDashMod.Model
{
    internal class CelesteStatsScreen
    {
        internal CelesteStatsScreen(Harmony harmony)
        {
            harmony.Patch(
                AccessTools.Method(typeof(StatsScreen), "DrawStats"),
                null,
                new HarmonyMethod(AccessTools.Method(GetType(), nameof(DrawCelesteMod)))
            );
        }

        private static void DrawCelesteMod()
        {
            if (!ModEntry.Preferences.IsEnabled) return;

            TextStyle textStyle = default;
            textStyle.color = Color.White;
            textStyle.layered = false;
            TextStyle p_style = textStyle;

            TextHelper.DrawStyleString(
                Game1.instance.contentManager.font.MenuFont,
                "Celeste Dash Mode",
                new Vector2(0, JumpGame.GAME_RECT.Height - 50),
                new Vector2(0, 0),
                p_style
            );
        }

    }
}
