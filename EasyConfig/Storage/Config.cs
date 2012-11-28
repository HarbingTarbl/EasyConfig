using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyConfigLib.Parsing;

namespace EasyConfigLib.Storage
{
	public class Config
	{
		private readonly Dictionary<string, Section> _sections;
		private readonly TextConfigReader _serial;

		public Section CurrentSection { get; private set; }

		public IEnumerator<Section> Sections
		{
			get
			{
				return _sections.Values.GetEnumerator();
			}
		}

		public Section this[string name]
		{
			get
			{
				Section sec;
				_sections.TryGetValue(name, out sec);
				return sec;
			}
		}

		public Section AddSection(string name)
		{
			if (_sections.ContainsKey(name))
				return null;

			var sec = new Section(name);
			_sections.Add(name, sec);
			return sec;
		}

		public Section RemoveSection(string name)
		{
			Section sec;
			if(_sections.TryGetValue(name, out sec))
				_sections.Remove(name);
			return sec;
		}

		public Section SetSection(string name)
		{
			if(_sections.ContainsKey(name))
			{
				CurrentSection = _sections[name];
			}
			else
			{
				CurrentSection = AddSection(name);
			}
			
			return CurrentSection;
		}

		public bool Write<TData>(string name, TData data)
		{
			if (CurrentSection == null || data == null)
				return false;

			if(data is IList)
			{
				var str = new StringBuilder();

				foreach(var item in (IList)data)
				{
					str.AppendFormat("{0},", item);
				}

				CurrentSection[name] = str.ToString();
				return true;
			}

			if(data is IDictionary)
			{
				var dict = (IDictionary)data;
				var str = new StringBuilder();
				foreach(DictionaryEntry k in (IDictionary)data)
				{
					str.Append(string.Format("{0}:{1}", k.Key, k.Value));
				}
				CurrentSection[name] = string.Join(",", str);
				return true;
			}

			CurrentSection[name] = data.ToString();
			return true;
		}

		public bool Read(string name, out object data, Type dataType)
		{
			data = null;

			if (CurrentSection == null)
				return false;

			if (!CurrentSection.ContainsKey(name))
				return false;

			switch (Type.GetTypeCode(dataType))
			{
				case TypeCode.Int32:
					int intv;
					if (!int.TryParse(CurrentSection[name], out intv))
						return false;
					data = intv;
					break;
				case TypeCode.String:
					data = CurrentSection[name].Value;
					break;
				case TypeCode.Single:
					float fv;
					if (!float.TryParse(CurrentSection[name], out fv))
						return false;
					data = fv;
					break;
				case TypeCode.Boolean:
					bool bv;
					if (!bool.TryParse(CurrentSection[name], out bv))
						return false;
					data = bv;
					break;
				default:
					var genericTypes = dataType.GetGenericArguments();
					if(dataType.GetInterface(typeof(IList).Name) != null)
					{
						var list = (IList)Activator.CreateInstance(dataType);
						var elements = CurrentSection[name].Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select((x, y) => x.Trim()).ToList();
						for (var i = 0; i < elements.Count; i++)
						{
							list.Add(Convert.ChangeType(elements[i], genericTypes[0]));
						}
						data = list;
						return true;
					}

					if(dataType.GetInterface(typeof(IDictionary).Name) != null)
					{
						var dict = (IDictionary)Activator.CreateInstance(dataType);
						var pairs = CurrentSection[name].Value.Split(new [] {","}, StringSplitOptions.RemoveEmptyEntries).Select((x, y) => x.Trim()).ToList();
						for(var i = 0; i < pairs.Count; i++)
						{
							var elements = pairs[i].Split(new [] {":"}, 2, StringSplitOptions.RemoveEmptyEntries);
							dict.Add(Convert.ChangeType(elements[0], genericTypes[0]), Convert.ChangeType(elements[1], genericTypes[1]));
						}
						data = dict;
						return true;		
					}
					return false;
			}
			return true;
		}

		public bool Read<TAct>(string name, out TAct data)
		{
			data = default(TAct);
			object tmp;

			if (!Read(name, out tmp, typeof(TAct)))
				return false;

			data = (TAct)tmp;
			return true;
		}

		public Config(string filename)
		{
			_serial = new TextConfigReader(filename);
			_sections = new Dictionary<string, Section>();
		}

		public void Save()
		{
			_serial.Save(this);
		}

		public void Load()
		{
			_serial.Read(this);
		}

	}
}
