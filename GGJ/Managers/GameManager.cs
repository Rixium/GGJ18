using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Games.Objects;
using GGJ.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GGJ.Managers {

    internal class GameManager
    {

        private static GameManager _instance;

        public static GameManager Instance => _instance ?? (_instance = new GameManager());

        public Rectangle MouseRect = new Rectangle(- 50, -50, 1, 1);
        public MouseState MouseState;
        public MouseState LastMouseState;

        public sbyte CurrentStoryLine = 0;

        public GameScreen GameScreen;
        public GameObject ActiveObject;

        public GameObject UsingObject;

        private float _alpha;
        private readonly float _alphaChange = 0.01f;

        public KeyboardState KeyState;
        public KeyboardState LastKeyState;

        public int TotalRadioUse = 0;
        public int TotalToiletUse = 0;
        public int FoodEaten = 0;
        public int DrankTimes = 0;
        public int TimesSlept = 0;

        public int CurrentDay = 0;

        public void Reset()
        {
            _instance = new GameManager();
        }

        public void Update()
        {
            MouseState = Mouse.GetState();
            KeyState = Keyboard.GetState();
            MouseRect.X = MouseState.X;
            MouseRect.Y = MouseState.Y;

            if (ScreenManager.Instance.CurrentScreen != null)
            {
                var t = ScreenManager.Instance.CurrentScreen.GetType();
                if (t != typeof(GameScreen) && ScreenManager.Instance.NextScreen.GetType() != typeof(GameScreen)
                    && t != typeof(EndScreen) && ScreenManager.Instance.NextScreen.GetType() != typeof(EndScreen))
                {
                    if (MediaPlayer.Volume - 0.01f >= 0)
                    {
                        MediaPlayer.Volume -= 0.01f;
                    }
                    else
                    {
                        MediaPlayer.Volume = 0;
                    }
                }

            }

            if (!ScreenManager.Instance.Changing)
            {
                ScreenManager.Instance.CurrentScreen.Update();
            }
            else
            {
                if (!ScreenManager.Instance.Changed)
                {
                    if (_alpha + _alphaChange >= 1)
                    {
                        ScreenManager.Instance.ActivateNextScreen();
                    }
                    else
                    {
                        _alpha += _alphaChange;
                    }
                }
                else
                {
                    if (_alpha - _alphaChange <= 0) {
                        ScreenManager.Instance.ScreenReady();
                    } else {
                        _alpha -= _alphaChange;
                    }
                }
            }

            LastMouseState = MouseState;
            LastKeyState = KeyState;
        }

        public void Paint(SpriteBatch spriteBatch)
        {

            if (ScreenManager.Instance.CurrentScreen != null)
            {
                ScreenManager.Instance.CurrentScreen.Paint(spriteBatch);
            }


            if (ScreenManager.Instance.Changing)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(ContentManager.Instance.Pixel, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.Black * _alpha);
                spriteBatch.End();
            }

            spriteBatch.Begin();
            spriteBatch.Draw(ContentManager.Instance.Mice[ContentManager.Instance.ActiveMouse], new Vector2(MouseRect.X, MouseRect.Y), Color.White);
            spriteBatch.End();
        }
    }
}
