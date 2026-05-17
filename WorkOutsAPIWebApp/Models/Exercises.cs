using System.ComponentModel.DataAnnotations;

namespace WorkOutsAPIWebApp.Models
{
    public class Exercises
    {
        public Exercises()
        {
            WorkOuts = new List<WorkOuts>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Назва вправи обов'язкова")]
        public string Name { get; set; }

        public int ExecutionTime { get; set; }
        public string Description { get; set; }

        public virtual ICollection<WorkOuts>? WorkOuts { get; set; }
    }
}