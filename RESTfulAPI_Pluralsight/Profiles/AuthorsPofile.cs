using AutoMapper;
using CourseLibrary.API.Entities;
using RESTfulAPI_Pluralsight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAPI_Pluralsight.Profiles
{
    public class AuthorsPofile:Profile
    {
        public AuthorsPofile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorForCreationDto, Author>();
            CreateMap<AuthorForUpdateDto, Author>();
        }
    }
}
