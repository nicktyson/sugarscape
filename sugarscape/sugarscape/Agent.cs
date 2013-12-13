using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace sugarscape {
	class Agent {
		private int posx;
		private int posy;
		private int age;
		private int sugar;

		private readonly int start_sugar;

		private int lifespan;
		private int metabolism;
		private int sight;

		private World world;

		private static Random r = new Random();

		public Agent(int x, int y, int sugar, int lifespan, int metabolism, int vision, World world) {
			posx = x;
			posy = y;
			this.sugar = sugar;
			this.start_sugar = sugar;
			this.lifespan = lifespan;
			this.metabolism = metabolism;
			sight = vision;
			this.world = world;
		}

		public enum Directions
		{
			NORTH,
			SOUTH,
			EAST,
			WEST
		}

		public void updateOneStep() {
			Directions[] directions = shuffleDirections();

			int newX = posx;
			int newY = posy;
			int destSugar = 0;

			foreach (Directions d in directions) {
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

				for (int i = 1; i <= sight; i++) {
					World.cell c = world.seeCell(posx + dx, posy + dy);
					if (c.sugar > destSugar && c.hasAgent() == false) {
						destSugar = c.sugar;
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
			if (sugar < 0 || age > lifespan) {
				die();
			}

			if (sugar > start_sugar) {
				reproduce();
			}
		}

		private Directions[] shuffleDirections() {
			return (new Directions[] {Directions.NORTH, Directions.SOUTH, Directions.EAST, Directions.WEST});
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
					
					int child_sugar = this.haveChild() + a.haveChild();
					
				}
			}
		}

		private void die() {

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
	}
}
