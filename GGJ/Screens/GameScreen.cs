using System;
using System.Collections.Generic;
using GGJ.Constants;
using GGJ.Games;
using GGJ.Games.Objects;
using GGJ.Games.Players;
using GGJ.Managers;
using GGJ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ.Screens {

    internal class GameScreen : Screen
    {

        public string RoomName = "The Bunker";

        private List<GameObject> gameObjects = new List<GameObject>();
        public Player Player;

        private bool _changingDay;
        private bool _fadeIn;
        private bool _fadeOut = true;
        private float _currentAlpha;
        private float _alphaChange = 0.01f;
        private byte _waitTimer;
        private readonly byte _maxWait = 100;

        private readonly int _maxInteractDistance = 140;

        private int _useTimer = 0;
        private const int _maxUseTimer = 20;

        public List<UiComponent> UiComponents = new List<UiComponent>();

        public GameScreen()
        {
            GameManager.Instance.GameScreen = this;

            gameObjects.Add(new Bed(new Vector2(974, 396)));
            gameObjects.Add(new Toilet(new Vector2(125, 400)));
            gameObjects.Add(new Food(new Vector2(500, 371)));
            gameObjects.Add(new Radio(new Vector2(680, 396)));
            gameObjects.Add(new Sink(new Vector2(335, 387)));

            gameObjects.Add(new GameBounds(new Rectangle(0, 0, GameConstants.GameWidth, 490)));
            gameObjects.Add(new GameBounds(new Rectangle(0, 0, 129, GameConstants.GameHeight)));
            gameObjects.Add(new GameBounds(new Rectangle(0, 650, GameConstants.GameWidth, 156)));
            gameObjects.Add(new GameBounds(new Rectangle(1150, 0, 305, GameConstants.GameHeight)));
            gameObjects.Add(new GameBounds(new Rectangle(436, 0, 50, 530)));
            gameObjects.Add(new GameBounds(new Rectangle(436, 600, 50, 530)));

            Player = new Player();

            UiComponents.Add(new TextPopup(RoomName, true));

            UiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 300), "Health", StatusBar.StatusType.Health));
            UiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 250), "Sanity", StatusBar.StatusType.Sanity));
            UiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 200), "Hunger", StatusBar.StatusType.Hunger));
            UiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 150), "Thirst", StatusBar.StatusType.Thirst));
            UiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 100), "Bladder", StatusBar.StatusType.Bladder));

            UiComponents.Add(new TextPopup("Day " + ++GameManager.Instance.CurrentDay, true));
        }

        public void NextDay()
        {
            _changingDay = true;
            _fadeOut = true;

            foreach (var c in new List<UiComponent>(UiComponents))
            {
                if (c.GetType() != typeof(TextPopup)) continue;

                var tp = (TextPopup) c;
                tp.StartFade();
            }
        }

        public bool CanMove(Rectangle rect)
        {
            foreach (var o in new List<GameObject>(gameObjects))
            {
                if (rect.Intersects(o.Bounds))
                {
                    return false;
                }
            }

            return true;
        }

        public override void Update()
        {
            if (!_changingDay)
            {
                base.Update();

                var interacting = false;

                foreach (var o in new List<GameObject>(gameObjects))
                {
                    if (o.Destroy)
                    {
                        gameObjects.Remove(o);
                        continue;
                    }

                    o.Update();
                    var distance = Vector2.Distance(
                        new Vector2(Player.Bounds.X + Player.Bounds.Width / 2,
                            Player.Bounds.Y + Player.Bounds.Height / 2),
                        new Vector2(o.Bounds.X + o.Bounds.Width / 2, o.Bounds.Y + o.Bounds.Height / 2));

                    if (!(distance < _maxInteractDistance) || interacting || o.ObjectType == ContentManager.ObjectType.GameBounds) continue;

                    interacting = true;
                    GameManager.Instance.ActiveObject = o;

                    if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.USE) &&
                        GameManager.Instance.LastKeyState.IsKeyUp(KeyBindings.USE))
                    {
                        o.Use();
                    }
                }

                if (!interacting)
                {
                    GameManager.Instance.ActiveObject = null;
                }

                

                Player.Update();

                if (_useTimer <= 0)
                {
                    CheckUse();
                    _useTimer = _maxUseTimer;
                }
                else
                {
                    _useTimer--;
                }

                CheckKeys();
            }
            else
            {
                if (_waitTimer <= 0)
                {
                    if (_fadeOut)
                    {
                        if (_currentAlpha + _alphaChange <= 1)
                        {
                            _currentAlpha += _alphaChange;
                        }
                        else
                        {
                            Player.Stop();

                            if (GameManager.Instance.CurrentDay % 3 == 0) {
                                Player.NextFace();
                            }

                            if (Player.Stats.Maxed())
                            {
                                Player.Stats.AddHealth(GameConstants.EndDayMaxedHealthChange);
                            }

                            Player.Stats.AddSanity(GameConstants.SleepSanity);
                            Player.Stats.AddThirst(GameConstants.SleepThirst);
                            Player.Stats.AddBladder(GameConstants.SleepBladder);
                            Player.Stats.AddHunger(GameConstants.SleepHunger);

                            _fadeIn = true;
                            _fadeOut = false;
                            _waitTimer = _maxWait;
                        }
                    }
                    else if (_fadeIn)
                    {
                        if (_currentAlpha - _alphaChange >= 0)
                        {
                            _currentAlpha -= _alphaChange;
                        }
                        else
                        {
                            _changingDay = false;
                            UiComponents.Add(new TextPopup("Day " + ++GameManager.Instance.CurrentDay, true));
                        }
                    }
                }
                else
                {
                    _waitTimer--;
                }
            }

            foreach (var c in new List<UiComponent>(UiComponents)) {
                c.Update();

                if (c.Destroy) {
                    UiComponents.Remove(c);
                }
            }
        }

        private void CheckUse()
        {
            if (GameManager.Instance.UsingObject == null) return;

            switch (GameManager.Instance.UsingObject.ObjectType)
            {
                case ContentManager.ObjectType.Bed:
                    break;
                case ContentManager.ObjectType.Radio:
                    break;
                case ContentManager.ObjectType.Toilet:
                    if (Player.Stats.Bladder > 0)
                    {
                        Player.Stats.AddBladder(GameConstants.UrinationSpeed);
                        Player.Stats.AddThirst(GameConstants.UrinationThirstSpeed);
                    }
                    else
                    {
                        GameManager.Instance.UsingObject = null;
                    }
                    break;
                case ContentManager.ObjectType.GameBounds:
                    break;
                case ContentManager.ObjectType.Food:
                    if (Player.Stats.Hunger > 0) {
                        Player.Stats.AddHunger(GameConstants.EatSpeed);
                        Player.Stats.AddThirst(GameConstants.EatThirstSpeed);
                    } else {
                        GameManager.Instance.UsingObject = null;
                    }
                    break;
                case ContentManager.ObjectType.Water:
                    if (Player.Stats.Thirst > 0) {
                        Player.Stats.AddThirst(GameConstants.DrinkSpeed);
                        Player.Stats.AddBladder(GameConstants.DrinkBladder);
                    } else {
                        GameManager.Instance.UsingObject = null;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckKeys()
        {
            if (GameManager.Instance.KeyState.IsKeyDown(Keys.F1) && GameManager.Instance.LastKeyState.IsKeyUp(Keys.F1))
            {
                GameManager.Instance.Debugging = !GameManager.Instance.Debugging;
            }
        }

        public bool StartUsing(GameObject obj)
        {
            if (GameManager.Instance.UsingObject != null) return false;

            GameManager.Instance.UsingObject = obj;
            return true;
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            base.Paint(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(ContentManager.Instance.Room, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White);

            foreach (var o in new List<GameObject>(gameObjects))
            {
                o.Paint(spriteBatch);
            }

            Player.Paint(spriteBatch);

            spriteBatch.Draw(ContentManager.Instance.RoomFront, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White);

            if (GameManager.Instance.ActiveObject != null)
            {
                var o = GameManager.Instance.ActiveObject;
                var stringWidth = ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(o.ToString()).X;
                var stringHeight = ContentManager.Instance.Fonts[ContentManager.FontTypes.Game]
                    .MeasureString(o.ToString()).Y;
                
                var padding = 5;
                var rect = new Rectangle((int) (o.Position.X + o.Bounds.Width / 2 - stringWidth / 2),
                    (int) (o.Position.Y - stringHeight), (int) (stringWidth + padding * 2),
                    (int) (stringHeight + padding * 2));

                spriteBatch.Draw(ContentManager.Instance.Pixel,
                    rect,
                    Color.Black * 0.5f);
                spriteBatch.DrawString(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game], o.ToString(), new Vector2(rect.X + padding, rect.Y + padding), Color.White);
            }

            foreach (var c in new List<UiComponent>(UiComponents))
            {
                c.Paint(spriteBatch);
            }

            if (_changingDay)
            {
                spriteBatch.Draw(ContentManager.Instance.Pixel, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.Black * _currentAlpha);
            }
            
            spriteBatch.Draw(ContentManager.Instance.Noise, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White * 0.2f);
            spriteBatch.End();
        }
    }
}
