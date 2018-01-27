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

        private readonly float _maxVelocity = 6;

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

        private const short _maxSanityTime = 80;
        private const short _maxThirstTime = 120;
        private const short _maxHungerTime = 150;
        private const short _maxBladderTime = 180;

        private bool wasRight = false;

        public Stats Stats = new Stats();

        public Player()
        {
            Position = new Vector2(820, 270);
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
                if (GameManager.Instance.GameScreen.CanMove(new Rectangle((int)(Bounds.X),
                    (int)(Bounds.Y + _currentYVelocity), Bounds.Width, Bounds.Height))) {
                    Position.Y = newPosition.Y;
                } else {
                    _currentYVelocity = 0;
                }

                if (GameManager.Instance.GameScreen.CanMove(new Rectangle((int)(Bounds.X + _currentXVelocity),
                    (int)(Bounds.Y), Bounds.Width, Bounds.Height))) {
                    Position.X = newPosition.X;
                } else {
                    _currentXVelocity = 0;

                }
            }

            _currentXVelocity *= _friction;
            _currentYVelocity *= _friction;

            if (Math.Abs(_currentXVelocity) < .3f)
            {
                _currentXVelocity = 0;
            }

            if (Math.Abs(_currentYVelocity) < 0.3f)
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

            CheckMovement();
            StatUpdate();
        }

        private void StatUpdate()
        {
            if (_sanityTick == 0)
            {
                
                _sanityTick = _maxSanityTime;
                Stats.AddSanity(-1);
            }
            else
            {
                _sanityTick--;
            }

            if (_hungerTick == 0)
            {
                _hungerTick = _maxHungerTime;
                Stats.AddHunger(1);
            }
            else
            {
                _hungerTick--;
            }

            if (_bladderTick == 0)
            {
                _bladderTick = _maxBladderTime;
                Stats.AddBladder(1);
            }
            else
            {
                _bladderTick--;
            }

            if (_thirstTick == 0)
            {
                _thirstTick = _maxThirstTime;
                Stats.AddThirst(1);
            }
            else
            {
                _thirstTick--;
            }

            if (Stats.Maxed())
            {
                Stats.AddHealth(-0.01f);

                if (Stats.Health <= 0)
                {
                    ScreenManager.Instance.ChangeScreen(new EndScreen(false));
                }
            }

        }

        public void NextFace()
        {
            if (_currentPlayerHead < ContentManager.Instance.PlayerHeads.Length - 1) {
                _currentPlayerHead++;
            }
        }
        private void CheckMovement()
        {
            if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.LEFT))
            {
                _currentXVelocity -= _velocityChange;
                wasRight = false;
            }
            else if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.RIGHT))
            {
                _currentXVelocity += _velocityChange;
                wasRight = true;
            }

            if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.UP))
            {
                _currentYVelocity -= _velocityChange;
            }
            else if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.DOWN))
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

            spriteBatch.Draw(ContentManager.Instance.PlayerHeads[_currentPlayerHead], Position,
                new Rectangle(0, 0, ContentManager.Instance.PlayerHeads[_currentPlayerHead].Width, ContentManager.Instance.PlayerHeads[_currentPlayerHead].Height),
                Color.White, 0, Vector2.Zero, 1, effect, 1);

            spriteBatch.Draw(ContentManager.Instance.PlayerLegAnimations[_currentAnimation][_currFrame], new Vector2(Position.X, Position.Y),
                new Rectangle(0, 0, ContentManager.Instance.PlayerLegAnimations[_currentAnimation][_currFrame].Width, ContentManager.Instance.PlayerLegAnimations[_currentAnimation][_currFrame].Height),
                Color.White, 0, Vector2.Zero, 1, effect, 1);

            spriteBatch.Draw(ContentManager.Instance.Shadow,
                new Rectangle(Bounds.X, Bounds.Y + Bounds.Height - 5, Bounds.Width, 10), Color.Black * 0.3f);
        }
    }
}