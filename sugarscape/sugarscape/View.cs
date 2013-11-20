using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace sugarscape {
	class View {

		private SpriteBatch sb;
		private Texture2D cellTexture;


		public View(GraphicsDevice gd) {
			sb = new SpriteBatch(gd);
		}

		public void loadContent(ContentManager c) {
			cellTexture = c.Load<Texture2D>("square");
		}


		public static void drawWorld(World w) {
			for (int i = 0; i < w.xSize; i++) {
				for (int j = 0; j < w.ySize; j++) {
					
				}
			}
		}

		public static void drawAgent(Agent a) {
			
		}
	}
}
