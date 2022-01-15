using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System;
using VRNetwork;
using System.Collections.Generic;
using System.Text.Json;

namespace Worms
{
    public class Game1 : Game
    {
        // main
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // global vars
        public int score;
        static public Game1 game;
        public static Map map;
        Worm worm;
        internal Camera camera;
        static public List<ViewObject> viewObjects;
        public string inf;
        public JsonSerializerOptions options;

        TimerCallback gcCallback;
        Timer gcTimer;

        // textures
        SpriteFont spriteFont;

        // game options
        const int pitsCount = 3;
        const int rocksCount = 10;
        public const int widht = 1280;
        public const int height = 720;

        // online vars
        string nickname;
        string ip;
        int port;
        public bool isHost;
        string[] nicknames;
        public static Client client;
        public static Server server;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = widht;
            _graphics.PreferredBackBufferHeight = height;

            options = new JsonSerializerOptions();
            options.Converters.Add(new JsonConverterMap());
        }

        protected override void Initialize()
        {
            game = this;
            viewObjects = new List<ViewObject>();

            map = new Map(100,10,50,100);
            camera = new Camera();

            // read cfg
            nickname = CfgReader.ValueRead(Environment.CurrentDirectory + "\\server.cfg", "nickname");
            ip = CfgReader.ValueRead(Environment.CurrentDirectory + "\\server.cfg", "ip");
            port = Convert.ToInt32(CfgReader.ValueRead(Environment.CurrentDirectory + "\\server.cfg", "port"));

            // online
            nicknames = new string[8];
            try
            {
                server = new Server(ip, port);
                server.StartServer();
                isHost = true;
                nicknames[0] = nickname;

                // цикл игры
                gcCallback = new TimerCallback(GameCycle);
                gcTimer = new Timer(gcCallback, null, 0, 150);
            }
            catch { }
            finally
            {
                client = new Client();
                client.Connect(ip, port);
                client.Send($"!n={nickname}");
                client.ReceiveObjectHandlerEvent += Client_ReceiveObjectHandlerEvent;
            }

            base.Initialize();
        }

        void GameCycle(object target)
        {
            server.SendClient(0, "!s=", true).ConfigureAwait(false);
        }

        private void Client_ReceiveObjectHandlerEvent(object obj)
        {
            if(obj.GetType() == typeof(string))
            {
                string msg = obj as string;
                string[] msgData = msg.Split('=');
                // ники
                if (msgData[0] == "!n")
                {
                    for (int i = 0; i < nicknames.Length; i++)
                    {
                        if (nicknames[i] == "" || nicknames[i] == null && isHost)
                        {
                            nicknames[i] = msgData[1];
                            break;
                        }
                    }
                    client.Send(nicknames);
                }

                if (msgData[0] == "!s" && worm != null)
                    worm.UpdateWorm(null);

                //  syn map
                if (msgData[0] == "!m")
                {
                    try
                    {
                        map = JsonSerializer.Deserialize<Map>(msgData[1], options);
                    }
                    catch { }
                }
            }

            // прием ников для нового клиента
            if (obj.GetType() == typeof(string[]))
            {
                nicknames = obj as string[];
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("font");
            Pit.texture = Content.Load<Texture2D>("Pit");
            Rock.texture = Content.Load<Texture2D>("Rock");
            Apple.texture = Content.Load<Texture2D>("Apple");
            WormPart.texture = Content.Load<Texture2D>("WormPart");

            worm = new Worm();

            map.GenerationMap();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // controls
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Up) && Worm.dir != 3)
                Worm.dir = 1;
            if (kstate.IsKeyDown(Keys.Down) && Worm.dir != 1)
                Worm.dir = 3;
            if (kstate.IsKeyDown(Keys.Left) && Worm.dir != 0)
                Worm.dir = 2;
            if (kstate.IsKeyDown(Keys.Right) && Worm.dir != 2)
                Worm.dir = 0;

            // restart
            if (kstate.IsKeyDown(Keys.R))
            {
                
            }

            // cheats
            if (kstate.IsKeyDown(Keys.I))
                worm.AddWormPart(1,false,true);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.SandyBrown);

            _spriteBatch.Begin();
            for (int i = 0; i < viewObjects.Count; i++)
            {
                viewObjects[i].Drawn(_spriteBatch);
            }

            string info = $"FPS:{1.0f / gameTime.ElapsedGameTime.TotalSeconds}\nCountParts:{Worm.wormParts.Count}\nScore:{score}\n[{Worm.wormParts[0].Pos.X};{Worm.wormParts[0].Pos.Y}]\n\n[Players]\n";
            for (int i = 0; i < nicknames.Length; i++)
            {
                info += nicknames[i] + "\n";
            }
            info += "\n[Info]" + inf;
            _spriteBatch.DrawString(spriteFont, info, new Vector2(0, 0), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
