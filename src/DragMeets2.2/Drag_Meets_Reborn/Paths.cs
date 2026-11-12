namespace Drag_Meets_Reborn
{
	/// <summary>
	/// Every filesystem location the script touches, in one place.
	/// </summary>
	/// <remarks>
	/// The doubled separator after "scripts" is deliberate. The shipped 2.2 binary spelled
	/// its paths that way, and debug.txt lines are compared against logs from that build,
	/// so the observable text is kept byte-for-byte. Windows resolves both spellings to
	/// the same file.
	/// </remarks>
	internal static class Paths
	{
		public const string Root = @"scripts\\DragMeets";
		public const string ConfigFile = Root + @"\config.ini";
		public const string DebugLog = Root + @"\debug.txt";
		public const string MeetsDir = Root + @"\Meets";
		public const string RacersDir = Root + @"\Racers";
		public const string RacersFile = RacersDir + @"\Racers.xml";
		public const string PlayerFile = RacersDir + @"\Player.xml";
	}
}
