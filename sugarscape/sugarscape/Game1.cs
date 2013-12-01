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

namespace sugarscape
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
		ContentManager content;
        SpriteBatch spriteBatch;

		View view;
		UserController user;

		World world;

		List<Agent> agents;

		private int framesPerUpdate;
		private int frameCount;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
			content = new ContentManager(Services);
			content.RootDirectory = "Content\\";


			world = new World(Constants.DEFAULT_WORLD_X, Constants.DEFAULT_WORLD_Y);
			agents = new List<Agent>();

			framesPerUpdate = Constants.START_FRAMES_PER_SIM_UPDATE;
			frameCount = 0;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
			view = new View(GraphicsDevice);
			user = new UserController();
			user.initialize();

			initWorld();
			initAgents();

            base.Initialize();
        }


		private void initWorld() {
			world.fillCells();
		}


		private void initAgents() {
			agents.Add(new Agent(1, 1, 5, 60, 1, 2, world));
			agents.Add(new Agent(6, 4, 5, 60, 1, 2, world));
		}


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			view.loadContent(content);
			// TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Check controls from UserController here
			user.update(view);


			frameCount++;
			if (frameCount >= framesPerUpdate) {
				world.updateOneStep();

				shuffleAgents();
				foreach (Agent a in agents) {
					a.updateOneStep();
				}

				frameCount = 0;
			}

            base.Update(gameTime);
        }

		/// <summary>
		/// Randomizes the order of agents
		/// </summary>
		private void shuffleAgents() {

		}


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

			view.drawWorld(world);

			foreach (Agent a in agents) {
				view.drawAgent(a);
			}

            base.Draw(gameTime);
        }
    }
}
