using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ambition.Entities.Common
{
    public class User: Entity
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }

        [StringLength(11)]
        public string MobilePhone { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsMobilePhoneConfirmed { get; set; }
    }
}
