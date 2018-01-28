using GGJ.Constants;
using GGJ.Managers;
using Microsoft.Xna.Framework;

namespace GGJ.Games.Objects {

    internal class Bed : GameObject {

        public Bed(Vector2 position) : base(position, ContentManager.ObjectType.Bed)
        {

        }

        public override string ToString()
        {
            return "Sleep [" + KeyBindings.Use + "]";
        }

        public override void Use()
        {
            GameManager.Instance.GameScreen.NextDay();
            ContentManager.Instance.Bed.Play(GameConstants.SoundLevel, 0, 0);
            GameManager.Instance.TimesSlept++;
        }

    }
}
