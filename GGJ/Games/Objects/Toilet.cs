using GGJ.Constants;
using GGJ.Managers;
using Microsoft.Xna.Framework;

namespace GGJ.Games.Objects {

    internal class Toilet : GameObject {

        public Toilet(Vector2 position) : base(position, ContentManager.ObjectType.Toilet)
        {

        }

        public override string ToString() {
            return "Use [" + KeyBindings.Use + "]";
        }


        public override void Use()
        {
            if (!GameManager.Instance.GameScreen.StartUsing(this)) return;
            GameManager.Instance.TotalToiletUse++;
            ContentManager.Instance.Toilet.Play();
        }
    }

}
