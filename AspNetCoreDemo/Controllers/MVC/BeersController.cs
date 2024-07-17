using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Helpers;
using AspNetCoreDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System;

namespace AspNetCoreDemo.Controllers.MVC
{
    public class BeersController : Controller
    {
        private readonly IBeersService _beersService;
        private readonly ModelMapper _mapper;

        public BeersController(IBeersService beersService, ModelMapper mapper)
        {
            _beersService = beersService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var beers = _beersService.GetAll();
            return View(beers);
        }

        public IActionResult Details(int id)
        {

            try
            {
                var beer = _beersService.GetById(id);
                var beerResponce = _mapper.Map(beer);
                return View(beerResponce);
            }
            catch (EntityNotFoundException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                Response.StatusCode = NotFound().StatusCode;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                Response.StatusCode = 500;
                return View("Error");
            }
        }
    }
}
