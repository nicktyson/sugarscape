using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sugarscape {

	class Util {
		private static Random r = new Random();

		public static void shuffleList<T>(IList<T> lst) {
			for (int i = lst.Count - 1; i > 0; i--) {
				int j = r.Next(i + 1);
				T tmp = lst[i];
				lst[i] = lst[j];
				lst[j] = tmp;
			}
		}
	}
}
