using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GGJ.Constants;
using GGJ.Managers;
using GGJ.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Games.Players
{
    internal class Player
    {
        private ContentManager.PlayerAnimation _currentAnimation = ContentManager.PlayerAnimation.Idle;

        private readonly float _maxVelocity = 8;

        private readonly float _velocityChange = 0.5f;

        private byte _currentPlayerHead = 0;

        private float _currentXVelocity;
        private float _currentYVelocity;

        private readonly float _friction = 0.85f;

        private byte _currFrame;
        private byte _maxFrameTime = 3;
        private byte _frameTime;

        private short _sanityTick;
        private short _thirstTick;
        private short _hungerTick;
        private short _bladderTick;

        private readonly short _maxSanityTime = (short)(80 / GameConstants.Difficulty);
        private readonly short _maxThirstTime = (short)(120 / GameConstants.Difficulty);
        private readonly short _maxHungerTime = (short)(75 / GameConstants.Difficulty);
        private readonly short _maxBladderTime = (short)(180 / GameConstants.Difficulty);

        private bool wasRight = false;

        public Stats Stats = new Stats();

        public enum Gender
        {
            Male,
            Female
        }

        private Gender _gender;

        public Player(Gender gender)
        {
            _gender = gender;
            Position = new Vector2(916, 285);
        }

        public Rectangle Bounds => new Rectangle((int) Position.X,
            (int) Position.Y + ContentManager.Instance.PlayerBodyAnimations[ContentManager.PlayerAnimation.Idle][0].Height -
            20, ContentManager.Instance.PlayerBodyAnimations[ContentManager.PlayerAnimation.Idle][0].Width, 20);

        public Vector2 Position;

        public void Update()
        {

            if (_frameTime >= _maxFrameTime)
            {
                if (_currFrame < ContentManager.Instance.PlayerBodyAnimations[_currentAnimation].Length - 1)
                {
                    _currFrame++;
                }
                else
                {
                    _currFrame = 0;
                }

                _frameTime = 0;
            }
            else
            {
                _frameTime++;
            }

            var newPosition = Position;

            newPosition.X += _currentXVelocity;
            newPosition.Y += _currentYVelocity;

            if (newPosition.X != Position.X || newPosition.Y != Position.Y) {
                if (newPosition.Y != Position.Y)
                {

                    Rectangle collideRect = Bounds;

                    if (newPosition.Y < Position.Y)
                    {
                        collideRect = new Rectangle((Bounds.X),
                            (int)Math.Floor(Bounds.Y + _currentYVelocity), Bounds.Width, Bounds.Height);
                    }
                    else
                    {
                        collideRect = new Rectangle((Bounds.X),
                            (int)Math.Ceiling(Bounds.Y + _currentYVelocity), Bounds.Width, Bounds.Height);
                    }

                    if (GameManager.Instance.GameScreen.CanMove(collideRect))
                    {
                        Position.Y = newPosition.Y;
                    }
                    else
                    {
                        _currentYVelocity = 0;
                    }
                }

                if (newPosition.X != Position.X)
                {
                    Rectangle collideRect = Bounds;

                    if (newPosition.X < Position.X)
                    {
                        collideRect = new Rectangle((int) Math.Floor(Bounds.X + _currentXVelocity),
                            (int) (Bounds.Y), Bounds.Width, Bounds.Height);
                    } else
                    {
                        collideRect = new Rectangle((int) Math.Ceiling(Bounds.X + _currentXVelocity),
                            (int) (Bounds.Y), Bounds.Width, Bounds.Height);
                    }

                    if (GameManager.Instance.GameScreen.CanMove(collideRect))
                    {
                        Position.X = newPosition.X;
                    }
                    else
                    {
                        _currentXVelocity = 0;

                    }
                }
            }

            _currentXVelocity *= _friction;
            _currentYVelocity *= _friction;

            if (Math.Abs(_currentXVelocity) < .3f || GameManager.Instance.UsingObject != null)
            {
                _currentXVelocity = 0;
            }

            if (Math.Abs(_currentYVelocity) < 0.3f || GameManager.Instance.UsingObject != null)
            {
                _currentYVelocity = 0;
            }

            if (_currentXVelocity != 0 || _currentYVelocity != 0)
            {
                if (_currentAnimation != ContentManager.PlayerAnimation.Walk)
                {
                    _currentAnimation = ContentManager.PlayerAnimation.Walk;
                    _currFrame = 0;
                    _frameTime = 0;
                }
            } else {
                if (_currentAnimation != ContentManager.PlayerAnimation.Idle)
                {
                    _currFrame = 0;
                    _frameTime = 0;
                    _currentAnimation = ContentManager.PlayerAnimation.Idle;
                }
            }

            if (GameManager.Instance.UsingObject == null)
            {
                CheckMovement();
            }

            StatUpdate();
        }

        public void Stop()
        {
            _currFrame = 0;
            _currentAnimation = ContentManager.PlayerAnimation.Idle;
            _frameTime = 0;
            _currentYVelocity = 0;
            _currentXVelocity = 0;
        }

        private void StatUpdate()
        {
            
            if (_sanityTick == 0)
            {   
                _sanityTick = _maxSanityTime;
                Stats.AddSanity((sbyte)-MathHelper.Clamp((GameManager.Instance.CurrentDay / 5 + 1), 1, 2));
            }
            else
            {
                _sanityTick--;
            }

            if (_hungerTick == 0)
            {
                if (GameManager.Instance.UsingObject != null)
                {
                    if (GameManager.Instance.UsingObject.ObjectType != ContentManager.ObjectType.Food)
                    {
                        _hungerTick = _maxHungerTime;
                        Stats.AddHunger(2);
                    }
                }
                else
                {
                    _hungerTick = _maxHungerTime;
                    Stats.AddHunger(2);
                }
            }
            else
            {
                _hungerTick--;
            }

            if (_bladderTick == 0)
            {
                if (GameManager.Instance.UsingObject != null) {
                    if (GameManager.Instance.UsingObject.ObjectType != ContentManager.ObjectType.Toilet)
                    {
                        _bladderTick = _maxBladderTime;
                        Stats.AddBladder(1);
                    }
                }
                else
                {
                    _bladderTick = _maxBladderTime;
                    Stats.AddBladder(1);
                }

            }
            else
            {
                _bladderTick--;
            }

            if (_thirstTick == 0)
            {
                if (GameManager.Instance.UsingObject != null)
                {
                    if (GameManager.Instance.UsingObject.ObjectType != ContentManager.ObjectType.Water)
                    {
                        _thirstTick = _maxThirstTime;
                        Stats.AddThirst(1);
                    }
                }
                else
                {
                    _thirstTick = _maxThirstTime;
                    Stats.AddThirst(1);
                }
            }
            else
            {
                _thirstTick--;
            }

            var numMaxed = Stats.Maxed();

            if (numMaxed == 0) return;

            Stats.AddHealth(-0.01f * numMaxed);

            if (Stats.Health <= 0)
            {
                ScreenManager.Instance.ChangeScreen(new EndScreen(ScreenManager.Instance.CurrentScreen.Game, false));
            }

        }

        public void NextFace()
        {
            if (_currentPlayerHead < ContentManager.Instance.PlayerHeads[_gender].Length - 1) {
                _currentPlayerHead++;
            }
        }
        private void CheckMovement()
        {
            if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.Left))
            {
                _currentXVelocity -= _velocityChange;
                wasRight = false;
            }
            else if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.Right))
            {
                _currentXVelocity += _velocityChange;
                wasRight = true;
            }

            if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.Up))
            {
                _currentYVelocity -= _velocityChange;
            }
            else if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.Down))
            {
                _currentYVelocity += _velocityChange;
            }

            _currentXVelocity = MathHelper.Clamp(_currentXVelocity, -_maxVelocity, _maxVelocity);
            _currentYVelocity = MathHelper.Clamp(_currentYVelocity, -_maxVelocity, _maxVelocity);
        }

        public void Paint(SpriteBatch spriteBatch)
        {
            SpriteEffects effect = SpriteEffects.None;

            if (_currentXVelocity > 0 || wasRight)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            spriteBatch.Draw(ContentManager.Instance.PlayerBodyAnimations[_currentAnimation][_currFrame], new Vector2(Position.X, Position.Y),
                new Rectangle(0, 0, ContentManager.Instance.PlayerBodyAnimations[_currentAnimation][_currFrame].Width, ContentManager.Instance.PlayerBodyAnimations[_currentAnimation][_currFrame].Height),
                Color.White, 0, Vector2.Zero, 1, effect, 1);


            spriteBatch.Draw(ContentManager.Instance.PlayerHeads[_gender][_currentPlayerHead], Position,
                new Rectangle(0, 0, ContentManager.Instance.PlayerHeads[_gender][_currentPlayerHead].Width, ContentManager.Instance.PlayerHeads[_gender][_currentPlayerHead].Height),
                Color.White, 0, Vector2.Zero, 1, effect, 1);

            spriteBatch.Draw(ContentManager.Instance.PlayerLegAnimations[_currentAnimation][_currFrame], new Vector2(Position.X, Position.Y),
                new Rectangle(0, 0, ContentManager.Instance.PlayerLegAnimations[_currentAnimation][_currFrame].Width, ContentManager.Instance.PlayerLegAnimations[_currentAnimation][_currFrame].Height),
                Color.White, 0, Vector2.Zero, 1, effect, 1);

            spriteBatch.Draw(ContentManager.Instance.Shadow,
                new Rectangle(Bounds.X, Bounds.Y + Bounds.Height - 5, Bounds.Width, 10), Color.Black * 0.3f);


            if (GameManager.Instance.UsingObject == null) return;
            if (GameManager.Instance.UsingObject.ObjectType == ContentManager.ObjectType.Radio) return;

            var statusRect = new Rectangle((int)Position.X + Bounds.Width / 2 - 50, (int)Position.Y - 20, 100, 15);
            sbyte statusValue = 0;

            spriteBatch.Draw(ContentManager.Instance.Pixel, statusRect, Color.Black * 0.5f);
            switch (GameManager.Instance.UsingObject.ObjectType)
            {
                case ContentManager.ObjectType.Bed:
                    break;
                case ContentManager.ObjectType.Radio:
                    break;
                case ContentManager.ObjectType.Toilet:
                    statusValue = Stats.Bladder;
                    break;
                case ContentManager.ObjectType.Water:
                    statusValue = Stats.Thirst;
                    break;
                case ContentManager.ObjectType.Food:
                    statusValue = Stats.Hunger;
                    break;
                case ContentManager.ObjectType.GameBounds:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var valueRect = new Rectangle(statusRect.X, statusRect.Y, (int)statusValue, 15);

            spriteBatch.Draw(ContentManager.Instance.Pixel, valueRect, Color.White * 0.5f);
        }
    }
}