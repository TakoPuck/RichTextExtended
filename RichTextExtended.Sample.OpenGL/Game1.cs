using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using RichTextExtended.Parser;
using RichTextExtended.Scanner;
using RichTextExtended.Tokenizer;
using System.IO;

namespace RichTextExtended.Sample.OpenGL
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BitmapFont _bmfont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _bmfont = BitmapFont.FromFile(GraphicsDevice, Path.Combine("Content", "Fonts", "WispelldomFnt.fnt"));

            RichTextScanner scanner = new();
            string input = "<c=red> Hi <w=1.1 2.2 0 h>there</w></c> !";
            var segments = scanner.Scan(input);
            var tokens = RichTextTokenizer.Tokenize(segments);
            var output = RichTextParser.Parse(tokens);

            int debug = 1;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_bmfont, "Hi", new Vector2(50, 50), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
