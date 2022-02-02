namespace VulnManager.Models
{
	public class ShodanInfo
	{
		public object os { get; set; }
		public int ip { get; set; }
		public int[] ports { get; set; }
		public string[] vulns { get; set; }
	}
}