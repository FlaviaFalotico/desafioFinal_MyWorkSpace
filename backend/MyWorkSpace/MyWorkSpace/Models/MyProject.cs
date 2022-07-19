using MyWorkSpace.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace MyWorkSpace.Models
{
    public class MyProject
    {        
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        List<MyTask> Tasks { get; set; }
    }
}