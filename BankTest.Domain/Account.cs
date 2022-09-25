using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Initial balance greater or equal to $100 is required")]
        public decimal Balance { get; set; }
        
        [ForeignKey("UserId")]
        [Required(ErrorMessage = "An User must be referenced")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

    }
}
