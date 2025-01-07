using mapa_back.Models;

namespace mapa_back.Data
{
	public class ChangedSchoolsResponse
	{
		public List<ChangedSchool> ChangedSchools { get; set; }
		public List<int> CorruptedRSPO { get; set; }

		public ChangedSchoolsResponse()
		{
			ChangedSchools = new List<ChangedSchool>();
			CorruptedRSPO = new List<int>();
		}
	}
}
