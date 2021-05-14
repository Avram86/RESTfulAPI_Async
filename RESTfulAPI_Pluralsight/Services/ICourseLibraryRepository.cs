using CourseLibrary.API.Entities;
using RESTfulAPI_Pluralsight.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{
    public interface ICourseLibraryRepository
    {    
        Task<IEnumerable<Course>> GetCoursesAsync(Guid authorId);
        Task<Course> GetCourseAsync(Guid authorId, Guid courseId);

        void AddCourse(Guid authorId, Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);


        Task<IEnumerable<Author>> GetAuthorsAsync();
        Task<IEnumerable<Author>> GetAuthorsAsync(AuthorsResourceParameters authorsResourceParameters);
        Task<Author> GetAuthorAsync(Guid authorId);
        Task<IEnumerable<Author>> GetAuthorsAsync(IEnumerable<Guid> authorIds);

        void AddAuthor(Author author);
        void DeleteAuthor(Author author);
        void UpdateAuthor(Author author);
        bool AuthorExists(Guid authorId);


        Task<bool> SaveAsync();
    }
}
