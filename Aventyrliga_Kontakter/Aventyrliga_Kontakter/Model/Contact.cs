using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Aventyrliga_Kontakter.Model
{
    public class Contact
    {
        public int ContactId { get; set; }


        [Required(ErrorMessage = "En E-post måste anges!")]
        [StringLength(50, ErrorMessage = "Epost ska vara max 50 tecken.")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$", ErrorMessage = "E-post verkar inte vara korrekt.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Förnamnet måste anges!")]
        [StringLength(50, ErrorMessage = "Förnamnet kan bestå av som mest 50 tecken.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Efternamnet måste anges!")]
        [StringLength(50, ErrorMessage = "Förnamnet kan bestå av som mest 50 tecken.")]
        public string LastName { get; set; }
    }
}