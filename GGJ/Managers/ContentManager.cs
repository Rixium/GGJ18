using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Games.Objects.RadioStuff;
using GGJ.Games.Players;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GGJ.Managers {

    /* Store all content variables and dictionaries here for easy access across the project.
     */

    internal class ContentManager
    {

        public enum FontTypes
        {
            Ui,
            Game,
            TitleFont
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
            Water,
            Food,
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
        public Texture2D Noise;
        public Texture2D RadioSpeech;
        public Texture2D Splash;

        // Dictionaries.
        public Dictionary<FontTypes, SpriteFont> Fonts = new Dictionary<FontTypes, SpriteFont>();
        public Dictionary<MouseType, Texture2D> Mice = new Dictionary<MouseType, Texture2D>();
        public Dictionary<ObjectType, Texture2D> Objects = new Dictionary<ObjectType, Texture2D>();

        public Dictionary<PlayerAnimation, Texture2D[]> PlayerBodyAnimations =
            new Dictionary<PlayerAnimation, Texture2D[]>();

        public Dictionary<PlayerAnimation, Texture2D[]> PlayerLegAnimations =
            new Dictionary<PlayerAnimation, Texture2D[]>();

        public List<Line> talkLines = new List<Line>();
        public List<Line> storyLines = new List<Line>();

        public Texture2D[] Food = new Texture2D[16];
        public Dictionary<Player.Gender, Texture2D[]> PlayerHeads = new Dictionary<Player.Gender, Texture2D[]>();

        // Scene
        public Texture2D Room;
        public Texture2D RoomFront;

        // UI Stuff.
        public MouseType ActiveMouse = MouseType.Pointer;
        
        // Sounds
        public SoundEffect MenuBlip;
        public SoundEffect Eat;
        public SoundEffect Drink;
        public SoundEffect Toilet;
        public SoundEffect Radio;
        public SoundEffect Bed;

        public Texture2D femaleButton;
        public Texture2D maleButton;


        // Music
        public Song Theme;

        // Setting up a singleton.
        public static ContentManager Instance => _instance ?? (_instance = new ContentManager());

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _content = content;
            
            // Loading in the content.
            Pixel = _content.Load<Texture2D>("Textures/Other/Pixel");

            Fonts.Add(FontTypes.Ui, _content.Load<SpriteFont>("Fonts/MenuFont"));
            Fonts.Add(FontTypes.Game, _content.Load<SpriteFont>("Fonts/GameFont"));
            Fonts.Add(FontTypes.TitleFont, _content.Load<SpriteFont>("Fonts/TitleFont"));

            Mice.Add(MouseType.Pointer, _content.Load<Texture2D>("Textures/Other/Cursors/mainCursor"));
            Mice.Add(MouseType.Hand, _content.Load<Texture2D>("Textures/Other/Cursors/handCursor"));

            Room = _content.Load<Texture2D>("Textures/Scene/room");
            RoomFront = _content.Load<Texture2D>("Textures/Scene/roomFront");

            Objects.Add(ObjectType.Bed, _content.Load<Texture2D>("Textures/Objects/bed"));
            Objects.Add(ObjectType.Toilet, _content.Load<Texture2D>("Textures/Objects/toilet"));
            Objects.Add(ObjectType.Radio, _content.Load<Texture2D>("Textures/Objects/radio"));
            Objects.Add(ObjectType.Water, _content.Load<Texture2D>("Textures/Objects/sink"));

            MenuBlip = _content.Load<SoundEffect>("Sounds/menuBlip");
            Eat = _content.Load<SoundEffect>("Sounds/eat");
            Drink = _content.Load<SoundEffect>("Sounds/drink");
            Radio = _content.Load<SoundEffect>("Sounds/radio");
            Toilet = _content.Load<SoundEffect>("Sounds/toilet");
            Bed = _content.Load<SoundEffect>("Sounds/bed");

            var idleLegAnimationFrames = new[]
            {
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1"),
                _content.Load<Texture2D>("Textures/Man/Legs/manIdle1")
            };

            var walkLegAnimationFrames = new Texture2D[9];

            for (var i = 1; i <= walkLegAnimationFrames.Length; i++)
            {
                walkLegAnimationFrames[i - 1] = _content.Load<Texture2D>("Textures/Man/Legs/manWalk" + i);
            }

            var idleBodyAnimationFrames = new Texture2D[9];

            for (var i = 1; i <= idleBodyAnimationFrames.Length; i++) {
                idleBodyAnimationFrames[i - 1] = _content.Load<Texture2D>("Textures/Man/Body/manIdle" + i);
            }


            var walkBodyAnimationFrames = new Texture2D[9];

            for (var i = 1; i <= walkBodyAnimationFrames.Length; i++) {
                walkBodyAnimationFrames[i - 1] = _content.Load<Texture2D>("Textures/Man/Body/" + i);
            }

            PlayerHeads.Add(Player.Gender.Male, new Texture2D[5]);
            
            for (var i = 1; i <= PlayerHeads[Player.Gender.Male].Length; i++)
            {
                PlayerHeads[Player.Gender.Male][i - 1] = _content.Load<Texture2D>("Textures/Man/Head/head" + i);
            }

            PlayerHeads.Add(Player.Gender.Female, new Texture2D[5]);

            for (var i = 1; i <= PlayerHeads[Player.Gender.Female].Length; i++) {
                PlayerHeads[Player.Gender.Female][i - 1] = _content.Load<Texture2D>("Textures/Woman/Head/head" + i);
            }

            for (var i = 1; i <= Food.Length; i++)
            {
                Food[i - 1] = _content.Load<Texture2D>("Textures/Objects/tins" + i);
            }

            PlayerLegAnimations.Add(PlayerAnimation.Idle, idleLegAnimationFrames);
            PlayerLegAnimations.Add(PlayerAnimation.Walk, walkLegAnimationFrames);
            PlayerBodyAnimations.Add(PlayerAnimation.Idle, idleBodyAnimationFrames);
            PlayerBodyAnimations.Add(PlayerAnimation.Walk, walkBodyAnimationFrames);

            Shadow = _content.Load<Texture2D>("Textures/Other/shadow");
            Theme = content.Load<Song>("Music/theme");
            Noise = _content.Load<Texture2D>("Textures/Other/noise");

            talkLines.Add(new Line("Hello..? Please..? Anybody..?", -5));
            talkLines.Add(new Line("...", -5));
            talkLines.Add(new Line("*static*", -5));
            talkLines.Add(new Line("We're safe, Don't worry. You'll be safe too.", 10));
            talkLines.Add(new Line("Can anybody hear me? Don't go outside.", -5));
            talkLines.Add(new Line("Help, HELP!", -10));
            talkLines.Add(new Line("I feel hope.", 5));
            talkLines.Add(new Line("If you hear me, I love you.", 5));
            talkLines.Add(new Line("I'm here, darling.", 2));
            talkLines.Add(new Line("Mummy, I can't see. It's dark.", -10));
            talkLines.Add(new Line("We're all gonna die.", -10));
            talkLines.Add(new Line("We're starving!", -10));
            talkLines.Add(new Line("I can't see a way out of this.", -5));
            talkLines.Add(new Line("Look to the future.", 10));
            talkLines.Add(new Line("May god be with us.", 10));
            talkLines.Add(new Line("Keep sane.", -5));
            talkLines.Add(new Line("She's gone, Oh god, No, Please.", -10));
            talkLines.Add(new Line("God will save us.", 10));
            talkLines.Add(new Line("We're all gonna die.", -10));
            talkLines.Add(new Line("Don't worry.", 5));
            talkLines.Add(new Line("Daddy? I don't know how to use this.", -10));
            talkLines.Add(new Line("We can survive this.", 10));

            storyLines.Add(new Line("Nationwide military dispatched to handle catastrophic event.", 5));
            storyLines.Add(new Line("People advised to stay underground, until further notice.", -5));
            storyLines.Add(new Line("Contamination is thought be contained, but unknown yet.", -5));
            storyLines.Add(new Line("Several military units lost in severe viral attack.", -10));
            storyLines.Add(new Line("We're transmitting this message in hope that everyone stays underground.", 0));
            storyLines.Add(new Line("Do not leave your bunker, no matter what.", -5));
            storyLines.Add(new Line("The end is near, units have spread controlled.", 5));
            storyLines.Add(new Line("Local areas contained, but virus still active.", -5));
            storyLines.Add(new Line("A potential cure has been discovered.", 20));
            storyLines.Add(new Line("Cure proven ineffective, await further notice.", -25));
            storyLines.Add(new Line("Miltary withdrawn from north, as remaining infected eliminated.", 10));
            storyLines.Add(new Line("New medicine testing underway.", 8));
            storyLines.Add(new Line("New medicine looking effective as test subject condition improves.", 15));
            storyLines.Add(new Line("Government forsee problem will be solved within the year.", -30));
            storyLines.Add(new Line("New technology will be used in mass treatment.", 15));
            storyLines.Add(new Line("Widespread treatment has began.", 10));
            storyLines.Add(new Line("Military are checking area for any remaining infected.", 5));
            storyLines.Add(new Line("The virus has been eliminated, it's safe.. you can come out.", 100));

            RadioSpeech = _content.Load<Texture2D>("Textures/Other/radioSpeech");
            Splash = _content.Load<Texture2D>("Textures/Other/splash");

            maleButton = _content.Load<Texture2D>("Textures/Other/manButton");
            femaleButton = _content.Load<Texture2D>("Textures/Other/womanButton");

        }
    }
}
