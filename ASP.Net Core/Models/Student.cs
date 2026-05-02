using System.ComponentModel.DataAnnotations;

namespace ASP.Net_Core.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        [MaxLength(255,ErrorMessage ="Name is required field")]
        public string Name { get; set; }
        [Required]
        public DateTime Enrolled { get; set; }
        public ICollection<StudentCourse> Enrollment { get; set; }
        [Required]
        public String ProfileImage { get; set; }
    }
}
