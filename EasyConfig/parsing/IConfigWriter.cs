using EasyConfig.storage;

namespace EasyConfig.parsing
{
	interface IConfigWriter
	{
		bool Save(Config config);
	}
}
