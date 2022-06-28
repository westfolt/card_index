using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CardIndexTests.Helpers
{
    internal class AuthorComparer : IEqualityComparer<AuthorDto>
    {
        public bool Equals([AllowNull] AuthorDto x, [AllowNull] AuthorDto y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.FirstName == y.FirstName
                   && x.LastName == y.LastName
                   && x.YearOfBirth == y.YearOfBirth
                   && x.TextCardIds.Count == y.TextCardIds.Count;
        }

        public int GetHashCode([DisallowNull] AuthorDto obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class GenreComparer : IEqualityComparer<GenreDto>
    {
        public bool Equals([AllowNull] GenreDto x, [AllowNull] GenreDto y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.Title == y.Title
                   && x.TextCardIds.Count == y.TextCardIds.Count;
        }

        public int GetHashCode([DisallowNull] GenreDto obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class TextCardComparer : IEqualityComparer<TextCardDto>
    {
        public bool Equals([AllowNull] TextCardDto x, [AllowNull] TextCardDto y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.Title == y.Title
                   && Math.Abs(x.CardRating - y.CardRating) < 10E-6
                   && x.GenreName == y.GenreName
                   && x.ReleaseDate == y.ReleaseDate
                   && x.AuthorIds.Count == y.AuthorIds.Count
                   && x.RateDetailsIds.Count == y.RateDetailsIds.Count;
        }

        public int GetHashCode([DisallowNull] TextCardDto obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class RateDetailComparer : IEqualityComparer<RateDetailDto>
    {
        public bool Equals([AllowNull] RateDetailDto x, [AllowNull] RateDetailDto y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.UserId == y.UserId
                   && x.TextCardId == y.TextCardId
                   && x.RateValue == y.RateValue
                   && x.FirstName == y.FirstName
                   && x.LastName == y.LastName
                   && x.CardName == y.CardName;
        }

        public int GetHashCode([DisallowNull] RateDetailDto obj)
        {
            return obj.GetHashCode();
        }
    }
    internal class UserRoleComparer : IEqualityComparer<UserRoleInfoModel>
    {
        public bool Equals([AllowNull] UserRoleInfoModel x, [AllowNull] UserRoleInfoModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.RoleName == y.RoleName;
        }

        public int GetHashCode([DisallowNull] UserRoleInfoModel obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class UserComparer : IEqualityComparer<UserInfoModel>
    {
        public bool Equals([AllowNull] UserInfoModel x, [AllowNull] UserInfoModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.FirstName == y.FirstName
                   && x.LastName == y.LastName
                   && x.DateOfBirth == y.DateOfBirth
                   && x.City == y.City
                   && x.Email == y.Email
                   && x.Phone == y.Phone
                   && x.UserRoles.Count == y.UserRoles.Count
                   && x.UserRoles.ToList()[0] == y.UserRoles.ToList()[0];
        }

        public int GetHashCode([DisallowNull] UserInfoModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
