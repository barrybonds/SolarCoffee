using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarCoffee.Web.ViewModels
{
    /// <summary>
    /// View for open SalesOrder 
    /// </summary>
    public class InvoiceModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public int CustomerId { get; set; }
        public List<SalesOrderItemModel> LineItems { get; set; }

        public bool IsPaid { get; set; }
    }

    /// <summary>
    /// View Model for Sales Order Items
    /// </summary>
    public class SalesOrderItemModel
    {
       public int Id { get; set; }
        public int Qunatity { get; set; }
        public ProductModel Product { get; set; }
    }
}
