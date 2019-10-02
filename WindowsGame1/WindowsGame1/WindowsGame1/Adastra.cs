using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Adastra
{
    /* 
     * To those who may read this code,
     * hello.
     * 
     * I would like to give an analogy about my code.
     * When you see a swan swimming gracefully across a lake, you think, "how beautiful."
     * The swan seems to effortlessly glide across that lake. However, you never see its legs.
     * For all you know there could be an engine down there propelling away.
     * Or the legs could be perfectly formed modding-compatable API-enabled superlegs that blow you away.
     * The legs could also be the worst piece of thrown together machinery you've ever seen, with bolts
     * not screwed all the way and duct taped-together plates barely being help on; giant inefficient blackened
     * coal furnaces chugging away to power the entire mess.
     * 
     * But you would never know, as you only see the swan.
    */

    /*
     * Special thanks to these people for helping me make the game.
     * Sam A. Awesome friend who inspired me to make the best game I could.
       Like seriously. This guy is amazing. No words can describe his sheer looks, intelligence, and character.
       If you think you realize how unbelievably incredible he is; trust me, you don't.
     * Max B. Wanted to be in the credits and helped test.
     * Jake M. Helped with ideas and concepts for the game.
     * Andy C. Helped me with any questions I had while coding.
     */ 


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Adastra : Microsoft.Xna.Framework.Game
    {
        #region Variables

        // Required.
        /// <summary>
        /// Graphics manager. It manages most of the graphics that are drawn.
        /// </summary>
        GraphicsDeviceManager graphics;

        /// <summary>
        /// The sprite batch used for drawing multiple sprites to the screen at once. 
        /// </summary>
        SpriteBatch spriteBatch;


        // Textures.
        /// <summary>
        /// The main font used for things like the UI and buttons.
        /// </summary>
        SpriteFont font;

        /// <summary>
        /// Another font used for the titles of the pause screen and store.
        /// </summary>
        SpriteFont specialFont;

        /// <summary>
        /// The background drawn behind everything.
        /// </summary>
        public static Texture2D backgroundImg;

        /* 
         * There are actually two maps that I'm referring to with "map." 
         * The map of the game (from here on, "world" or "game map"),
         * and the overview map (from here on, "overview map"), drawn when m is pressed.
         * This is the background of the overview map.
         */
        /// <summary>
        /// The background of the map. Drawn when the overview map is drawn.
        /// </summary>
        public static Texture2D mapBackgroundImg;

        /// <summary>
        /// The background of the store. Drawn when the store is up.
        /// </summary>
        public static Texture2D storeBackgroundImg;

        /// <summary>
        /// The image drawn when the game is paused.
        /// </summary>
        public static Texture2D pauseScreenImage;

        /// <summary>
        /// The image of any buttons in the store/pause screen.
        /// </summary>
        public static Texture2D buttonImage;

        /// <summary>
        /// The main texture of the asteroids.
        /// </summary>
        public static Texture2D astrImage;

        /// <summary>
        /// The texture of the asteroid used when the asteroid is "dead."
        /// </summary>
        public static Texture2D astrImageDestroyed;

        /// <summary>
        /// Texture of the planet in the top right corner of the world.
        /// </summary>
        public static Texture2D planetImage;

        /// <summary>
        /// Texture of the station in the bottom left corner of the world.
        /// </summary>
        public static Texture2D stationImage;

        /// <summary>
        /// The texture of the shots that the player and enemies fire.
        /// </summary>
        public static Texture2D shotImage;

        /// <summary>
        /// The image of missiles that are fired from the Light Corvette.
        /// </summary>
        public static Texture2D missileImage;

        /// <summary>
        /// The image of the default ship, the Light Cruiser.
        /// </summary>
        public static Texture2D lightCruiserImg;
        
        /// <summary>
        /// Image of the light corvette.
        /// </summary>
        public static Texture2D lightCorvetteImg;

        /// <summary>
        /// Image of the light frigate.
        /// </summary>
        public static Texture2D lightFrigateImg;

        /// <summary>
        /// Image of the boss ship.
        /// </summary>
        public static Texture2D bossImage;

        /// <summary>
        /// Images of the explosion that happen after an enemy dies.
        /// </summary>
        public static List<Texture2D> explosionImages;

        /// <summary>
        /// Image of the destination of the player. It's the blue circle that replaces the mouse cursor.
        /// </summary>
        public static Texture2D destImage;

        /// <summary>
        /// Image that represents the player on the overview map.
        /// </summary>
        public static Texture2D playerMapPosImg;

        /// <summary>
        /// Image of the player's health in the user interface.
        /// </summary>
        public static Texture2D UIHealthImage;

        /// <summary>
        /// Image of the player's max health in the user interface.
        /// It's the darker-red rectangle drawn behind the health.
        /// </summary>
        public static Texture2D UIHealthImageBack;

        /// <summary>
        /// Backdrop of the name of the ship in the top left.
        /// </summary>
        public static Texture2D UIShipNameImage;


        // The map.
        /// <summary>
        /// The current game map that is drawn and updated.
        /// </summary>
        public static Map theMap;


        // Overlays.
        /// <summary>
        /// The store. You can buy shtuff from it. It's connected to the station in the bottom left corner.
        /// </summary>
        public static Overlay store;

        /// <summary>
        /// The pause screen. Pauses the game with a menu for quitting.
        /// TODO: make this show the controls or add a controls button.
        /// </summary>
        public static Overlay pauseScreen;


        // Entities.
        /// <summary>
        /// The player that the user controls.
        /// </summary>
        public static Player player;

        /// <summary>
        /// The spawn point of the player. This moves with the rest of the entities on the map.
        /// This is used to draw the overview map, meaning that none of the entities on the map move
        /// with the player on the overview map.
        /// </summary>
        public static SpawnPoint playerSpawn;


        // Input states.
        /// <summary>
        /// The current keyboard state at the time of updating.
        /// </summary>
        KeyboardState currKeyboardState;

        /// <summary>
        /// The previous keyboard state from the previous update.
        /// </summary>
        KeyboardState prevKeyboardState;

        /// <summary>
        /// The current mouse state at the time of updating.
        /// </summary>
        MouseState currMouseState;

        /// <summary>
        /// The mouse state from the previous update.
        /// </summary>
        MouseState prevMouseState;

        /// <summary>
        /// The current state of the gamepad at the time of updating.
        /// </summary>
        GamePadState currGamePadState;

        /// <summary>
        /// The previous state of the gamepad at the time of the last update.
        /// </summary>
        GamePadState prevGamePadState;


        // Miscellaneous.
        /// <summary>
        /// Counts the number of game updates. Resets after 100 (1 2/3 seconds).
        /// </summary>
        int counter;

        /// <summary>
        /// Whether or not the overview map is drawn over everything.
        /// </summary>
        bool mapDrawn;

        /// <summary>
        /// Whether or not the store overlay is being drawn.
        /// </summary>
        bool storeDrawn;

        /// <summary>
        /// Whether or not the pause screen is being drawn.
        /// </summary>
        bool pauseDrawn;

        /// <summary>
        /// Whether or not the game is updating.
        /// </summary>
        bool gameRunning;

        /// <summary>
        /// Whether or not enought time has passed that the player can fire another missile.
        /// </summary>
        bool canFireMissile;

        /// <summary>
        /// Counts the number of updates until the player can fire another missile (1 second).
        /// </summary>
        int missileCounter;

        #endregion

        #region RequiredFunctions

        /// <summary>
        /// Construct a new game, setting the root directory and making the graphics manager.
        /// </summary>
        public Adastra()
        {
            // Create the graphics.
            graphics = new GraphicsDeviceManager(this);

            // Set the root directory for any content added.
            Content.RootDirectory = "Content";

            // Set the preferred size of the window.
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 800;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// TODO: put anything that shouldn't be in LoadContent() in here.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize the base game.
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

            // Load any textures needed.
            // Fonts
            font = Content.Load<SpriteFont>("UIFont");
            specialFont = Content.Load<SpriteFont>("SpecialFont");
            // Entities
            astrImage = Content.Load<Texture2D>("asteroid");
            astrImageDestroyed = Content.Load<Texture2D>("asteroidDestroyed");
            stationImage = Content.Load<Texture2D>("station");
            shotImage = Content.Load<Texture2D>("shot");
            missileImage = Content.Load<Texture2D>("missile");
            planetImage = Content.Load<Texture2D>("planet");
            // Ships
            lightCruiserImg = Content.Load<Texture2D>("lightCruiser");
            lightCorvetteImg = Content.Load<Texture2D>("lightCorvette");
            lightFrigateImg = Content.Load<Texture2D>("lightFrigate");
            bossImage = Content.Load<Texture2D>("boss");
            // User interface
            UIHealthImage = Content.Load<Texture2D>("UIHealth");
            UIHealthImageBack = Content.Load<Texture2D>("UIHealthBack");
            UIShipNameImage = Content.Load<Texture2D>("UIShipName");
            // Backgrounds
            storeBackgroundImg = Content.Load<Texture2D>("storeBackground");
            backgroundImg = Content.Load<Texture2D>("background");
            mapBackgroundImg = Content.Load<Texture2D>("mapBackground");
            pauseScreenImage = Content.Load<Texture2D>("pauseScreen");
            // Buttons
            buttonImage = Content.Load<Texture2D>("button");
            // Random
            destImage = Content.Load<Texture2D>("dest");
            playerMapPosImg = Content.Load<Texture2D>("playerMapPos");
            // The explosion images. Done with a for loop to save space.
            explosionImages = new List<Texture2D>();
            for (int i = 1; i <= 8; i++)
            {
                explosionImages.Add(Content.Load<Texture2D>("explosion\\explosion" + i.ToString()));
            }

            // Create the store.
            store = new Overlay(
                storeBackgroundImg,
                Keys.B, // opens with b.
                new List<Button>()
                {
                    new Button(buttonImage, "Repair", new Rectangle(12, 222, 284, 95), "Repair Ship", 65),
                    new Button(buttonImage, "Hull", new Rectangle(308, 222, 284, 95), "Upgrade Hull", 65),
                    new Button(buttonImage, "Engine", new Rectangle(604, 222, 284, 95), "Upgrade Engines", 65),
                    new Button(buttonImage, "Light Cruiser", new Rectangle(12, 325, 284, 95), "Buy Light Cruiser", 110),
                    new Button(buttonImage, "Light Corvette", new Rectangle(308, 325, 284, 95), "Buy Light Corvette", 110),
                    new Button(buttonImage, "Light Frigate", new Rectangle(604, 325, 284, 95), "Buy Light Frigate", 110)
                }
            );

            // Create the pause screen.
            pauseScreen = new Overlay(
                pauseScreenImage,
                Keys.Escape, // opens with escape.
                new List<Button>()
                {
                    new Button(buttonImage, "Exit", new Rectangle(458 - 150, 400 - 100, 284, 95), "Exit Game", -1),
                    new Button(buttonImage, "Restart", new Rectangle(458 - 150, 503 - 100, 284, 95), "Restart Game", -1),
                    new Button(buttonImage, "Return", new Rectangle(458 - 150, 606 - 100, 284, 95), "Return", -1)
                }
            );

            // Load the map.
            loadMap();

            // Create the player and its default ship.
            player = new Player(new LightCruiser());
            player.ship.health += 50; player.ship.maxHealth += 50; // <- balance fix; additional health for the player.
            // TODO: make all ships have more health

            // Along with the player's spawn point.
            playerSpawn = new SpawnPoint();

            // Add the player's spawn position to the update list.
            theMap.entityList.Add(playerSpawn); // yes this is necessary to be able to move it, Scott.

            // Set the player's "position", to help with shot displacements and distances and other things.
            // The player will always be in the center of the screen.
            Player.pos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

            // Set the game update counters to 0.
            counter = 0;
            missileCounter = 0;

            // Set the game to running
            gameRunning = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO : Unload any non ContentManager content here
        }

        #endregion

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Increment the main counter.
            counter++;

            // Update the missileCounter, allowing the player to fire a new missile every 1 second.
            if (canFireMissile == false)
            {
                missileCounter++;
            }
            if (missileCounter > 60)
            {
                canFireMissile = true;
                missileCounter = 0;
            }

            // Make a new random, to be used throughout the update.
            Random rand = new Random();

            // Update the controls and set the player's angle/movement.
            // Checks if the game is running inside of the loop, because some things in it need to
            // be checked when the game is paused (unpause key).
            updateControls();

            // Update all the entities if the game is running.
            if (gameRunning)
            {
                updateEntities(rand);
            }

            // Reset the counter if it's reached 100.
            if (counter == 100)
            {
                counter = 0;
            }

            // Reset the game if the player's dead.
            if (player.ship.health <= 0)
            {
                this.LoadContent();
            }

            // Update the base game. This is required.
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Fill the screen with black. Not technically necessary but just in case.
            GraphicsDevice.Clear(Color.Black);

            // Begin the spriteBatch's drawingness.
            spriteBatch.Begin();

            // Draw the backgrounds of the current map.
            foreach (BackgroundTile bg in theMap.backgroundList)
            {
                spriteBatch.Draw(bg.image, bg.pos, Color.White);
            }

            // Draw all of the entities and their healths above their heads.
            // TODO: make the boss's health bar shorter/more compact.
            foreach (Entity en in theMap.entityList)
            {
                if (en.image != null)
                {
                    spriteBatch.Draw(en.image, en.pos, null, Color.White, en.angle, en.origin, 1.0F, SpriteEffects.None, 1.0F);
                }
                if (en.GetType() == typeof(Enemy))
                {
                    spriteBatch.Draw(UIHealthImage,
                        new Rectangle((int)en.pos.X - (en.image.Bounds.Width / 2), (int)en.pos.Y - (en.image.Bounds.Height / 2) - 5,
                            en.image.Width - (130 - en.health), 10),
                        Color.White);
                }
            }

            // Draw the player with all of its values & angles and origins and shtuff.
            spriteBatch.Draw(player.ship.image, Player.pos, null, Color.White, player.angle, player.ship.origin, 1.0F, SpriteEffects.None, 1.0F);

            // Draw the user interface.
            // Health.
            spriteBatch.Draw(UIHealthImageBack, new Rectangle(3, 761, player.ship.maxHealth * 4 + 6, 36), Color.White);
            spriteBatch.Draw(UIHealthImage, new Rectangle(6, 764, player.ship.health * 4, 30), Color.White);
            // Ship name.
            spriteBatch.Draw(UIShipNameImage, new Rectangle(5, 7, (int)font.MeasureString(player.ship.name).X + 15, (int)font.MeasureString(player.ship.name).Y + 5), Color.White);
            spriteBatch.DrawString(font, player.ship.name, new Vector2(12, 12), Color.White);
            // Money.
            spriteBatch.DrawString(font, player.money.ToString() + " credits", new Vector2(8, 26), Color.White);
            // Controls.
            spriteBatch.DrawString(font, "WASD - accelerate", new Vector2(8, 48), Color.White);
            spriteBatch.DrawString(font, "Shift - decelerate", new Vector2(8, 64), Color.White);
            spriteBatch.DrawString(font, "M - map", new Vector2(8, 80), Color.White);
            spriteBatch.DrawString(font, "B - buy (at station)", new Vector2(8, 96), Color.White);
            spriteBatch.DrawString(font, "Left Click - fire a shot", new Vector2(8, 112), Color.White);
            if (player.ship is LightCorvette) spriteBatch.DrawString(font, "Right Click - fire a missile", new Vector2(8, 128), Color.White);

            // Draw the map overview map's everything.
            if (mapDrawn)
            {
                // Draw the overview map's background.
                spriteBatch.Draw(mapBackgroundImg, new Rectangle(150, 100, 900, 600), Color.White);

                // Draw all of the entities onto this map using a ridiculous equation.
                foreach (Entity en in theMap.entityList)
                {
                    if (en.image != null)
                    {
                        spriteBatch.Draw(
                            en.image,
                            new Rectangle(
                                // <bragging> I figured this equation out myself. </bragging> (except with a little help)
                                (int)((-(playerSpawn.pos.X - en.pos.X) / 15.6F) + (7200 / 15.6F) - (en.image.Width / 32) + 150),
                                (int)((-(playerSpawn.pos.Y - en.pos.Y) / 17.3333333F) + (4800 / 17.3333333F) - (en.image.Height / 32) + 100),
                                (int)(en.image.Width / 16),
                                (int)(en.image.Height / 16)),
                            Color.White);
                    }
                }
                // Draw the player's position on the overview map using the sameish equation.
                float x = (-playerSpawn.pos.X / 15.6F) + (7200 / 15.6F) + (600 / 15.6F) - (4 / 15.6F) + 150;
                float y = (-playerSpawn.pos.Y / 17.3333333F) + (4800 / 17.3333333F) + (400 / 17.3333333F) - (4 / 17.3333333F) + 100;
                if (x > 150 && x < graphics.PreferredBackBufferWidth - 150 && 
                    y > 100 && y < graphics.PreferredBackBufferHeight - 100)
                {
                    // Position and label
                    spriteBatch.Draw(
                        playerMapPosImg,
                        new Vector2(x, y),
                        Color.White);
                    spriteBatch.DrawString(
                        font,
                        "You are here",
                        new Vector2(x - 40, y + 10),
                        Color.Red);
                }
                else
                {
                    // Only a label in the center of the map
                    spriteBatch.DrawString(
                        font,
                        "You are off the map",
                        new Vector2(graphics.PreferredBackBufferWidth/2 - 80, graphics.PreferredBackBufferHeight/2 + 50),
                        Color.Red);
                }
                
            }

            // Draw the store overlay over everything.
            if (storeDrawn)
            {
                // Draw the store's background.
                spriteBatch.Draw(store.image, new Vector2(150, 100), Color.White);
                // Draw the text "Store" at the top because people need to know it's a store.
                spriteBatch.DrawString(
                    specialFont, 
                    "Store", 
                    new Vector2((store.image.Width / 2) - (specialFont.MeasureString("Store").X / 2) + 150, 110), 
                    Color.White);

                // Draw all of the buttons.
                foreach (Button b in store.buttons)
                {
                    // Draw the button.
                    spriteBatch.Draw(
                        b.image, 
                        new Rectangle(b.pos.X + 150, b.pos.Y + 100, b.pos.Width, b.pos.Height), 
                        Color.White);
                    // Draw the button's text.
                    spriteBatch.DrawString(
                        font,
                        b.text,
                        new Vector2(
                            b.pos.X + 150 + b.image.Width / 2 - font.MeasureString(b.text).X / 2,
                            b.pos.Y + 100 + b.image.Height / 2 - font.MeasureString(b.text).Y / 2),
                        Color.White);
                    // Draw the button's cost.
                    spriteBatch.DrawString(
                        font,
                        b.cost.ToString() + " credits",
                        new Vector2(
                            b.pos.X + 150 + b.image.Width / 2 - font.MeasureString(b.cost.ToString() + " credits").X / 2,
                            b.pos.Y + 100 + b.image.Height / 2 + 15 - font.MeasureString(b.cost.ToString() + " credits").Y / 2),
                        Color.White);
                }
            }

            // Draw the pause screen.
            if (pauseDrawn)
            {
                // Draw the pause screen's background.
                spriteBatch.Draw(pauseScreen.image, Vector2.Zero, Color.White);
                // Draw the text "Paused" because people need to know that it's paused.
                spriteBatch.DrawString(
                    specialFont, 
                    "Paused", 
                    new Vector2((pauseScreen.image.Width / 2) - (specialFont.MeasureString("Paused").X / 2), 110), 
                    Color.LightGray);
                // Draw the pause screen's buttons.
                foreach (Button b in pauseScreen.buttons)
                {
                    // Draw the button image.
                    spriteBatch.Draw(
                        b.image,
                        new Rectangle(b.pos.X + 150, b.pos.Y + 100, b.pos.Width, b.pos.Height),
                        Color.LightGray);
                    // Draw the button's text.
                    spriteBatch.DrawString(
                        font,
                        b.text,
                        new Vector2(
                            b.pos.X + 150 + b.image.Width / 2 - font.MeasureString(b.text).X / 2,
                            b.pos.Y + 100 + b.image.Height / 2 - font.MeasureString(b.text).Y / 2),
                        Color.White);
                }
            }

            // Draw the destination image (the little blue circle), where the mouse is.
            spriteBatch.Draw(destImage, new Vector2(player.destPos.X - destImage.Width / 2, player.destPos.Y - destImage.Height / 2), Color.White);

            // End the drawingness.
            spriteBatch.End();

            // Draw the base. This is required.
            base.Draw(gameTime);
        }

        /// <summary>
        /// Updates the controls (keyboard, mouse, gamepad) of the user and sets
        /// the player's angle and movement speed for that update.
        /// </summary>
        private void updateControls()
        {
            // Get the current state of the keyboard, mouse, and gamepad.
            currKeyboardState = Keyboard.GetState();
            currMouseState = Mouse.GetState();
            currGamePadState = GamePad.GetState(PlayerIndex.One);

            // If a gamepad is connected, set the destination position to where the right stick is.
            if (currGamePadState.IsConnected)
            {
                if (currGamePadState.ThumbSticks.Right.X != 0 || currGamePadState.ThumbSticks.Right.Y != 0)
                {
                    player.destPos = new Vector2(
                        Player.pos.X + currGamePadState.ThumbSticks.Right.X * 300,
                        Player.pos.Y - currGamePadState.ThumbSticks.Right.Y * 300);
                }
            }
            // Else set the dest pos to where the mouse is.
            else
            {
                player.destPos = new Vector2(currMouseState.X, currMouseState.Y);
            }

            // Do all of the following as long as the game isn't paused.
            if (gameRunning)
            {
                // Find the destination angle between the player's current position and the destination position.
                player.destAngle = (float)Math.Atan2(player.destPos.Y - Player.pos.Y, player.destPos.X - Player.pos.X) + (float)Math.PI / 2;

                // While the player's angle isn't the dest angle, move the player's angle closer and closer to it.
                // Eventually that will happen.
                // For now and probably ever just set the player's angle to the 
                // angle between the player and the destination position.
                if (player.angle != player.destAngle)
                {
                    player.angle = player.destAngle;
                }

                // Then do some math to determine the angle of the player.
                // Converts it to radians/degrees or something.
                player.angle += player.amountSpin;
                player.angle = player.angle % MathHelper.TwoPi;

                // Decide the amount to move the player right/left depending on their keyboard/pad.
                // The player's ship has a max speed, too.
                if (currGamePadState.IsConnected)
                {
                    if (player.amountMoveX <= player.ship.maxSpeed)
                    {
                        player.amountMoveX += currGamePadState.ThumbSticks.Left.X * player.ship.accelSpeed;
                    }
                    if (player.amountMoveY <= player.ship.maxSpeed)
                    {
                        player.amountMoveY += currGamePadState.ThumbSticks.Left.Y * player.ship.accelSpeed;
                    }
                }
                if (currKeyboardState.IsKeyDown(Keys.A)
                    && Math.Abs(player.amountMoveX) <= player.ship.maxSpeed
                    && Math.Abs(player.amountMoveY) <= player.ship.maxSpeed)
                {
                    player.amountMoveX -= (float)(player.ship.accelSpeed * Math.Sin(player.angle + Math.PI/2f));
                    player.amountMoveY -= (float)(player.ship.accelSpeed * Math.Cos(player.angle + Math.PI/2f));
                }
                if (currKeyboardState.IsKeyDown(Keys.D)
                    && Math.Abs(player.amountMoveX) <= player.ship.maxSpeed
                    && Math.Abs(player.amountMoveY) <= player.ship.maxSpeed)
                {
                    player.amountMoveX += (float)(player.ship.accelSpeed * Math.Sin(player.angle + Math.PI/2f));
                    player.amountMoveY += (float)(player.ship.accelSpeed * Math.Cos(player.angle + Math.PI/2f));
                }

                // If they're holding down W, add some to their move speed.
                // Again, the player's ship's max's speed's.
                if (currKeyboardState.IsKeyDown(Keys.W) 
                    && Math.Abs(player.amountMoveX) <= player.ship.maxSpeed 
                    && Math.Abs(player.amountMoveY) <= player.ship.maxSpeed
                    || (currGamePadState.IsConnected && currGamePadState.ThumbSticks.Left.Y == 1.0F))
                {
                    player.amountMoveX += (float)(player.ship.accelSpeed * Math.Sin(player.angle));
                    player.amountMoveY += (float)(player.ship.accelSpeed * Math.Cos(player.angle));
                }
                // Same for S only decrement the speed.
                if (currKeyboardState.IsKeyDown(Keys.S)
                    && Math.Abs(player.amountMoveX) <= player.ship.maxSpeed
                    && Math.Abs(player.amountMoveY) <= player.ship.maxSpeed
                    || (currGamePadState.IsConnected && currGamePadState.ThumbSticks.Left.Y == -1.0F ))
                {
                    player.amountMoveX -= (float)(player.ship.accelSpeed * Math.Sin(player.angle));
                    player.amountMoveY -= (float)(player.ship.accelSpeed * Math.Cos(player.angle));
                }

                // If the player is holding down leftshift (or B on the gamepad), slow down the ship.
                if (currKeyboardState.IsKeyDown(Keys.LeftShift) || currGamePadState.IsConnected &&
                    currGamePadState.IsButtonDown(Buttons.B))
                {
                    if (player.amountMoveY > 0)
                    {
                        player.amountMoveY -= 0.25F;
                    }
                    else if (player.amountMoveY < 0)
                    {
                        player.amountMoveY += 0.25F;
                    }
                    if (player.amountMoveX > 0)
                    {
                        player.amountMoveX -= 0.25F;
                    }
                    else if (player.amountMoveX < 0)
                    {
                        player.amountMoveX += 0.25F;
                    }
                }

                // Reset speeds if they are too fast
                player.amountMoveX = (Math.Abs(player.amountMoveX) >= player.ship.maxSpeed)
                    ? ((player.amountMoveX > 0) ? 1 : -1) * player.ship.maxSpeed
                    : player.amountMoveX;
                player.amountMoveY = (Math.Abs(player.amountMoveY) >= player.ship.maxSpeed)
                    ? ((player.amountMoveY > 0) ? 1 : -1) * player.ship.maxSpeed
                    : player.amountMoveY;

                // Shoot two shots if the left mouse button (right trigger) was just pressed.
                if (currMouseState.LeftButton == ButtonState.Pressed /*&& prevMouseState.LeftButton == ButtonState.Released*/ 
                    && player.cooldown <= 0
                    || (currGamePadState.IsConnected && currGamePadState.IsButtonDown(Buttons.RightTrigger) &&
                    prevGamePadState.IsButtonUp(Buttons.RightTrigger)))
                {
                    // Reset their shot cooldown
                    player.cooldown = 20;

                    // Good luck.
                    // Props to Christian for suggesting randomish shots.
                    Random rand = new Random();
                    float randAngle = (float)rand.NextDouble() / MathHelper.TwoPi;
                    Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, player.angle);

                    foreach (Vector2 shotOffset in player.ship.shotDisplacements)
                    {
                        Vector2 shotPos = Vector2.Transform(shotOffset, rotation) + Player.pos;
                        Shot shot = new Shot(shotPos, player.angle + randAngle, true);
                        shot.constantMovement = true; shot.amountMoveY = 8; theMap.entityList.Add(shot);
                    }
                }
                // Decrement the cooldown every tick when not shooting
                else if (player.cooldown > 0)
                {
                    player.cooldown -= 1;
                }

                // Shoot a missile if the player's ship can fire them, and if they've right clicked (left trigger).
                if (player.ship.missileDisplacements != null && canFireMissile
                    && (currMouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released 
                    || (currGamePadState.IsConnected && currGamePadState.IsButtonDown(Buttons.LeftTrigger) && prevGamePadState.IsButtonUp(Buttons.LeftTrigger))))
                {
                    // More good luck to you.
                    Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, player.angle);

                    Entity destEn = null;
                    foreach (Entity en in theMap.entityList)
                    {
                        if (Vector2.Distance(Player.pos, en.pos) < (destEn != null ? Vector2.Distance(Player.pos, destEn.pos) : 3600)
                         && en.GetType() == typeof(Enemy))
                        {
                            destEn = en;
                        }
                    }
                    foreach (Vector2 missileOffset in player.ship.missileDisplacements)
                    {
                        Vector2 missilePos = Vector2.Transform(missileOffset, rotation) + Player.pos;
                        Missile missile = new Missile(missilePos, player.angle, true, destEn);
                        missile.constantMovement = true; missile.AI.controller = missile;
                        missile.AI.controllerMissile = missile; theMap.entityList.Add(missile);
                    }
                    canFireMissile = false;
                }
            }
            // #ENDIF
            // The rest is called even if the game is paused.

            // Open the store if player presses b near the station. Needed outside of the gameRunning if because
            // opening/closing the store (un)pauses the game.
            if (((currKeyboardState.IsKeyDown(store.key) && prevKeyboardState.IsKeyUp(store.key)) || 
                (currGamePadState.IsConnected && currGamePadState.Buttons.Y == ButtonState.Pressed && prevGamePadState.Buttons.Y == ButtonState.Released)) &&
                Vector2.Distance(Player.pos, store.at.pos) + player.ship.image.Width / 2 + store.at.radius <= 1200 &&
                !pauseDrawn && !mapDrawn)
            {
                storeDrawn = !storeDrawn;
                gameRunning = !gameRunning;
            }

            // If the player clicks on one of the buttons in the store, buy the button.
            Button b = store.getClickedOn(currMouseState, currGamePadState); // for clearer code.
            if (((currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released) ||
                (currGamePadState.IsConnected && currGamePadState.IsButtonDown(Buttons.RightTrigger) &&
                prevGamePadState.IsButtonUp(Buttons.RightTrigger))) && storeDrawn && b != null &&
                player.money >= b.cost)
            {
                player.money -= b.cost;
                // TODO: make buttons have their own functions to do these.
                if (b.name == "Repair")
                {
                    player.ship.health = player.ship.maxHealth;
                }
                else if (b.name == "Hull")
                {
                    player.ship.health += 30;
                    player.ship.maxHealth += 30;
                }
                else if (b.name == "Engine")
                {
                    player.ship.maxSpeed += 3;
                }
                else if (b.name == "Light Cruiser")
                {
                    player.ship = new LightCruiser();
                }
                else if (b.name == "Light Corvette")
                {
                    player.ship = new LightCorvette();
                }
                else if (b.name == "Light Frigate")
                {
                    player.ship = new LightFrigate();

                }
            }

            // If the player hits m (or x on gamepad), set the map (in)visible and (un)pause the game.
            if ((currKeyboardState.IsKeyDown(Keys.M) && prevKeyboardState.IsKeyUp(Keys.M) ||
                currGamePadState.IsConnected && currGamePadState.IsButtonDown(Buttons.X) && prevGamePadState.IsButtonUp(Buttons.X))
                && !pauseDrawn && !storeDrawn)
            {
                mapDrawn = !mapDrawn;
                gameRunning = !gameRunning;
            }

            // Check the pause menu's buttons.
            b = pauseScreen.getClickedOn(currMouseState, currGamePadState);
            if ((currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released ||
                currGamePadState.IsConnected && currGamePadState.IsButtonDown(Buttons.RightTrigger) && prevGamePadState.IsButtonUp(Buttons.RightTrigger)) &&
                pauseDrawn && b != null)
            {
                if (b.name == "Exit")
                {
                    this.Exit();
                }
                else if (b.name == "Restart")
                {
                    this.LoadContent();
                    this.gameRunning = false;
                }
                else if (b.name == "Return")
                {
                    pauseDrawn = false;
                    gameRunning = true;
                }
            }

            // Pause the game if the player hits escape (start).
            if ((currKeyboardState.IsKeyDown(pauseScreen.key) && prevKeyboardState.IsKeyUp(pauseScreen.key) ||
                currGamePadState.IsConnected && currGamePadState.IsButtonDown(Buttons.Start) && prevGamePadState.IsButtonUp(Buttons.Start))
                && !mapDrawn && !storeDrawn)
            {
                pauseDrawn = !pauseDrawn;
                gameRunning = !gameRunning;
            }

            // Set the previous keyboard, mouse, and gamepad states.
            prevKeyboardState = currKeyboardState;
            prevMouseState = currMouseState;
            prevGamePadState = currGamePadState;
        }

        /// <summary>
        /// Updates all of the entities, including 'moving' the player by moving them all, 
        /// updating their AI, collision, and movement.
        /// </summary>
        private void updateEntities(Random r)
        {
            // Then move the player, doing some trig to move them forward, backward, left, or right
            // at their angle based on their speed (amountMoveX, amountMoveY).
            // BUT. Don't actually move the player, instead move every single entity and the backround in
            // the opposite direction, to make the screen seem to follow the player.
            foreach (Entity en in theMap.entityList)
            {
                /*en.pos = new Vector2(en.pos.X - (float)(Math.Sin(player.angle) * player.amountMoveY),
                                     en.pos.Y + (float)(Math.Cos(player.angle) * player.amountMoveY));
                en.pos = new Vector2(en.pos.X - (float)(Math.Sin(player.angle + Math.PI / 2) * player.amountMoveX),
                                     en.pos.Y + (float)(Math.Cos(player.angle + Math.PI / 2) * player.amountMoveX));*/
                en.pos = new Vector2(en.pos.X - player.amountMoveX,
                                     en.pos.Y + player.amountMoveY);
            }

            // Then move the backgrounds.
            foreach (BackgroundTile bg in theMap.backgroundList)
            {
                /*bg.pos = new Vector2(bg.pos.X - (float)(Math.Sin(player.angle) * player.amountMoveY / 4),
                                     bg.pos.Y + (float)(Math.Cos(player.angle) * player.amountMoveY / 4));
                bg.pos = new Vector2(bg.pos.X - (float)(Math.Sin(player.angle + Math.PI / 2) * player.amountMoveX / 4),
                                     bg.pos.Y + (float)(Math.Cos(player.angle + Math.PI / 2) * player.amountMoveX / 4));*/
                bg.pos = new Vector2(bg.pos.X - player.amountMoveX / 4f,
                                     bg.pos.Y + player.amountMoveY / 4f);
            }

            // Update all of the entities.
            for (int i = theMap.entityList.Count - 1; i >= 0; i--)
            {
                // Added this because of some bug I was too lazy to fix.
                // Index out of range exception. TODO: fix this.
                if (i > theMap.entityList.Count - 1)
                    i--;

                // Update the entity's Ai, which determines where it should move.
                // Put its return value into updateReturn (uR) so if it's other than 1,
                // the entity can do something. TODO: remove this.
                int ur = theMap.entityList[i].AI.update();

                // Update the entity's per 5 updates ai. Also assigns its return value.
                if (counter % 5 == 0)
                {
                    int ur5 = theMap.entityList[i].AI.update5();
                }

                // Update the entity's per 100 updates ai. Also also assigns its return value.
                if (counter % 100 == 0)
                {
                    int ur100 = theMap.entityList[i].AI.update100(r);
                }

                // If something other than 0 was returned in update(), call the entity's specialUpdate().
                if (ur != 0)
                {
                    theMap.entityList[i].specialUpdate(ur);
                }

                // Collision detection. Goes through every entity again to check if that entity is within
                // this entity's radius. If so, colliding is true and entity number 2 loses 10 health.
                bool colliding = false;
                for (int j = theMap.entityList.Count - 1; j >= 0; j--)
                {
                    if (theMap.entityList[i] != theMap.entityList[j])
                    {
                        if (Math.Abs(Vector2.Distance(theMap.entityList[i].pos, theMap.entityList[j].pos)) <= theMap.entityList[j].radius)
                        {
                            if (!(theMap.entityList[i].GetType() == typeof(Enemy) && theMap.entityList[j].GetType() == typeof(Asteroid))
                             && !(theMap.entityList[i].GetType() == typeof(Asteroid) && theMap.entityList[j].GetType() == typeof(Enemy)))
                            { // ^ enemies and asteroids don't collide. enemies also don't collide with anything, done down below.
                                colliding = true;
                                if (!theMap.entityList[j].invincible)
                                {
                                    theMap.entityList[j].health -= 10;
                                }
                            }
                        }
                    }
                }
                // And if this entity is colliding with the player the player loses health and 
                // colliding is set to true.
                if (Math.Abs(Vector2.Distance(theMap.entityList[i].pos, Player.pos)) <= player.ship.radius + theMap.entityList[i].radius)
                {
                    colliding = true;
                    int damage = 0;
                    switch(theMap.entityList[i])
                    {
                        case Shot s:
                            if (s.playerShot)
                            {
                                colliding = false;
                                damage = 0;
                            }
                            else damage = 5;
                            break;
                        case Asteroid a:
                            damage = 1; break;
                        case Missile m:
                            if (m.playerShot)
                            {
                                colliding = false;
                                damage = 0;
                            }
                            else damage = 12;
                            break;
                        default:
                            break;
                    }
                    
                    player.ship.health -= damage;
                }

                // Move the entity, if it's AI has chosen to do so.
                if (!colliding)
                {
                    // Set the entity's angle, which should be edited in update5() or update100().
                    theMap.entityList[i].angle += theMap.entityList[i].amountSpin;
                    theMap.entityList[i].angle = theMap.entityList[i].angle % MathHelper.TwoPi;

                    // Set the entity's new position.
                    theMap.entityList[i].pos = new Vector2(
                        theMap.entityList[i].pos.X + (float)(Math.Sin(theMap.entityList[i].angle) * theMap.entityList[i].amountMoveY),
                        theMap.entityList[i].pos.Y - (float)(Math.Cos(theMap.entityList[i].angle) * theMap.entityList[i].amountMoveY));
                }
                // Destroy this entity if it is colliding (and not an enemy) or if its health is less than 0.
                if (colliding && theMap.entityList[i].GetType() != typeof(Enemy) || theMap.entityList[i].health <= 0)
                {
                    theMap.entityList[i].destroy(null);
                }

                // Explosions stuff. They're only alive for a certain amount of time, and have an animation.
                // Shots also have a limited lifespan. TODO: make missiles have a limited lifespan and move slower.
                if (i < theMap.entityList.Count && (theMap.entityList[i].GetType() == typeof(Explosion)
                    || theMap.entityList[i].GetType() == typeof(Shot) || theMap.entityList[i].GetType() == typeof(Missile)))
                {
                    theMap.entityList[i].aliveCounter--;
                    if (theMap.entityList[i].GetType() == typeof(Explosion) && theMap.entityList[i].aliveCounter % 5 == 0)
                    {
                        theMap.entityList[i].image = explosionImages[theMap.entityList[i].aliveCounter / 5];
                    }
                    if (theMap.entityList[i].aliveCounter <= 0)
                    {
                        theMap.entityList.Remove(theMap.entityList[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the map from map.txt.
        /// </summary>
        private void loadMap()
        {
            // Create a new empty map.
            theMap = new Map();

            // Make a random.
            Random rand = new Random();

            // Read the text file.
            using (var stream = TitleContainer.OpenStream("Content\\map.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    // Split the file into an array of strings to be read individually.
                    foreach (String s in reader.ReadToEnd().Split('\n'))
                    {
                        // Then split the individual line into an array of strings representing the
                        // words separated by spaces.
                        String[] line = s.Split(' ');

                        // Make an asteroid if the first line is Asteroid. 
                        // 2nd and 3rd lines are the x/y coords in pixels of the asteroid.
                        if (line[0] == "Asteroid")
                        {
                            Asteroid tempAsteroid = new Asteroid(System.Convert.ToInt32(line[1]), System.Convert.ToInt32(line[2]));
                            tempAsteroid.AI.controller = tempAsteroid;
                            theMap.addEntity(tempAsteroid);
                        }
                        
                        // Enemies. 2nd line determines the enemy's ship. 3rd and 4th are x/y.
                        else if (line[0] == "Enemy")
                        {
                            Ship ship;
                            switch(line[1])
                            {
                                case "LightCruiser": ship = new LightCruiser(); break;
                                case "LightCorvette": ship = new LightCorvette(); break;
                                case "LightFrigate": ship = new LightFrigate(); break;
                                default: ship = new LightCruiser(); break;
                            }

                            Enemy tempEnemy = new Enemy(
                                ship,
                                new Vector2(System.Convert.ToInt32(line[2]), System.Convert.ToInt32(line[3])),
                                ship.maxHealth,
                                new EnemyAI());
                            tempEnemy.AI.controller = tempEnemy; tempEnemy.AI.controllerEnemy = tempEnemy;

                            theMap.addEntity(tempEnemy);
                        }

                        // Planets. 2nd and 3rd lines are x/y.
                        else if (line[0] == "Planet")
                        {
                            Planet tempPlanet = new Planet(System.Convert.ToInt32(line[1]), System.Convert.ToInt32(line[2]));
                            tempPlanet.AI.controller = tempPlanet;
                            theMap.addEntity(tempPlanet);
                        }

                        // Stations. 2/3 are x/y.
                        else if (line[0] == "Station")
                        {
                            Station tempStation = new Station(System.Convert.ToInt32(line[1]), System.Convert.ToInt32(line[2]));
                            tempStation.AI.controller = tempStation;
                            store.at = tempStation;
                            theMap.addEntity(tempStation);
                        }

                        // The boss. 2/3 are x/y.
                        else if (line[0] == "Boss")
                        {
                            Enemy tempBoss = new Enemy(
                                new BossShip(),
                                new Vector2(System.Convert.ToInt32(line[1]), System.Convert.ToInt32(line[2])),
                                new BossShip().health,
                                new EnemyAI()); tempBoss.AI.controller = tempBoss; tempBoss.AI.controllerEnemy = tempBoss;
                            theMap.addEntity(tempBoss);
                        }
                    }
                }
            }

            theMap.setGenericBackgrounds();
        }
    }
}
