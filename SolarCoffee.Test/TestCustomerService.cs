using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Services;
using SolarCoffee.Data;
using SolarCoffee.Services.Customer;
using SolarCoffee.Data.Models;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using Moq;

namespace SolarCoffee.Test
{
    public class TestCustomerService
    {
        [Fact]
        public void CustomerService_GetsAllCustomers_GivenTheyExist(){
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("gets_all").Options;

            using var context = new SolarDbContext(options);
            //sut stands for "system under test"
            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer { Id = 122322 });
            sut.CreateCustomer(new Customer { Id = 213 });

            var allCustomers = sut.GetAllCustomers();
            allCustomers.Count.Should().Be(2);
        }

        [Fact]
        public void CustomerService_CreatesCustomer_GivenNewCustomerObject() {
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("Add_writes_to_database").Options;

            using var context = new SolarDbContext(options);
            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer { Id = 18645 });
            context.Customers.Single().Id.Should().Be(18645);
        }

        [Fact]
        public void CustomerService_DeleteCustomer_Givenid()
        {
            var options = new DbContextOptionsBuilder<SolarDbContext>()
                .UseInMemoryDatabase("delete_one").Options;

            using var context = new SolarDbContext(options);
            var sut = new CustomerService(context);

            sut.DeleteCustomer(18645);
            var allCustomers = sut.GetAllCustomers();
            allCustomers.Count().Should().Be(0);
        }

        [Fact]
        public void CustomerService_OrderByLastName_WhenGetAllCusomersInvoked()
        {
            //Arrange
            var data = new List<Customer> {
              new Customer{ Id=123, LastName="Zulu"},
              new Customer { Id=323, LastName="Alpha"},
              new Customer { Id=-89, LastName="Xu"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();

            mockSet.As<IQueryable<Customer>>()
                .Setup(g => g.Provider)
                .Returns(data.Provider);

            mockSet.As<IQueryable<Customer>>()
                .Setup(g => g.ElementType).Returns(data.ElementType);

            mockSet.As<IQueryable<Customer>>()
                .Setup(g => g.GetEnumerator())
                .Returns(data.GetEnumerator());

            var mockContext = new Mock<SolarDbContext>();
            mockContext.Setup(g => g.Customers)
                .Returns(mockSet.Object);

            //Act
            var sut = new CustomerService(mockContext.Object);
            var customer = sut.GetAllCustomers();

            //Assert
            customer.Count.Should().Be(3);
            customer[0].Id.Should().Be(323);
            customer[1].Id.Should().Be(-89);
            customer[2].Id.Should().Be(123);
        }



    }
}
