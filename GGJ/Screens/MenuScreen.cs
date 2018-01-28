using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GGJ.Games.Players;
using GGJ.Managers;
using GGJ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GGJ.Screens {

    internal class MenuScreen : Screen
    {

        private readonly string _gameTitle = "Strangers";

        private readonly int _titleWidth;
        private readonly int _titleHeight;
        private readonly int _titleX;
        private readonly int _titleY;

        private List<Button> _buttons = new List<Button>();

        private List<MessagePopup> _messagePopups = new List<MessagePopup>();

        public MenuScreen(Game1 game) : base(game)
        {
            _titleWidth = (int)ContentManager.Instance.Fonts[ContentManager.FontTypes.TitleFont].MeasureString(_gameTitle).X;
            _titleHeight = (int)ContentManager.Instance.Fonts[ContentManager.FontTypes.TitleFont].MeasureString(_gameTitle).Y;
            _titleX = 100;
            _titleY = GameConstants.GameHeight - 300;

            _buttons.Add(new Button("start", new Vector2(_titleX, _titleY + 80), Color.Black * 0, Color.White, Button.ButtonTag.Start));
            _buttons.Add(new Button("how to play", new Vector2(_titleX, _titleY + 120), Color.Black * 0, Color.White, Button.ButtonTag.HowToPlay));
            _buttons.Add(new Button("options", new Vector2(_titleX, _titleY + 160), Color.Black * 0, Color.White, Button.ButtonTag.Options));
            _buttons.Add(new Button("quit", new Vector2(_titleX, _titleY + 200), Color.Black * 0, Color.White, Button.ButtonTag.Finish));

            GameManager.Instance.Reset();
        }

        public static int CryptoRandom(int max)
        {
            var r = RandomNumberGenerator.Create();
            var b = new byte[4];
            r.GetBytes(b);

            return (int)Math.Round((double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue * (max - 1));

        }
        public override void Update()
        {
            base.Update();
            ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Pointer;


            var isHovering = false;

            if (CryptoRandom(100) < 5)
            {
                var text = ContentManager.Instance.talkLines[CryptoRandom(ContentManager.Instance.talkLines.Count)].Text;
                var textWidth = ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(text).X;
                var textHeight = ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(text).Y;

                var m = new MessagePopup(text, new Vector2(CryptoRandom((int)(GameConstants.GameWidth - textWidth)), CryptoRandom((int)(GameConstants.GameHeight - textHeight))));
                _messagePopups.Add(m);;
            }

            foreach (var m in new List<MessagePopup>(_messagePopups))
            {
                m.Update();

                if (m.Destroy)
                {
                    _messagePopups.Remove(m);
                }
            }

            foreach (var b in _buttons)
            {
               
                b.Update();

                if (!b.Bounds.Intersects(GameManager.Instance.MouseRect))
                {
                    b.Hovering = false;
                    continue;
                }

                if (!isHovering)
                {
                    b.Hovering = true;
                    isHovering = true;

                    if (GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed)
                    {
                        switch (b.Tag)
                        {
                            case Button.ButtonTag.Start:
                                ScreenManager.Instance.ChangeScreen(new CharacterSelectScreen(Game));
                                break;
                            case Button.ButtonTag.Options:
                                ScreenManager.Instance.ChangeScreen(new OptionsScreen(Game));
                                break;
                            case Button.ButtonTag.Finish:
                                Game.Exit();
                                break;
                            case Button.ButtonTag.HowToPlay:
                                ScreenManager.Instance.ChangeScreen(new HowToPlayScreen(Game));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                else
                {
                    b.Hovering = false;
                }
            }

            if (!isHovering) return;
            ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Hand;
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            base.Paint(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            spriteBatch.Draw(ContentManager.Instance.Pixel, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.Black);

            foreach (var m in new List<MessagePopup>(_messagePopups)) {
                m.Paint(spriteBatch);
            }

            spriteBatch.Draw(ContentManager.Instance.Pixel, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.Black * 0.7f);

            spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.TitleFont], _gameTitle, new Vector2(_titleX, _titleY), Color.White);

            foreach (var b in _buttons)
            {
                b.Paint(spriteBatch);
            }

            spriteBatch.Draw(ContentManager.Instance.Noise, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White * 0.2f);

            spriteBatch.End();
        }
    }

}
