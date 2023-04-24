using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab4.Services;
using System.Text.Json.Nodes;

namespace Lab4.Pages
{
	public class GeneralModel : PageModel
	{
        protected IDataReader _dataReader;
        public string _pageName;
        public JsonNode title;
        public JsonNode features;
        public string[] activeState;

        public GeneralModel(IDataReader dataService, string name)
		{
			_dataReader = dataService;
			_pageName = name;
            features = dataService.GetData("features");

        }
	}
}
