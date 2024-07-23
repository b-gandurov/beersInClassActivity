﻿using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Helpers;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Models.ViewModels;
using AspNetCoreDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Data;
using System.Linq;
using System.Net;

namespace AspNetCoreDemo.Controllers.MVC
{
    public class BeersController : Controller
    {
        private readonly IBeersService _beersService;
        private readonly ModelMapper _mapper;
        private readonly IStylesService _stylesService;
        private readonly IUsersService _usersService;
        private readonly AuthManager _authManager;

        public BeersController(IBeersService beersService, ModelMapper mapper, IStylesService stylesService, IUsersService usersService, AuthManager authManager)
        {
            _beersService = beersService;
            _mapper = mapper;
            _stylesService = stylesService;
            _usersService = usersService;
            _authManager = authManager;

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
                ViewData["id"] = beer.Id;
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

        [HttpGet]
        [IsAuthenticated]
        public IActionResult Create()
        {

            var beerViewModel = new BeerViewModel
            {
                Styles = new SelectList(_stylesService.GetAll(), "Id", "Name")
            };
            return View(beerViewModel);
        }

        [HttpPost]
        [IsAuthenticated]
        public IActionResult Create(BeerViewModel beerViewModel)
        {
            beerViewModel.Styles = new SelectList(_stylesService.GetAll(), "Id", "Name");
            if (!ModelState.IsValid)
            {

                return View(beerViewModel);
            }
            try
            {
                var newBeer = new Beer
                {
                    Name = beerViewModel.Name,
                    Abv = beerViewModel.Abv,
                    StyleId = beerViewModel.StyleId
                };
                newBeer.Image = "/images/default.png";
                var userName = HttpContext.Session.GetString("CurrentUser");
                var user = _usersService.GetByUsername(userName);
                newBeer.User = user;
                _beersService.Create(newBeer, user);
                return RedirectToAction("Details", new { id = newBeer.Id });
            }
            catch (DuplicateEntityException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(beerViewModel);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                Response.StatusCode = 500;
                return View("Error");
            }
        }
        [HttpGet]
        [IsAuthenticated]
        public IActionResult Edit(int id)
        {
            var beer = _beersService.GetById(id);
            var beerViewModel = new BeerViewModel
            {
                Name = beer.Name,
                Abv = beer.Abv,
                StyleId = beer.StyleId,
                Styles = new SelectList(_stylesService.GetAll(), "Id", "Name")
            };
            return View(beerViewModel);
        }

        [HttpPost]
        [IsAuthenticated]
        public IActionResult Edit(int id, BeerViewModel beerViewModel)
        {
            if (!ModelState.IsValid)
            {
                beerViewModel.Styles = new SelectList(_stylesService.GetAll(), "Id", "Name");
                return View(beerViewModel);
            }

            var beerToUpdate = _beersService.GetById(id);
            var user = _usersService.GetById(1);


            beerToUpdate.Abv = beerViewModel.Abv;
            beerToUpdate.StyleId = beerViewModel.StyleId;

            _beersService.Update(id, beerToUpdate, user);
            return RedirectToAction("Details", new { id = beerToUpdate.Id });
        }

        [HttpGet]
        [IsAuthenticated]
        public IActionResult Delete(int id)
        {
            var beer = _beersService.GetById(id);
            return View(beer);
        }

        [HttpPost, ActionName("Delete")]
        [IsAuthenticated]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _usersService.GetById(1);
            _beersService.Delete(id, user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index(string name, string type, double? minAbv, double? maxAbv, string sortBy, string sortOrder = "asc", int pageNumber = 1, int pageSize = 2)
        {
            var filterParameters = new BeerQueryParameters
            {
                Name = name,
                Style = type,
                MinAbv = minAbv,
                MaxAbv = maxAbv,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            var beers = _beersService.FilterBy(filterParameters);
            var paginatedBeers = beers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(beers.Count() / (double)pageSize);

            return View(paginatedBeers);
        }
    }
}
