using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using card_index_DAL.Entities;

namespace CardIndexTests.Helpers
{
    internal class AuthorDtoComparer : IEqualityComparer<AuthorDto>
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
                   && x.TextCardIds?.Count == y.TextCardIds?.Count;
        }

        public int GetHashCode([DisallowNull] AuthorDto obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class GenreDtoComparer : IEqualityComparer<GenreDto>
    {
        public bool Equals([AllowNull] GenreDto x, [AllowNull] GenreDto y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.Title == y.Title
                   && x.TextCardIds?.Count == y.TextCardIds?.Count;
        }

        public int GetHashCode([DisallowNull] GenreDto obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class TextCardDtoComparer : IEqualityComparer<TextCardDto>
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
                   && x.AuthorIds?.Count == y.AuthorIds?.Count
                   && x.RateDetailsIds?.Count == y.RateDetailsIds?.Count;
        }

        public int GetHashCode([DisallowNull] TextCardDto obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class RateDetailDtoComparer : IEqualityComparer<RateDetailDto>
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
    internal class UserRoleInfoModelComparer : IEqualityComparer<UserRoleInfoModel>
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

    internal class UserInfoModelComparer : IEqualityComparer<UserInfoModel>
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
                   && x.UserRoles?.Count == y.UserRoles?.Count
                   && x.UserRoles.ToList()[0] == y.UserRoles.ToList()[0];
        }

        public int GetHashCode([DisallowNull] UserInfoModel obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class AuthorComparer : IEqualityComparer<Author>
    {
        public bool Equals([AllowNull] Author x, [AllowNull] Author y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.FirstName == y.FirstName
                   && x.LastName == y.LastName
                   && x.YearOfBirth == y.YearOfBirth
                   && x.TextCards?.Count == y.TextCards?.Count;
        }

        public int GetHashCode([DisallowNull] Author obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class GenreComparer : IEqualityComparer<Genre>
    {
        public bool Equals([AllowNull] Genre x, [AllowNull] Genre y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.Title == y.Title
                   && x.TextCards?.Count == y.TextCards?.Count;
        }

        public int GetHashCode([DisallowNull] Genre obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class TextCardComparer : IEqualityComparer<TextCard>
    {
        public bool Equals([AllowNull] TextCard x, [AllowNull] TextCard y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.Title == y.Title
                   && Math.Abs(x.CardRating - y.CardRating) < 10E-6
                   && x.GenreId == y.GenreId
                   && x.ReleaseDate == y.ReleaseDate
                   && x.Authors?.Count == y.Authors?.Count
                   && x.RateDetails?.Count == y.RateDetails?.Count;
        }

        public int GetHashCode([DisallowNull] TextCard obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class RateDetailComparer : IEqualityComparer<RateDetail>
    {
        public bool Equals([AllowNull] RateDetail x, [AllowNull] RateDetail y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.UserId == y.UserId
                   && x.TextCardId == y.TextCardId
                   && x.RateValue == y.RateValue
                   && x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] RateDetail obj)
        {
            return obj.GetHashCode();
        }
    }
    internal class UserRoleComparer : IEqualityComparer<UserRole>
    {
        public bool Equals([AllowNull] UserRole x, [AllowNull] UserRole y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                   && x.Name == y.Name
                   && x.RoleDescription == y.RoleDescription;
        }

        public int GetHashCode([DisallowNull] UserRole obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class UserComparer : IEqualityComparer<User>
    {
        public bool Equals([AllowNull] User x, [AllowNull] User y)
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
                   && x.PhoneNumber == y.PhoneNumber;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.GetHashCode();
        }
    }
}
