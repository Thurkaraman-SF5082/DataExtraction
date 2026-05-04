using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataExtraction.Models
{
    [Table("BoldInsights")]
    public class BoldInsightsEntity
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("assignee")]
        public string Assignee { get; set; }
        [Column("Amount After Discount")]
        public double AmountAfterDiscount { get; set; }
        // [Column("Commission")]
        public double Commission { get; set; }
        [Column("Payment Frequency")]
        public string PaymentFrequency { get; set; }
    }
}