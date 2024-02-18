using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain;

namespace Customer.API.Entities
{
    public class CatalogCustomer : EntityBase<int>
    {
        [Required]
        [MaxLength(15)]
        public string? UserName { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string? FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string? EmailAddress { get; set; }
    }
}
