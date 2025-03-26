using BehaviorTree;
using HarmonyLib;
using JumpKing.Player;

namespace JumpKing_CelesteDashMod.Model
{
    [HarmonyPatch(typeof(JumpState))]
    internal class CelesteJump
    {
        internal CelesteJump(Harmony harmony)
        {
            harmony.Patch(
                AccessTools.Method(typeof(JumpState), "DoJump"),
                new HarmonyMethod(AccessTools.Method(GetType(), nameof(Jump))),
                null
            );
        }

        private static bool Jump(ref float p_intensity)
        {
            if (!ModEntry.Preferences.IsEnabled) return true;

            if (p_intensity <= 0.2f)
            {
                p_intensity = 0.301f;
            }
            else
            {
                p_intensity = 0.8f;
            }

            return true;
        }

    }
}
