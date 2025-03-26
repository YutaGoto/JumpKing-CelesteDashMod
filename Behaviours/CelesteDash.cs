using JumpKing;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Controller;
using JumpKing.Player;
using JumpKing_CelesteDashMod.Model;
using System;

namespace JumpKing_CelesteDashMod.Behaviours
{
    internal class CelesteDash : IBodyCompBehaviour
    {
        private bool celesteDashFlag;
        private readonly float celesteDashVelocity = 8.46f;

        public bool ExecuteBehaviour(BehaviourContext behaviourContext)
        {
            if (!ModEntry.Preferences.IsEnabled) return true;

            BodyComp bodyComp = behaviourContext.BodyComp;
            CustomPadState state = ModEntry.PadInstance.GetPressed();
            PadState m_padState = ControllerManager.instance.GetPadState();

            if (bodyComp.IsOnGround)
            {
                celesteDashFlag = false;
            }

            if (!bodyComp.IsOnGround)
            {
                if (m_padState.right) bodyComp.Velocity.X += PlayerValues.WALK_SPEED / 100.0f;
                if (m_padState.left) bodyComp.Velocity.X -= PlayerValues.WALK_SPEED / 100.0f;
            }

            if (state.dash && !celesteDashFlag)
            {
                if (m_padState.up && m_padState.right)
                {
                    bodyComp.Velocity.Y = -celesteDashVelocity / (float)Math.Sqrt(2);
                    bodyComp.Velocity.X = PlayerValues.SPEED;
                }
                else if (m_padState.up && m_padState.left)
                {
                    bodyComp.Velocity.Y = -celesteDashVelocity / (float)Math.Sqrt(2);
                    bodyComp.Velocity.X = -PlayerValues.SPEED;
                }
                else if (m_padState.down && m_padState.right)
                {
                    bodyComp.Velocity.Y = celesteDashVelocity / (float)Math.Sqrt(2);
                    bodyComp.Velocity.X = PlayerValues.SPEED;
                }
                else if (m_padState.down && m_padState.left)
                {
                    bodyComp.Velocity.Y = celesteDashVelocity / (float)Math.Sqrt(2);
                    bodyComp.Velocity.X = -PlayerValues.SPEED;
                }
                else if (m_padState.down)
                {
                    bodyComp.Velocity.Y = celesteDashVelocity / (float)Math.Sqrt(1.5);
                    bodyComp.Velocity.X = 0.0f;
                }
                else if (m_padState.right)
                {
                    bodyComp.Velocity.Y = -0.1f;
                    bodyComp.Velocity.X = PlayerValues.SPEED * (float)Math.Sqrt(2);
                }
                else if (m_padState.left)
                {
                    bodyComp.Velocity.Y = -0.1f;
                    bodyComp.Velocity.X = -PlayerValues.SPEED * (float)Math.Sqrt(2);
                }
                else
                {
                    bodyComp.Velocity.Y = -celesteDashVelocity / (float)Math.Sqrt(1.5);
                    bodyComp.Velocity.X = 0.0f;
                }

                Game1.instance?.contentManager?.audio?.player?.Jump?.PlayOneShot();
                celesteDashFlag = true;
            }

            return true;
        }
    }
}
