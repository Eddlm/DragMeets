using System.Xml;

namespace Drag_Meets_Reborn
{
	/// <summary>
	/// A meet file names its spawn point either as a &lt;Trigger&gt; or, when the author omitted
	/// one, as a &lt;WaitingZone&gt;. Thirteen of the twenty-two shipped meets rely on the second
	/// form, which is why the log fills with "NO MEET TRIGGER FOUND" on a normal load.
	/// </summary>
	internal static class MeetXml
	{
		public static XmlNode SpawnNode(XmlDocument document, out bool usedWaitingZone)
		{
			XmlNode trigger = document.SelectSingleNode("//Trigger");
			usedWaitingZone = trigger == null;
			return trigger ?? document.SelectSingleNode("//WaitingZone");
		}
	}
}
