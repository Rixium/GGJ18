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

    internal class MenuScreen : Screen
    {

        private readonly string _gameTitle = "Stranger";

        private readonly int _titleWidth;
        private readonly int _titleHeight;
        private readonly int _titleX;
        private readonly int _titleY;

        private List<Button> _buttons = new List<Button>();

        public MenuScreen()
        {
            _titleWidth = (int)ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui].MeasureString(_gameTitle).X;
            _titleHeight = (int)ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui].MeasureString(_gameTitle).Y;
            _titleX = GameConstants.GameWidth - 100 - _titleWidth / 2;
            _titleY = 100;

            _buttons.Add(new Button("start", new Vector2(_titleX, _titleY + 100), Color.Black, Color.White, Button.ButtonTag.Start));
            _buttons.Add(new Button("options", new Vector2(_titleX, _titleY + 150), Color.Black, Color.White, Button.ButtonTag.Options));
        }

        public override void Update()
        {
            base.Update();
            ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Pointer;

            var isHovering = false;

            foreach (var b in _buttons)
            {
               
                b.Update();

                if (!b.Bounds.Intersects(GameManager.Instance.MouseRect))
                {
                    b.Hovering = false;
                    continue;
                }

                b.Hovering = true;
                isHovering = true;

                if (GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed && b.Tag == Button.ButtonTag.Start)
                {
                    ScreenManager.Instance.ChangeScreen(new GameScreen());
                }
                
                break;
            }

            if (!isHovering) return;
            ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Hand;
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            base.Paint(spriteBatch);

            spriteBatch.Begin();

            spriteBatch.Draw(ContentManager.Instance.Pixel, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.Black);
            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui], "Stranger", new Vector2(_titleX, _titleY), Color.White);

            foreach (var b in _buttons)
            {
                b.Paint(spriteBatch);
            }

            spriteBatch.End();
        }
    }

}
