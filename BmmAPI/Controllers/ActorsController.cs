﻿using AutoMapper;
using BmmAPI.DTOs;
using BmmAPI.Entities;
using BmmAPI.Helpres;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BmmAPI.Controllers
{
    [ApiController]
    [Route("api/actors")]



    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;
        private readonly string containerName = "actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper,
            IFileStorageService fileStorageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet("paged")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(queryable);
            var actors = await queryable.OrderBy(x=>x.Name).Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(actors);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IndexActorsDTO>> Get()
        {
            var top = 8;

           var isActor = await context.Actors
            .ToListAsync();


            var indexActorsDTO = new IndexActorsDTO();
            indexActorsDTO.IsActor = mapper.Map<List<ActorDTO>>(isActor);
            return indexActorsDTO;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return mapper.Map<ActorDTO>(actor);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = mapper.Map<Actor>(actorCreationDTO); 
            if(actorCreationDTO.Picture != null) 
            {
                actor.Picture = await fileStorageService.SaveFile(containerName, actorCreationDTO.Picture);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            // Update properties individually
            if (actorCreationDTO.Name != null)
            {
                actor.Name = actorCreationDTO.Name;
            }

            if (actorCreationDTO.DateOfBirth != null)
            {
                actor.DateOfBirth = actorCreationDTO.DateOfBirth;
            }

            if (actorCreationDTO.Biography != null)
            {
                actor.Biography = actorCreationDTO.Biography;
            }

            if (actorCreationDTO.Picture != null)
            {
                actor.Picture = await fileStorageService.EditFile(containerName, actorCreationDTO.Picture, actor.Picture);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) 
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }
            context.Remove(actor);
            await context.SaveChangesAsync();
            await fileStorageService.DeleteFile(actor.Picture, containerName);
            return NoContent();


        }
    }
}
