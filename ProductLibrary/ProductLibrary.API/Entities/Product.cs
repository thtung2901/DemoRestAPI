using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductLibrary.API.Entities
{
    public class Product
    {
        [Key]       
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public Guid CategoryId { get; set; }
    }
}
