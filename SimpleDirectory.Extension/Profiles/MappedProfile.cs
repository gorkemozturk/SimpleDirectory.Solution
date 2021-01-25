using AutoMapper;
using SimpleDirectory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleDirectory.Extension.Profiles
{
    public class MappedProfile : Profile
    {
        public MappedProfile()
        {
            CreateMap<Person, PersonListDTO>()
                .ForMember(d => d.FullName, o => o.MapFrom(src => src.GetFullName()));

            CreateMap<Person, PersonDetailDTO>()
                .ForMember(d => d.FullName, o => o.MapFrom(src => src.GetFullName()))
                .ForMember(d => d.Contacts, o => o.MapFrom(src => src.Contacts.Select(c => new ContactListDTO 
                { 
                    Id = c.Id,
                    Type = c.Type.ToString(),
                    Body = c.Body
                }).ToArray()));

            CreateMap<ContactInsertDTO, Contact>().ReverseMap();
        }
    }
}
