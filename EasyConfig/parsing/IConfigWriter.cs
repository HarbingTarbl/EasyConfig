using EasyConfig.Storage;

namespace EasyConfig.Parsing
{
	interface IConfigWriter
	{
		bool Save(Config config);
	}
}
