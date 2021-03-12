using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Avespoir.Core.Language {


	internal class LanguageDictionary : IDictionary<Database.Enums.Language, string> {

		private readonly Dictionary<Database.Enums.Language, string> keyValuePairs = new Dictionary<Database.Enums.Language, string>();

		/// <summary>
		/// Dictionary for language.
		/// </summary>
		/// <remarks>Japanese must always be implemented.</remarks>
		/// <param name="Default_ja_JP">{0} is prefix+command string</param>
		internal LanguageDictionary(string Default_ja_JP) {
			keyValuePairs.Add(Database.Enums.Language.ja_JP, Default_ja_JP);
		}

		/// <summary>
		/// Gets or sets the element with the specified key.
		/// </summary>
		/// <remarks>If the key is not found, <see cref="Database.Enums.Language.ja_JP"/> will be selected automatically.</remarks>
		/// <param name="key">The key of the element to get or set.</param>
		/// <returns>The element with the specified key.</returns>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		/// <exception cref="NotSupportedException">The property is set and the <see cref="IDictionary{TKey, TValue}"/> is read-only.</exception>
		public string this[Database.Enums.Language key] {
			get {
				try {
					return keyValuePairs[key];
				}
				catch (KeyNotFoundException) {
					return keyValuePairs[Database.Enums.Language.ja_JP];
				}
			}
			set => keyValuePairs[key] = value;
		}

		public ICollection<Database.Enums.Language> Keys => keyValuePairs.Keys;

		public ICollection<string> Values => keyValuePairs.Values;

		public int Count => keyValuePairs.Count;

		public bool IsReadOnly => ((ICollection<KeyValuePair<Database.Enums.Language, string>>) keyValuePairs).IsReadOnly;

		/// <summary>
		/// Adds an element with the provided key and value to the <see cref="IDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <remarks>cannot add ja_JP</remarks>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">
		/// <para>The object to use as the value of the element to add.</para>
		/// {0} is prefix+command string
		/// </param>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		/// <exception cref="ArgumentException">An element with the same key already exists in the <see cref="IDictionary{TKey, TValue}"/>.</exception>
		/// <exception cref="NotSupportedException">The <see cref="IDictionary{TKey, TValue}"/> is read-only.</exception>
		public void Add(Database.Enums.Language key, string value) => keyValuePairs.Add(key, value);

		/// <summary>
		/// Adds an item to the <see cref="ICollection{T}"/>.
		/// </summary>
		/// <remarks>cannot add ja_JP</remarks>
		/// <param name="item">
		/// <para>The object to add to the <see cref="ICollection{T}"/>.</para>
		/// Value: {0} is prefix+command string
		/// </param>
		/// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
		public void Add(KeyValuePair<Database.Enums.Language, string> item) =>
			((ICollection<KeyValuePair<Database.Enums.Language, string>>) keyValuePairs).Add(item);

		/// <summary>
		/// Removes all items from the <see cref="ICollection{T}"/>.
		/// </summary>
		/// <remarks>cannot clear LanguageDictionary.</remarks>
		/// <exception cref="NotSupportedException">cannot clear LanguageDictionary.</exception>
		public void Clear() => throw new NotSupportedException("cannot clear LanguageDictionary.");

		public bool Contains(KeyValuePair<Database.Enums.Language, string> item) =>
			((ICollection<KeyValuePair<Database.Enums.Language, string>>) keyValuePairs).Contains(item);

		public bool ContainsKey(Database.Enums.Language key) => keyValuePairs.ContainsKey(key);

		public void CopyTo(KeyValuePair<Database.Enums.Language, string>[] array, int arrayIndex) =>
			((ICollection<KeyValuePair<Database.Enums.Language, string>>) keyValuePairs).CopyTo(array, arrayIndex);

		public IEnumerator<KeyValuePair<Database.Enums.Language, string>> GetEnumerator() => keyValuePairs.GetEnumerator();

		/// <summary>
		/// Removes the element with the specified key from the <see cref="IDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <remarks>cannot remove ja_JP</remarks>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		/// true if the element is successfully removed; otherwise, false. This method also
		/// returns false if key was not found in the original <see cref="IDictionary{TKey, TValue}"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		/// <exception cref="NotSupportedException">The <see cref="IDictionary{TKey, TValue}"/> is read-only.</exception>
		public bool Remove(Database.Enums.Language key) {
			if (key == Database.Enums.Language.ja_JP) return false;
			else return keyValuePairs.Remove(key);
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
		/// </summary>
		/// <remarks>cannot remove ja_JP</remarks>
		/// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
		/// <returns>
		/// true if item was successfully removed from the <see cref="ICollection{T}"/>;
		/// otherwise, false. This method also returns false if item is not found in the
		/// original <see cref="ICollection{T}"/>.
		/// </returns>
		/// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
		public bool Remove(KeyValuePair<Database.Enums.Language, string> item) {
			if (item.Key == Database.Enums.Language.ja_JP) return false;
			else return ((ICollection<KeyValuePair<Database.Enums.Language, string>>) keyValuePairs).Remove(item);
		}

		public bool TryGetValue(Database.Enums.Language key, [MaybeNullWhen(false)] out string value) =>
			keyValuePairs.TryGetValue(key, out value);

		IEnumerator IEnumerable.GetEnumerator() => keyValuePairs.GetEnumerator();
	}
}
