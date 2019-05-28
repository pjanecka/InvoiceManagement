using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InvoiceManagement.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        [DisplayName("Bill To")]
        public string BillTo { get; set; }
        
        //[Required]
        public List<Item> Items { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double Total { get => Items?.Sum(x => x.Price * x.Quantity) ?? 0.0; }

        public bool Paid { get; set; }
    }
}
