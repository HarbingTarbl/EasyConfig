using System;
using System.IO;
using System.Linq;
using EasyConfigLib.Storage;

namespace EasyConfigLib.Parsing
{
	public class TextConfigReader
	{
		public readonly string Filename;
		/**TODO
		public string ValueDelim;
		public string SectionDelim;
		public string PairDelim;
		 */

		public bool Save(Config c)
		{
			try
			{
				using(var writer = new StreamWriter(Filename, false))
				{
					for (var e = c.Sections; e.MoveNext();)
					{
						writer.WriteLine("[{0}]", e.Current.Name);
						for(var v = e.Current.Values; v.MoveNext();)
						{
							writer.WriteLine("{0} = {1}", v.Current.Key, v.Current.Value.Value);
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}

			return true;
		}

		public void Read(Config cfg)
		{
			try
			{
				using (var stream = new StreamReader(Filename))
				{
					while (!stream.EndOfStream)
					{
						var line = stream.ReadLine();
						if (line == null)
							continue;

						line = line.Trim();

						if(!line.Contains("="))
						{
							var secstr = line.Substring(line.IndexOf('[') + 1, line.LastIndexOf(']') - 1);
							cfg.AddSection(secstr);
							cfg.SetSection(secstr);
						}
						else
						{
							var args = line.Split('=').Select(s => s.Trim());
							cfg.Write(args.First(), args.Last());
						}
					}
				}
			}
			catch (Exception)
			{
				
				throw;
			}
		}


		public TextConfigReader(string filename)
		{
			Filename = filename;
		}
	}
}
