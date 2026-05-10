using System.ComponentModel.DataAnnotations;

namespace WorkOutsAPIWebApp.Models
{
    public class Category
    {
        public Category()
        {
            WorkOuts = new List<WorkOuts>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Назва категорії обов'язкова")]
        [Display(Name = "Категорія")]
        public string CategoryName { get; set; }

        public virtual ICollection<WorkOuts> WorkOuts { get; set; }
    }
}