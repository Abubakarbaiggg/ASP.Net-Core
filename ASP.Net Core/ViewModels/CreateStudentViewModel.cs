using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.Net_Core.ViewModels
{
    public class CreateStudentViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Enrolled { get; set; }
        public IFormFile ProfileImage { get; set; }

        public IList<SelectListItem> Courses { get; set; }
    }
}
