using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ.Managers {

    /* Store all content variables and dictionaries here for easy access across the project.
     */

    internal class ContentManager
    {

        public enum FontTypes
        {
            Ui,
            Game
        }

        public enum MouseType
        {
            Pointer,
            Hand
        }

        public enum ObjectType
        {
            Bed,
            Radio,
            Toilet,
            GameBounds
        }

        public enum PlayerAnimation
        {
            Idle,
            Walk
        }

        private static ContentManager _instance;
        private Microsoft.Xna.Framework.Content.ContentManager _content;

        // Assets variables.
        public Texture2D Pixel;
        public Texture2D Shadow;

        // Dictionaries.
        public Dictionary<FontTypes, SpriteFont> Fonts = new Dictionary<FontTypes, SpriteFont>();
        public Dictionary<MouseType, Texture2D> Mice = new Dictionary<MouseType, Texture2D>();
        public Dictionary<ObjectType, Texture2D> Objects = new Dictionary<ObjectType, Texture2D>();

        public Dictionary<PlayerAnimation, Texture2D[]> PlayerBodyAnimations =
            new Dictionary<PlayerAnimation, Texture2D[]>();

        public Dictionary<PlayerAnimation, Texture2D[]> PlayerLegAnimations =
            new Dictionary<PlayerAnimation, Texture2D[]>();

        public Texture2D[] PlayerHeads = new Texture2D[6];

        // Scene
        public Texture2D Room;
        public Texture2D RoomFront;

        // UI Stuff.
        public MouseType ActiveMouse = MouseType.Pointer;
        
        // Sounds
        public SoundEffect MenuBlip;

        // Setting up a singleton.
        public static ContentManager Instance => _instance ?? (_instance = new ContentManager());

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _content = content;
            
            // Loading in the content.
            Pixel = _content.Load<Texture2D>("Textures/Other/Pixel");

            Fonts.Add(FontTypes.Ui, _content.Load<SpriteFont>("Fonts/MenuFont"));
            Fonts.Add(FontTypes.Game, _content.Load<SpriteFont>("Fonts/GameFont"));

            Mice.Add(MouseType.Pointer, _content.Load<Texture2D>("Textures/Other/Cursors/mainCursor"));
            Mice.Add(MouseType.Hand, _content.Load<Texture2D>("Textures/Other/Cursors/handCursor"));

            Room = _content.Load<Texture2D>("Textures/Scene/room");
            RoomFront = _content.Load<Texture2D>("Textures/Scene/roomFront");

            Objects.Add(ObjectType.Bed, _content.Load<Texture2D>("Textures/Objects/bed"));
            MenuBlip = _content.Load<SoundEffect>("Sounds/menuBlip");

            var idleLegAnimationFrames = new[]
            {
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle2")
            };

            var walkLegAnimationFrames = new Texture2D[9];

            for (var i = 1; i <= walkLegAnimationFrames.Length; i++)
            {
                walkLegAnimationFrames[i - 1] = _content.Load<Texture2D>("Textures/Man/Legs/manWalk" + i);
            }

            var idleBodyAnimationFrames = new[]
            {
                _content.Load<Texture2D>("Textures/Man/Body/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Body/manIdle2")
            };

            var walkBodyAnimationFrames = new Texture2D[9];

            for (var i = 1; i <= walkBodyAnimationFrames.Length; i++) {
                walkBodyAnimationFrames[i - 1] = _content.Load<Texture2D>("Textures/Man/Body/" + i);
            }


            for (var i = 1; i < PlayerHeads.Length; i++)
            {
                PlayerHeads[i - 1] = _content.Load<Texture2D>("Textures/Man/Head/head" + i);
            }


            PlayerLegAnimations.Add(PlayerAnimation.Idle, idleLegAnimationFrames);
            PlayerLegAnimations.Add(PlayerAnimation.Walk, walkLegAnimationFrames);
            PlayerBodyAnimations.Add(PlayerAnimation.Idle, idleBodyAnimationFrames);
            PlayerBodyAnimations.Add(PlayerAnimation.Walk, walkBodyAnimationFrames);

            Shadow = _content.Load<Texture2D>("Textures/Other/shadow");
        }
    }
}
