using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
	class DialogBox : UIElement
	{
		// Fields
		private Button okButton;
		private SpriteFont font;
		private int maxLineWidth;
		private string text;
		private bool show;
		private int margin;

		// Properties
		public Button BoxButton
		{
			get { return okButton; }
		}

		public int MaxLineWidth
		{
			get { return maxLineWidth; }
			set { maxLineWidth = value; }
		}

		public bool Show
		{
			get { return show; }
			set { show = value; }
		}

		// Constructor
		public DialogBox(Texture2D sprite, Rectangle position, Button okButton, SpriteFont font, int maxLineWidth, string text) : base(sprite, position)
		{
			this.okButton = okButton;
			this.font = font;
			margin = 15;
			this.maxLineWidth = maxLineWidth - margin;
			this.text = text;
			show = false;
		}

		// Methods
		/// <summary>
		/// Wrap a given text within the dialog box
		/// </summary>
		/// <param name="text">The text to wrap</param>
		/// <returns>A wrapped tetx that will fit inside the dialog box</returns>
		public string WrapText(string text)
		{
			if (font.MeasureString(text).X < MaxLineWidth)
			{
				return text;
			}

			string[] words = text.Split(' ');
			StringBuilder wrappedText = new StringBuilder();
			float linewidth = 0f;
			float spaceWidth = font.MeasureString(" ").X;
			for (int i = 0; i < words.Length; ++i)
			{
				Vector2 size = font.MeasureString(words[i]);
				if (linewidth + size.X < MaxLineWidth)
				{
					linewidth += size.X + spaceWidth;
				}
				else
				{
					wrappedText.Append("\n");
					linewidth = size.X + spaceWidth;
				}
				wrappedText.Append(words[i]);
				wrappedText.Append(" ");
			}

			return wrappedText.ToString();
		}

		public void Draw(SpriteBatch sb, Color textColor)
		{
			sb.Draw(sprite, position, Color.Black);

			// Wrap Text
			string wrappedText = WrapText(text);

			// Create Vector from Rectangle
			Vector2 positionVec = new Vector2(position.X + margin, position.Y);

			// Draw the text within the box
			sb.DrawString(font, wrappedText, positionVec, textColor);
		}
	}
}
