using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGJ.Screens;

namespace GGJ.Managers {

    internal class ScreenManager
    {

        private static ScreenManager _instance;

        public static ScreenManager Instance => _instance ?? (_instance = new ScreenManager());

        public Screen NextScreen { get; set; }
        public bool Changing { get; private set; }
        public bool Changed { get; private set; }

        public void ChangeScreen(Screen screen)
        {
            NextScreen = screen;
            Changed = false;
            Changing = true;
        }

        public void ActivateNextScreen()
        {
            CurrentScreen = NextScreen;
            Changed = true;
        }

        public void ScreenReady()
        {
            Changing = false;
            Changed = false;
        }

        public Screen CurrentScreen { get; private set; }
    }
}
