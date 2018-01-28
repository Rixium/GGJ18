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
        }

        public override void Update()
        {
        }
    }
}
