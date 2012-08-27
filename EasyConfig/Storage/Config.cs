using System;
using System.Collections.Generic;
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
			if (CurrentSection == null)
				return false;

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
