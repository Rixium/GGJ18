using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.UI {

    internal class PictureButton
    {
        private readonly Texture2D _image;
        private readonly Vector2 _pos;
        private bool _hovering;

        public void Hover(bool value)
        {
            if (value && !_hovering)
            {
                ContentManager.Instance.MenuBlip.Play(GameConstants.SoundLevel, 0, 0);
            }

            _hovering = value;
        }
        public PictureButton(Texture2D img, Vector2 pos)
        {
            _image = img;
            _pos = pos;
        }

        public Rectangle Bounds => new Rectangle((int)_pos.X, (int)_pos.Y, _image.Width, _image.Height);

        public void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_image, _pos, Color.White);

            if (!_hovering)
            {
                spriteBatch.Draw(_image, _pos, Color.Black * 0.3f);
            }
        }
    }
}
