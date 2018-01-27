using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Games;
using GGJ.Managers;
using GGJ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Screens {

    internal class EndScreen : Screen
    {

        private readonly TextPopup _message;
        private readonly string _endString;

        public EndScreen(bool won)
        {

            if (won)
            {
                _endString = "You escaped in " + GameManager.Instance.CurrentDay + " days.";
            } else
            {
                _endString = "You died in " + GameManager.Instance.CurrentDay + " days.";
            }

            _message = new TextPopup(_endString, false);
        }

        public override void Update()
        {
            _message.Update();
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            _message.Paint(spriteBatch);
            spriteBatch.End();
        }

    }
}
