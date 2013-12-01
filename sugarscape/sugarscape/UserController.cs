using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace sugarscape {
	class UserController {

		private KeyboardState oldKS;
		private KeyboardState newKS;


		public void initialize() {
			oldKS = Keyboard.GetState();
			newKS = oldKS;
		}


		public void update(View v) {
			oldKS = newKS;
			newKS = Keyboard.GetState();

			//camera movement
			if (newKS.IsKeyDown(Keys.W)) {
				v.moveCamera(View.Camera_Directions.UP);
			}
			if (newKS.IsKeyDown(Keys.S)) {
				v.moveCamera(View.Camera_Directions.DOWN);
			}
			if (newKS.IsKeyDown(Keys.A)) {
				v.moveCamera(View.Camera_Directions.LEFT);
			}
			if (newKS.IsKeyDown(Keys.D)) {
				v.moveCamera(View.Camera_Directions.RIGHT);
			}
			if (newKS.IsKeyDown(Keys.Q)) {
				v.zoomCamera(View.Zoom_Directions.IN);
			}
			if (newKS.IsKeyDown(Keys.E)) {
				v.zoomCamera(View.Zoom_Directions.OUT);
			}
		}
	}
}
