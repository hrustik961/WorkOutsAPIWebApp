using System.ComponentModel.DataAnnotations;

namespace WorkOutsAPIWebApp.Models
{
    public class WorkOuts
    {
        public WorkOuts()
        {
            Exercises = new List<Exercises>();
            Categories = new List<Category>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Назва обов'язкова")]
        public string Name { get; set; }

        public string Description { get; set; }
        public int Duration { get; set; }
        public string Level { get; set; }
        public string Equipment { get; set; }
        public int UsersId { get; set; }
        public virtual Users Users { get; set; }

        public virtual ICollection<Exercises> Exercises { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}