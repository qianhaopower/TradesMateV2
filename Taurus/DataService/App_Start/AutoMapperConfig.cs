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
							   .ForMember(dest => dest.TradeTypes, opts => opts.MapFrom(src => src.CompanyServices.Select(p => p.Type).ToList()))
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
			 .ForMember(dest => dest.CreateTime, opts => opts.MapFrom(src => src.AddedDateTime))
					 .ForMember(dest => dest.ResponseText, opts => opts.MapFrom(src => src.ResponseText))
					   .ForMember(dest => dest.IsRead, opts => opts.MapFrom(src => src.IsRead))
						.ForMember(dest => dest.UserIdTo, opts => opts.MapFrom(src => src.UserIdTo))
				   .ForMember(dest => dest.ResponseAction, opts => opts.MapFrom(src => src.ResponseAction.ToString()))
					   ;
			// CreateMap<Message, MessageModel>()
			//.ForMember(dest => dest.CreateTime, opts => opts.MapFrom(src => src.AddedDateTime))
			//         .ForMember(dest => dest.IsWaitingForResponse, opts => opts.MapFrom(src => src.IsWaitingForResponse))
			//          .ForMember(dest => dest.HasResponse, opts => opts.MapFrom(src => src.HasResponse))
			//         .ForMember(dest => dest.IsRead, opts => opts.MapFrom(src => src.IsRead))
			//                      .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			//          .ForMember(dest => dest.MessageText, opts => opts.MapFrom(src => src.MessageText))
			//          .ForMember(dest => dest.MessageType, opts => opts.MapFrom(src => (int)src.MessageType))

			//           ;

			CreateMap<Message, MessageDetailModel>()
		.ForMember(dest => dest.CreateTime, opts => opts.MapFrom(src => src.AddedDateTime))
				 .ForMember(dest => dest.ClientName, opts => opts.MapFrom(src => src.Client != null ? src.Client.FullName : null))
				  .ForMember(dest => dest.CompanyName, opts => opts.MapFrom(src => src.Company != null ? src.Company.Name : null))
				   .ForMember(dest => dest.PropertyName, opts => opts.MapFrom(src => src.Property != null ? src.Property.Name : null))
					.ForMember(dest => dest.MemberName, opts => opts.MapFrom(src => src.Member != null ? src.Member.FullName : null))

					  .ForMember(dest => dest.PropertyAddress, opts => opts.MapFrom(src => src.PropertyAddress))
					  .ForMember(dest => dest.Section, opts => opts.MapFrom(src => src.Section))
					  .ForMember(dest => dest.ServiceType, opts => opts.MapFrom(src => src.ServiceType))
						.ForMember(dest => dest.PropertyId, opts => opts.MapFrom(src => src.PropertyId))
						  .ForMember(dest => dest.ClientId, opts => opts.MapFrom(src => src.ClientId))
						  .ForMember(dest => dest.CompanyId, opts => opts.MapFrom(src => src.CompanyId))

					 .ForMember(dest => dest.UserIdTo, opts => opts.MapFrom(src => src.UserIdTo))
				  .ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role.ToString()))
				   .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
				 .ForMember(dest => dest.IsWaitingForResponse, opts => opts.MapFrom(src => src.IsWaitingForResponse))
					.ForMember(dest => dest.IsRead, opts => opts.MapFrom(src => src.IsRead))
				  .ForMember(dest => dest.MessageText, opts => opts.MapFrom(src => src.MessageText))
				  .ForMember(dest => dest.MessageType, opts => opts.MapFrom(src => src.MessageType))
				  .ForMember(dest => dest.MessageResponse, opts => opts.MapFrom(src => Mapper.Map<MessageResponse, MessageResponseModel>(src.MessageResponse)))
				   ;


			CreateMap<Property, PropertyModel>()
			.ForMember(dest => dest.Comment, opts => opts.MapFrom(src => src.Comment))
			.ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.Narrative, opts => opts.MapFrom(src => src.Narrative))
			.ForMember(dest => dest.Condition, opts => opts.MapFrom(src => src.Condition))
			.ForMember(dest => dest.AddressDisplay, opts => opts.MapFrom(src => src.Address == null ? null :

			string.Format("{0} {1} {2} {3} {4} {5}",
			src.Address.Line1,
			src.Address.Line2,
			src.Address.Line3,
			src.Address.Suburb,
			src.Address.State,
			src.Address.PostCode)

			));
			CreateMap<Attachment, AttachmentModel>()
				.ForMember(dest => dest.CreatedDateTime, opts => opts.MapFrom(src => src.AddedDateTime));

            CreateMap<WorkItemTemplate, WorkItemTemplateModel>();
            CreateMap<WorkItem, WorkItemModel>()
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => src.Status.ToString()))



            ;
		}
    }
}
