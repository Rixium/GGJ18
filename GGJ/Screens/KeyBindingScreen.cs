using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Managers;
using GGJ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ.Screens {

    internal class KeyBindingScreen : Screen {

        private List<Binding> _bindings = new List<Binding>();
        private Button _backButton;
        private bool _hasActive;

        public KeyBindingScreen(Game1 game) : base(game)
        {
            var startVector2 = new Vector2(GameConstants.GameWidth / 2 - ContentManager.Instance.Fonts[ContentManager.FontTypes.TitleFont].MeasureString("Keybindings").X / 2 - 300, 200);

            _bindings.Add(new Binding(Binding.KeyType.Up, startVector2));
            _bindings.Add(new Binding(Binding.KeyType.Down, startVector2 + new Vector2(0, 80)));
            _bindings.Add(new Binding(Binding.KeyType.Left, startVector2 + new Vector2(0, 160)));
            _bindings.Add(new Binding(Binding.KeyType.Right, startVector2 + new Vector2(0, 240)));
            _bindings.Add(new Binding(Binding.KeyType.Use, startVector2 + new Vector2(600, 0)));
            _bindings.Add(new Binding(Binding.KeyType.Pause, startVector2 + new Vector2(600, 80)));

            _backButton = new Button("back", new Vector2(GameConstants.GameWidth / 2 - ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui].MeasureString("back").X + 10, GameConstants.GameHeight - 100),
                Color.Black * 0, Color.White, Button.ButtonTag.Finish);
        }

        public override void Update()
        {
            base.Update();

            ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Pointer;

            var isHovering = false;

            _backButton.Update();

            foreach (var b in _bindings)
            {
                b.Update();

                _hasActive = b.Active;

                if (!b.Bounds.Intersects(GameManager.Instance.MouseRect))
                {
                    b.Hovering = false;
                    continue;
                }

                b.Hovering = true;
                isHovering = true;

                if (GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed &&
                    GameManager.Instance.LastMouseState.LeftButton == ButtonState.Released)
                {
                    b.Active = true;
                }

                break;
            }

            if (!isHovering)
            {
                if (!_backButton.Bounds.Intersects(GameManager.Instance.MouseRect))
                {
                    _backButton.Hovering = false;
                }
                else
                {
                    _backButton.Hovering = true;

                    if (GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed)
                    {
                        ScreenManager.Instance.ChangeScreen(new OptionsScreen(Game));
                    }

                    ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Hand;
                }
            }
            else
            {
                ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Hand;
            }
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            base.Paint(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            SpriteFont f = ContentManager.Instance.Fonts[ContentManager.FontTypes.TitleFont];
            spriteBatch.DrawString(f, "Keybindings", new Vector2(GameConstants.GameWidth / 2 - f.MeasureString("Keybindings").X / 2, 100), Color.White);

            foreach (var b in  _bindings)
            {
                b.Paint(spriteBatch);
            }

            spriteBatch.Draw(ContentManager.Instance.Noise,
                new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White * 0.2f);

            _backButton.Paint(spriteBatch);

            spriteBatch.End();
        }
    }
}
