using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.UI {

    class FastMessage : MessagePopup
    {

        private bool _fadeIn = true;
        
        public FastMessage(string text, Vector2 position) : base(text, position)
        {
            Position.X -= ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(text).X / 2;
            AlphaChange = 0.01f;
            CurrentAlpha = 0;
        }

        public override void Update()
        {
            if (!_fadeIn) return;

            Position.Y--;

            if (CurrentAlpha + AlphaChange <= 1)
            {
                CurrentAlpha += AlphaChange;
            }
            else
            {
                _fadeIn = false;
            }
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            base.Paint(spriteBatch);
        }
    }
}
