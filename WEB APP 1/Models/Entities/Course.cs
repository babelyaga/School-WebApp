using System.ComponentModel.DataAnnotations;

namespace WEB_APP_1.Models.Entities
{
    public class Course
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name Of The Course Is Required")]
        [StringLength(100, ErrorMessage = "Course name must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string CourseName { get; set; } = null!;

        [Required(ErrorMessage = "Subject Of The Course Is Required")]
        [StringLength(100, ErrorMessage = "Subject must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Date Of The Course Is Required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location Is Required")]
        [StringLength(100, ErrorMessage = "Location must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "Is Active Status Is Required")]
        [Display(Name = "Is Active")]

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        public bool IsActive { get; set; }
    }
}
