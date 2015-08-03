/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System.Collections.Generic;
using NUnit.Framework;

namespace Ullet.Strix.Functional.Tests.Unit
{
  [TestFixture]
  public class MaybeDictionaryTests
  {
    [Test]
    public void CanAddKeyAndValueToDictionary()
    {
      var dictionary =
        new MaybeDictionary<string, string> { { "key", "value" } };

      Assert.That((string)dictionary["key"], Is.EqualTo("value"));
    }

    [Test]
    public void CanAddMultipleKeysAndValuesToDictionary()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key1", "value1"},
          {"key2", "value2"},
          {"key3", "value3"}
        };

      Assert.That(
        new[]
          {
            (string)dictionary["key1"],
            (string)dictionary["key2"],
            (string)dictionary["key3"]
          },
        Is.EqualTo(new[] { "value1", "value2", "value3" }));
    }

    [Test]
    public void AddReturnsFalseIfKeyAlreadyAdded()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key", "value"}
        };

      var result = dictionary.Add("key", "other-value");

      Assert.That(result, Is.False);
    }

    [Test]
    public void DoesNotAddElementIfKeyAlreadyAdded()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key", "value"},
          {"key", "other-value"}
        };

      Assert.That(dictionary, Has.Count.EqualTo(1));
      Assert.That((string)dictionary["key"], Is.EqualTo("value"));
    }

    [Test]
    public void AddReturnsTrueIfSuccessfullyAdded()
    {
      var dictionary = new MaybeDictionary<string, string>();

      var result = dictionary.Add("key", "value");

      Assert.That(result, Is.True);
      Assert.That((string)dictionary["key"], Is.EqualTo("value"));
    }

    [Test]
    public void CanClearAllElements()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key", "value"}
        };

      dictionary.Clear();

      Assert.That(dictionary, Is.Empty);
    }

    [Test]
    public void CanTestContainsKeyWhenKeyExists()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key", "value"}
        };

      Assert.That(dictionary.ContainsKey("key"), Is.True);
    }

    [Test]
    public void CanTestContainsKeyWhenKeyDoesNotExist()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key", "value"}
        };

      Assert.That(dictionary.ContainsKey("other-key"), Is.False);
    }

    [Test]
    public void NeverContainsNullKey()
    {
      var dictionary = new MaybeDictionary<string, string>();

      Assert.That(dictionary.ContainsKey(null), Is.False);
    }

    [Test]
    public void CanCountNumberOfElements()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key1", "value1"},
          {"key2", "value2"},
          {"key3", "value3"}
        };

      Assert.That(dictionary.Count, Is.EqualTo(3));
    }

    [Test]
    public void HasEnumeratorOfKeyValuePairs()
    {
      var enumerator = new MaybeDictionary<string, string>
        {
          {"key1", "value1"},
          {"key2", "value2"},
          {"key3", "value3"}
        }.GetEnumerator();
      var enumerated = new List<KeyValuePair<string, string>>();

      while (enumerator.MoveNext())
        enumerated.Add(enumerator.Current);

      Assert.That(
        enumerated,
        Is.EquivalentTo(new[]
          {
            new KeyValuePair<string, string>("key1", "value1"),
            new KeyValuePair<string, string>("key2", "value2"),
            new KeyValuePair<string, string>("key3", "value3")
          }));
    }

    [Test]
    public void CanGetCollectionOfKeys()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key1", "value1"},
          {"key2", "value2"},
          {"key3", "value3"}
        };

      var keys = dictionary.Keys;

      Assert.That(keys, Is.EquivalentTo(new[] { "key1", "key2", "key3" }));
    }

    [Test]
    public void CanRemoveElementByItsKey()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key1", "value1"},
          {"key2", "value2"},
          {"key3", "value3"}
        };

      dictionary.Remove("key2");

      Assert.That(
        dictionary,
        Is.EquivalentTo(
          new Dictionary<string, string>
            {
              {"key1", "value1"},
              {"key3", "value3"}
            }));
    }

    [Test]
    public void RemoveReturnsTrueIfElementisRemoved()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key1", "value1"},
          {"key2", "value2"},
          {"key3", "value3"}
        };

      Assert.That(dictionary.Remove("key2"), Is.True);
    }

    [Test]
    public void RemoveReturnsFalseIfKeyNotFound()
    {
      var dictionary = new MaybeDictionary<string, string>();

      Assert.That(dictionary.Remove("key"), Is.False);
    }

    [Test]
    public void NothingRemovedIfKeyIsNull()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key1", "value1"},
          {"key2", "value2"},
          {"key3", "value3"}
        };

      dictionary.Remove(null);

      Assert.That(
        dictionary,
        Is.EquivalentTo(
          new Dictionary<string, string>
            {
              {"key1", "value1"},
              {"key2", "value2"},
              {"key3", "value3"}
            }));
    }

    [Test]
    public void IndexerAddsNewElementForNewKey()
    {
      var dictionary = new MaybeDictionary<string, string>();

      dictionary["key"] = "value";

      Assert.That((string)dictionary["key"], Is.EqualTo("value"));
    }

    [Test]
    public void IndexerOverwritesValueForExistingKey()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key", "value"}
        };

      dictionary["key"] = "other-value";

      Assert.That((string)dictionary["key"], Is.EqualTo("other-value"));
    }

    [Test]
    public void IndexerAddsNothingIfKeyIsNull()
    {
      var dictionary = new MaybeDictionary<string, string>();

      dictionary[null] = "value";

      Assert.That(dictionary, Is.Empty);
    }

    [Test]
    public void IndexerAddsNothingIfValueHasNoValue()
    {
      var dictionary = new MaybeDictionary<string, string>();

      dictionary["key"] = Fn.Nothing<string>();

      Assert.That(dictionary, Is.Empty);
    }

    [Test]
    public void IndexerDoesNotOverwriteIfValueHasNoValue()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key", "value"}
        };

      dictionary["key"] = Fn.Nothing<string>();

      Assert.That((string)dictionary["key"], Is.EqualTo("value"));
    }

    [Test]
    public void CanGetValueOfElementByItsKey()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key1", "value1"},
          {"key2", "value2"},
          {"key3", "value3"}
        };

      Maybe<string> value = dictionary["key2"];

      Assert.That((string)value, Is.EqualTo("value2"));
    }

    [Test]
    public void IndexerReturnsNoValueOfTypeIfKeyNotFound()
    {
      // ReSharper disable once CollectionNeverUpdated.Local
      var dictionary = new MaybeDictionary<string, string>();

      Maybe<string> maybe = dictionary["missing-key"];

      Assert.That(maybe.HasValue, Is.False);
    }

    [Test]
    public void IndexerReturnsNoValueOfAnotherTypeIfKeyNotFound()
    {
      // ReSharper disable once CollectionNeverUpdated.Local
      var dictionary = new MaybeDictionary<string, int>();

      Maybe<int> maybe = dictionary["missing-key"];

      Assert.That(maybe.HasValue, Is.False);
    }

    [Test]
    public void IndexerReturnsNoValueOfTypeIfKeyNull()
    {
      // ReSharper disable once CollectionNeverUpdated.Local
      var dictionary = new MaybeDictionary<string, int>();

      Maybe<int> maybe = dictionary[null];

      Assert.That(maybe.HasValue, Is.False);
    }

    [Test]
    public void CanGetCollectionOfValues()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key1", "value1"},
          {"key2", "value2"},
          {"key3", "value3"}
        };

      var values = dictionary.Values;

      Assert.That(
        values, Is.EquivalentTo(new[] {"value1", "value2", "value3"}));
    }
  }
}
