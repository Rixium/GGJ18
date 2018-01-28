using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.UI {

    internal class MessagePopup : UiComponent
    {

        protected readonly string Text;
        protected Vector2 Position;

        protected readonly float YChange = 0.05f;

        protected float AlphaChange = 0.05f;
        protected float CurrentAlpha = 1;

        protected sbyte MessageTimer = 100;

        public MessagePopup(string text, Vector2 position)
        {
            Text = text;
            Position = position;
        }

        public override void Update()
        {
            Position.Y -= YChange;

            if (MessageTimer <= 0)
            {
                if (CurrentAlpha - AlphaChange >= 0)
                {
                    CurrentAlpha -= AlphaChange;
                }
                else
                {
                    Destroy = true;
                }
            }
            else
            {
                MessageTimer--;
            }
            base.Update();
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], Text, Position, Color.White * CurrentAlpha);
        }

    }
}
