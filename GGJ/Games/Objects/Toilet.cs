using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Constants;
using GGJ.Managers;
using Microsoft.Xna.Framework;

namespace GGJ.Games.Objects {

    internal class Toilet : GameObject {

        public Toilet(Vector2 position) : base(position, ContentManager.ObjectType.Toilet)
        {

        }

        public override string ToString() {
            return "Use [" + KeyBindings.USE + "]";
        }


        public override void Use()
        {
            GameManager.Instance.GameScreen.StartUsing(this);
        }
    }

}
