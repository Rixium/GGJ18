using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GGJ.Games;
using GGJ.Games.Objects;
using GGJ.Games.Players;
using GGJ.Managers;
using GGJ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Screens {

    internal class GameScreen : Screen
    {

        public string RoomName = "The Bunker";

        private List<GameObject> gameObjects = new List<GameObject>();
        public Player Player;

        private readonly int _maxInteractDistance = 100;

        private List<UiComponent> uiComponents = new List<UiComponent>();

        public GameScreen()
        {
            GameManager.Instance.GameScreen = this;

            gameObjects.Add(new Bed(new Vector2(780, 416)));

            gameObjects.Add(new GameBounds(new Rectangle(0, 0, GameConstants.GameWidth, 490)));
            gameObjects.Add(new GameBounds(new Rectangle(0, 0, 330, GameConstants.GameHeight)));
            gameObjects.Add(new GameBounds(new Rectangle(0, 650, GameConstants.GameWidth, 156)));
            gameObjects.Add(new GameBounds(new Rectangle(940, 0, 305, GameConstants.GameHeight)));

            Player = new Player();

            uiComponents.Add(new TextPopup(RoomName));

            uiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 300), "Health"));
            uiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 250), "Sanity"));
            uiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 200), "Hunger"));
            uiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 150), "Thirst"));
            uiComponents.Add(new StatusBar(new Vector2(20, GameConstants.GameHeight - 100), "Bladder"));
            NextDay();
        }

        private void NextDay()
        {
            uiComponents.Add(new TextPopup("Day " + ++GameManager.Instance.CurrentDay));
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
                    new Vector2(Player.Bounds.X + Player.Bounds.Width / 2, Player.Bounds.Y + Player.Bounds.Height / 2),
                    new Vector2(o.Bounds.X + o.Bounds.Width / 2, o.Bounds.Y + o.Bounds.Height / 2));

                if (!(distance < _maxInteractDistance) || interacting) continue;

                interacting = true;
                GameManager.Instance.ActiveObject = o;
            }

            if (!interacting)
            {
                GameManager.Instance.ActiveObject = null;
            }

            foreach (var c in new List<UiComponent>(uiComponents))
            {
                c.Update();

                if (c.Destroy)
                {
                    uiComponents.Remove(c);
                }
            }

            Player.Update();
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

            foreach (var c in new List<UiComponent>(uiComponents))
            {
                c.Paint(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
