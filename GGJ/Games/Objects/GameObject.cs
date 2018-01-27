using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Games.Objects {

    internal class GameObject
    {
        public ContentManager.ObjectType ObjectType;
        protected Vector2 _position;
        public bool Destroy;

        public GameObject(Vector2 position, ContentManager.ObjectType objectType)
        {
            _position = position;
            ObjectType = objectType;
        }

        public virtual Rectangle Bounds => new Rectangle((int) _position.X, (int) _position.Y,
            ContentManager.Instance.Objects[ObjectType].Width, ContentManager.Instance.Objects[ObjectType].Height);

        public Vector2 Position => _position;

        public virtual void Update()
        {

        }

        public virtual void Paint(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentManager.Instance.Shadow, new Rectangle((int)_position.X, (int)_position.Y + ContentManager.Instance.Objects[ObjectType].Height - 5, ContentManager.Instance.Objects[ObjectType].Width, 10), Color.Black * 0.5f);
            spriteBatch.Draw(ContentManager.Instance.Objects[ObjectType], _position, Color.White);
        }

        public virtual void Use()
        {

        }

    }

}
