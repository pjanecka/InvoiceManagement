using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double Total { get => Price * Quantity;  }
    }
}
