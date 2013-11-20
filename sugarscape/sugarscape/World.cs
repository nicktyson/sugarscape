using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sugarscape {
	class World {

		private cell[,] cells;
		public int xSize;
		public int ySize;

		private Random random = new Random();

		public World (int xSize, int ySize) {
			cells = new cell[xSize, ySize];
			this.xSize = xSize;
			this.ySize = ySize;
		}

		public struct cell {
			public int sugar;
			public int maxSugar;
			public bool hasAgent;
			public int x;
			public int y;

			public cell(int startingSugar, int sugarCapacity, int x, int y) {
				sugar = startingSugar;
				maxSugar = sugarCapacity;
				hasAgent = false;
				this.x = x;
				this.y = y;
			}
		}

		public void fillCells() {
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					cells[i, j].sugar = random.Next(5);
					cells[i, j].maxSugar = 15;
					cells[i, j].hasAgent = false;
					cells[i, j].x = i;
					cells[i, j].y = j;
				}
			}
		}

		public void updateOneStep() {
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					if (cells[i, j].sugar < cells[i, j].maxSugar) {
						cells[i, j].sugar += 1;
					}
				}
			}
		}

		public cell seeCell(int x, int y) {
			return cells[(x + xSize) % xSize, (y + ySize) % ySize];
		}

		public void moveAgent(int oldx, int oldy, int newx, int newy) {
			cells[(oldx + xSize) % xSize, (oldy + ySize) % ySize].hasAgent = false;

			cells[(newx + xSize) % xSize, (newy + ySize) % ySize].sugar = 0;
			cells[(newx + xSize) % xSize, (newy + ySize) % ySize].hasAgent = true;
		}
	}
}
