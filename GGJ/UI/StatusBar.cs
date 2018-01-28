using System;
using System.Diagnostics;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.UI {

    internal class StatusBar : UiComponent
    {

        private Vector2 _position;
        private string _statusText;

        private int _lastValue;

        private int _currValue;

        private Color _currentColor = Color.White;
        private const byte _padding = 5;

        private readonly byte _textHeight;
        private short _valueWidth;

        private byte _showTimer = 0;
        private byte _maxShowTimer = 160;

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

            switch (type)
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
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            _lastValue = _currValue;
        }

        public override void Update()
        {

            if (_showTimer > 0)
            {
                _showTimer--;
            }

            var updated = false;

            switch (Type)
            {
                case StatusType.Health:
                    if (_currValue != (int) GameManager.Instance.GameScreen.Player.Stats.Health)
                    {
                        _lastValue = _currValue;
                        _currValue = (int) GameManager.Instance.GameScreen.Player.Stats.Health;
                        _currentColor = _currValue < 25 ? Color.Red : Color.White;
                        updated = true;
                    }
                    break;
                case StatusType.Sanity:
                    if (_currValue != GameManager.Instance.GameScreen.Player.Stats.Sanity)
                    {
                        _lastValue = _currValue;
                        _currValue = GameManager.Instance.GameScreen.Player.Stats.Sanity;
                        _currentColor = _currValue < 25 ? Color.Red : Color.White;
                        updated = true;
                    }
                    break;
                case StatusType.Hunger:
                    if (_currValue != GameManager.Instance.GameScreen.Player.Stats.Hunger)
                    {
                        _lastValue = _currValue;
                        _currValue = GameManager.Instance.GameScreen.Player.Stats.Hunger;
                        _currentColor = _currValue > 80 ? Color.Red : Color.White;
                        updated = true;
                    }

                    break;
                case StatusType.Thirst:
                    if (_currValue != GameManager.Instance.GameScreen.Player.Stats.Thirst)
                    {
                        _lastValue = _currValue;
                        _currValue = GameManager.Instance.GameScreen.Player.Stats.Thirst;
                        _currentColor = _currValue > 80 ? Color.Red : Color.White;
                        updated = true;
                    }

                    break;
                case StatusType.Bladder:
                    if (_currValue != GameManager.Instance.GameScreen.Player.Stats.Bladder)
                    {
                        _lastValue = _currValue;
                        _currValue = GameManager.Instance.GameScreen.Player.Stats.Bladder;
                        _currentColor = _currValue > 80 ? Color.Red : Color.White;
                        updated = true;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (updated)
            {
                _showTimer = _maxShowTimer;
            }

        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], _statusText, _position - new Vector2(0, _textHeight - _padding), Color.White);
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], _currValue.ToString(), _position, _currentColor);

            if (_currValue == _lastValue || _showTimer <= 0) return;

            _valueWidth = (short)ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_currValue.ToString()).X;

            if (_lastValue < _currValue)
            {
                spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], "+" + (_currValue - _lastValue), _position + new Vector2(_valueWidth + _padding, 0), Color.White);
            }
            else
            {
                spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], "-" + Math.Abs(_currValue - _lastValue), _position + new Vector2(_valueWidth + _padding, 0), Color.White);
            }
            
        }
    }

}
