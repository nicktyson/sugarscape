﻿using System;
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
			culture = new byte[Constants.CULTURAL_TAG_LENGTH];
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
			if (sugar > start_sugar && alive) {
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

					Agent child = new Agent(site.x, site.y, child_sugar, lifespan, child_met, child_vis, world);
					world.addAgent(child);
				}
			}
		}

		private void influenceNeighbors() {

		}

		private void die() {
			alive = false;
		}

		public bool isFertile() {
			return (sugar >= start_sugar && age > Constants.FERTILITY_AGE);
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

		public List<World.cell> EmptyNeighbors {
			get {
				return emptyNeighbors;
			}
		}
	}
}
