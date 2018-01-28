using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using GGJ.Managers;
using GGJ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ.Screens {

    internal class HowToPlayScreen : Screen
    {

        private Button _backButton;

        public HowToPlayScreen(Game1 game) : base(game)
        {
            _backButton = new Button("back", new Vector2(GameConstants.GameWidth / 2 - ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui].MeasureString("back").X + 10, GameConstants.GameHeight - 100),
                Color.Black * 0, Color.White, Button.ButtonTag.Finish);
        }

        public override void Update()
        {
            base.Update();

            ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Pointer;

            _backButton.Update();

            if (!_backButton.Bounds.Intersects(GameManager.Instance.MouseRect)) {
                _backButton.Hovering = false;
                return;
            }

            _backButton.Hovering = true;

            if (GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed) {
                ScreenManager.Instance.ChangeScreen(new MenuScreen(Game));
            }

            ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Hand;
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            base.Paint(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            _backButton.Paint(spriteBatch);

            SpriteFont f = ContentManager.Instance.Fonts[ContentManager.FontTypes.Game];
            SpriteFont f2 = ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui];

            spriteBatch.DrawString(f, "How to play", new Vector2(GameConstants.GameWidth / 2 - f.MeasureString("How to play").X / 2, 100), Color.White);
            spriteBatch.DrawString(f2, "Keep contact with the outside world, with the use of the radio.", new Vector2(GameConstants.GameWidth / 2 - f2.MeasureString("Keep contact with the outside world, with the use of the radio.").X / 2, 200), Color.White);
            spriteBatch.DrawString(f2, "Manage your stats, and don't let them get too high, or you'll lose health.", new Vector2(GameConstants.GameWidth / 2 - f2.MeasureString("Manage your stats, and don't let them get too high, or you'll lose health.").X / 2, 250), Color.White);
            spriteBatch.DrawString(f2, "Sanity can be regained using the bed.", new Vector2(GameConstants.GameWidth / 2 - f2.MeasureString("Sanity can be regained using the bed.").X / 2, 310), Color.White);
            spriteBatch.DrawString(f2, "Hunger can be lowered by eating food.", new Vector2(GameConstants.GameWidth / 2 - f2.MeasureString("Hunger can be lowered by eating food.").X / 2, 360), Color.White);
            spriteBatch.DrawString(f2, "Thirst can be lowered by drinking from the sink.", new Vector2(GameConstants.GameWidth / 2 - f2.MeasureString("Thirst can be lowered by drinking from the sink.").X / 2, 410), Color.White);
            spriteBatch.DrawString(f2, "Bladder can be lowered by using the toilet.", new Vector2(GameConstants.GameWidth / 2 - f2.MeasureString("Bladder can be lowered by using the toilet.").X / 2, 460), Color.White);
            spriteBatch.DrawString(f2, "Keep using the radio, it's the only way to know if you've survived.", new Vector2(GameConstants.GameWidth / 2 - f2.MeasureString("Keep using the radio, it's the only way to know if you've survived.").X / 2, 510), Color.White);
            spriteBatch.DrawString(f, "Good luck.", new Vector2(GameConstants.GameWidth / 2 - f.MeasureString("Good luck.").X / 2, 550), Color.White);
            spriteBatch.Draw(ContentManager.Instance.Noise, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White * 0.2f);
            spriteBatch.End();
        }
    }
}
