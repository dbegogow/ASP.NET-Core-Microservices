using System;
using AutoMapper;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PlatformService.SyncDataService.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(
            IPlatformRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._commandDataClient = commandDataClient;
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
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = this._mapper.Map<Platform>(platformCreateDto);

            this._repository.CreatePlatform(platformModel);
            this._repository.SaveChanges();

            var platformReadDto = this._mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await this._commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { platformReadDto.Id }, platformReadDto);
        }
    }
}
