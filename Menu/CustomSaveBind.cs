using BehaviorTree;
using EntityComponent.BT;
using EntityComponent;

namespace JumpKing_CelesteDashMod.Menu
{
    public class CustomSaveBind : EntityBTNode
    {
        public CustomSaveBind(Entity p_entity) : base(p_entity)
        {
        }

        protected override BTresult MyRun(TickData p_data)
        {
            ModEntry.Preferences.ForceUpdate();
            return BTresult.Success;
        }
    }
}
