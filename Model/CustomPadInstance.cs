

using HarmonyLib;
using JumpKing.Controller;
using System.Diagnostics;

namespace JumpKing_CelesteDashMod.Model
{
    internal class CustomPadInstance
    {

        private IPad pad;
        private CustomPadState last_state;
        private CustomPadState current_state;

        public CustomPadInstance(Harmony harmony)
        {
            harmony.Patch(
                AccessTools.Method("JumpKing.Controller.ControllerManager:Update"),
                postfix: new HarmonyMethod(AccessTools.Method(typeof(CustomPadInstance), nameof(MyUpdate)))
            );
        }

        public void Update(object __instance)
        {
            IPad traverse_pad = Traverse.Create(__instance).Field("_current_main").Field("m_pad").GetValue<IPad>();
            if (traverse_pad != pad)
            {
                pad = traverse_pad;
            }

            if (pad != null)
            {
                this.last_state = this.current_state;
                this.current_state = this.GetPadState();
            }
        }

        public static void MyUpdate(object __instance)
        {
            ModEntry.PadInstance.Update(__instance);
        }

        public CustomPadState GetState()
        {
            return this.current_state;
        }

        public CustomPadState GetPressed()
        {
            return new CustomPadState
            {
                dash = (!last_state.dash && current_state.dash),
            };
        }

        private CustomPadState GetPadState()
        {
            int[] pressedButtons = this.pad.GetPressedButtons();
            return new CustomPadState
            {
                dash = IsPressed(pressedButtons, ModEntry.Preferences.KeyBindings[EBinding.Dash]),
            };
        }

        private bool IsPressed(int[] pressed, int[] bind)
        {
            foreach (int num in pressed)
            {
                foreach (int num2 in bind)
                {
                    if (num == num2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public struct CustomPadState
    {
        public bool dash;
    }
}
