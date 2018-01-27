using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Constants;
using GGJ.Managers;
using Microsoft.Xna.Framework;

namespace GGJ.Games.Objects {

    internal class Radio : GameObject {

        public Radio(Vector2 position) : base(position, ContentManager.ObjectType.Radio)
        {
        }

        public override string ToString()
        {
            return "Attempt Contact [" + KeyBindings.USE + "]";
        }

        public override void Use()
        {

        }
    }

}
