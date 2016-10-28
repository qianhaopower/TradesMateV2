using AutoMapper;
using DataService.Infrastructure;
using DataService.Models;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;



namespace DataService
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {



            Mapper.Initialize(x =>
            {
                x.AddProfile<MappingProfile>();

            });
            ////company
            //Mapper.Initialize(cfg => cfg.CreateMap<Company, CompanyModel>()
            //              .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
            //               .ForMember(dest => dest.CompanyName, opts => opts.MapFrom(src => src.Name))
            //                 .ForMember(dest => dest.CompanyId, opts => opts.MapFrom(src => src.Id))
            //               .ForMember(dest => dest.CreditCard, opts => opts.MapFrom(src => src.CreditCard))                         
            //               );

            ////application user
            //Mapper.Initialize(cfg => cfg.CreateMap<ApplicationUser, UserModel>()
            //             .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
            //              .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
            //                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
            //              .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
            //              );

        }
    }

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Company, CompanyModel>()
                 .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
                           .ForMember(dest => dest.CompanyName, opts => opts.MapFrom(src => src.Name))
                             .ForMember(dest => dest.CompanyId, opts => opts.MapFrom(src => src.Id))
                           .ForMember(dest => dest.CreditCard, opts => opts.MapFrom(src => src.CreditCard))
                           ;

            CreateMap<ApplicationUser, UserModel>()
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                          .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
                            .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
                          .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
                           .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.Id))
                            ;

            CreateMap<Member, MemberModel>()
              .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                        .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
                         .ForMember(dest => dest.MemberId, opts => opts.MapFrom(src => src.Id))

                        .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
                          ;

            CreateMap<MessageResponse, MessageResponseModel>()
             .ForMember(dest => dest.CreatTime, opts => opts.MapFrom(src => src.AddedDateTime))
                     .ForMember(dest => dest.ResponseText, opts => opts.MapFrom(src => src.ResponseText))
                   .ForMember(dest => dest.ResponseAction, opts => opts.MapFrom(src => src.ResponseAction.ToString()))
                       ;
            CreateMap<Message, MessageModel>()
           .ForMember(dest => dest.CreatTime, opts => opts.MapFrom(src => src.AddedDateTime))
                    .ForMember(dest => dest.ClientName, opts => opts.MapFrom(src => src.Client != null ? src.Client.FullName : null))
                     .ForMember(dest => dest.CompanyName, opts => opts.MapFrom(src => src.Company != null ? src.Company.Name : null))
                      .ForMember(dest => dest.PropertyName, opts => opts.MapFrom(src => src.Property != null ? src.Property.Name : null))
                       .ForMember(dest => dest.MemberName, opts => opts.MapFrom(src => src.Member != null ? src.Member.FullName : null))

                     .ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role.ToString()))
                    .ForMember(dest => dest.Pending, opts => opts.MapFrom(src => src.Pending))
                     .ForMember(dest => dest.MessageText, opts => opts.MapFrom(src => src.MessageText))
                     .ForMember(dest => dest.MessageType, opts => opts.MapFrom(src => (int)src.MessageType))
                     .ForMember(dest => dest.MessageResponse, opts => opts.MapFrom(src => Mapper.Map<MessageResponse, MessageResponseModel>(src.MessageResponse)))
                      ;
        }
    }
}
