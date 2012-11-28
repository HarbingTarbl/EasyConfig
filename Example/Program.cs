using System;
using System.Collections.Generic;
using EasyConfigLib.Storage;

namespace Example
{
	class ProgramConfig
		: EasyConfig
	{
		[Field("Value Int", "Main")]
		public int vint = 20;
		[Field("Value Float")]
		public float vfloat = 100.5f;
		[Field("Value String")]
		public string vstr = "Test";
		[Field("Value Bool")]
		public bool vbool = false;
		[Field]
		public int autonamedfield = 100;
		[Field]
		public Dictionary<string, float> list = new Dictionary<string, float>();


		public ProgramConfig(string filename)
			: base(filename)
		{
			
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var cfg = new ProgramConfig("../../cfg.cfg");
			cfg.vint = 100;
			cfg.WriteValues();
			cfg.Save();

			Console.WriteLine("List:{0}", cfg.list.Count);
		}
	}
}
