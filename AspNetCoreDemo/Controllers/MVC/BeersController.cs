using AspNetCoreDemo.Exceptions;
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
using System.Collections.Generic;
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
        //public IActionResult Index()
        //{
        //    var beers = _beersService.GetAll();
        //    return View(beers);
        //}

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

        public IActionResult Index([FromQuery] BeerQueryParameters filterParameters)
        {
            if (string.IsNullOrEmpty(filterParameters.SortBy))
            {
                filterParameters.SortBy = "name";
            }
            if (string.IsNullOrEmpty(filterParameters.SortOrder))
            {
                filterParameters.SortOrder = "asc";
            }

            var beers = _beersService.FilterBy(filterParameters);
            var totalCount = _beersService.GetTotalCount(filterParameters);

            ViewBag.CurrentPage = filterParameters.PageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)totalCount / filterParameters.PageSize);
            ViewBag.Name = filterParameters.Name;
            ViewBag.Style = filterParameters.Style;
            ViewBag.MinAbv = filterParameters.MinAbv;
            ViewBag.MaxAbv = filterParameters.MaxAbv;
            ViewBag.SortBy = filterParameters.SortBy;
            ViewBag.SortOrder = filterParameters.SortOrder;
            ViewBag.PageNumbers = GeneratePageNumbers(ViewBag.CurrentPage, ViewBag.TotalPages);

            return View(beers);
        }

        private List<int?> GeneratePageNumbers(int currentPage, int totalPages)
        {
            const int maxPagesToShow = 1;
            var pages = new List<int?>();

            if (totalPages <= maxPagesToShow)
            {
                for (int i = 1; i <= totalPages; i++)
                {
                    pages.Add(i);
                }
            }
            else
            {
                pages.Add(1);
                if (currentPage > 3)
                {
                    pages.Add(null);
                }

                int startPage = Math.Max(2, currentPage - 1);
                int endPage = Math.Min(totalPages - 1, currentPage + 1);

                for (int i = startPage; i <= endPage; i++)
                {
                    pages.Add(i);
                }

                if (currentPage < totalPages-2)
                {
                    pages.Add(null);
                }
                pages.Add(totalPages);
            }

            return pages;
        }
    }
}

