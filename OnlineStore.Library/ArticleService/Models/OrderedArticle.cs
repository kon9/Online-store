using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineStore.Library.Common.Interfaces;
using OnlineStore.Library.OrdersService.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineStore.Library.ArticleService.Models
{
    public class OrderedArticle : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "numeric(12,4)")]
        public decimal Price { get; set; }

        [Column(TypeName = "int")]
        [Required]
        public int Quantity { get; set; }

        [NotMapped]
        public decimal Total => Price * Quantity;

        [Required] public string PriceListName { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime ValidTo { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        [JsonIgnore]
        public virtual Order Order { get; set; }
    }
}