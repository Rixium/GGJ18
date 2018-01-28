using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Constants;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Games.Objects {

    internal class Food : GameObject {

        private int _foodCount = 16;
        private int _activeFoodImage = 0;

        public Food(Vector2 position) : base(position, ContentManager.ObjectType.Food)
        {

        }

        public override Rectangle Bounds => new Rectangle((int)_position.X, (int)_position.Y,
            ContentManager.Instance.Food[_activeFoodImage].Width, ContentManager.Instance.Food[_activeFoodImage].Height);


        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentManager.Instance.Food[_activeFoodImage], _position, Color.White);
        }

        public override string ToString() {
            return "Eat [" + KeyBindings.Use + "]";
        }


        public override void Use()
        {
            if (!GameManager.Instance.GameScreen.StartUsing(this)) return;

            ContentManager.Instance.Eat.Play(GameConstants.SoundLevel, 0, 0);

            _foodCount--;

            GameManager.Instance.FoodEaten++;

            if (_activeFoodImage < ContentManager.Instance.Food.Length - 1)
            {
                _activeFoodImage++;
            }

            if (_foodCount == 0)
            {
                Destroy = true;
            }
        }
    }
}
