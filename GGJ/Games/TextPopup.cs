using GGJ.Managers;
using GGJ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Games {

    internal class TextPopup : UiComponent
    {

        private readonly string _text;

        private float _alpha;
        private const float _alphaChange = 0.01f;

        private bool _fadeIn = true;
        private bool _fadeOut;
        private bool _shouldFadeOut;

        private short _waitTimer = 0;
        private const short _maxWait = 100;

        public bool IsShowing;

        private readonly float _textWidth;
        private readonly float _textHeight;

        public TextPopup(string text, bool shouldFadeOut)
        {
            _text = text;
            _shouldFadeOut = shouldFadeOut;

            _textWidth = ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_text).X;
            _textHeight = ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_text).X;
        }

        public override void Update()
        {
            if (_fadeIn)
            {
                if (_alpha + _alphaChange <= 1)
                {
                    _alpha += _alphaChange;
                }
                else
                {
                    _fadeIn = false;
                    IsShowing = true;
                    _waitTimer = _maxWait;
                }
            } else if (_fadeOut && _shouldFadeOut)
            {
                if (_alpha - _alphaChange >= 0)
                {
                    _alpha -= _alphaChange;
                }
                else
                {
                    Destroy = true;
                    _fadeOut = false;
                }
            }

            if (_waitTimer <= 0) return;

            _waitTimer--;

            if (_waitTimer <= 0)
            {
                _fadeOut = true;
            }
        }

        public void StartFade()
        {
            _fadeIn = false;
            _fadeOut = true;
            _waitTimer = 0;
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], _text, new Vector2(GameConstants.GameWidth / 2 - _textWidth / 2, GameConstants.GameHeight / 2 - _textHeight / 2), Color.White * _alpha);
        }
    }

}
