using GGJ.Constants;
using GGJ.Managers;
using Microsoft.Xna.Framework;

namespace GGJ.Games.Objects {

    internal class Sink : GameObject {

        public Sink(Vector2 position) : base(position, ContentManager.ObjectType.Water)
        {

        }

        public override string ToString()
        {
            return "Drink [" + KeyBindings.USE + "]";
        }

        public override void Use()
        {
            GameManager.Instance.GameScreen.StartUsing(this);
        }
    }

}
