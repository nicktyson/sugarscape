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

		private readonly int start_sugar;

		private int lifespan;
		private int metabolism;
		private int vision;

		private World world;

		private bool alive;

		private static Random r = new Random();

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
		}

		public enum Directions
		{
			NORTH,
			SOUTH,
			EAST,
			WEST
		}

		private static Directions[] dirs = new Directions[] { Directions.NORTH, Directions.SOUTH, Directions.EAST, Directions.WEST };

		public void updateOneStep() {
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
					World.cell c = world.seeCell(posx + dx*i, posy + dy*i);
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
			age++;

			world.moveAgent(posx, posy, newX, newY, this);
			posx = newX;
			posy = newY;

            sugar -= metabolism;

			
			
			if (Constants.DEATH_STARVATION) {
				if (sugar < 0) {
					die();
				}
			}

			if (sugar > start_sugar && alive) {
				reproduce();
			}

			if (Constants.DEATH_AGE) {
				if (age >= lifespan) {
					die();
				}
			}			
		}

		private Directions[] shuffleDirections() {
			for (int i = 3; i > 0; i--) {
				int j = r.Next(i + 1);
				Directions tmp = dirs[i];
				dirs[i] = dirs[j];
				dirs[j] = tmp;
			}
			return dirs;
		}

		private void reproduce() {
			List<Agent> partners = new List<Agent>();
			List<World.cell> places = new List<World.cell>();


			for(int i = -1; i < 2; i++) {
				for(int j = -1; j < 2; j++) {
					if (i == 0 && j == 0) {
						continue;
					}

					World.cell c = world.seeCell(posx + i, posy + j);

					if (c.hasAgent()) {
						if (c.a.isFertile()) {
							partners.Add(c.a);
						}
					} else {
						places.Add(c);
					}
				}
			}
			
			Util.shuffleList(partners);
			Util.shuffleList(places);

			foreach (Agent a in partners) {
				if (places.Count > 0) {
					World.cell site = places[0];
					places.RemoveAt(0);

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

					Agent child = new Agent(site.x, site.y, child_sugar, lifespan, child_met, child_vis, world);
					world.addAgent(child);
				}
			}
		}

		private void die() {
			alive = false;
		}

		public bool isFertile() {
			return (sugar >= start_sugar);
		}

		public int haveChild() {
			int endowment = sugar / 2;
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
	}
}
