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
            return "Drink [" + KeyBindings.Use + "]";
        }

        public override void Use()
        {
            if (GameManager.Instance.GameScreen.StartUsing(this))
            {

                GameManager.Instance.DrankTimes++;
                ContentManager.Instance.Drink.Play(GameConstants.SoundLevel, 0, 0);
            }
        }
    }

}
