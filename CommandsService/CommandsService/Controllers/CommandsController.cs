using System;
using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!this._repository.PlatformExits(platformId))
            {
                return NotFound();
            }

            var commands = this._repository
                .GetCommandsForPlatform(platformId);

            return Ok(this._mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }
    }
}
