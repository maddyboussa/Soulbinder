using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Soulbinder
{
    class Keybind
    {
        // Fields
        private Keys keybind;
        private Keys tempBind;
        private Keys defaultBind;
        private List<Keys> keyInputs;
        private Button keybindDisplay;
        private Button keybindButton;

        // Properties
        public Keys Bind { get { return keybind; } set { keybind = value; } }
        public Keys DefaultBind { get { return defaultBind; }}

        // Constructor
        public Keybind(Keys keybind, Keys defaultBind, Button keybindDisplay, Button keybindButton)
        {
            this.keybind = keybind;
            tempBind = keybind;
            this.defaultBind = defaultBind;
            keyInputs = new List<Keys>();
            this.keybindDisplay = keybindDisplay;
            this.keybindButton = keybindButton;
        }

        // Methods

        public void ChangeBind(MouseState ms, KeyboardState kb)
        {
                // Click on button
            if (keybindDisplay.Clicked(ms))
            {
                keybindDisplay.WaitingInput = true;
            }

                // Change Keybind
            if (keybindDisplay.WaitingInput)
            {
                if (kb.GetPressedKeys().Length > 0)
                {
                    keyInputs.Add(kb.GetPressedKeys()[0]);
                }

                if (keyInputs.Count > 0)
                {
                    tempBind = keyInputs[0];
                    keybindDisplay.WaitingInput = false;
                    keyInputs.Clear();
                }
            }
        }

        public void Save()
        {
            keybind = tempBind;
        }

        public void SoftReset()
        {
            tempBind = keybind;
        }

        public void HardReset()
        {
            keybind = defaultBind;
        }

        public void Draw(SpriteBatch sb)
        {
            if (keybindDisplay.WaitingInput)
            {
                keybindDisplay.Draw(sb, "Waiting Input...", keybindDisplay.ButtonShape, Alignment.Center, Color.Gray, Color.Red);
            }
            else keybindDisplay.Draw(sb, tempBind.ToString(), keybindDisplay.ButtonShape, Alignment.Center, Color.Gray, Color.White);
        }
    }
}
