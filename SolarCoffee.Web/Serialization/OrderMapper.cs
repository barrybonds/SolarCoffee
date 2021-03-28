using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Serialization
{
    /// <summary>
    /// Handles mapping Order data models to and from related View Models
    /// </summary>
    public static class OrderMapper
    {
        /// <summary>
        /// Maps an InvoiceModel  view modelto a SalesOrder data Model
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public static SalesOrder SerializeInvoiceToOrder(InvoiceModel invoice)
        {
            var salesOrderItems = invoice.LineItems.Select(item => new SalesOrderItem { 
                    Id = item.Id,
                    Quantity = item.Qunatity,
                    Product = ProductMapper.SerializeProductModel(item.Product)
                    }).ToList();

            return new SalesOrder
            {
                SalesOrderItems = salesOrderItems,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };
        }

        /// <summary>
        ///  Maps a collection of SalesOrders (data) to OrderModels (view models)
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static List<OrderModel> SerializeOrdersToViewModels(IEnumerable<SalesOrder> orders) {
            return orders.Select(order => new OrderModel
            {
                Id = order.Id,
                CreatedOn = order.CreatedOn,
                Updatedon = order.UpdatedOn,
                SalesOrderItems = SerializesSalesOrderItems(order.SalesOrderItems),
                Customer = CustomerMapper.SerializeCustomer(order.Customer),
                IsPaid = order.IsPaid
            }).ToList();
        }
        /// <summary>
        /// Maps a collection of SaleOrderItems (data) to SalesOrderItemsModels (view Models)
        /// </summary>
        /// <param name="orderItems"></param>
        /// <returns></returns>
        private static List<SalesOrderItemModel> SerializesSalesOrderItems(IEnumerable<SalesOrderItem> orderItems) {
            return orderItems.Select(item => new SalesOrderItemModel
            {
                Id = item.Id,
                Qunatity = item.Quantity,
                Product = ProductMapper.SerializeProductModel(item.Product)
            }).ToList();
        }



    }
}
