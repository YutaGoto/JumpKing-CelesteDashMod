
using JumpKing.PauseMenu.BT.Actions;

namespace JumpKing_CelesteDashMod.Menu
{
    internal class ToggleEnabled : ITextToggle
    {
        public ToggleEnabled() : base(ModEntry.Preferences.IsEnabled) { }

        protected override string GetName() => "Enable";

        protected override void OnToggle() => ModEntry.Preferences.IsEnabled = toggle;
    }
}
