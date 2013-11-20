using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace sugarscape {
	class View {

		private SpriteBatch spritebatch;
		private Texture2D cellTexture;
		private Texture2D agentTexture;

        private float zoomLevel = 1.0f;
        private Vector2 cameraPosition = new Vector2();

		public View(GraphicsDevice gd) {
			spritebatch = new SpriteBatch(gd);
		}

		public void loadContent(ContentManager c) {
			cellTexture = c.Load<Texture2D>("square_bordered");
			agentTexture = c.Load<Texture2D>("square");
		}


		public void drawWorld(World w) {
            spritebatch.Begin();
			for (int i = 0; i < w.xSize; i++) {
				for (int j = 0; j < w.ySize; j++) {
					Vector2 cellPosition = new Vector2(i * (cellTexture.Width), j * (cellTexture.Height));
                    spritebatch.Draw(cellTexture, Vector2.Add(cameraPosition, cellPosition), Color.White);
				}
			}
            spritebatch.End();
		}

		public void drawAgent(Agent a) {
			spritebatch.Begin();
			spritebatch.Draw(agentTexture, Vector2.Add(cameraPosition, new Vector2(a.Posx, a.Posy)), Color.White);
			spritebatch.End();
		}
	}
}
