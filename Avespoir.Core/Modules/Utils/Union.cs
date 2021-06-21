using System;
using System.Collections.Generic;
using System.Text;

namespace Avespoir.Core.Modules.Utils {

	struct Union<T1, T2> {

		internal enum BoxedType {
			T1,
			T2,
			OtherOrNull
		}

		T1 Value1 { get; }

		T2 Value2 { get; }

		internal BoxedType CurrentType { get; }

		internal bool IsNull { get; }

		internal Union(object Value) {
			if (Value is T1 ValueT1) {
				Value1 = ValueT1;
				Value2 = default;
				IsNull = ValueT1 is null;
				CurrentType = IsNull ? BoxedType.OtherOrNull : BoxedType.T1;
			}
			else if (Value is T2 ValueT2) {
				Value1 = default;
				Value2 = ValueT2;
				IsNull = ValueT2 is null;
				CurrentType = IsNull ? BoxedType.OtherOrNull : BoxedType.T2;
			}
			else {
				Value1 = default;
				Value2 = default;
				CurrentType = BoxedType.OtherOrNull;
				IsNull = true;
			}
		}

		internal static Union<T1, T2> CreateNull() => new Union<T1, T2>(null);

		public static implicit operator Union<T1, T2>(T1 Value) => new Union<T1, T2>(Value);

		public static implicit operator Union<T1, T2>(T2 Value) => new Union<T1, T2>(Value);

		public static implicit operator T1(Union<T1, T2> Value) {
			if (!Value || Value.CurrentType != BoxedType.T1) return default;
			else return Value.Value1;
		}

		public static implicit operator T2(Union<T1, T2> Value) {
			if (!Value || Value.CurrentType != BoxedType.T2) return default;
			else return Value.Value2;
		}

		public static implicit operator bool(Union<T1, T2> Value) => !Value.IsNull;
	}
}
