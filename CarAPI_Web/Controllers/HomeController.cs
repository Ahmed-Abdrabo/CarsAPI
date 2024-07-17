using AutoMapper;
using Car_Utility;
using CarAPI_Web.Models;
using CarAPI_Web.Models.Dto;
using CarAPI_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CarAPI_Web.Controllers
{
    public class HomeController : Controller
    {
		private readonly ICarService _carService;
		private readonly IMapper _mapper;

		public HomeController(ICarService carService, IMapper mapper)
		{
			_carService = carService;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index()
		{
			List<CarDTO> list = new();
			var response = await _carService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.AccessToken));
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<CarDTO>>(Convert.ToString(response.Result));
			}

			return View(list);
		}

		
    }
}
