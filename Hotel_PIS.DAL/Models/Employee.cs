using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hotel_PIS.DAL
{
    public class Employee : BasePerson
    {
        public string Password { get; set; }
        /// <summary>
        /// null meaning -> unlimited
        /// </summary>
        public DateTime? ContractDueDae { get; set; }
        public int? RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        [JsonIgnore]
        public Role? Role { get; set; }

    }
}