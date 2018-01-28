using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.UI {

    internal class Button {

        public enum ButtonTag {
            Start,
            Options,
            Finish,
            HowToPlay
        }

        public ButtonTag Tag;

        private string _text;

        private Color _backgroundColor;
        private Color _fontColor;

        private readonly int _buttonPadding = 10;
        private readonly int _textWidth;
        private readonly int _textHeight;

        private bool _hovering;

        public Button(string text, Vector2 pos, Color backgroundColor, Color fontColor, ButtonTag tag) {
            _text = text;
            _backgroundColor = backgroundColor;
            _fontColor = fontColor;

            Tag = tag;

            _textWidth = (int)ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui].MeasureString(_text).X;
            _textHeight = (int)ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui].MeasureString(_text).Y;


            Bounds = new Rectangle((int)pos.X, (int)pos.Y, ((_buttonPadding * 2) + _textWidth), ((_buttonPadding * 2) + _textHeight));
        }

        public void Update() {

        }

        public Rectangle Bounds { get; }

        public bool Hovering {
            get => _hovering;
            set {
                if (!_hovering && value) {
                    ContentManager.Instance.MenuBlip.Play(GameConstants.SoundLevel, 0, 0);
                }

                _hovering = value;
            }
        }

        public void Paint(SpriteBatch spriteBatch) {
            spriteBatch.Draw(ContentManager.Instance.Pixel, Bounds, _backgroundColor);
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui], _text, new Vector2(Bounds.X + _buttonPadding, Bounds.Y + _buttonPadding), _fontColor);

            if (Hovering) {
                spriteBatch.Draw(ContentManager.Instance.Pixel, Bounds, Color.Black * 0.3f);
            }
        }
    }
}