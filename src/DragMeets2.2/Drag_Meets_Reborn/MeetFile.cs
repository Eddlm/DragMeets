using System.Xml;
using GTA.Math;

namespace Drag_Meets_Reborn
{
	/// <summary>
	/// One meet file, read once when the script loads. Both the trigger list and the spawn path
	/// read from this list, so spawning a meet costs no disk access and the point the player drives
	/// into is the same value the spawn path matches on.
	/// </summary>
	internal sealed class MeetFile
	{
		/// <summary>Full path, as the loader and the log name it.</summary>
		public readonly string Path;

		/// <summary>The parsed file. Every entry owns its own document; they are never shared.</summary>
		public readonly XmlDocument Document;

		/// <summary>The spawn point as written in the file: the value a match is tested against.</summary>
		public readonly Vector3 Spawn;

		/// <summary>The same point with its decimals removed, which is where the blip is placed.</summary>
		public readonly Vector3 TruncatedSpawn;

		/// <summary>A &lt;Touge&gt; element makes this a street race meet rather than a drag meet.</summary>
		public readonly bool IsTouge;

		public MeetFile(string path, XmlDocument document, Vector3 spawn, Vector3 truncatedSpawn, bool isTouge)
		{
			Path = path;
			Document = document;
			Spawn = spawn;
			TruncatedSpawn = truncatedSpawn;
			IsTouge = isTouge;
		}
	}
}
