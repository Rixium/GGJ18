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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GGJ.Screens {

    internal class EndScreen : Screen
    {

        private readonly TextPopup _message;
        private readonly List<FastMessage> _popups = new List<FastMessage>();

        private int _endTimer = 300;

        public EndScreen(Game1 game, bool won) : base(game)
        {
            string endString;

            if (won)
            {
                endString = "You survived the " + GameManager.Instance.CurrentDay + " day quarantine.";
            } else
            {
                if (GameManager.Instance.CurrentDay == 1)
                {
                    endString = "You died in " + GameManager.Instance.CurrentDay + " day.";
                }
                else
                {
                    endString = "You died in " + GameManager.Instance.CurrentDay + " days.";
                }
            }

            _popups.Add((new FastMessage("Stats",
                new Vector2(GameConstants.GameWidth / 2, GameConstants.GameHeight / 2 + 60))));
            _popups.Add((new FastMessage("Times using radio: " + GameManager.Instance.TotalRadioUse,
                new Vector2(GameConstants.GameWidth / 2, GameConstants.GameHeight / 2 + 100))));
            _popups.Add((new FastMessage("Number of food eaten: " + GameManager.Instance.FoodEaten,
                new Vector2(GameConstants.GameWidth / 2, GameConstants.GameHeight / 2 + 120))));
            _popups.Add((new FastMessage("Quenched thirst: " + GameManager.Instance.DrankTimes,
                new Vector2(GameConstants.GameWidth / 2, GameConstants.GameHeight / 2 + 140))));
            _popups.Add((new FastMessage("Hit the sack: " + GameManager.Instance.TimesSlept,
                new Vector2(GameConstants.GameWidth / 2, GameConstants.GameHeight / 2 + 160))));
            _popups.Add((new FastMessage("Emptied the tank: " + GameManager.Instance.TotalToiletUse,
                new Vector2(GameConstants.GameWidth / 2, GameConstants.GameHeight / 2 + 180))));

            _message = new TextPopup(endString, false);
        }

        public override void Update()
        {
            _message.Update();

            foreach (var p in _popups)
            {
                p.Update();
            }

            if (MediaPlayer.Volume - 0.0015f >= 0)
            {
                MediaPlayer.Volume -= 0.0015f;
            }
            else
            {
                MediaPlayer.Volume = 0;
            }

            if (!_message.IsShowing) return;
            if (_endTimer > 0)
            {
                _endTimer--;
            }
            else
            {
                ScreenManager.Instance.ChangeScreen(new MenuScreen(Game));
            }
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            _message.Paint(spriteBatch);

            foreach (var p in _popups)
            {
                p.Paint(spriteBatch);
            }

            spriteBatch.End();
        }

    }
}
