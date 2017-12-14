using System;
using System.Collections.Generic;
using PremiumDiesel.Model.DTOs;

namespace PremiumDiesel.Service.ClientCustomers
{
    public interface IClientCustomerService : IDisposable
    {
        IEnumerable<ClientCustomerDTO> GetAll();

        ClientCustomerDTO Get(int id);

        IEnumerable<ClientCustomerDTO> GetClientCustomers();

        ClientCustomerDTO Update(int id, ClientCustomerDTO clientCustomerDTO);

        ClientCustomerDTO Create(ClientCustomerDTO clientCustomerDTO);

        bool Delete(int id);

        bool Delete(int? clientId, int customerId);

        bool DeleteAll(int customerId);
    }
}