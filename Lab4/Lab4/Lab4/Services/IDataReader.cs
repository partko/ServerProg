using System.Text.Json.Nodes;

namespace Lab4.Services
{
	public interface IDataReader
	{
		public JsonNode GetData(string key);
	}
}
