namespace Drag_Meets_Reborn
{
	/// <summary>Every filesystem location the script touches, in one place.</summary>
	internal static class Paths
	{
		public const string Root = @"scripts\DragMeets";
		public const string ConfigFile = Root + @"\config.ini";
		public const string DebugLog = Root + @"\debug.txt";
		public const string MeetsDir = Root + @"\Meets";
		public const string RacersDir = Root + @"\Racers";
		public const string RacersFile = RacersDir + @"\Racers.xml";
		public const string PlayerFile = RacersDir + @"\Player.xml";
	}
}
