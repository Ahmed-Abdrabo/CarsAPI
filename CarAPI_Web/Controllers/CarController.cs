using AutoMapper;
using CarAPI_Web.Models;
using CarAPI_Web.Models.Dto;
using CarAPI_Web.Services;
using CarAPI_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarAPI_Web.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IMapper _mapper;

        public CarController(ICarService carService,IMapper mapper)
        {
            _carService = carService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexCar()
        {
            List<CarDTO> list = new();
            var response = await _carService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list=JsonConvert.DeserializeObject<List<CarDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

		public async Task<IActionResult> CreateCar()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateCar(CarCreateDTO CarDto)
		{
			if (ModelState.IsValid)
			{

				var response = await _carService.CreateAsync<APIResponse>(CarDto);
				if (response != null && response.IsSuccess)
				{
					return RedirectToAction(nameof(IndexCar));
				}
			}
			return View(CarDto);
		}

        public async Task<IActionResult> UpdateCar(int carId)
        {
            var response = await _carService.GetAsync<APIResponse>(carId);
            if (response != null && response.IsSuccess)
            {
                CarDTO carDto = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<CarUpdateDTO>(carDto));
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCar(CarUpdateDTO carDto)
        {
            if (ModelState.IsValid)
            {

                var response = await _carService.UpdateAsync<APIResponse>(carDto);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexCar));
                }
            }
            return View(carDto);
        }

		public async Task<IActionResult> DeleteCar(int carId)
		{
			var response = await _carService.GetAsync<APIResponse>(carId);
			if (response != null && response.IsSuccess)
			{
				CarDTO carDto = JsonConvert.DeserializeObject<CarDTO>(Convert.ToString(response.Result));
				return View(carDto);
			}
			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteCar(CarDTO carDto)
		{

			var response = await _carService.DeleteAsync<APIResponse>(carDto.Id);
			if (response != null && response.IsSuccess)
			{
				return RedirectToAction(nameof(IndexCar));
			}

			return View(carDto);
		}
	}
}
