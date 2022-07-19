using System.ComponentModel.DataAnnotations;

namespace MyWorkSpace.Models
{
    public class MyTask
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

        public int MyProjectId { get; set; }
        public MyProject MyProject { get; set; }
    }
}
