using EasyConfig.Parsing;
using EasyConfig.Storage;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var parser = new TextConfigReader("../../cfg.cfg");
			var cfg = parser.Read();
			cfg.AddSection("Main");
			cfg["Main"]["Value Int"] = "10";
			cfg["Main"]["Value Float"] = 20.5f;
			cfg.SetSection("Main");
			cfg.Write("Value String", "String");
			cfg.Write("Value Bool", true);
			//parser.Read(cfg);
			parser.Save(cfg);
		}
	}
}
