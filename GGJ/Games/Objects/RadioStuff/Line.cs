using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGJ.Games.Objects.RadioStuff {

    internal class Line
    {
        private sbyte _sanityVal;
        private string _text;

        public Line(string text, sbyte sanityVal)
        {
            _text = text;
            _sanityVal = sanityVal;
        }

    }

}
