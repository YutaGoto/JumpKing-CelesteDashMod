﻿using BehaviorTree;
using EntityComponent.BT;
using EntityComponent;
using JumpKing.Controller;
using JumpKing.PauseMenu.BT;
using JumpKing.PauseMenu;
using JumpKing;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JumpKing_CelesteDashMod.Menu
{
    public class CustomBindDisplay : EntityBTNode, IMenuItem, UnSelectable
    {
        private EBinding m_button;
        private SpriteFont Font => Game1.instance.contentManager.font.MenuFontSmall;

        internal CustomBindDisplay(Entity p_entity, EBinding p_button)
            : base(p_entity)
        {
            m_button = p_button;
        }

        public  void Draw(int x, int y, bool selected)
        {
            var m_pad = ControllerManager.instance.GetMain();

            // key type
            string p_string = m_button.ToString() + " : ";

            var pad = m_pad.GetPad();

            MenuItemHelper.Draw(x, y, p_string, Color.Gray, Font);

            int x2 = GetSize().X;
            foreach (int bind in ModEntry.Preferences.KeyBindings[m_button])
            {
                x += (int)((float)x2 / 3f);
                p_string = pad.ButtonToString(bind);

                p_string = FormatString(p_string);
                MenuItemHelper.Draw(x, y, p_string, Color.Gray, Font);
            }

            if (ModEntry.Preferences.KeyBindings[m_button].Length == 0)
            {
                x += (int)((float)x2 / 3f);
                p_string = "-";
                MenuItemHelper.Draw(x + (int)((float)x2 / 3f * 1f), y, p_string, Color.Gray, Font);
            }
        }

        private string FormatString(string p_string)
        {
            int num = GetSize().X / 3;
            if (MenuItemHelper.GetSize(p_string, Font).X > num)
            {
                while (MenuItemHelper.GetSize(p_string, Font).X > num)
                {
                    p_string = p_string.Substring(0, p_string.Length - 1);
                }

                return p_string.Substring(0, p_string.Length - 1) + "*";
            }

            return p_string;
        }

        public Point GetSize()
        {
            return MenuItemHelper.GetSize("xbox 360 controller 1____         ", Font);
        }

        protected override BTresult MyRun(TickData p_data)
        {
            return BTresult.Failure;
        }
    }
}
