using BehaviorTree;
using EntityComponent;
using EntityComponent.BT;

namespace JumpKing_CelesteDashMod.Menu
{
    internal class CustomBindDefault: EntityBTNode
    {
        public CustomBindDefault(Entity p_entity) : base(p_entity)
        {
        }

        protected override BTresult MyRun(TickData p_data)
        {
            ModEntry.Preferences.KeyBindings = new Preferences().KeyBindings;
            return BTresult.Success;
        }
    }
}
