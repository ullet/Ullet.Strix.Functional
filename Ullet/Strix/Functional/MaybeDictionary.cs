/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional
{
  using System.Collections;
  using System.Collections.Generic;

  /// <summary>
  /// Dictionary of keys and values returning <see cref="T:Maybe{TValue}"/> from
  /// indexer instead of simply a <typeparamref name="TValue"/>.
  /// </summary>
  /// <remarks>
  /// Looking up a value for a key not in the dictionary returns
  /// <see cref="T:Maybe{TValue}"/> with no value rather than throw an
  /// exception.
  /// Does NOT implement interface <see cref="T:IDictionary{TKey, TValue}"/>.
  /// The dictionary interface expects exceptions to be thrown for missing and
  /// null keys, but this class intentionally breaks that convention.
  /// </remarks>
  public class MaybeDictionary<TKey, TValue>
    : IEnumerable<KeyValuePair<TKey, Maybe<TValue>>>
  {
    private readonly IDictionary<TKey, Maybe<TValue>> _innerDictionary;

    /// <summary>
    /// Create a new empty dictionary.
    /// </summary>
    public MaybeDictionary()
    {
      _innerDictionary = new Dictionary<TKey, Maybe<TValue>>();
    }

    /// <summary>
    /// Adds an element with the provided key and value to the dictionary.
    /// </summary>
    /// <param name="key">
    /// The object to use as the key of the element to add.
    /// </param>
    /// <param name="value">
    /// The object to use as the value of the element to add.
    /// </param>
    public bool Add(TKey key, TValue value)
    {
      if (Equals(key, null) || ContainsKey(key))
        return false;
      _innerDictionary.Add(key, value);
      return true;
    }

    /// <summary>
    /// Removes all items from the dictionary.
    /// </summary>
    public void Clear()
    {
      _innerDictionary.Clear();
    }

    /// <summary>
    /// Determines whether the dictionary contains an element with the specified
    /// key.
    /// </summary>
    /// <returns>
    /// true if the dictionary contains an element with the key;
    /// otherwise, false.
    /// </returns>
    /// <param name="key">The key to locate in the dictionary.</param>
    public bool ContainsKey(TKey key)
    {
      return !Equals(key, null) && _innerDictionary.ContainsKey(key);
    }

    /// <summary>
    /// Gets the number of elements contained in the dictionary.
    /// </summary>
    /// <returns>
    /// The number of elements contained in the dictionary.
    /// </returns>
    public int Count
    {
      get { return _innerDictionary.Count; }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<KeyValuePair<TKey, Maybe<TValue>>> GetEnumerator()
    {
      return _innerDictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    /// <summary>
    /// Gets a collection containing the keys of the dictionary.
    /// </summary>
    /// <returns>
    /// A collection containing the keys of the dictionary.
    /// </returns>
    public ICollection<TKey> Keys
    {
      get { return _innerDictionary.Keys; }
    }

    /// <summary>
    /// Removes the element with the specified key from the dictionary.
    /// </summary>
    /// <returns>
    /// true if the element is successfully removed; otherwise, false.
    /// This method also returns false if <paramref name="key"/> was not
    /// found in the original dictionary.
    /// </returns>
    /// <param name="key">The key of the element to remove.</param>
    public bool Remove(TKey key)
    {
      return !Equals(null, key) && _innerDictionary.Remove(key);
    }

    /// <summary>
    /// Gets or sets the element with the specified key.
    /// </summary>
    /// <returns>
    /// A <see cref="T:Maybe{TValue}"/>.  Will contain the value of the element
    /// with the specified key if the key is found, otherwise no value.
    /// </returns>
    /// <param name="key">The key of the element to get or set.</param>
    /// <remarks>
    /// Returns <see cref="T:Maybe{TValue}"/> with no value if key not found or
    /// key is null.
    /// Element will not be set if key is null, an exception will not be thrown.
    /// Element will not be set if set <see cref="T:Maybe{TValue}"/> has no
    /// value, an exception will not be thrown.
    /// </remarks>
    public Maybe<TValue> this[TKey key]
    {
      get
      {
        return ContainsKey(key)
          ? _innerDictionary[key]
          : Maybe.Nothing<TValue>();
      }
      set
      {
        if (!Equals(null, key))
          _innerDictionary[key] = value;
      }
    }

    /// <summary>
    /// Gets a collection containing the values in the dictionary.
    /// </summary>
    /// <returns>
    /// A collection containing the values in the dictionary.
    /// </returns>
    public ICollection<Maybe<TValue>> Values
    {
      get { return _innerDictionary.Values; }
    }
  }
}
