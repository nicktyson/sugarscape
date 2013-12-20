using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sugarscape {
	public static class Constants {

		//these are all defaults that could be changeable by the user

		public const int DEFAULT_WORLD_X = 50;
		public const int DEFAULT_WORLD_Y = 50;

		//true makes the world a torus
		public const bool WORLD_LOOPS = false;

		public const int START_AGENTS_COUNT = 100;
		
		//basically never used
		public const int MAX_AGENTS = 20;

		//only some generation modes use these
		public const int WORLD_INITIAL_SUGAR_MIN = 0;
		public const int WORLD_INITIAL_SUGAR_MAX = 4;

		//decrease to run faster
		public const int START_FRAMES_PER_SIM_UPDATE = 10;

		public enum World_Gen_Mode
		{
			RANDOM,
			TWO_RIDGES,
			TWO_HILLS
		}

		public const World_Gen_Mode worldGenMode = World_Gen_Mode.TWO_HILLS;

		public enum Agent_Gen_Mode
		{
			RANDOM,
			HARDCODED,
			SQUARES
		}

		public const Agent_Gen_Mode agentGenMode = Agent_Gen_Mode.RANDOM;

		public enum Growback_Rules {
			INSTANT,
			STANDARD,
			SEASONAL
		}

		public const Growback_Rules growbackRule = Growback_Rules.SEASONAL;

		public const int SEASON_LENGTH = 100;
		public const int SEASONAL_GROWBACK_PERIOD = 6;

		public const bool DEATH_AGE = true;
		public const bool DEATH_STARVATION = true;
		public const bool REPRODUCTION_ON = true;

		public const int MET_MIN = 1;
		public const int MET_MAX = 4;

		public const int VISION_MIN = 1;
		public const int VISION_MAX = 6;

		public const int MAX_AGE_MIN = 60;
		public const int MAX_AGE_MAX = 100;

		public const int FERTILITY_AGE = 15;

		public const int INITIAL_SUGAR_MIN = 50;
		public const int INITIAL_SUGAR_MAX = 100;

		public const bool CULTURE_ON = true;
		//should probably be odd
		public const int CULTURAL_TAG_LENGTH = 11;

		//view / application constants
		public const float CAMERA_SPEED = 4.0f;
		public const float ZOOM_SPEED = 0.02f;

		public enum View_Modes
		{
			NONE,
			CULTURE,
			METABOLISM,
			VISION,
			AGE
		}
		public const View_Modes viewMode = View_Modes.NONE;
	}
}
