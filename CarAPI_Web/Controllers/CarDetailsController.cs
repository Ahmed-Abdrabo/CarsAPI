using AutoMapper;
using Car_Utility;
using CarAPI_Web.Models;
using CarAPI_Web.Models.Dto;
using CarAPI_Web.Models.VM;
using CarAPI_Web.Services;
using CarAPI_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;

namespace CarAPI_Web.Controllers
{
    public class CarDetailsController : Controller
    {
        private readonly ICarDetailsService _carDetailsService;
        private readonly ICarService _carService;
        private readonly IMapper _mapper;

        public CarDetailsController(ICarDetailsService carDetailsService, IMapper mapper, ICarService carService)
        {
            _carDetailsService = carDetailsService;
            _mapper = mapper;
            _carService = carService;
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexCarDetails()
        {
            List<CarDetailsDTO> list = new();
            var response = await _carDetailsService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CarDetailsDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCarDetails()
        {
            CarDetailsCreateVM CarDetailsVM = new();
            var response = await _carService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                CarDetailsVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Make,
                        Value = i.Id.ToString()
                    }); 
            }
            return View(CarDetailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCarDetails(CarDetailsCreateVM CarDto)
        {
            if (ModelState.IsValid)
            {

                var response = await _carDetailsService.CreateAsync<APIResponse>(CarDto.CarDetails);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexCarDetails));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }   
            }
            var resp = await _carService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSuccess)
            {
                CarDto.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Make,
                        Value = i.Id.ToString()
                    }); 
            }
            return View(CarDto);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCarDetails(int CarDetailsId)
        {
            CarDetailsUpdateVM CarDetailsVM = new();    
            var response = await _carDetailsService.GetAsync<APIResponse>(CarDetailsId);
            if (response != null && response.IsSuccess)
            {
                CarDetailsDTO carDto = JsonConvert.DeserializeObject<CarDetailsDTO>(Convert.ToString(response.Result));
                CarDetailsVM.CarDetails= _mapper.Map<CarDetailsUpdateDTO>(carDto);
            }
            response = await _carService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                CarDetailsVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Make,
                        Value = i.Id.ToString()
                    }); 
                return View(CarDetailsVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCarDetails(CarDetailsUpdateVM CarDto)
        {
            if (ModelState.IsValid)
            {

                var response = await _carDetailsService.UpdateAsync<APIResponse>(CarDto.CarDetails);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexCarDetails));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            var resp = await _carService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSuccess)
            {
                CarDto.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Make,
                        Value = i.Id.ToString()
                    });
            }
            return View(CarDto);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCarDetails(int CarDetailsId)
        {
            CarDetailsDeleteVM CarDetailsVM = new();
            var response = await _carDetailsService.GetAsync<APIResponse>(CarDetailsId);
            if (response != null && response.IsSuccess)
            {
                CarDetailsDTO carDto = JsonConvert.DeserializeObject<CarDetailsDTO>(Convert.ToString(response.Result));
                CarDetailsVM.CarDetails = carDto;
            }
            response = await _carService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                CarDetailsVM.CarList = JsonConvert.DeserializeObject<List<CarDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Make,
                        Value = i.Id.ToString()
                    });
                return View(CarDetailsVM);
            }
            return NotFound();      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCarDetails(CarDetailsDeleteVM carDto)
        {

            var response = await _carDetailsService.DeleteAsync<APIResponse>(carDto.CarDetails.CarDetailsId);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexCarDetails));
            }

            return View(carDto);
        }
    }
}
