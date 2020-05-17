using Aplicacion.Cursos;
using AutoMapper;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // x => x.Instructores: proviene de CursoDto
            // z => z.InstructoresLink.Select(a => a.Instructor): Es el origen que se mapea
            CreateMap<Curso, CursoDto>()
                .ForMember(
                    x => x.Instructores,
                    y => y.MapFrom(z => z.InstructoresLink.Select(a => a.Instructor).ToList()));
            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Instructor, InstructorDto>();
        }
    }
}
