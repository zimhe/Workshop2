using UnityEngine;
using System.Collections;

namespace FlatLighting {

	public class ValueWrapper<T> {

		public T Value { get; set; }

		public ValueWrapper() { }

		public ValueWrapper(T value) { 
			this.Value = value; 
		}

		public static implicit operator T(ValueWrapper<T> wrapper) {
			if (wrapper == null) {
				return default(T);
			}
			return wrapper.Value;
		}

		public static implicit operator ValueWrapper<T>(T value) {
			return new ValueWrapper<T>(value);
		}
	}
}
