using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_project.Models
{
    public class OrderSummary
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderSummaryId { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public string GamesOrdered { get; set; }
        public int OrderNumber { get; set; }
    }

}
