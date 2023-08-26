using OnlineStore.Library.Common.Interfaces;
using OnlineStore.Library.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Library.ArticleService.Models
{
    public class PriceList : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        [ForeignKey("Article")]
        public Guid ArticleId { get; set; }

        [Column(TypeName = "numeric(12,4)")]
        public decimal Price { get; set; }

        [Required] public string Name { get; set; } = PriceListNames.Default;

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime ValidTo { get; set; }
    }
}