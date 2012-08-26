using System.Collections.Generic;

namespace EasyConfig.storage
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
		private Config _parent;
		private readonly Dictionary<string, string> _values; 

		public Section(string name, Config parent)
		{
			_name = name;
			_parent = parent;
			_values = new Dictionary<string, string>();
		}

		public IEnumerator<KeyValuePair<string, string>> Values
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

		public string this[string key]
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
