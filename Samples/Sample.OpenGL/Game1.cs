using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using RichTextExtended.Source;
using RichTextExtended.Source.Scanner;
using System.Diagnostics;
using System.IO;

namespace Sample.OpenGL
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

            Scanner scanner = new();
            string input = "<\\<<<<<speed=1>Hi>>\\<>\\\\<o=2><a=2>\\<b==3>< <wave=1 2 3><color=red>R<//color><color=green>G</color><color=blue>B</color/></wave>!\\</speed=2>\\<";
            var b = Tokenizer.Tokenize(scanner.Scan(input));

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
