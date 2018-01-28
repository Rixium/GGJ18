using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Games.Players;
using GGJ.Managers;
using GGJ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ.Screens {

    internal class CharacterSelectScreen : Screen
    {

        private PictureButton _maleButton;
        private PictureButton _femaleButton;

        public CharacterSelectScreen(Game1 game) : base(game)
        {
            _maleButton = new PictureButton(ContentManager.Instance.maleButton, new Vector2(GameConstants.GameWidth / 2 - ContentManager.Instance.maleButton.Width / 2 - ContentManager.Instance.maleButton.Width / 2 - 10, GameConstants.GameHeight / 2 - ContentManager.Instance.maleButton.Height / 2));
            _femaleButton = new PictureButton(ContentManager.Instance.femaleButton, new Vector2(GameConstants.GameWidth / 2 - ContentManager.Instance.femaleButton.Width / 2 + ContentManager.Instance.femaleButton.Width / 2 + 10, GameConstants.GameHeight / 2 - ContentManager.Instance.maleButton.Height / 2));
        }

        public override void Update()
        {
            if (GameManager.Instance.MouseRect.Intersects(_maleButton.Bounds))
            {
                _maleButton.Hover(true);
                _femaleButton.Hover(false);

                if (GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed) {
                    ScreenManager.Instance.ChangeScreen(new GameScreen(Game, Player.Gender.Male));
                }

                ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Hand;
            } else if (GameManager.Instance.MouseRect.Intersects(_femaleButton.Bounds))
            {
                _femaleButton.Hover(true);
                _maleButton.Hover(false);

                if (GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed)
                {
                    ScreenManager.Instance.ChangeScreen(new GameScreen(Game, Player.Gender.Female));
                }
                ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Hand;
            }
            else
            {
                ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Pointer;
            }

            base.Update();
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            base.Paint(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
                null);

            _maleButton.Paint(spriteBatch);
            _femaleButton.Paint(spriteBatch);
            spriteBatch.Draw(ContentManager.Instance.Noise,
                new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White * 0.2f);

            spriteBatch.End();
        }
    }
}
