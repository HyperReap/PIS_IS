using System.ComponentModel.DataAnnotations;

namespace Hotel_PIS.DAL
{
    public class BasePerson : BaseModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [EmailAddress]
        //TOOD consider required
        public string Email { get; set; }
    }
}