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
				case Constants.World_Gen_Mode.TWO_RIDGES:
					fillTwoRidges();
					break;
				case Constants.World_Gen_Mode.TWO_HILLS:
					fillTwoHills();
					break;
			}

		}

		public void fillRandom() {
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					cells[i, j].sugar = random.Next(Constants.WORLD_INITIAL_SUGAR_MIN, Constants.WORLD_INITIAL_SUGAR_MAX + 1);
					cells[i, j].maxSugar = cells[i, j].sugar;
					cells[i, j].a = null;
					cells[i, j].x = i;
					cells[i, j].y = j;
				}
			}
		}

		public void fillTwoRidges() {
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

		public void fillTwoHills() {
			//centers at 16/16 and 34/34
			//radii of 5-6
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					int dist;
					int dist1;
					int dist2;

					double xdif = Math.Abs(15 - i);
					double ydif = Math.Abs(15 - j);
					dist1 = (int)(Math.Sqrt(xdif * xdif + ydif * ydif));

					xdif = Math.Abs(35 - i);
					ydif = Math.Abs(35 - j);
					dist2 = (int)(Math.Sqrt(xdif * xdif + ydif * ydif));

					dist = Math.Min(dist1, dist2);

					if (dist < 6) {
						cells[i, j].sugar = 4;
					} else if (dist < 12) {
						cells[i, j].sugar = 3;
					} else if (dist < 18) {
						cells[i, j].sugar = 2;
					} else if (dist < 25) {
						cells[i, j].sugar = 1;
					} else {
						cells[i, j].sugar = 0;
					}
					
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
				case Constants.Growback_Rules.SEASONAL:
					growbackSeasonal();
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

		private void growbackSeasonal() {
			for (int i = 0; i < xSize; i++) {
				for (int j = 0; j < ySize; j++) {
					if (Math.Floor((double)gameEngine.totalFrames / (double)Constants.SEASON_LENGTH) % 2 == 0) {
						if (j > Constants.DEFAULT_WORLD_Y / 2) {
							if (cells[i, j].sugar < cells[i, j].maxSugar) {
								cells[i, j].sugar += 1;
							}
						} else if (gameEngine.totalFrames % Constants.SEASONAL_GROWBACK_PERIOD == 0) {
							if (cells[i, j].sugar < cells[i, j].maxSugar) {
								cells[i, j].sugar += 1;
							}							
						}
					} else {
						if (j > Constants.DEFAULT_WORLD_Y / 2) {
							if (gameEngine.totalFrames % Constants.SEASONAL_GROWBACK_PERIOD == 0) {
								if (cells[i, j].sugar < cells[i, j].maxSugar) {
									cells[i, j].sugar += 1;
								}
							}
						} else {
							if (cells[i, j].sugar < cells[i, j].maxSugar) {
								cells[i, j].sugar += 1;
							}
						}
					}

					
				}
			}
		}

		public cell seeCell(int x, int y) {
			return cells[(x + xSize) % xSize, (y + ySize) % ySize];
		}

		public bool inBounds(int x, int y) {
			return (x >= 0 && y >= 0 && x < this.xSize && y < this.ySize);
		}

		public void moveAgent(int oldx, int oldy, int newx, int newy, Agent a) {
			cells[(oldx + xSize) % xSize, (oldy + ySize) % ySize].a = null;

			cells[(newx + xSize) % xSize, (newy + ySize) % ySize].sugar = 0;
			cells[(newx + xSize) % xSize, (newy + ySize) % ySize].a = a;
		}

		public bool addAgent(Agent a) {
			if(cells[a.Posx, a.Posy].a == null) {
				cells[a.Posx, a.Posy].a = a;
				gameEngine.addAgent(a);
				return true;
			} else {
				return false;
			}
		}

		public bool initialSpawnAgent(Agent a) {
			if (cells[a.Posx, a.Posy].a == null) {
				cells[a.Posx, a.Posy].a = a;
				return true;
			} else {
				return false;
			}
		}

		public void removeAgent(Agent a) {
			cells[a.Posx, a.Posy].a = null;
		}
	}
}
