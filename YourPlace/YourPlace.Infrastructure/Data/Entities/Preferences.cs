using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Preferences
    {
        [Key]
        public int PreferencesID { get; set; }

        [Required]
        public Location Location { get; set; }

        [Required]
        public Tourism Tourism { get; set; }

        [Required]
        public Atmosphere Atmosphere { get; set; }

        [Required]
        public Company Company { get; set; }

        [Required]
        public Pricing Pricing { get; set; }

        [AllowNull]
        [NotMapped]
        public string UserId { get; set; }

        ///<summary>
        /// в следващ етап от развитието на приложението за всеки потребител ще се пазят неговите предпочитания и той ще може да ги редактира
        /// това е удобно и с цел персонализирани реклами и оферти
        /// </summary>

        public Preferences()
        {

        }
        public Preferences(Location location, Tourism tourism, Atmosphere atmosphere, Company company, Pricing pricing)
        {
            Location = location;
            Tourism = tourism;
            Atmosphere = atmosphere;
            Company = company;
            Pricing = pricing;
        }
    }
}
