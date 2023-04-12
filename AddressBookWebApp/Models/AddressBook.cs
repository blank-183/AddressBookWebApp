using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBookWebApp.Models
{
    public class AddressBook
    {
        [Key]
        public int AddressBookId { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Address Line")]
        [Required]
        public string AddressLine { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string Province { get; set; }

        [DisplayName("Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }

        [NotMapped]
        public string FullName
        {
            get { 
                if (MiddleName != null)
                {
                    return FirstName + " " + MiddleName + " " + LastName;
                }
                
                return FirstName + " " + LastName; 
            }
        }
    }
}
