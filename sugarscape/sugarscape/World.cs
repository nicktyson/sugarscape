using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sugarscape {
	public class World {

		private cell[,] cells;
		public int xSize;
		public int ySize;

		private Game1 gameEngine;

		private Random random = new Random();

		public World (int xSize, int ySize, Game1 gameEngine) {
			cells = new cell[xSize, ySize];
			this.xSize = xSize;
			this.ySize = ySize;
			this.gameEngine = gameEngine;
		}

		public struct cell {
			public int sugar;
			public int maxSugar;
			public Agent a;
			public int x;
			public int y;

			public cell(int startingSugar, int sugarCapacity, int x, int y) {
				sugar = startingSugar;
				maxSugar = sugarCapacity;
				a = null;
				this.x = x;
				this.y = y;
			}

			public bool hasAgent() {
				return (a != null);
			}
		}

		/// <summary>
		/// Initialize the status of each cell
		/// </summary>
		public void fillCells() {
			switch (Constants.worldGenMode) {
				case Constants.World_Gen_Mode.RANDOM:
					fillRandom();
					break;
				case Constants.World_Gen_Mode.TWO_HILLS:
					fillTwoHills();
					break;
			}

		}

		public void fillRandom() {
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					cells[i, j].sugar = random.Next(5);
					cells[i, j].maxSugar = cells[i, j].sugar;
					cells[i, j].a = null;
					cells[i, j].x = i;
					cells[i, j].y = j;
				}
			}
		}

		public void fillTwoHills() {
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					cells[i, j].sugar = Math.Abs(i - xSize / 2) / 2;
					cells[i, j].maxSugar = cells[i, j].sugar;
					cells[i, j].a = null;
					cells[i, j].x = i;
					cells[i, j].y = j;
				}
			}
		}

		public void updateOneStep() {
			switch (Constants.growbackRule) {
				case Constants.Growback_Rules.INSTANT:
					growbackInstant();
					break;
				case Constants.Growback_Rules.STANDARD:
					growbackStandard();
					break;
			}
		}

		private void growbackStandard() {
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					if (cells[i, j].sugar < cells[i, j].maxSugar) {
						cells[i, j].sugar += 1;
					}
				}
			}
		}

		private void growbackInstant() {
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					cells[i, j].sugar = cells[i, j].maxSugar;
				}
			}
		}

		public cell seeCell(int x, int y) {
			return cells[(x + xSize) % xSize, (y + ySize) % ySize];
		}

		public void moveAgent(int oldx, int oldy, int newx, int newy, Agent a) {
			cells[(oldx + xSize) % xSize, (oldy + ySize) % ySize].a = null;

			cells[(newx + xSize) % xSize, (newy + ySize) % ySize].sugar = 0;
			cells[(newx + xSize) % xSize, (newy + ySize) % ySize].a = a;
		}

		public void addAgent(Agent a) {
			if(cells[a.Posx, a.Posy].a == null) {
				cells[a.Posx, a.Posy].a = a;
				gameEngine.addAgent(a);
			} else {
				
			}
		}

		public void removeAgent(Agent a) {
			cells[a.Posx, a.Posy].a = null;
		}
	}
}
