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
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Location = c.Location
                }).ToArray()));
        }
    }
}
