using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Services;
using FluentAssertions;
using SolarCoffee.Data;
using SolarCoffee.Services.Customer;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Test
{
    public class TestCustomerService
    {
        [Fact]
        public void CustomerService_GetsAllCustomers_GivenTheyExist(){
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("gets_all").Options;

            using var context = new SolarDbContext(options);

            var sut = new CustomerService(context);

            sut.CreateCustomer( new Customer { Id = 122322 });
            sut.CreateCustomer( new Customer { Id = 213 });

            var allCustomers = sut.GetAllCustomers();
            allCustomers.Count.Should().Be(2);
        }


    }
}
