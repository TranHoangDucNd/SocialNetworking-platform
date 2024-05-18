using Microsoft.AspNetCore.Mvc.RazorPages;
using WebDating.Entities.ProfileEntities;

namespace WebDating.DTOs
{
    public class TestDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string KnowAs { get; set; }
        public string Gender { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public Interest InteresName { get; set; }
        public string City { get; set; }
        public int DatingProfileId { get; set; }
        public int Age { get; set; }
        public Gender DatingObject { get; set; }
    }
}
