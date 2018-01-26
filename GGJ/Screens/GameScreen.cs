using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GGJ.Games.Objects;
using GGJ.Games.Players;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Screens {

    internal class GameScreen : Screen
    {

        private List<GameObject> gameObjects = new List<GameObject>();
        private Player player;

        private readonly int _maxInteractDistance = 100;

        public GameScreen()
        {
            GameManager.Instance.GameScreen = this;

            gameObjects.Add(new Bed(new Vector2(780, 416)));

            gameObjects.Add(new GameBounds(new Rectangle(0, 0, GameConstants.GameWidth, 504)));
            gameObjects.Add(new GameBounds(new Rectangle(0, 0, 330, GameConstants.GameHeight)));
            gameObjects.Add(new GameBounds(new Rectangle(0, 650, GameConstants.GameWidth, 156)));
            gameObjects.Add(new GameBounds(new Rectangle(940, 0, 305, GameConstants.GameHeight)));

            player = new Player();
        }

        public bool CanMove(Rectangle rect)
        {
            foreach (var o in gameObjects)
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
            base.Update();

            float distance;
            var interacting = false;

            foreach (var o in gameObjects)
            {
                o.Update();
                distance = Vector2.Distance(
                    new Vector2(player.Bounds.X + player.Bounds.Width / 2, player.Bounds.Y + player.Bounds.Height / 2),
                    new Vector2(o.Bounds.X + o.Bounds.Width / 2, o.Bounds.Y + o.Bounds.Height / 2));

                if (!(distance < _maxInteractDistance) || interacting) continue;

                interacting = true;
                GameManager.Instance.ActiveObject = o;
            }

            if (!interacting)
            {
                GameManager.Instance.ActiveObject = null;
            }

            player.Update();
        }

        public override void Paint(SpriteBatch spriteBatch)
        {
            base.Paint(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(ContentManager.Instance.Room, new Rectangle(0, 0, GameConstants.GameWidth, GameConstants.GameHeight), Color.White);

            foreach (var o in gameObjects)
            {
                o.Paint(spriteBatch);
            }

            player.Paint(spriteBatch);

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

            spriteBatch.End();
        }
    }
}
