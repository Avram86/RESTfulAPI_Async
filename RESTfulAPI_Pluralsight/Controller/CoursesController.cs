using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using RESTfulAPI_Pluralsight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAPI_Pluralsight.Controller
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController:ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesForAuthorAsync(Guid authorId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var coursesFromRepo =await _courseLibraryRepository.GetCoursesAsync(authorId);
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(coursesFromRepo));
        }

        [HttpGet("{courseId}", Name ="GetCourseForAuthor")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourseForAuthorAsync(Guid authorId, Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var course =await _courseLibraryRepository.GetCourseAsync(authorId, courseId);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CourseDto>(course));
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreatecourseForAction(Guid authorId, CourseForCreationDto courseForCreation)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseEntity = _mapper.Map<Course>(courseForCreation);

            _courseLibraryRepository.AddCourse(authorId, courseEntity);
            await _courseLibraryRepository.SaveAsync();

            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);

            return CreatedAtRoute("GetCourseForAuthor", 
                new { authorId=authorId, courseId=courseToReturn.Id},
                courseToReturn);
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourseForAuthorAsync(Guid authorId, Guid courseId, CourseForUpdateDto courseForUpdateDto)
        {
            if(!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo =await _courseLibraryRepository.GetCourseAsync(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                var courseToAdd = _mapper.Map<Course>(courseForUpdateDto);
                courseToAdd.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId ,courseToAdd);
                await _courseLibraryRepository.SaveAsync();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForAuthor",
                    new { authorId=authorId, courseId=courseToReturn.Id},
                    courseToReturn);
            }

            //map the entity courseFromRepo to a CourseForUpdateDto
            //apply the updated fields
            //map courseForUpdateDto back to Entity
                        //(source,     destination)
            _mapper.Map(courseForUpdateDto, courseForAuthorFromRepo);

            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);
            await _courseLibraryRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourseForAuthorAsync(Guid authorId, Guid CourseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseToBeDeletedFromRepo =await _courseLibraryRepository.GetCourseAsync(authorId, CourseId);

            if (courseToBeDeletedFromRepo == null)
            {
                return NotFound();
            }

            _courseLibraryRepository.DeleteCourse(courseToBeDeletedFromRepo);
            await _courseLibraryRepository.SaveAsync();

            return NoContent();
        }
    }
}
