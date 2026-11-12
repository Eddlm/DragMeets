using System;
using System.IO;
using GTA;

namespace Drag_Meets_Reborn
{
	/// <summary>
	/// Diagnostic log for a session under observation. Every line carries a millisecond stamp
	/// relative to the first tick, so events can be ordered and timed.
	/// </summary>
	/// <remarks>
	/// This is scaffolding, not the shipped format: the 2.2 debug.txt lines stay as they are and
	/// these lines are meant to be deleted once the runtime is documented.
	/// </remarks>
	internal static class Log
	{
		private static int start = -1;
		private static int exceptions;

		private static int Since()
		{
			int now = Game.GameTime;
			if (start < 0)
			{
				start = now;
			}
			return now - start;
		}

		public static void I(string message)
		{
			try
			{
				File.AppendAllText(Paths.DebugLog, string.Concat("\n[t=", Since(), "] ", message));
			}
			catch
			{
			}
		}

		/// <summary>
		/// Appends exactly what it is given, with no stamp and no newline of its own: the shape the
		/// shipped 2.2 build wrote. Every line the script emits goes through here.
		/// </summary>
		public static void Raw(string text)
		{
			File.AppendAllText(Paths.DebugLog, text);
		}

		/// <summary>
		/// Logs the first few exceptions in full, then stays quiet so a per-tick failure
		/// cannot turn the log into thousands of identical lines.
		/// </summary>
		public static void Exception(Exception ex)
		{
			exceptions++;
			if (exceptions > 3)
			{
				return;
			}
			I(string.Concat("EXCEPTION #", exceptions, ": ", ex));
		}
	}
}
