﻿/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional.Tests.Unit
{
  using System.Collections.Generic;
  using System.Linq;
  using NUnit.Framework;

  [TestFixture]
  public class MaybeDictionaryTests
  {
    [Test]
    public void CanAddKeyAndValueToDictionary()
    {
      var dictionary =
        new MaybeDictionary<string, string> { { "key", "value" } };

      Assert.That(dictionary["key"].GetOrElse(""), Is.EqualTo("value"));
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
            dictionary["key1"].GetOrElse(""),
            dictionary["key2"].GetOrElse(""),
            dictionary["key3"].GetOrElse("")
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
      Assert.That(dictionary["key"].GetOrElse(""), Is.EqualTo("value"));
    }

    [Test]
    public void AddReturnsTrueIfSuccessfullyAdded()
    {
      var dictionary = new MaybeDictionary<string, string>();

      var result = dictionary.Add("key", "value");

      Assert.That(result, Is.True);
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
      var enumerated = new List<KeyValuePair<string, Maybe<string>>>();

      while (enumerator.MoveNext())
        enumerated.Add(enumerator.Current);

      Assert.That(
        enumerated,
        Is.EquivalentTo(new[]
          {
            new KeyValuePair<string, Maybe<string>>("key1", "value1"),
            new KeyValuePair<string, Maybe<string>>("key2", "value2"),
            new KeyValuePair<string, Maybe<string>>("key3", "value3")
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
        dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.GetOrElse("")),
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
        dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.GetOrElse("")),
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

      Assert.That(dictionary["key"].GetOrElse(""), Is.EqualTo("value"));
    }

    [Test]
    public void IndexerOverwritesValueForExistingKey()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key", "value"}
        };

      dictionary["key"] = "other-value";

      Assert.That(dictionary["key"].GetOrElse(""), Is.EqualTo("other-value"));
    }

    [Test]
    public void IndexerAddsNothingIfKeyIsNull()
    {
      var dictionary = new MaybeDictionary<string, string>();

      dictionary[null] = "value";

      Assert.That(dictionary, Is.Empty);
    }

    [Test]
    public void IndexerAddsMaybeNothing()
    {
      var dictionary = new MaybeDictionary<string, string>();

      dictionary["key"] = Maybe.Nothing<string>();

      Assert.That(dictionary["key"].IsNothing, Is.True);
    }

    [Test]
    public void IndexerOverwritesIfSetToNothing()
    {
      var dictionary = new MaybeDictionary<string, string>
        {
          {"key", "value"}
        };

      dictionary["key"] = Maybe.Nothing<string>();

      Assert.That(dictionary["key"].IsNothing, Is.True);
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

      Assert.That(value.GetOrElse(""), Is.EqualTo("value2"));
    }

    [Test]
    public void IndexerReturnsNothingIfKeyNotFound()
    {
      var dictionary = new MaybeDictionary<string, string>();

      Maybe<string> maybe = dictionary["missing-key"];

      Assert.That(maybe.IsNothing, Is.True);
    }

    [Test]
    public void IndexerReturnsNothingIfKeyNull()
    {
      var dictionary = new MaybeDictionary<string, int>();

      Maybe<int> maybe = dictionary[null];

      Assert.That(maybe.IsNothing, Is.True);
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
        values, Is.EquivalentTo(
        new[]
        {
          Maybe.Just("value1"),
          Maybe.Just("value2"),
          Maybe.Just("value3")
        }));
    }
  }
}
