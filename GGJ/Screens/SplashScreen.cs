using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Screens {

    internal class SplashScreen : Screen
    {

        private byte _splashTimer = 80;

        public SplashScreen(Game1 game) : base(game)
        {
        }

        public override void Update()
        {
            if (_splashTimer > 0)
            {
                _splashTimer--;
            }
            else
            {
                ScreenManager.Instance.ChangeScreen(new MenuScreen(Game));
            }
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(ContentManager.Instance.Splash, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(ContentManager.Instance.Noise, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White * 0.2f);
            spriteBatch.End();
        }

    }

}
