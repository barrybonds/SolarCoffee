using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _db;
        public CustomerService(SolarDbContext dbContext) {
            _db = dbContext;
        }
        /// <summary>
        /// Adds a new Customer record
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>ServiceResponse<Customer></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = true,
                    Message = "The customer added",
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = false,
                    Message = e.StackTrace ,
                    Time = DateTime.UtcNow,
                    Data = customer
                };

            }
        }
        /// <summary>
        /// Delete a customer record
        /// </summary>
        /// <param name="id">int customer primary key</param>
        /// <returns>ServiceResponse<bool></returns>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var cusomer = _db.Customers.Find(id);
            var now = DateTime.UtcNow;

            if (cusomer == null) {
                return new ServiceResponse<bool>
                {
                    Time = now,
                    IsSuccess = false,
                    Message = "Customer to delet not found",
                    Data = false
                };
            }
            try
            {
                _db.Customers.Remove(cusomer);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Customer Deleted",
                    Time = now,
                    Data = true
                };


            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Time = now,
                    Data = false
                };
            }

        }
        /// <summary>
        /// Retrieves a list of Customers from the database
        /// </summary>
        /// <returns>List<Customer></returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers
                 .Include(customer => customer.PrimaryAddress)
                 .OrderBy(customer => customer.LastName)
                 .ToList();
        }
        /// <summary>
        /// Get a customer record by primary key
        /// </summary>
        /// <param name="id">int customer primary key</param>
        /// <returns>ServiceResponse<bool></returns>
        public Data.Models.Customer GetById(int id)
        {
            return _db.Customers.Find(id);
        }
    }
}
