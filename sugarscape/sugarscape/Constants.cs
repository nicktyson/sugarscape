using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sugarscape {
	public static class Constants {

		//these are all defaults that should be changeable by the user

		public const int DEFAULT_WORLD_X = 25;
		public const int DEFAULT_WORLD_Y = 25;

		public const bool WORLD_LOOPS = false;

		public const int START_AGENTS_COUNT = 5;
		public const int MAX_AGENTS = 20;

		public const int START_FRAMES_PER_SIM_UPDATE = 60;

		public enum World_Gen_Mode
		{
			RANDOM,
			TWO_HILLS
		}

		public const World_Gen_Mode worldGenMode = World_Gen_Mode.TWO_HILLS;

		public enum Agent_Gen_Mode
		{
			RANDOM,
			HARDCODED
		}

		public const Agent_Gen_Mode agentGenMode = Agent_Gen_Mode.RANDOM;

		public enum Growback_Rules {
			INSTANT,
			STANDARD,
			SEASONAL
		}

		public const Growback_Rules growbackRule = Growback_Rules.STANDARD;

		public const int SEASON_LENGTH = 20;

		public const bool DEATH_AGE = true;
		public const bool DEATH_STARVATION = true;
		public const bool REPRODUCTION_ON = false;

		public const int MAX_AGE_MIN = 40;
		public const int MAX_AGE_MAX = 60;

		public const int FERTILITY_AGE = 15;

		public const int INITIAL_SUGAR_MIN = 20;
		public const int INITIAL_SUGAR_MAX = 30;

		//view / application constants
		public const float CAMERA_SPEED = 4.0f;
		public const float ZOOM_SPEED = 0.02f;
	}
}
