using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace sugarscape {
	public class Agent {
		private int posx;
		private int posy;
		private int age;
		private int sugar;
		private byte[] culture;

		private readonly int start_sugar;
		private readonly int lifespan;
		private readonly int metabolism;
		private readonly int vision;

		private List<Agent> neighbors = new List<Agent>();
		private List<World.cell> emptyNeighbors = new List<World.cell>();

		private World world;

		private bool alive;

		private static Random r = new Random();

		public enum Colors
		{
			RED,
			BLUE
		}
		private Colors color;

		public enum Directions
		{
			NORTH,
			SOUTH,
			EAST,
			WEST
		}
		private static Directions[] dirs = new Directions[] { Directions.NORTH, Directions.SOUTH, Directions.EAST, Directions.WEST };


		public Agent(int x, int y, int sugar, int lifespan, int metabolism, int vision, World world) {
			posx = x;
			posy = y;
			this.sugar = sugar;
			this.start_sugar = sugar;
			this.lifespan = lifespan;
			this.metabolism = metabolism;
			this.vision = vision;
			this.world = world;
			alive = true;
			age = 0;

			culture = new byte[Constants.CULTURAL_TAG_LENGTH];
			for (int i = 0; i < Constants.CULTURAL_TAG_LENGTH; i++) {
				culture[i] = (byte) r.Next(2);
			}
			calculateColor();
		}

		public Agent(int x, int y, int sugar, int lifespan, int metabolism, int vision, World world, byte[] culture) {
			posx = x;
			posy = y;
			this.sugar = sugar;
			//if (sugar <= Constants.INITIAL_SUGAR_MAX) {
				this.start_sugar = sugar;
			//} else {
			//	this.start_sugar = Constants.INITIAL_SUGAR_MAX;
			//}
			this.lifespan = lifespan;
			this.metabolism = metabolism;
			this.vision = vision;
			this.world = world;
			alive = true;
			age = 0;

			this.culture = culture;
			calculateColor();
		}

		public void updateOneStep() {
			calculateColor();

			move();

			age++;

			sugar -= metabolism;

			updateNeighbors();

			//if you ran out of sugar, die
			if (Constants.DEATH_STARVATION) {
				if (sugar < 0) {
					die();
				}
			}

			if (Constants.CULTURE_ON) {
				influenceNeighbors();
			}

			//if you have enough sugar after moving, try to reproduce
			if (sugar > start_sugar && alive && Constants.REPRODUCTION_ON) {
				reproduce();
			}

			//if you are too old, die
			if (Constants.DEATH_AGE) {
				if (age >= lifespan) {
					die();
				}
			}			
		}

		private void move() {
			shuffleDirections();

			int newX = posx;
			int newY = posy;
			int destSugar = world.seeCell(posx, posy).sugar;
			int destDist = 0;

			//look each direction
			foreach (Directions d in dirs) {
				int dx = 0;
				int dy = 0;

				switch (d) {
					case Directions.NORTH:
						dy = -1;
						break;
					case Directions.SOUTH:
						dy = 1;
						break;
					case Directions.EAST:
						dx = 1;
						break;
					case Directions.WEST:
						dx = -1;
						break;
				}

				for (int i = 1; i <= vision; i++) {
					if (!Constants.WORLD_LOOPS && !world.inBounds(posx + dx * i, posy + dy * i)) {
						continue;
					}

					World.cell c = world.seeCell(posx + dx * i, posy + dy * i);
					if ((c.sugar > destSugar && c.hasAgent() == false) ||
							(c.sugar == destSugar && c.hasAgent() == false && i < destDist)) {
						destSugar = c.sugar;
						destDist = i;
						newX = c.x;
						newY = c.y;
					}
				}
			}

			sugar += destSugar;

			world.moveAgent(posx, posy, newX, newY, this);
			posx = newX;
			posy = newY;
		}

		private void shuffleDirections() {
			Util.shuffleList(dirs);
		}

		public void updateNeighbors() {
			neighbors.Clear();

			int i = 0;
			int j = 1;
			checkSpot(i, j);
			i = 0;
			j = -1;
			checkSpot(i, j);
			i = 1;
			j = 0;
			checkSpot(i, j);
			i = -1;
			j = 0;
			checkSpot(i, j);

			Util.shuffleList(neighbors);
		}

		public void updateEmptyNeighbors() {
			emptyNeighbors.Clear();

			int i = 0;
			int j = 1;
			checkSpotEmpty(i, j);
			i = 0;
			j = -1;
			checkSpotEmpty(i, j);
			i = 1;
			j = 0;
			checkSpotEmpty(i, j);
			i = -1;
			j = 0;
			checkSpotEmpty(i, j);
		}

		private void checkSpot(int i, int j) {
			if (Constants.WORLD_LOOPS || world.inBounds(posx + i, posy + j)) {
				World.cell c = world.seeCell(posx + i, posy + j);

				if (c.hasAgent()) {
					neighbors.Add(c.a);
				} else {
					emptyNeighbors.Add(c);
				}
			}
		}

		private void checkSpotEmpty(int i, int j) {
			if (Constants.WORLD_LOOPS || world.inBounds(posx + i, posy + j)) {
				World.cell c = world.seeCell(posx + i, posy + j);

				if (!c.hasAgent()) {
					emptyNeighbors.Add(c);
				}
			}
		}

		private void reproduce() {
			Util.shuffleList(neighbors);

			foreach (Agent a in neighbors) {
				if (!a.isFertile()) {
					continue;
				}

				updateEmptyNeighbors();
				List<World.cell> birthplaces = new List<World.cell>(emptyNeighbors);
				a.updateEmptyNeighbors();
				birthplaces.AddRange(a.EmptyNeighbors);
				Util.shuffleList(birthplaces);

				if (birthplaces.Count > 0) {
					World.cell site = birthplaces[0];
					//birthplaces.RemoveAt(0);

					int child_sugar = this.haveChild() + a.haveChild();
					int child_vis;
					int child_met;

					if (r.Next(2) == 0) {
						child_vis = this.vision;
					} else {
						child_vis = a.vision;
					}

					if (r.Next(2) == 0) {
						child_met = this.metabolism;
					} else {
						child_met = a.metabolism;
					}

					byte[] kidCulture = new byte[Constants.CULTURAL_TAG_LENGTH];
					byte[] spouseCulture = a.Culture;
					for (int i = 0; i < spouseCulture.Length; i++) {
						if (spouseCulture[i] == culture[i]) {
							kidCulture[i] = culture[i];
						} else {
							if (r.Next(2) == 0) {
								kidCulture[i] = culture[i];
							} else {
								kidCulture[i] = spouseCulture[i];
							}
						}
					}

					Agent child = new Agent(site.x, site.y, child_sugar, lifespan, child_met, child_vis, world, kidCulture);
					world.addAgent(child);
				}
			}
		}

		private void influenceNeighbors() {
			foreach (Agent a in neighbors) {
				int tagPos = r.Next(Constants.CULTURAL_TAG_LENGTH);
				a.setCultureTag(tagPos, this.culture[tagPos]);
				a.calculateColor();
			}
		}

		public void calculateColor() {
			int zeros = 0;
			int ones = 0;
			foreach (byte tag in culture) {
				if (tag == 0) {
					zeros++;
				} else if (tag == 1) {
					ones++;
				}
			}
			if (zeros > ones) {
				color = Colors.RED;
			} else {
				color = Colors.BLUE;
			}
		}

		private void die() {
			alive = false;
		}

		public bool isFertile() {
			return (sugar >= start_sugar && age > Constants.FERTILITY_AGE);
		}

		public int haveChild() {
			int endowment = start_sugar / 2;
			sugar = sugar - endowment;
			return endowment;
		}

		public int Posx {
			get {
				return posx;
			}
			set {
				posx = value;
			}
		}

		public int Posy {
			get {
				return posy;
			}
			set {
				posy = value;
			}
		}

		public bool IsAlive {
			get {
				return alive;
			}
		}

		public List<World.cell> EmptyNeighbors {
			get {
				return emptyNeighbors;
			}
		}

		public byte[] Culture {
			get {
				return culture;
			}
		}

		public void setCultureTag(int index, byte val) {
			if (index >= culture.Length) {
				return;
			}

			culture[index] = val;
		}

		public Colors Color {
			get {
				return color;
			}
		}

		public int Metabolism {
			get {
				return metabolism;
			}
		}

		public int Vision {
			get {
				return vision;
			}
		}

		public int Age {
			get {
				return age;
			}
		}

		public int LifeSpan {
			get {
				return lifespan;
			}
		}
	}
}
