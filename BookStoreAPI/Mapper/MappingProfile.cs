using AutoMapper;
using BookStoreClassLibrary.Core.Enums;
using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.Util;
using BookStoreAPI.DTO.Book;
using BookStoreAPI.DTO.Author;
using BookStoreAPI.DTO.User;

namespace BookStoreAPI.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRegistrationDTO>().ReverseMap().ForMember(user => user.UserName,
                                                                            userModel => userModel.MapFrom(src => src.Email))
                                                               .ForMember(user => user.Role,
                                                                            userModel => userModel.MapFrom(src => src.Role.GetEnumValueByDescription<Role>()));

            CreateMap<Book, CreateBookDTO>().ReverseMap();
            CreateMap<Author, CreateAuthorDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Author, AuthorPreviewDTO>().ReverseMap();
            CreateMap<Book, BookPreviewDTO>().ReverseMap();
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}
