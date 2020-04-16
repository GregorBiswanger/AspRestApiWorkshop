using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspRestApiWorkshop.Models;
using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AspRestApiWorkshop.Controllers
{
    [Route("api/v{version:apiVersion}/camps/{moniker}/[controller]")]
    [ApiController]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public TalksController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _campRepository = campRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet()]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {
            try
            {
                var result = await _campRepository.GetTalksByMonikerAsync(moniker);

                if (result == null)
                {
                    return NotFound();
                }

                return _mapper.Map<TalkModel[]>(result)
                    .Select(talkModel => CreateLinksForTalk(moniker, talkModel))
                    .ToArray();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkModel>> Get(string moniker, int id)
        {
            try
            {
                var talk = await _campRepository.GetTalkByMonikerAsync(moniker, id);

                if (talk == null)
                {
                    return NotFound();
                }

                return CreateLinksForTalk(moniker, _mapper.Map<TalkModel>(talk));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel talkModel)
        {
            try
            {
                var camp = await _campRepository.GetCampAsync(moniker);
                if(camp == null)
                {
                    return BadRequest("Camp does not exist");
                }

                var talk = _mapper.Map<Talk>(talkModel);
                talk.Camp = camp;

                if(talkModel.Speaker == null)
                {
                    return BadRequest("Speaker ID is required");
                }

                var speaker = await _campRepository.GetSpeakerAsync(talkModel.Speaker.SpeakerId);
                if(speaker == null) 
                {
                    return BadRequest("Speaker could not be found");
                }

                talk.Speaker = speaker;

                _campRepository.Add(talk);

                if(await _campRepository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction(HttpContext, 
                        "Get",
                        values: new { moniker, id = talk.TalkId });

                    return Created(url, _mapper.Map<TalkModel>(talk));
                }

                return BadRequest("Failed to save new Talk");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TalkModel>> Put(string moniker, int id, TalkModel talkModel)
        {
            try
            {
                var talk = await _campRepository.GetTalkByMonikerAsync(moniker, id, true);
                if(talk == null)
                {
                    return NotFound("Could not find the talk");
                }

                _mapper.Map(talkModel, talk);

                if(talkModel.Speaker != null)
                {
                    var speaker = await _campRepository.GetSpeakerAsync(talkModel.Speaker.SpeakerId);
                    if(speaker != null)
                    {
                        talk.Speaker = speaker;
                    }
                }

                if(await _campRepository.SaveChangesAsync())
                {
                    return _mapper.Map<TalkModel>(talk);
                }

                return BadRequest("Failed to update database.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(string moniker, int id)
        {
            try
            {
                var talk = await _campRepository.GetTalkByMonikerAsync(moniker, id);
                if(talk == null)
                {
                    return BadRequest("Failed to find the talk to delete.");
                }

                _campRepository.Delete(talk);

                if(await _campRepository.SaveChangesAsync())
                {
                    return Ok();
                }

                return BadRequest("Failed to delete the talk");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        private TalkModel CreateLinksForTalk(string moniker, TalkModel talkModel)
        {
            talkModel.Links.Add(new LinkDto(
                _linkGenerator.GetUriByAction(HttpContext, "Get", "talks", new { version = "2.0", moniker, id = talkModel.TalkId }),
                "self",
                "GET"
                ));

            talkModel.Links.Add(new LinkDto(
                _linkGenerator.GetUriByAction(HttpContext, "Delete", "talks", new { version = "2.0", moniker, id = talkModel.TalkId }),
                "delete_talk",
                "DELETE"
                ));

            talkModel.Links.Add(new LinkDto(
                _linkGenerator.GetUriByAction(HttpContext, "Put", "talks", new { version = "2.0", moniker, id = talkModel.TalkId }),
                "edit_talk",
                "PUT"
                ));

            talkModel.Links.Add(new LinkDto(
                _linkGenerator.GetUriByAction(HttpContext, "Post", "talks", new { version = "2.0", moniker }),
                "create_talk",
                "POST"
                ));

            return talkModel;
        }

        [HttpOptions]
        public IActionResult GetTalksOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,PUT,DELETE,POST");
            return Ok();
        }
    }
}