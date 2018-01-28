using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGJ.Games.Objects.RadioStuff {

    internal class Line
    {
        public sbyte SanityValue;
        public string Text;

        public Line(string text, sbyte sanityVal)
        {
            Text = text;
            SanityValue = sanityVal;
        }

    }

}
