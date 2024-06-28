using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_APP_1.Models.Entities
{
    public class Student
    {
        
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First Name Is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name Is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(10, ErrorMessage = "{0} Format Isn't Correct", MinimumLength = 10)]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Course Is Required")]
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
