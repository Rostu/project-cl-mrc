using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JADE
{
    public class TextData
    {
        private Control[] checkBoxes;
        private string text;

        public TextData() { }

        public TextData(Control[] checkBoxes, string text) 
        {
            this.checkBoxes = checkBoxes;
            this.text = text;
        }

        public Control[] CheckBoxes
        {
            get
            {
                return checkBoxes;
            }
            set
            {
                checkBoxes = value;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
    }
}
