using System;
using System.Diagnostics;
using GGJ.Constants;
using GGJ.Managers;
using GGJ.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Games.Objects {

    internal class Radio : GameObject
    {

        public string Message = "";
        public bool ShowingMessage;

        public short MessageTimer;
        public const short MaxMessageTimer = 300;

        public short MessageWidth;
        public short MessageHeight;
        public readonly sbyte MessageMargin = 20;
        public readonly sbyte MessagePadding = 5;

        public Radio(Vector2 position) : base(position, ContentManager.ObjectType.Radio)
        {
        }

        public override string ToString()
        {
            return "Scan [" + KeyBindings.Use + "]";
        }

        public override void Update()
        {
            base.Update();

            if (!ShowingMessage) return;

            if (MessageTimer > 0)
            {
                MessageTimer--;
            }
            else
            {
                ShowingMessage = false;

                if (GameManager.Instance.CurrentStoryLine >= ContentManager.Instance.storyLines.Count)
                {
                    ScreenManager.Instance.ChangeScreen(new EndScreen(ScreenManager.Instance.CurrentScreen.Game, true));
                }
            }
        }

        public override void Use()
        {
            if (!GameManager.Instance.GameScreen.StartUsing(this)) return;
            GameManager.Instance.TotalRadioUse++;
            ContentManager.Instance.Radio.Play(GameConstants.SoundLevel, 0, 0);
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentManager.Instance.Shadow, new Rectangle((int)_position.X, (int)_position.Y + ContentManager.Instance.Objects[ObjectType].Height - 5, ContentManager.Instance.Objects[ObjectType].Width, 10), Color.Black * 0.5f);
            spriteBatch.Draw(ContentManager.Instance.Objects[ObjectType], _position, Color.White);
        }

        public void ShowMessage(string message)
        {
            ShowingMessage = true;
            Message = message;

            MessageTimer = (short) MathHelper.Clamp(Message.Length * 8, 100, MaxMessageTimer);

            MessageWidth = (short)(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(Message).X + MessagePadding * 2);
            MessageHeight = (short)(ContentManager.Instance.Fonts[ContentManager.FontTypes.Game].MeasureString(Message).Y + MessagePadding * 2);

        }
    }

}
