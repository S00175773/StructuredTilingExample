using CameraNS;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Tiler;
using Tiling;
using Microsoft.Xna.Framework.Audio;

namespace TileBasedPlayer20172018
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        SoundEffect explosion;
        SoundEffect shoot;
        SoundEffect backgroundAudio;
        SoundEffect gameOver;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int tileWidth = 64;
        int tileHeight = 64;
        List<TileRef> TileRefs = new List<TileRef>();
        List<Collider> colliders = new List<Collider>();
        string[] backTileNames = { "blue box", "pavement", "ground", "green", "home", "exit"};
        bool allTanksDestroyed = true;
        public enum TileType { BLUEBOX, PAVEMENT, GROUND, GREEN ,HOME, EXIT };
        int[,] tileMap = new int[,]
    {
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,4,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,3,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,1,1,1,1,2,2,2,2,2,2,2,2,2,3,2,2,1,1,2,2,2,2,2,2,2,1,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,1,1,1,1,2,2,2,2,2,2,2,2,1,1,1,1,1,1,2,2,2,2,2,2,2,3,2,3,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,1,2,1,1,2,2,2,2,2,2,2,3,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,1,3,1,1,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,1,1,1,1,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,1,1,1,1,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,3,1,1,1,3,1,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,5,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
    };
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            new Camera(this, Vector2.Zero, 
                new Vector2(tileMap.GetLength(1) * tileWidth, tileMap.GetLength(0) * tileHeight));
            new InputEngine(this);
            Services.AddService(new TilePlayer(this, new Vector2(64, 128), new List<TileRef>()
            {
                new TileRef(15, 2, 0),
                new TileRef(15, 3, 0),
                new TileRef(15, 4, 0),
                new TileRef(15, 5, 0),
                new TileRef(15, 6, 0),
                new TileRef(15, 7, 0),
                new TileRef(15, 8, 0),
            }, 64, 64, 0f));
            SetColliders(TileType.GROUND);
            SetColliders(TileType.BLUEBOX);
            SetColliders(TileType.GREEN);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);
            Services.AddService(Content.Load<Texture2D>(@"Tiles/tank tiles 64 x 64"));

            // Tile References to be drawn on the Map corresponding to the entries in the defined 
            // Tile Map
            // "free", "pavement", "ground", "blue", "home", "exit" 
            TileRefs.Add(new TileRef(4, 2, 0));
            TileRefs.Add(new TileRef(3, 3, 1));
            TileRefs.Add(new TileRef(6, 3, 2));
            TileRefs.Add(new TileRef(6, 2, 3));
            TileRefs.Add(new TileRef(0, 2, 4));
            TileRefs.Add(new TileRef(1, 2, 5));

            // Names fo the Tiles            
            new SimpleTileLayer(this, backTileNames, tileMap, TileRefs, tileWidth, tileHeight);

            List<Tile> greenTiles = SimpleTileLayer.GetNamedTiles("green");

            //sentry sprite
            for (int i = 0; i < greenTiles.Count; i++)
            {
                TileSentry sentry = new TileSentry(this, new Vector2(greenTiles[i].X * 64, greenTiles[i].Y * 64), new List<TileRef>()
            {
                new TileRef(21, 2, 0),
                new TileRef(21, 3, 0),
                new TileRef(21, 4, 0),
                new TileRef(21, 5, 0),
                new TileRef(21, 6, 0),
                new TileRef(21, 7, 0),
                new TileRef(21, 8, 0),
            }, 64, 64, 0f);
            }

            explosion = Content.Load<SoundEffect>("SoundFiles/Explosion");
            backgroundAudio = Content.Load<SoundEffect>("SoundFiles/Battle_in_the_winter");
            gameOver = Content.Load<SoundEffect>("SoundFiles/Game_Over");
            shoot = Content.Load<SoundEffect>("SoundFiles/TankShot");

            // TODO: use this.Content to load your game content here
        }

        public void SetColliders(TileType t)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    if (tileMap[y, x] == (int)t)
                    {
                        colliders.Add(new Collider(this,
                            Content.Load<Texture2D>(@"Tiles/collider"),
                            x, y
                            ));
                    }

                }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (allTanksDestroyed)
            {
                //not working, should set tile at [15,37] to the exit tile once every sentry was destroyed
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            base.Draw(gameTime);
        }
    }
}
