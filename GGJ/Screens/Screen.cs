using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Screens {

    internal class Screen
    {

        public Game1 Game;

        public Screen(Game1 game)
        {
            Game = game;
        }

        public virtual void Update()
        {

        }

        public virtual void Paint(SpriteBatch spriteBatch)
        {

        }

    }
}
