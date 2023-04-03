using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab4.Services;
using static Lab4.Pages.GeneralModel;

namespace Lab4.Pages
{
    public class featuresModel : GeneralModel
	{
		public featuresModel(IDataReader reader) : base(reader, "features")
		{
		}

		public void OnGet()
		{
			title = _dataReader.GetData(_pageName)["title"];
		}
	}
}
