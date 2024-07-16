using System.Collections.Generic;
using System.Linq;

using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Helpers;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemo.Controllers
{
    [ApiController]
    [Route("api/beers")]
    public class BeersApiController : ControllerBase
    {
        private readonly IBeersService beersService;
        private readonly ModelMapper modelMapper;
        private readonly AuthManager authManager;

        public BeersApiController(IBeersService beersService, ModelMapper modelMapper, AuthManager authManager)
        {
            this.beersService = beersService;
            this.modelMapper = modelMapper;
            this.authManager = authManager;
        }

        [HttpGet("")]
        public IActionResult Get([FromQuery] BeerQueryParameters filterParameters)
        {
            var beers = this.beersService
                .FilterBy(filterParameters) // Use the query parameters to filter the beers
                .Select(beer => this.modelMapper.Map(beer)); // Convert each Beer to a BeerResponseDto

            return this.Ok(beers);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                Beer beer = this.beersService.GetById(id);
                BeerResponseDto beerResponseDto = this.modelMapper.Map(beer);

                return this.Ok(beerResponseDto);
            }
            catch (EntityNotFoundException e)
            {
                return this.NotFound(e.Message);
            }
        }

        [HttpPost("")]
        public IActionResult Create([FromHeader] string credentials, [FromBody] BeerRequestDto beerDto)
        {
            try
            {
                User user = this.authManager.TryGetUser(credentials);
                Beer beer = this.modelMapper.Map(beerDto);
                Beer createdBeer = this.beersService.Create(beer, user);
                BeerResponseDto createdBeerDto = this.modelMapper.Map(createdBeer);

                return this.CreatedAtAction(nameof(GetById), new { id = createdBeer.Id }, createdBeerDto);
            }
            catch (UnauthorizedOperationException e)
            {
                return this.Unauthorized(e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return this.Conflict(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromHeader] string credentials, [FromBody] BeerRequestDto beerDto)
        {
            try
            {
                User user = this.authManager.TryGetUser(credentials);
                Beer beer = this.modelMapper.Map(beerDto);

                Beer updatedBeer = this.beersService.Update(id, beer, user);
                BeerResponseDto beerResponseDto = this.modelMapper.Map(updatedBeer);

                return this.Ok(beerResponseDto);
            }
            catch (UnauthorizedOperationException e)
            {
                return this.Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return this.NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromHeader] string credentials)
        {
            try
            {
                User user = this.authManager.TryGetUser(credentials);
                bool beerDeleted = this.beersService.Delete(id, user);

                return this.Ok(beerDeleted);
            }
            catch (UnauthorizedOperationException e)
            {
                return this.Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return this.NotFound(e.Message);
            }
        }
    }
}
