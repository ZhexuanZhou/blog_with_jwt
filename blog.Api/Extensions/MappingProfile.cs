using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using blog.Core.Entities;
using blog.Infrastructure.ViewModels;

namespace blog.Api.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationViewModel, User>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<LoginViewModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<Tag, TagViewModel>();

            CreateMap<Post, PostViewModel>()
                .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PostTags))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.User.LastName))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime))
                .ForMember(dest => dest.LastModify, opt => opt.MapFrom(src => src.LastModify))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PostTags.Select(y => y.Tag).ToList()));

            //CreateMap<PostViewModel, Post>()
            //    .ForMember(dest => dest.PostTags, opt => opt.MapFrom(src => src.Tags))
            //    .AfterMap((src, dest) =>
            //    {
            //        foreach (var e in dest.PostTags)
            //        {
            //            e.Post = dest;
            //        }
            //    });
            //CreateMap<TagViewModel, PostTag>()
            //   .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src));
            //CreateMap<TagViewModel, Tag>();

            CreateMap<User, AuthorViewModel>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.LastName));

            CreateMap<Author, AuthorViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<PostViewModel, Post>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime))
                .ForMember(dest => dest.LastModify, opt => opt.MapFrom(src => src.LastModify));

            CreateMap<PostUpdateViewModel, Post>();
            CreateMap<Post, PostUpdateViewModel>().
                ForMember(dest => dest.Tags, opt => opt.Ignore());
        }
    }
}
