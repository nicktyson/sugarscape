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

		private int lifespan;
		private int metabolism;
		private int sight;

		private World world;

		private static Random r = new Random();

		public Agent() {

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

			foreach (Directions d in directions) {
				switch (d) {
					case Directions.NORTH:
						break;
					case Directions.SOUTH:
						break;
					case Directions.EAST:
						break;
					case Directions.WEST:
						world.seeCell(posx, posy);
						break;
				}
			}

            sugar -= metabolism;
			if (sugar < 0) {

			}
		}

		private Directions[] shuffleDirections() {
			return (new Directions[] {Directions.NORTH, Directions.SOUTH, Directions.EAST, Directions.WEST});
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
