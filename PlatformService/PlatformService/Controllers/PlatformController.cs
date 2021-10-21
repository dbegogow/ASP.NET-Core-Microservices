using System;
using System.Collections.Generic;
using AutoMapper;
using PlatformService.Data;
using Microsoft.AspNetCore.Mvc;
using PlatformService.DTOs;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public PlatformController(IPlatformRepo repository, IMapper mapper)
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
    }
}
