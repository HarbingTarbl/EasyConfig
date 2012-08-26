using System.Collections.Generic;

namespace EasyConfigLib.Storage
{
	public class Section
	{
		public string Name
		{
			get
			{
				return _name;
			}
		}

		private readonly string _name;
		private readonly Dictionary<string, AValue> _values; 

		public Section(string name)
		{
			_name = name;
			_values = new Dictionary<string, AValue>();
		}

		public IEnumerator<KeyValuePair<string, AValue>> Values
		{
			get
			{
				return _values.GetEnumerator();
			}
		}

		public bool AddKey(string key, string value)
		{
			if (_values.ContainsKey(key))
				return false;

			_values.Add(value, key);
			return true;
		}

		public bool RemoveKey(string key)
		{
			if (!_values.ContainsKey(key))
				return false;

			_values.Remove(key);

			return true;
		}

		public bool ContainsKey(string key)
		{
			return _values.ContainsKey(key);
		}

		public AValue this[string key]
		{
			get
			{
				return _values[key];
			}

			set
			{
				_values[key] = value;
			}
		}
	}
}
