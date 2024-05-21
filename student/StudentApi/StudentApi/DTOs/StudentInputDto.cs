using System.ComponentModel.DataAnnotations;

namespace StudentApi.DTOs
{
    public class StudentInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        public int GroupId { get; set; }
    }
}