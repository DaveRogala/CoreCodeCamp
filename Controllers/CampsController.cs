using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public CampsController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }
        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
        {
           try
           {
             var results = await _repository.GetAllCampsAsync(includeTalks);
  
             return _mapper.Map<CampModel[]>(results);
           }
           catch (Exception ex)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure, inner exception {ex.Message}");

           }
        }
        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await _repository.GetCampAsync(moniker, includeTalks);

                if (result == null) return NotFound($"{moniker} not found");

                return _mapper.Map<CampModel>(result);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure, inner exception {ex.Message}");

            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                var results = await _repository.GetAllCampsByEventDate(theDate, includeTalks);

                if (!results.Any()) return NotFound($"No camps found on date: {theDate}");

                return _mapper.Map<CampModel[]>(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure, inner exception {ex.Message}");
            }
        }
        public async Task<ActionResult<CampModel>> Post(CampModel model)
        {
            try
            {
                var existingCamp = await _repository.GetCampAsync(model.Moniker);
                if(existingCamp != null)
                {
                    return BadRequest($"{model.Moniker} already exists.  Use PATCH or PUT to update");
                }
                var location = _linkGenerator.GetPathByAction("Get", "Camps", new { moniker = model.Moniker });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest($"Could not user moniker: {model.Moniker}");
                }
                var camp = _mapper.Map<Camp>(model);
                _repository.Add(camp);
                if(await _repository.SaveChangesAsync())
                {
                    return Created(location,_mapper.Map<CampModel>(camp));
                }
                
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure, inner exception {ex.Message}");
            }
            return BadRequest();

        }
        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> Put(string moniker, CampModel model)
        {
            try
            {
                var oldCamp = await _repository.GetCampAsync(moniker);
                if (oldCamp == null)
                {
                    return NotFound($"{moniker} not found");
                }

                _mapper.Map(model, oldCamp);
                if(await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<CampModel>(oldCamp);
                }

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure, inner exception {ex.Message}");
            }
            return BadRequest();
        }
    }
}
