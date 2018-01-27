using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.UI {

    internal class StatusBar : UiComponent
    {

        private Vector2 _position;
        private string _statusText;
        private const byte _padding = 10;
        private readonly byte _textHeight;

        public StatusBar(Vector2 position, string statusText)
        {
            _position = position;
            _statusText = statusText;
            _textHeight = (byte)ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_statusText).Y;
        }

        public override void Update()
        {
            
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], _statusText, _position - new Vector2(0, _padding + _textHeight), Color.White);
        }
    }

}
