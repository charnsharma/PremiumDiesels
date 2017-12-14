using System;
using System.Collections.Generic;
using PremiumDiesel.Model.DTOs;

namespace PremiumDiesel.Service.Clients
{
    public interface IClientService : IDisposable
    {
        IEnumerable<ClientDTO> GetAll();

        ClientDTO Get(int id);

        ClientDTO Update(int id, ClientDTO clientDTO);

        ClientDTO Create(ClientDTO clientDTO);

        bool Delete(int id);
    }
}