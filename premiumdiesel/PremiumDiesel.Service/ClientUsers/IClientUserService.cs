using System;
using System.Collections.Generic;
using PremiumDiesel.Model.DTOs;

namespace PremiumDiesel.Service.ClientUsers
{
    public interface IClientUserService : IDisposable
    {
        ClientUserDTO GetClientUserByUserId(string userId);

        int? GetUserClientId(string userId);

        IEnumerable<ClientUserDTO> GetUsers();

        ClientUserDTO Update(int id, ClientUserDTO clientUserDTO);

        ClientUserDTO Create(ClientUserDTO clientUserDTO);

        bool Delete(int id);

        bool Delete(int clientId, string userId);

        bool DeleteAll(int clientId);
    }
}