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

        private float zoomLevel = 0.5f;
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
					Vector2 cellPosition = new Vector2(i * cellTexture.Width, j * cellTexture.Height);
					cellPosition *= zoomLevel;
                    //spritebatch.Draw(cellTexture, Vector2.Add(cameraPosition, cellPosition), Color.Multiply(Color.White, (float) w.seeCell(i, j).sugar / 15.0f));
					spritebatch.Draw(cellTexture, Vector2.Add(cameraPosition, cellPosition), null, Color.Multiply(Color.White, (float)w.seeCell(i, j).sugar / 15.0f), 0.0f, new Vector2(), zoomLevel, SpriteEffects.None, 1.0f);
				}
			}
            spritebatch.End();
		}

		public void drawAgent(Agent a) {
			spritebatch.Begin();
			Vector2 agentPosition = new Vector2(a.Posx * cellTexture.Width, a.Posy * cellTexture.Height);
			agentPosition *= zoomLevel;
			//spritebatch.Draw(agentTexture, Vector2.Add(cameraPosition, new Vector2(a.Posx * cellTexture.Width, a.Posy * cellTexture.Height)), Color.Maroon);
			if (a.Color == Agent.Colors.RED) {
				spritebatch.Draw(agentTexture, Vector2.Add(cameraPosition, agentPosition), null, Color.Maroon, 0.0f, new Vector2(), zoomLevel, SpriteEffects.None, 1.0f);
			} else {
				spritebatch.Draw(agentTexture, Vector2.Add(cameraPosition, agentPosition), null, Color.Green, 0.0f, new Vector2(), zoomLevel, SpriteEffects.None, 1.0f);
			}
			spritebatch.End();
		}

		public enum Camera_Directions
		{
			RIGHT,
			LEFT,
			UP,
			DOWN
		}

		public void moveCamera(Camera_Directions dir) {
			switch (dir) {
				case Camera_Directions.UP:
					cameraPosition.Y += Constants.CAMERA_SPEED;
					break;
				case Camera_Directions.DOWN:
					cameraPosition.Y -= Constants.CAMERA_SPEED;
					break;
				case Camera_Directions.RIGHT:
					cameraPosition.X -= Constants.CAMERA_SPEED;
					break;
				case Camera_Directions.LEFT:
					cameraPosition.X += Constants.CAMERA_SPEED;
					break;
			}
		}

		public enum Zoom_Directions
		{
			IN,
			OUT
		}

		public void zoomCamera(Zoom_Directions dir) {
			switch (dir) {
				case Zoom_Directions.IN:
					zoomLevel += Constants.ZOOM_SPEED;
					break;
				case Zoom_Directions.OUT:
					zoomLevel -= Constants.ZOOM_SPEED;
					break;
			}
		}
	}
}
