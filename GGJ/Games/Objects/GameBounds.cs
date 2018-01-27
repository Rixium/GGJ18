using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Games.Objects {

    internal class GameBounds : GameObject {

        private readonly Rectangle _rect;

        public GameBounds(Rectangle rect) : base(new Vector2(rect.X, rect.Y), ContentManager.ObjectType.GameBounds)
        {
            _rect = rect;
        }

        public override Rectangle Bounds => _rect;

        public override void Paint(SpriteBatch spriteBatch)
        {

            if (GameManager.Instance.Debugging)
            {
                spriteBatch.Draw(ContentManager.Instance.Pixel, _rect, Color.Red * 0.5f);
            }
        }

        public override void Update()
        {
        }
    }
}
