using System;
using System.Collections.Generic;
using System.Linq;
using GGJ.Constants;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Games.Players
{
    internal class Player
    {
        private ContentManager.PlayerAnimation _currentAnimation = ContentManager.PlayerAnimation.Idle;

        private readonly float _maxVelocity = 6;

        private readonly float _velocityChange = 0.5f;

        private float _currentXVelocity = 0;
        private float _currentYVelocity = 0;

        private readonly float _friction = 0.8f;

        private int _currFrame = 0;

        public Player()
        {
            Position = new Vector2(780, 416);
        }

        public Rectangle Bounds => new Rectangle((int) Position.X,
            (int) Position.Y + ContentManager.Instance.PlayerAnimations[ContentManager.PlayerAnimation.Idle][0].Height -
            30, ContentManager.Instance.PlayerAnimations[ContentManager.PlayerAnimation.Idle][0].Width, 30);

        public Vector2 Position;

        public void Update()
        {
            
            if (_currFrame < ContentManager.Instance.PlayerAnimations[_currentAnimation].Length - 1)
            {
                _currFrame++;
            }
            else
            {
                _currFrame = 0;
            }

            var newPosition = Position;

            newPosition.X += _currentXVelocity;
            newPosition.Y += _currentYVelocity;

            if (newPosition.X != Position.X || newPosition.Y != Position.Y)
            {
                if (GameManager.Instance.GameScreen.CanMove(new Rectangle((int) (Bounds.X),
                    (int) (Bounds.Y + _currentYVelocity), Bounds.Width, Bounds.Height)))
                {
                    Position.Y = newPosition.Y;
                }
                else
                {
                    _currentYVelocity = 0;
                }

                if (GameManager.Instance.GameScreen.CanMove(new Rectangle((int) (Bounds.X + _currentXVelocity),
                    (int) (Bounds.Y), Bounds.Width, Bounds.Height)))
                {
                    Position.X = newPosition.X;
                } else
                {
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

            CheckMovement();
        }

        private void CheckMovement()
        {
            if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.LEFT))
            {
                _currentXVelocity -= _velocityChange;
            }
            else if (GameManager.Instance.KeyState.IsKeyDown(KeyBindings.RIGHT))
            {
                _currentXVelocity += _velocityChange;
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

            if (_currentXVelocity > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            spriteBatch.Draw(ContentManager.Instance.PlayerAnimations[_currentAnimation][_currFrame], Position,
                new Rectangle(0, 0, ContentManager.Instance.PlayerAnimations[_currentAnimation][_currFrame].Width, ContentManager.Instance.PlayerAnimations[_currentAnimation][_currFrame].Height),
                Color.White, 0, Vector2.Zero, 1, effect, 1);
            spriteBatch.Draw(ContentManager.Instance.Shadow,
                new Rectangle(Bounds.X, Bounds.Y + Bounds.Height - 5, Bounds.Width, 10), Color.Black * 0.3f);
        }
    }
}