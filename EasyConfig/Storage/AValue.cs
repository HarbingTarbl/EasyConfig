
namespace EasyConfig.Storage
{
	public class AValue
	{
		public string Value;

		public static implicit operator string(AValue v)
		{
			return v.Value;
		}

		public static implicit operator AValue(int v)
		{
			return new AValue { Value = v.ToString() };
		}

		public static implicit operator AValue(bool v)
		{
			return new AValue { Value = v.ToString() };
		}

		public static implicit operator AValue(string v)
		{
			return new AValue { Value = v };
		}

		public static implicit operator AValue(float v)
		{
			return new AValue { Value = v.ToString() };
		}
	}
}
