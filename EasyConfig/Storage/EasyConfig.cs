using System;
using System.Reflection;

namespace EasyConfigLib.Storage
{
	public abstract class EasyConfig
		: Config
	{
		private readonly FieldInfo[] fields;

		protected EasyConfig(string filename)
			: base(filename)
		{
			Load();
			fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
			Array.Sort(fields, (f, s) => f.FieldHandle.Value.ToInt64().CompareTo(s.FieldHandle.Value.ToInt64()));
			ReadValues();
			Save();
		}

		public void ReadValues()
		{
			var t = GetType();
			var lastSection = "";
			foreach (var field in fields)
			{
				var attrs = field.GetCustomAttributes(typeof (FieldAttribute), false);
				if (attrs.Length == 0) continue;

				var attr = (FieldAttribute) attrs[0];
				var fieldName = attr.Name == "" ? field.Name : attr.Name;

				if (attr.Section != ""
				    && attr.Section != lastSection)
				{
					lastSection = attr.Section;
					SetSection(lastSection);
				}

				object value;
				if (!Read(fieldName, out value, field.FieldType))
				{
					value = field.GetValue(this);
					Write(fieldName, value);
				}

				field.SetValue(this, value);
			}
		}

		public void WriteValues()
		{
			var t = GetType();
			var lastSection = "";
			foreach(var field in fields)
			{
				var attrs = field.GetCustomAttributes(typeof(FieldAttribute), false);
				if (attrs.Length == 0) continue;

				var attr = (FieldAttribute)attrs[0];
				var fieldName = attr.Name == "" ? field.Name : attr.Name;
				var fieldValue = field.GetValue(this);

				if(attr.Section != "" &&
					attr.Section != lastSection)
				{
					lastSection = attr.Section;
					SetSection(lastSection);
				}

				Write(fieldName, fieldValue);
			}
		}



		[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
		protected class FieldAttribute
			: Attribute
		{

			public FieldAttribute(string name = "", string section = "")
			{

				Name = name;
				Section = section;
			}

			public string Section;
			public string Name;
		}
	}
}
