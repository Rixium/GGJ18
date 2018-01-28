using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Constants;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.UI {

    internal class Binding {

        public enum KeyType
        {
            Up,
            Down,
            Left,
            Right,
            Use,
            Pause
        }

        public bool Active;

        public KeyType Type;
        private readonly Vector2 _pos;

        private string _keyString;

        public bool Hovering;

        public Binding(KeyType type, Vector2 pos)
        {
            Type = type;
            _pos = pos;

            switch (Type) {
                case KeyType.Up:
                    _keyString = KeyBindings.Up.ToString();
                    break;
                case KeyType.Down:
                    _keyString = KeyBindings.Down.ToString();
                    break;
                case KeyType.Left:
                    _keyString = KeyBindings.Left.ToString();
                    break;
                case KeyType.Right:
                    _keyString = KeyBindings.Right.ToString();
                    break;
                case KeyType.Use:
                    _keyString = KeyBindings.Use.ToString();
                    break;
                case KeyType.Pause:
                    _keyString = KeyBindings.Pause.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Update()
        {
            if (!Active) return;
            if (GameManager.Instance.KeyState.GetPressedKeys().Length <= 0) return;

            switch (Type) {
                case KeyType.Up:
                    KeyBindings.Up = GameManager.Instance.KeyState.GetPressedKeys()[0];
                    _keyString = KeyBindings.Up.ToString();
                    break;
                case KeyType.Down:
                    KeyBindings.Down = GameManager.Instance.KeyState.GetPressedKeys()[0];
                    _keyString = KeyBindings.Down.ToString();
                    break;
                case KeyType.Left:
                    KeyBindings.Left = GameManager.Instance.KeyState.GetPressedKeys()[0];
                    _keyString = KeyBindings.Left.ToString();
                    break;
                case KeyType.Right:
                    KeyBindings.Right = GameManager.Instance.KeyState.GetPressedKeys()[0];
                    _keyString = KeyBindings.Right.ToString();
                    break;
                case KeyType.Use:
                    KeyBindings.Use = GameManager.Instance.KeyState.GetPressedKeys()[0];
                    _keyString = KeyBindings.Use.ToString();
                    break;
                case KeyType.Pause:
                    KeyBindings.Pause = GameManager.Instance.KeyState.GetPressedKeys()[0];
                    _keyString = KeyBindings.Pause.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Active = false;
        }

        public Rectangle Bounds => new Rectangle((int) _pos.X + 200, (int) _pos.Y - 5,
            (int) ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_keyString).X +
            10, 50);

        public void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], Type.ToString(), _pos, Color.White);

            if (Hovering)
            {
                spriteBatch.Draw(ContentManager.Instance.Pixel,
                    new Rectangle((int) _pos.X + 200, (int) _pos.Y - 5,
                        (int) ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_keyString).X +
                        10, 50), Color.White);
            }
            else
            {
                spriteBatch.Draw(ContentManager.Instance.Pixel,
                    new Rectangle((int)_pos.X + 200, (int)_pos.Y - 5,
                        (int)ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_keyString).X +
                        10, 50), Color.White * 0.9f);
            }

            if (Active)
            {
                spriteBatch.Draw(ContentManager.Instance.Pixel,
                    new Rectangle((int)_pos.X + 200, (int)_pos.Y - 5,
                        (int)ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(_keyString).X +
                        10, 50), Color.LightSeaGreen);
            }

            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], _keyString, _pos + new Vector2(205, 0), Color.Black);

        }
    }
}
