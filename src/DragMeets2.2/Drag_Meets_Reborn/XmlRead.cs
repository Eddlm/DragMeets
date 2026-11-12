using System.Globalization;
using System.Xml;
using GTA.Math;

namespace Drag_Meets_Reborn
{
	/// <summary>
	/// The script reads X/Y/Z triples out of meet files in two different ways. Both are kept:
	/// they do not agree, and the difference is shipped behaviour.
	/// </summary>
	internal static class XmlRead
	{
		/// <summary>Coordinates as floats, the common case.</summary>
		public static Vector3 Vector(XmlNode parent)
		{
			return new Vector3(Coordinate(parent, "X"), Coordinate(parent, "Y"), Coordinate(parent, "Z"));
		}

		/// <summary>
		/// The same triple truncated to whole metres. Three call sites parse through int.Parse
		/// before widening, which drops the fraction; preserved deliberately, not a tidy-up.
		/// </summary>
		public static Vector3 TruncatedVector(XmlNode parent)
		{
			return new Vector3(Truncated(parent, "X"), Truncated(parent, "Y"), Truncated(parent, "Z"));
		}

		private static float Coordinate(XmlNode parent, string axis)
		{
			return float.Parse(parent.SelectSingleNode(axis).InnerText, CultureInfo.InvariantCulture);
		}

		private static float Truncated(XmlNode parent, string axis)
		{
			return (float)int.Parse(parent.SelectSingleNode(axis).InnerText, CultureInfo.InvariantCulture);
		}
	}
}
