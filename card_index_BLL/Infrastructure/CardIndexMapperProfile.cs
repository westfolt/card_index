using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Models;
using card_index_DAL.Entities;

namespace card_index_BLL.Infrastructure
{
    public class CardIndexMapperProfile:Profile
    {
        public CardIndexMapperProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dto => dto.TextCardIds,
                    a => a.MapFrom(x => x.TextCards.Select(tc => tc.Id)))
                .ReverseMap();

            CreateMap<Genre, GenreDto>()
                .ForMember(dto => dto.TextCardIds,
                    g => g.MapFrom(x => x.TextCards.Select(tc => tc.Id)))
                .ReverseMap();

            CreateMap<RateDetail, RateDetailDto>()
                .ForMember(dto => dto.FirstName, rd => rd.MapFrom(x => x.User.FirstName))
                .ForMember(dto => dto.LastName, rd => rd.MapFrom(x => x.User.LastName))
                .ForMember(dto => dto.CardName, rd => rd.MapFrom(x => x.TextCard.Title))
                .ReverseMap();

            CreateMap<TextCard, TextCardDto>()
                .ForMember(dto => dto.AuthorIds,
                    tc => tc.MapFrom(x => x.Authors.Select(a => a.Id)))
                .ForMember(dto => dto.RateDetailsIds,
                    tc => tc.MapFrom(x => x.RateDetails.Select(rd => rd.Id)))
                .ForMember(dto => dto.GenreName, tc => tc.MapFrom(x => x.Genre.Title))
                .ReverseMap();

            CreateMap<User, UserInfoModel>()
                .ReverseMap();

            CreateMap<UserRole, UserRoleInfoModel>()
                .ForMember(dto => dto.RoleName, ur => ur.MapFrom(x => x.Name))
                .ReverseMap();
        }
    }
}
