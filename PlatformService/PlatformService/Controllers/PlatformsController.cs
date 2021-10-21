using System;
using AutoMapper;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepo repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms...");

            var platformItems = this._repository
                .GetAllPlatforms();

            return Ok(this._mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = this._repository
                .GetPlatformById(id);

            if (platformItem != null)
            {
                return Ok(this._mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = this._mapper.Map<Platform>(platformCreateDto);

            this._repository.CreatePlatform(platformModel);
            this._repository.SaveChanges();

            var platformReadDto = this._mapper.Map<PlatformReadDto>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new { platformReadDto.Id }, platformReadDto);
        }
    }
}
