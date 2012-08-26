using System;
using System.IO;
using System.Linq;
using EasyConfig.storage;

namespace EasyConfig.parsing
{
	public class TextConfigReader
		: IConfigReader, IConfigWriter
	{
		public readonly string Filename;
		/**TODO
		public string ValueDelim;
		public string SectionDelim;
		public string PairDelim;
		 */

		public void Write(Config c)
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
							writer.WriteLine("{0} = {1}", v.Current.Key, v.Current.Value);
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Config Read()
		{
			try
			{
				var cfg = new Config();
				using (var stream = new StreamReader(Filename))
				{
					while (!stream.EndOfStream)
					{
						var line = stream.ReadLine();
						if (line == null)
							continue;

						line = line.Trim();

						if(line.Contains("="))
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

			return null;
		}


		public TextConfigReader(string filename)
		{
			Filename = filename;
		}
	}
}
