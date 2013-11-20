using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sugarscape {
	class World {

		private cell[,] cells;
		public int xSize;
		public int ySize;

		public World (int xSize, int ySize) {
			cells = new cell[xSize, ySize];
			this.xSize = xSize;
			this.ySize = ySize;
		}

		public struct cell {
			public int sugar;
			public int maxSugar;
			public bool hasAgent;

			public cell(int startingSugar, int sugarCapacity) {
				sugar = startingSugar;
				maxSugar = sugarCapacity;
				hasAgent = false;
			}
		}

		public void fillCells() {
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					cells[i, j].sugar = 1;
					cells[i, j].maxSugar = 5;
					cells[i, j].hasAgent = false;
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
			return cells[x, y];
		}
	}
}
