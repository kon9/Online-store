using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Library.OrdersService.Models
{
    public class Order : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        [Required]
        public Guid AddressId { get; set; }

        [Required]
        [Column(TypeName = "uniqueidentifier")]
        public Guid UserId { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        public List<OrderedArticle> Articles { get; set; }
    }
}