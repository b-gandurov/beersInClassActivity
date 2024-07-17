using System.Collections.Generic;
using System.Linq;
using AspNetCoreDemo.Exceptions;
using AspNetCoreDemo.Helpers;
using AspNetCoreDemo.Models;
using AspNetCoreDemo.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemo.Controllers.API
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
            var beers = beersService
                .FilterBy(filterParameters) // Use the query parameters to filter the beers
                .Select(beer => modelMapper.Map(beer)); // Convert each Beer to a BeerResponseDto

            return Ok(beers);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                Beer beer = beersService.GetById(id);
                BeerResponseDto beerResponseDto = modelMapper.Map(beer);

                return Ok(beerResponseDto);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("")]
        public IActionResult Create([FromHeader] string credentials, [FromBody] BeerRequestDto beerDto)
        {
            try
            {
                User user = authManager.TryGetUser(credentials);
                Beer beer = modelMapper.Map(beerDto);
                Beer createdBeer = beersService.Create(beer, user);
                BeerResponseDto createdBeerDto = modelMapper.Map(createdBeer);

                return CreatedAtAction(nameof(GetById), new { id = createdBeer.Id }, createdBeerDto);
            }
            catch (UnauthorizedOperationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromHeader] string credentials, [FromBody] BeerRequestDto beerDto)
        {
            try
            {
                User user = authManager.TryGetUser(credentials);
                Beer beer = modelMapper.Map(beerDto);

                Beer updatedBeer = beersService.Update(id, beer, user);
                BeerResponseDto beerResponseDto = modelMapper.Map(updatedBeer);

                return Ok(beerResponseDto);
            }
            catch (UnauthorizedOperationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromHeader] string credentials)
        {
            try
            {
                User user = authManager.TryGetUser(credentials);
                bool beerDeleted = beersService.Delete(id, user);

                return Ok(beerDeleted);
            }
            catch (UnauthorizedOperationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
