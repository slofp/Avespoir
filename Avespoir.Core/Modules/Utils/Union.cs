using System;
using System.Diagnostics.CodeAnalysis;

namespace Avespoir.Core.Modules.Utils {

	/// <summary>
	/// 2つの型のうちどちらかを入れることができます
	/// </summary>
	/// <typeparam name="T1">1つ目の型</typeparam>
	/// <typeparam name="T2">2つ目の型</typeparam>
	struct Union<T1, T2> {

		/// <summary>
		/// どちらの方が含まれているかを識別するための列挙
		/// </summary>
		internal enum BoxedType {
			T1,
			T2
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
				CurrentType = BoxedType.T1;
			}
			else if (Value is T2 ValueT2) {
				Value1 = default;
				Value2 = ValueT2;
				IsNull = ValueT2 is null;
				CurrentType = BoxedType.T2;
			}
			else throw new ArgumentException(nameof(Value));
		}

		internal bool IsT1() => CurrentType == BoxedType.T1;

		internal bool IsT2() => CurrentType == BoxedType.T2;

		internal static Union<T1, T2> CreateNull() => new Union<T1, T2>(null);

		public static implicit operator Union<T1, T2>(T1 Value) => new Union<T1, T2>(Value);

		public static implicit operator Union<T1, T2>(T2 Value) => new Union<T1, T2>(Value);

		public static explicit operator T1(Union<T1, T2> Value) {
			if (!Value.IsNull || Value.CurrentType != BoxedType.T1) throw new NullReferenceException(nameof(T1));
			else return Value.Value1;
		}

		public static explicit operator T2(Union<T1, T2> Value) {
			if (!Value.IsNull || Value.CurrentType != BoxedType.T2) throw new NullReferenceException(nameof(T2));
			else return Value.Value2;
		}

		internal bool TryConvertT1([MaybeNullWhen(false)] out T1 Result) {
			Result = Value1;

			return !IsNull || CurrentType == BoxedType.T1;
		}

		internal bool TryConvertT2([MaybeNullWhen(false)] out T2 Result) {
			Result = Value2;

			return !IsNull || CurrentType == BoxedType.T2;
		}
	}
}
