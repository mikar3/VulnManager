namespace VulnManager.Models
{
    public class CveInfo
    {
        public Impact impact { get; set; }
    }

    public class Impact
    {
        public BaseMetricV2 baseMetricV2 { get; set; }
    }

    public class BaseMetricV2
    {
        public CvssV2 cvssV2 { get; set; }
    }

    public class CvssV2
    {
        public double baseScore { get; set; }
    }

}
