using UnityEngine;

namespace FlatLighting {
	public class RangeWithStepAttribute : PropertyAttribute {

		public readonly int min;
		public readonly int max;
		public readonly int step;

		public RangeWithStepAttribute(int min, int max, int step) {
			this.min = min;
			this.max = max;
			this.step = step;
		}

		public RangeWithStepAttribute(int min, int max) {
			this.step = 1;
			this.min = min;
			this.max = max;
		}
	}
}
