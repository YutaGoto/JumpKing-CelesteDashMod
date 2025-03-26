
using BehaviorTree;
using EntityComponent.BT;
using EntityComponent;
using HarmonyLib;
using JumpKing.PauseMenu.BT.Actions.BindController;
using JumpKing.PauseMenu.BT.Actions;
using JumpKing.PauseMenu.BT;
using JumpKing.PauseMenu;
using JumpKing;
using IDrawable = JumpKing.Util.IDrawable;
using LanguageJK;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using JumpKing.Util;

namespace JumpKing_CelesteDashMod.Menu
{
    class MenuOptions
    {
        private static Traverse _drawables;
        public static List<IDrawable> MenuFactoryDrawables
        {
            get => _drawables.GetValue<List<IDrawable>>();
            set => _drawables.SetValue(value);
        }

        internal static BTsimultaneous CreateSaveStatesBindControls(object __instance)
        {
            var _this = Traverse.Create(__instance);
            _drawables = _this.Field("m_drawables");

            var _entity = _this.Field("m_entity").GetValue<Entity>();
            var gui_left = _this.Field("CONTROLS_GUI_FORMAT_LEFT").GetValue<GuiFormat>();
            var gui_right = _this.Field("CONTROLS_GUI_FORMAT_RIGHT").GetValue<GuiFormat>();

            MenuSelector menuSelector = new MenuSelector(gui_left);

            BTsimultaneous btsimultaneous = new BTsimultaneous(new IBTnode[0]);
            btsimultaneous.AddChild(menuSelector);
            MenuFactoryDrawables.Add(menuSelector);

            // left
            SpriteFont menuFontSmall = Game1.instance.contentManager.font.MenuFontSmall;
            menuSelector.AddChild<TextButton>(new TextButton(language.MENUFACTORY_INPUT_SCAN_FOR_DEVICES, new GetSlimDevices(), menuFontSmall));
            menuSelector.AddChild<SelectDevice>(new SelectDevice(_entity));
            int count = MenuFactoryDrawables.Count;
            IBTnode p_child = MakeBindController(0, _entity);
            IBTnode p_child2 = MakeBindController(1, _entity);
            menuSelector.AddChild<TextButton>(new TextButton(language.MENUFACTORY_INPUT_BIND_PRIMARY, p_child, menuFontSmall));
            menuSelector.AddChild<TextButton>(new TextButton(language.MENUFACTORY_INPUT_BIND_SECONDARY, p_child2, menuFontSmall));

            BTsequencor btsequencor = new BTsequencor();
            btsequencor.AddChild(new CustomBindDefault(_entity));
            btsequencor.AddChild(new SetBBKeyNode<bool>(_entity, "BBKEY_UNSAVED_CHANGED", true));
            menuSelector.AddChild<TextButton>(new TextButton(language.MENUFACTORY_INPUT_DEFAULT, btsequencor, menuFontSmall));

            BTsequencor btsequencor2 = new BTsequencor();
            btsequencor2.AddChild(new CustomSaveBind(_entity));
            btsequencor2.AddChild(new SetBBKeyNode<bool>(_entity, "BBKEY_UNSAVED_CHANGED", true));
            menuSelector.AddChild<SaveNotifier>(new SaveNotifier(_entity, new TextButton(language.MENUFACTORY_SAVE, btsequencor2, menuFontSmall)));


            menuSelector.Initialize(true);
            menuSelector.GetBounds();

            // right
            DisplayFrame displayFrame = new DisplayFrame(gui_right, BTresult.Running);
            displayFrame.AddChild<CustomBindDisplay>(new CustomBindDisplay(_entity, EBinding.Dash));

            displayFrame.Initialize();

            var drawables = MenuFactoryDrawables;
            drawables.Insert(count, displayFrame);
            MenuFactoryDrawables = drawables;

            btsimultaneous.AddChild(new StaticNode(displayFrame, BTresult.Failure));
            return btsimultaneous;
        }

        private static IBTnode MakeBindController(int p_order_index, Entity entity)
        {
            GuiFormat p_format = new GuiFormat
            {
                anchor_bounds = new Rectangle(0, 0, 480, 360),
                anchor = new Vector2(1f, 1f) / 2f,
                all_margin = 16,
                element_margin = 8,
                all_padding = 16
            };

            // could be improved
            BindCatchSave p_child = new BindCatchSave(entity);
            CustomBindDefault p_child2 = new CustomBindDefault(entity);
            MenuSelector menuSelector = new MenuSelector(p_format);
            menuSelector.AllowEscape = false;
            MenuSelectorBack p_child3 = new MenuSelectorBack(menuSelector);
            BTsequencor btsequencor = new BTsequencor();
            btsequencor.AddChild(p_child2);
            btsequencor.AddChild(new SetBBKeyNode<bool>(entity, "BBKEY_UNSAVED_CHANGED", true));
            btsequencor.AddChild(p_child3);
            TimerAction timerAction = new TimerAction(language.MENUFACTORY_REVERTS_IN, 5, Color.Gray, btsequencor);
            menuSelector.AddChild<TextInfo>(new TextInfo(language.MENUFACTORY_KEEPCHANGES, Color.Gray));
            menuSelector.AddChild<TimerAction>(timerAction);
            menuSelector.AddChild<TextButton>(new TextButton(language.MENUFACTORY_NO, btsequencor));
            menuSelector.AddChild<TextButton>(new TextButton(language.MENUFACTORY_YES, p_child3));
            menuSelector.SetNodeForceRun(timerAction);
            menuSelector.Initialize(false);

            var drawables = MenuFactoryDrawables;
            drawables.Add(menuSelector);
            MenuFactoryDrawables = drawables;

            BTsequencor btsequencor2 = new BTsequencor();
            btsequencor2.AddChild(p_child);
            btsequencor2.AddChild(new WaitUntilNoMenuInput());
            btsequencor2.AddChild(MakeBindButtonMenu(EBinding.Dash, p_format, p_order_index, entity));
            btsequencor2.AddChild(new WaitUntilNoInput(entity));
            btsequencor2.AddChild(menuSelector);

            BTselector btselector = new BTselector(new IBTnode[0]);
            btselector.AddChild(btsequencor2);
            btselector.AddChild(new PlaySFX(Game1.instance.contentManager.audio.menu.MenuFail));
            return btselector;
        }

        private static BindButtonFrame MakeBindButtonMenu(EBinding p_button, GuiFormat p_format, int p_order_index, Entity m_entity)
        {
            BTsequencor btsequencor = new BTsequencor();
            btsequencor.AddChild(new WaitUntilNoInput(m_entity));
            btsequencor.AddChild(new CustomBindButton(m_entity, p_button, p_order_index));
            btsequencor.AddChild(new SetBBKeyNode<bool>(m_entity, "BBKEY_UNSAVED_CHANGED", true));
            BindButtonFrame bindButtonFrame = new BindButtonFrame(p_format, btsequencor);
            bindButtonFrame.AddChild<TextButton>(new TextButton(Util.ParseString(language.MENUFACTORY_PRESS_BUTTON, new object[]
            {
                p_button
            }), btsequencor));
            bindButtonFrame.Initialize();

            var draw = MenuFactoryDrawables;
            draw.Add(bindButtonFrame);
            MenuFactoryDrawables = draw;

            return bindButtonFrame;
        }
    }
}
