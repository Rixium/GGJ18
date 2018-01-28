
using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ.UI {

    internal class Slider
    {

        public enum SliderType
        {
            Music,
            Sound,
            Difficulty
        }

        private Vector2 _sliderPos;
        private readonly Vector2 _barPos;

        private readonly string _label;
        private SliderType Type;

        public bool Active;

        public Slider(Vector2 barPos, string label, SliderType type)
        {
            _barPos = barPos;
            _label = label;
            Type = type;

            switch (Type)
            {
                case SliderType.Music:
                    _sliderPos = new Vector2(_barPos.X + GameConstants.MusicLevel * 100, _barPos.Y - 5);
                    break;
                case SliderType.Sound:
                    _sliderPos = new Vector2(_barPos.X + GameConstants.SoundLevel * 100, _barPos.Y - 5);
                    break;
                case SliderType.Difficulty:
                    _sliderPos = new Vector2(_barPos.X + GameConstants.Difficulty * 100, _barPos.Y - 5);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Rectangle Bounds => new Rectangle((int)_sliderPos.X, (int)_sliderPos.Y, 15, 10);

        public void Update()
        {

            if (Active && GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed)
            {
                _sliderPos.X = MathHelper.Clamp(GameManager.Instance.MouseRect.X, _barPos.X, _barPos.X + 100);

                switch (Type)
                {
                    case SliderType.Music:
                        GameConstants.MusicLevel = (_sliderPos.X - _barPos.X) / 100;
                        break;
                    case SliderType.Sound:
                        GameConstants.SoundLevel = (_sliderPos.X - _barPos.X) / 100;
                        break;
                    case SliderType.Difficulty:
                        GameConstants.Difficulty = MathHelper.Clamp(_sliderPos.X - _barPos.X, 40, 100) / 100;
                        Debug.WriteLine(GameConstants.Difficulty);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if (!GameManager.Instance.MouseRect.Intersects(Bounds))
                {
                    Active = false;
                    return;
                }

                if (GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed)
                {
                    Active = true;
                }
            }
        }

        public void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], _label, 
                new Vector2(_barPos.X + 50 - ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_label).X / 2, 
                    _barPos.Y - 5 - ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_label).Y), Color.White);

            spriteBatch.Draw(ContentManager.Instance.Pixel, new Rectangle((int)_barPos.X, (int)_barPos.Y, 100, 5), Color.White);
            spriteBatch.Draw(ContentManager.Instance.Pixel, new Rectangle((int)_sliderPos.X, (int)_sliderPos.Y, 15, 10), Color.White);
        }


    }
}
