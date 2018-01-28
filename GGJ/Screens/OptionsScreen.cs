using System.Collections.Generic;
using GGJ.Managers;
using GGJ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ.Screens {

    internal class OptionsScreen : Screen {

        private List<Button> _buttons = new List<Button>();

        private List<Slider> _sliders = new List<Slider>();

        public OptionsScreen(Game1 game) : base(game)
        {
            _buttons.Add(new Button("back", new Vector2(GameConstants.GameWidth / 2 - ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui].MeasureString("back").X + 10, GameConstants.GameHeight - 100),
                Color.Black * 0, Color.White, Button.ButtonTag.Finish));
            _buttons.Add(new Button("keybindings", new Vector2(GameConstants.GameWidth / 2 - ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui].MeasureString("keybindings").X / 2 - 10, 100),
                Color.Black * 0, Color.White, Button.ButtonTag.Start));
            _buttons.Add(new Button("fullscreen", new Vector2(GameConstants.GameWidth / 2 - ContentManager.Instance.Fonts[ContentManager.FontTypes.Ui].MeasureString("fullscreen").X / 2 - 10, GameConstants.GameHeight - 150),
                Color.Black * 0, Color.White, Button.ButtonTag.Toggle));

            _sliders.Add(new Slider(new Vector2(GameConstants.GameWidth / 2 - 50, 250), "Music Level", Slider.SliderType.Music));

            _sliders.Add(new Slider(new Vector2(GameConstants.GameWidth / 2 - 50, 350), "Sound Level", Slider.SliderType.Sound));

            _sliders.Add(new Slider(new Vector2(GameConstants.GameWidth / 2 - 50, 450), "Difficulty Level", Slider.SliderType.Difficulty));
        }

        public override void Update()
        {
            base.Update();

            var isHovering = false;

            foreach (var s in _sliders)
            {
                s.Update();

                
                if (GameManager.Instance.MouseRect.Intersects(s.Bounds))
                {
                    isHovering = true;
                }

                if (s.Active) break;
            }

            if (!isHovering)
            {
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

                    if (GameManager.Instance.MouseState.LeftButton == ButtonState.Pressed)
                    {
                        switch (b.Tag)
                        {
                            case Button.ButtonTag.Start:
                                ScreenManager.Instance.ChangeScreen(new KeyBindingScreen(Game));
                                break;
                            case Button.ButtonTag.Finish:
                                ScreenManager.Instance.ChangeScreen(new MenuScreen(Game));
                                break;
                            case Button.ButtonTag.Toggle:
                                var full = Game.Fullscreen();
                                if (full)
                                {
                                    b.SetText("Fullscreen");
                                }
                                else
                                {
                                    b.SetText("Windowed");
                                }

                                break;
                        }
                    }

                    break;
                }
            }

            if (!isHovering)
            {
                ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Pointer;
                return;
            }

            ContentManager.Instance.ActiveMouse = ContentManager.MouseType.Hand;
        }

        public override void Paint(SpriteBatch spriteBatch)
        {

            base.Paint(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);


            foreach (var b in _buttons)
            {
                b.Paint(spriteBatch);
            }

            foreach (var s in _sliders)
            {
                s.Paint(spriteBatch);
            }

            spriteBatch.Draw(ContentManager.Instance.Noise,
                new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White * 0.2f);
            spriteBatch.End();

        }
    }
}
