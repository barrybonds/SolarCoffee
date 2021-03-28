using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolarCoffee.Services.Order
{
   public interface IOrderService
    {
        List<SalesOrder> GetOrders();
        ServiceResponse<bool> GenerateInvoiceFororder(SalesOrder order);
        ServiceResponse<bool> MarkFulfilled(int id);
        ServiceResponse<bool> GenerateOpenOrder(SalesOrder order);
    }
}
