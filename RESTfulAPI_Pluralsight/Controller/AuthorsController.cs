using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using RESTfulAPI_Pluralsight.Models;
using RESTfulAPI_Pluralsight.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAPI_Pluralsight.Controller
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController:ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(
            //for filtering
            [FromQuery]AuthorsResourceParameters authorsResourceParameters)
        {
            var authorsFromRepo =await _courseLibraryRepository.GetAuthorsAsync(authorsResourceParameters);
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        [HttpGet("{authorId:guid}", Name ="GetAuthor")]
        public async Task<ActionResult<AuthorDto>> GetAuthorAsync(Guid authorId)
        {
            var authorFromRepo =await _courseLibraryRepository.GetAuthorAsync(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto authorForCreationDto)
        {
            var authorEntity = _mapper.Map<Author>(authorForCreationDto);

            _courseLibraryRepository.AddAuthor(authorEntity);
            await _courseLibraryRepository.SaveAsync();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", new { authorId=authorToReturn.Id}, authorToReturn);
        }

        [HttpPut("{authorId}")]
        public async Task<IActionResult> UpdateAuthor(Guid authorId, AuthorForUpdateDto authorForUpdateDto)
        {
            var authorFromRepo =await _courseLibraryRepository.GetAuthorAsync(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            //copiem valorile editate peste valorile entitatii
            _mapper.Map(authorForUpdateDto, authorFromRepo);

            _courseLibraryRepository.UpdateAuthor(authorFromRepo);
            await _courseLibraryRepository.SaveAsync();

            return NoContent();
        }
        
        [HttpDelete("{authorId}")]
        public async Task<ActionResult> DeleteAuthor(Guid authorId)
        {
            var authorFromRepo =await _courseLibraryRepository.GetAuthorAsync(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _courseLibraryRepository.DeleteAuthor(authorFromRepo);
            await _courseLibraryRepository.SaveAsync();

            return NoContent();
        }
    }
}
