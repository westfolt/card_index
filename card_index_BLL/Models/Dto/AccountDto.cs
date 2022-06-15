using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using card_index_BLL.Validation;

namespace card_index_BLL.Models.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Firstname is empty")]
        [RegularExpression(@"[\w,.-']{3,}", ErrorMessage = "More than 3 characters, no numbers")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is empty")]
        [RegularExpression(@"[\w,.-']{4,}", ErrorMessage = "More than 4 characters, no numbers")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Age is required")]
        [BirthDate(18, 100)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Firstname is empty")]
        [RegularExpression(@"[\w,.-']{3,}", ErrorMessage = "More than 3 characters, no numbers")]
        public string City { get; set; }
        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress(ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        [RegularExpression(@"\+[\d]{1, 3}\([\d]{3}\)[\d] { 7}",
            ErrorMessage = "Phone should be +X(XXX)XXXXXXX")]
        public string Phone { get; set; }
    }
}
