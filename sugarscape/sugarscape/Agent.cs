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

		public void updateOneStep() {
			int[] directions = shuffleDirections();

			world.seeCell(posx, posy);
		}

		private int[] shuffleDirections() {
			return (new int[] {0, 1, 2, 3});
		}
	}
}
