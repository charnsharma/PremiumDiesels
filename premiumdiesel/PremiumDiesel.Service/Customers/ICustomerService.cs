using System;
using System.Collections.Generic;
using PremiumDiesel.Model.DTOs;

namespace PremiumDiesel.Service.Customers
{
    public interface ICustomerService : IDisposable
    {
        IEnumerable<CustomerDTO> GetAll();

        CustomerDTO Get(int id);

        IEnumerable<CustomerDTO> GetCustomers();

        CustomerDTO Update(int id, CustomerDTO customerDTO);

        CustomerDTO Create(CustomerDTO customerDTO);

        bool Delete(int id);

        bool Remove(int id);
    }
}