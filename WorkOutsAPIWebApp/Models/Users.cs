using System.ComponentModel.DataAnnotations;

namespace WorkOutsAPIWebApp.Models
{
    public class Users
    {
        public Users()
        {
            WorkOuts = new List<WorkOuts>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Нікнейм")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        public string Email { get; set; }

        public virtual ICollection<WorkOuts> WorkOuts { get; set; }
    }
}