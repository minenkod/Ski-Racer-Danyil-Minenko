using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Skir_Racer
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {

        #region "Variables"


        enum gameState
        {
            //Enum is used to deterimine what the game should be doing. 
            attract,
            playing,
            paused,
            newGame,
            gameover
        }
        gameState CurrentGameState; // Stores the gamestate. 

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SoundEffect soundNearFlag; //Variables to store the sound file then play it when the game needs to. 
        SoundEffect soundCheese; //
        SoundEffect soundSnowball; // 

        SnowEffect snowEffectClass; //Each of the classes unique methods is called throghout the game. 
        Cheese cheese;//
        Skier skier;
        Snowball snowball;
        FootPrintManager PrintManager;
        List<Sprite> PlayField = new List<Sprite>(); //This is the main list that holds every sprite in the game. 
        //using the list the game can save, load, update and draw it self in one call. 

        List<Flag> flags = new List<Flag>();//Flags are put into there own list for the collision detection. 
        //
        int space = 400;//This is the space between the left and right flag. 
        int time; //Cheese is spawned according to the amount of time passed. 
        int gameTicks; //Variable is used to track the time passed and if the an event should be fired. 
        int lives = 3; //the amount of lives the player starts of with when the application is started. 
        int score = 0; //The score the player currently has. 
        int highestScore = 0;// The highest score scored in the session. 
        int safeSpot; // the safe spot for the skier to respawn in and not get hit again. 
        int attractTimer; //Timer determines how fast the sprite should move. 
        string mode; //Sprite Font displays the current state of the game. 
        int speed = 2; // The speed of the game. 
        bool stopCheese = false; //used to determine if the cheese is currently spawned. 
        bool cheeseAvailable = true; //Used to see if the cheese spawn timer has been reset and if the cheese is allowed to spawn again. 
        SpriteFont font; //The heads up display font. 
        public float Ticks; //The ticks elapsed in the current game. 

        Vector2 gameStatePosition; //The possition of head up display font. 
        Vector2 scorePosition;//
        Vector2 livesPosition;//
        Vector2 highScorePosition;//

        Rectangle leftFlagRect; //All rectangles used to detect collision. 
        Rectangle skierRect;  //
        Rectangle snowballRect;//
        Rectangle snowEffectRect;//
        Rectangle cheeseRect;//

        Texture2D flagTex { get; set; }   //Used to store the sprite textures and then draw them.
        Texture2D snowballTex;
        Texture2D cheeseTex;
        Texture2D snowEffectTex;
        Texture2D skierUp;
        Texture2D skierLeft;
        Texture2D skierRight;
        Texture2D skierDown;
        Texture2D lineTex; //Line that is used to detect the skier going through flag. 
        int[] flagxCord = { 200, 230, 250, 230, 200, 230, 250, 230, 200, 230 }; //Both arrays are used as 
        int[] flagyCord = { 5, 40, 90, 140, 190, 240, 290, 340, 390, 440 }; //coordinates for the flag possitions. 


        #endregion

        #region "Starting and restarting game"
        private void newGame()
        {
            //All of the lists are cleared and prepared for the next game.
            PlayField.Clear();
            flags.Clear();
            LoadContent();
            cheeseAvailable = true;
            CurrentGameState = gameState.playing;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //All of the texures, sprite fonts and sounds are loaded into the game variables. 
            soundNearFlag = Content.Load<SoundEffect>("sounds/click"); //The sounds are loaded from the sound folder
            soundSnowball = Content.Load<SoundEffect>("sounds/snowball");
            soundCheese = Content.Load<SoundEffect>("sounds/cheese");
            flagTex = Content.Load<Texture2D>("graphics/flag"); //the graphics are loaded from the graphics folder
            snowballTex = Content.Load<Texture2D>("graphics/snowbal");
            cheeseTex = Content.Load<Texture2D>("graphics/cheese");
            snowEffectTex = Content.Load<Texture2D>("graphics/snowEffect");
            skierUp = Content.Load<Texture2D>("graphics/up");
            skierLeft = Content.Load<Texture2D>("graphics/left");
            skierRight = Content.Load<Texture2D>("graphics/right");
            skierDown = Content.Load<Texture2D>("graphics/skier");
            font = Content.Load<SpriteFont>("graphics/font");
            lineTex = Content.Load<Texture2D>("graphics/line");

            //Variables are set to the defaults. This is usefull when starting a new game. 
            cheeseAvailable = true;
            time = 0;
            lives = 3;
            score = 0;
            gameTicks = 0;
            safeSpot = 300;

            snowballRect = new Rectangle(-10, -10, Window.ClientBounds.Width / 10, Window.ClientBounds.Width / 10);  //The new rectangles are created. 
            leftFlagRect = new Rectangle(0, 0, Window.ClientBounds.Width / 30, Window.ClientBounds.Width / 30);
            skierRect = new Rectangle(100, 55, Window.ClientBounds.Width / 20, Window.ClientBounds.Width / 20);
            cheeseRect = new Rectangle(50, 50, Window.ClientBounds.Width / 30, Window.ClientBounds.Width / 30);
            snowEffectRect = new Rectangle(1, 1, 1, 1);

            for (int i = 0; i < flagxCord.Length; i++) //The flags are added into both the playfield list aswell as there own flag list to detect collision. 
            {
                Flag FlagLeft = new Flag(flagTex, leftFlagRect, new Vector2(flagxCord[i], flagyCord[i]), new Vector2(0, 1));
                PlayField.Add(FlagLeft);
                flags.Add(FlagLeft);
            }

            skier = new Skier(skierDown, skierRect, new Vector2(safeSpot, 50), new Vector2(4, 4));
            PlayField.Add(skier);
            int safe = 200 + space / 2; //The safespot is reset. 
            Random r = new Random();
            snowball = new Snowball(snowballTex, snowballRect, new Vector2(-25, 100), new Vector2(2, 0));
            PrintManager = new FootPrintManager(Content.Load<Texture2D>("graphics/footPrint"), new Vector2(400, 240)); //This is the manager for the particle effect. 
            PlayField.Add(snowball); //The snowball is added to the playfield. 
            cheese = new Cheese(cheeseTex, cheeseRect, new Vector2(0, -30), new Vector2(0, 0));
            PlayField.Add(cheese);
            snowEffectClass = new SnowEffect(snowEffectTex, snowEffectRect, new Vector2(1, 1), new Vector2(1, 0));
            PlayField.Add(snowEffectClass);

        }
        private void attractMode()
        {
            Random r = new Random();
            attractTimer += 1;
            if (attractTimer == 3)  //The skier will move when the timer is at 3. 
            {
                attractTimer = 0;

                if (skier.position.X < 500) //Conditions that check if the skier has left the screen .
                {
                    skier.Update(this, "right", GraphicsDevice.Viewport.Height, skierDown);
                }
                if (skier.position.Y < 400)
                {
                    skier.Update(this, "down", GraphicsDevice.Viewport.Height, skierDown);
                }
                if (skier.position.Y >= 250)
                {
                    skier.setPossition(safeSpot, 10); //Reset the skier posstion back to its spawn location. 
                }
            }
        }
        protected override void UnloadContent()
        {
        }
       
        #endregion
      
        #region "Save and Load"

        private void save()
        {
            StreamWriter sw = new StreamWriter("save.txt"); // A stream writter will be created to edit the save file. 
            foreach (Sprite sprite in PlayField)
            {
                sprite.Save(sw);
            }
            sw.WriteLine("Cheese:" + cheeseAvailable); //Save if cheese was available. 
            sw.WriteLine("Lives:" + lives);
            sw.WriteLine("Score:" + score);
            sw.WriteLine("Time:" + time);   //Time is used for spawning the cheese. 
            sw.WriteLine("Flag Time:" + flags[0].timer);  //With the time saved the speed is also saved. 
            sw.WriteLine("Flag Space:" + flags[0].space);
            sw.Close(); //The stream is closed. 
        }

        private void load()  // The load method reads each lines and applies the appropriate line of code. 
        {
            string[] lines = File.ReadAllLines("save.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                //get the lives and score from the line
                if (lines[i].StartsWith("Lives:"))
                {
                    //found the line that is lives
                    string livesStr = lines[i].Substring(lines[i].IndexOf(":") + 1); //The string is trimmed leaving only the part the is used. 
                    lives = int.Parse(livesStr);
                }

                else if (lines[i].StartsWith("Score:"))
                {
                    //found the line that is score
                    string scoreStr = lines[i].Substring(lines[i].IndexOf(":") + 1);
                    score = int.Parse(scoreStr);
                }
                else if (lines[i].StartsWith("Cheese:"))
                {
                    //found the line that is cheese
                    string cheeseStr = lines[i].Substring(lines[i].IndexOf(":") + 1);
                    cheeseAvailable = Convert.ToBoolean(cheeseStr);
                }
                else if (lines[i].StartsWith("Time:"))
                {
                    string timestr = lines[i].Substring(lines[i].IndexOf(":") + 1);
                    time = int.Parse(timestr);
                }
                else if (lines[i].StartsWith("Flag Time:"))
                {
                    string flagTimeStr = lines[i].Substring(lines[i].IndexOf(":") + 1);

                    foreach (Flag flag in flags)
                    {
                        flag.timer = int.Parse(flagTimeStr);
                    }
                }
                else if (lines[i].StartsWith("Flag Space:"))
                {
                    string spaceStr = lines[i].Substring(lines[i].IndexOf(":") + 1);
                    space = int.Parse(spaceStr);
                    foreach (Flag flag in flags)
                    {
                        flag.space = space;
                    }
                }
                else
                {
                    string strx = lines[i].Substring(0, lines[i].IndexOf(">")); //The string is trimmed and the usefull data is gathered, 
                    string stry = lines[i].Substring(lines[i].IndexOf(">") + 1);
                    stry = stry.Substring(0, stry.IndexOf(":")); //The symbols give an index where to cut the string. 
                    int x = int.Parse(strx);
                    int y = int.Parse(stry);
                    PlayField[i].setPossition(x, y); //The x and y possition of the sprite has been parsed now the posstion can be set. 
                    string posX = lines[i];
                    posX = posX.Substring(posX.IndexOf(":") + 1);
                    posX = posX.Substring(0, posX.IndexOf(":"));
                    string posY = lines[i];
                    posY = posY.Substring(posY.IndexOf(":") + 1);
                    posY = posY.Substring(posY.IndexOf(":") + 1);
                    int px = int.Parse(posX);
                    int py = int.Parse(posY);
                    PlayField[i].setSpeed(px, py); //The x and y possition of the sprite has been parsed now the speed can be set. 
                }
            }
        }

        private void manageCheese()
        {
            if (time == 200 && cheeseAvailable == true && stopCheese == false)  //All three conditions need to be met for the cheese to spawn again .
            {
                Random r = new Random();
                int availableX = r.Next(flags[0].rectangle.X + 10, flags[0].rightRectangleC.X - 10);//The cheese needs to be spawned in a reachable possition  
                int availableY = r.Next(5, 400);     //The random coordinates are set to give results that are reachable possitions. 
                cheese.setPossition(availableX, availableY);
                cheeseAvailable = false;
            }

            if (time == 500)  //If cheese is not picked up in 8 seconds then its respawned in a reachable possition. 
            {
                cheese.setPossition(-100, -100); //Cheese is put out of sight. 
                cheeseAvailable = true;
                time = 0;
            }
        }

        #endregion

        #region "Update and Collision"
        protected override void Update(GameTime gameTime)
        {
            PrintManager.EmitterLocation = new Vector2(skier.position.X + 15, skier.position.Y); //The particle emmiter location will be set to the skier posstion. 
            PrintManager.Update();     //The particles will now be rendered .

            snowEffectClass.speed.Y = flags[0].speed.Y; //The snow is made to be the same speed as the flags to give an illusion of movement. 
            snowball.speed.X = flags[0].speed.Y + 4; // The snowballs will be set to the speed of the flags plus an extra 4 for speed. 
            //This way as the game progresses the snowballs become faster with the flags. 
            if (lives == 0)   //The player has lost all their lives
            {
                if (score > highestScore && CurrentGameState == gameState.playing) //If they beat the highscore record it. 
                {
                    highestScore = score; //The highscore is set to the high score. 
                }
                newGame();//start a new game and return to attract mode untill the user enters play mode again. 
                CurrentGameState = gameState.attract;
            }

            if (CurrentGameState == gameState.attract) //In this mode the skier will move around the field. 
            {
                mode = "Attract/Demo"; //The mode varaible will display the game state on the HUD for the user to see. 
                attractMode();   //start attract mode.
            }

            if (CurrentGameState == gameState.playing)
            {
                safeSpot = (flags[0].rectangle.X + flags[0].rightRectangleC.X) / 2;   //This is the X possition that is safe from getting hit by flags. 
                time += 1;
                gameTicks = (int)gameTime.ElapsedGameTime.TotalSeconds;
            }


            if (CurrentGameState == gameState.paused)
            {
                stopCheese = true;     //Prevent the cheese from spawning
                foreach (Flag flag in flags)
                {
                    flag.speed = new Vector2(0, 0);  //Take away all the speed from the game object and pause the flag timers. 
                    flag.pause = true;
                }

                snowball.speed.X = flags[0].speed.Y;
                cheese.speed = new Vector2(0, 0);
                stopCheese = false;     //spawn cheese
            }

            if (CurrentGameState == gameState.newGame)
            {
                newGame();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            foreach (Sprite s in PlayField)  //Call each of the update method for each game object in the list. 
            {
                s.Update(this);
            }

            snowEffectClass.setGraphics(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            rollSnowball();
            manageCheese();
            checkCollision();
            keyPress();
            base.Update(gameTime);
        }
        private void checkCollision()
        {
            if (skier.rectangle.Intersects(cheese.rectangle))
            {
                soundCheese.Play(); //The cheese is collected, play sound
                cheese.setPossition(-100, -100); //Cheese is put out of sight. 
                cheeseAvailable = true;         //control variables are reset to spawn more cheese. 
                time = 0;                       //
                lives += 1;                     //Player Lives are incremented. 
            }
            if (snowball.rectangle.Intersects(skier.rectangle))  //The skier is hit by the snowball
            {
                Random r = new Random();
                int randomY = r.Next(1, 430);
                soundSnowball.Play();
                lives -= 1;
                skier.setPossition(safeSpot, 100); //Reset the skier posstion to where he wont get hit. 
                snowball.setPossition(-10, randomY);
            }
            //checking the flag collision. 
            foreach (Flag flag in flags)
            {
                //detecting skier going through the flag
                if (skier.rectangle.Intersects(flag.lineRectangle()))   //this means the skier went past a flag so increment the score. 
                {
                    flag.active = false;
                    score += 1;
                }

                //Detecting when to play sound
                if (skier.rectangle.Intersects(flag.leftRectangle))     //The skier is too close to the left flag 
                {
                    soundNearFlag.Play();  //Play a buzzing sound informing the player of the danger

                }

                if (skier.rectangle.Intersects(flag.rightRectangle))//The skier is too close to the right flag 
                {
                    soundNearFlag.Play();
                }

                if (skier.rectangle.Intersects(flag.rectangle)) //The skier has hit the left flag. 
                {
                    skier.setPossition(safeSpot, 100);
                    lives -= 1;
                }

                if (skier.rectangle.Intersects(flag.rightRectangleC)) //The skier has hit the right flag. 
                {
                    skier.setPossition(safeSpot, 100);
                    lives -= 1;
                }
            }
        }

        private void keyPress() //Listen for kepress and pass parameters to the skier sprite
        {
            KeyboardState key = Keyboard.GetState();
            if (CurrentGameState == gameState.playing) //Only allows the player to move skier if the game is in playing mode. 
            {
                mode = "Playing";
                if (key.IsKeyDown(Keys.Left))
                {
                    skier.Update(this, "left", GraphicsDevice.Viewport.Width, skierLeft);
                }
                if (key.IsKeyDown(Keys.Right))
                {
                    skier.Update(this, "right", GraphicsDevice.Viewport.Width, skierRight);
                }
                if (key.IsKeyDown(Keys.Up))
                {
                    //
                    skier.Update(this, "up", GraphicsDevice.Viewport.Height, skierUp);
                }
                if (key.IsKeyDown(Keys.Down))
                {
                    skier.Update(this, "down", GraphicsDevice.Viewport.Height, skierDown);
                }
            }

            if (key.IsKeyDown(Keys.S)) //Save game
            {
                save();
            }
            if (key.IsKeyDown(Keys.Q)) //Exit the game 
            {
                Exit();
            }
            if (key.IsKeyDown(Keys.R)) //Load last save state
            {
                load();
            }
            if (key.IsKeyDown(Keys.P)) //pause the game
            {
                CurrentGameState = gameState.paused;
                PrintManager.paused = true;
                if (flags[0].speed != new Vector2(0, 0))
                {
                    flags[0].storedSpeed = flags[0].speed;
                }
                skier.storedSpeed = skier.speed;
                mode = "Paused";  //The mode will be set to paused. This will be detect and all the sprite speeds will be set to 0 untill the game is resumed. 
            }

            if (key.IsKeyDown(Keys.O)) //Resume the game
            {
                CurrentGameState = gameState.playing;
                PrintManager.paused = false;
                skier.speed = skier.storedSpeed;
                foreach (Flag flag in flags)
                {
                    flag.speed = flags[0].storedSpeed;
                    flag.pause = false;
                }
                snowball.speed.X = flags[0].speed.Y;
                cheese.speed = new Vector2(0, 0);
                stopCheese = false;     //spawn cheese
            }

            if (key.IsKeyDown(Keys.N)) //start a new game
            {
                CurrentGameState = gameState.newGame;
            }
        }
        private void rollSnowball()
        {
            Random r = new Random();
            int interval = r.Next(1, 100);  //Set the range further appart for a longer interval. 
            //Snowball is rolled at a random interval. 
            if (interval == 20)
            {
                snowball.roll = true;
            }
        }
        private void checkSkierPosition()
        {
            int allowedArea = flags[0].rectangle.X + flags[0].rightRectangleC.X;   //This is the X possition that is safe from getting hit by flags. 
        }
       


        #endregion

        #region "Drawing"
        private void drawFont()
        {
            string scoreString = "Score: " + score.ToString();
            string livesString = "Lives: " + lives.ToString();
            string highestScoreString = "High Score: " + highestScore.ToString();
            scorePosition = new Vector2(50, 100);
            livesPosition = new Vector2(50, 150);
            highScorePosition = new Vector2(50, 200);
            gameStatePosition = new Vector2(50, 250);
            // Find the center of the string

            Vector2 FontOrigin = new Vector2(44, 14);
            // Draw the string
            spriteBatch.DrawString(font, scoreString, scorePosition, Color.Black,  //The font is drawn with it own posstion.
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(font, livesString, livesPosition, Color.Black,
               0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(font, highestScoreString, highScorePosition, Color.Black,
              0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(font, mode, gameStatePosition, Color.Black,
             0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">The amount of time passed since the game began.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkTurquoise);
            snowEffectClass.DrawSnow(spriteBatch);     //The snoweffect is drawn. 
            spriteBatch.Begin();
            PrintManager.Draw(spriteBatch);
            foreach (Sprite s in PlayField) //Each of the sprite inside the main sprite list is drawn onto the gamescreen. 
            {
                s.Draw(spriteBatch);
            }
            drawFont(); //The hud is drawn. 
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

        #endregion
       


}