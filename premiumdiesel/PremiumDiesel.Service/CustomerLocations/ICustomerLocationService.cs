using System;
using System.Collections.Generic;
using PremiumDiesel.Model.DTOs;

namespace PremiumDiesel.Service.CustomerLocations
{
    public interface ICustomerLocationService : IDisposable
    {
        IEnumerable<CustomerLocationDTO> GetAll();

        IEnumerable<CustomerLocationDTO> GetAllMyCustomerLocations();

        CustomerLocationDTO Get(int id);

        CustomerLocationDTO GetHeadOffice(int customerId);

        bool HasHeadOffice(int customerId);

        IEnumerable<CustomerLocationDTO> GetCustomerLocationsByCustomerId(int customerId);

        CustomerLocationDTO Update(int id, CustomerLocationDTO customerLocationDTO);

        CustomerLocationDTO Create(CustomerLocationDTO customerLocationDTO);

        bool Delete(int id);
    }
}