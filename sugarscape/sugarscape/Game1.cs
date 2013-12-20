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
		List<Agent> agents2;

		private int framesPerUpdate;
		private int frameCount;

		Random rand = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
			content = new ContentManager(Services);
			content.RootDirectory = "Content\\";


			world = new World(Constants.DEFAULT_WORLD_X, Constants.DEFAULT_WORLD_Y, this);
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

			agents = new List<Agent>(Constants.MAX_AGENTS);
			agents2 = new List<Agent>(Constants.MAX_AGENTS);

			initWorld();
			initAgents();

            base.Initialize();
        }


		private void initWorld() {
			world.fillCells();
		}


		private void initAgents() {
			switch (Constants.agentGenMode) {
				case Constants.Agent_Gen_Mode.HARDCODED:
					genAgentsHardcoded();
					break;
				case Constants.Agent_Gen_Mode.RANDOM:
					genAgentsRandom();
					break;
				case Constants.Agent_Gen_Mode.SQUARES:
					genAgentsSquares();
					break;
			}
		}

		private void genAgentsRandom() {
			for (int i = 0; i < Constants.START_AGENTS_COUNT; i++) {
				int x = rand.Next(Constants.DEFAULT_WORLD_X);
				int y = rand.Next(Constants.DEFAULT_WORLD_Y);

				int met = rand.Next(Constants.MET_MIN, Constants.MET_MAX + 1);
				int vis = rand.Next(Constants.VISION_MIN, Constants.VISION_MAX + 1);
				int life = rand.Next(Constants.MAX_AGE_MIN, Constants.MAX_AGE_MAX + 1);
				int sug = rand.Next(Constants.INITIAL_SUGAR_MIN, Constants.INITIAL_SUGAR_MAX + 1);

				Agent a = new Agent(x, y, sug, life, met, vis, world);
				if (world.initialSpawnAgent(a)) {
					agents.Add(a);
				}
			}
		}

		private void genAgentsHardcoded() {
			agents.Add(new Agent(1, 1, 5, 60, 2, 3, world));
			agents.Add(new Agent(2, 2, 5, 60, 2, 3, world));
		}

		private void genAgentsSquares() {
			for (int i = 0; i < Constants.START_AGENTS_COUNT / 2; i++) {
				int x = rand.Next(20);
				int y = rand.Next(20);

				int met = rand.Next(Constants.MET_MIN, Constants.MET_MAX + 1);
				int vis = rand.Next(Constants.VISION_MIN, Constants.VISION_MAX + 1);
				int life = rand.Next(Constants.MAX_AGE_MIN, Constants.MAX_AGE_MAX + 1);
				int sug = rand.Next(Constants.INITIAL_SUGAR_MIN, Constants.INITIAL_SUGAR_MAX + 1);

				Agent a = new Agent(x, y, sug, life, met, vis, world);
				if (world.initialSpawnAgent(a)) {
					agents.Add(a);
				}
			}
			for (int i = 0; i < Constants.START_AGENTS_COUNT / 2; i++) {
				int x = rand.Next(Constants.DEFAULT_WORLD_X - 20, Constants.DEFAULT_WORLD_X);
				int y = rand.Next(Constants.DEFAULT_WORLD_Y - 20, Constants.DEFAULT_WORLD_Y);

				int met = rand.Next(Constants.MET_MIN, Constants.MET_MAX + 1);
				int vis = rand.Next(Constants.VISION_MIN, Constants.VISION_MAX + 1);
				int life = rand.Next(Constants.MAX_AGE_MIN, Constants.MAX_AGE_MAX + 1);
				int sug = rand.Next(Constants.INITIAL_SUGAR_MIN, Constants.INITIAL_SUGAR_MAX + 1);

				Agent a = new Agent(x, y, sug, life, met, vis, world);
				if (world.initialSpawnAgent(a)) {
					agents.Add(a);
				}
			}

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

				foreach (Agent a in agents) {
					a.updateOneStep();
					if (a.IsAlive) {
						agents2.Add(a);
					} else {
						world.removeAgent(a);
					}
				}
				shuffleAgents();

				frameCount = 0;
			}

            base.Update(gameTime);
        }

		/// <summary>
		/// Randomizes the order of agents
		/// </summary>
		private void shuffleAgents() {
			agents.Clear();
			
			//shuffle agents2
			Util.shuffleList(agents2);

			//swap arrays
			List<Agent> temp = agents;
			agents = agents2;
			agents2 = temp;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void addAgent(Agent newAgent) {
			agents2.Add(newAgent);
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
