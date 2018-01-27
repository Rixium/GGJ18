using System;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.UI {

    internal class StatusBar : UiComponent
    {

        private Vector2 _position;
        private string _statusText;

        private int _currValue;
        private int _maxValue;

        private const byte _padding = 10;
        private readonly byte _textHeight;

        public StatusType Type;

        public enum StatusType
        {
            Health,
            Sanity,
            Hunger,
            Thirst,
            Bladder
        }

        public StatusBar(Vector2 position, string statusText, StatusType type)
        {
            _position = position;
            _statusText = statusText;
            _textHeight = (byte)ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_statusText).Y;

            Type = type;
        }

        public override void Update()
        {
            switch (Type)
            {
                case StatusType.Health:
                    _currValue = (int)GameManager.Instance.GameScreen.Player.Stats.Health;
                    break;
                case StatusType.Sanity:
                    _currValue = GameManager.Instance.GameScreen.Player.Stats.Sanity;
                    break;
                case StatusType.Hunger:
                    _currValue = GameManager.Instance.GameScreen.Player.Stats.Hunger;
                    break;
                case StatusType.Thirst:
                    _currValue = GameManager.Instance.GameScreen.Player.Stats.Thirst;
                    break;
                case StatusType.Bladder:
                    _currValue = GameManager.Instance.GameScreen.Player.Stats.Bladder;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], _statusText, _position - new Vector2(0, _textHeight - _padding), Color.White);
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], _currValue.ToString(), _position, Color.White);
        }
    }

}
