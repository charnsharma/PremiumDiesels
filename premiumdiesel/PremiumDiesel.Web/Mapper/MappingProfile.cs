using AutoMapper;
using PremiumDiesel.Model.DTOs;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Web.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                // customer
                cfg.CreateMap<Customer, CustomerDTO>();
                cfg.CreateMap<CustomerDTO, Customer>();
                cfg.CreateMap<Customer, CustomerHistory>();
                // client
                cfg.CreateMap<Client, ClientDTO>();
                cfg.CreateMap<ClientDTO, Client>();
                //clientCustomer
                cfg.CreateMap<ClientCustomer, ClientCustomerDTO>();
                cfg.CreateMap<ClientCustomerDTO, ClientCustomer>();
                cfg.CreateMap<ClientCustomerHistory, ClientCustomer>();
                cfg.CreateMap<ClientCustomer, ClientCustomerHistory>();
                // clientUser
                cfg.CreateMap<ClientUser, ClientUserDTO>();
                cfg.CreateMap<ClientUserDTO, ClientUser>();
                // customerLocation
                cfg.CreateMap<CustomerLocation, CustomerLocationDTO>();
                cfg.CreateMap<CustomerLocationDTO, CustomerLocation>();
                cfg.CreateMap<CustomerLocation, CustomerLocationHistory>();
                // product
                cfg.CreateMap<ProductDTO, Product>();
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<Product, ProductHistory>();
                cfg.CreateMap<ProductHistory, Product>();
                // workOrder
                cfg.CreateMap<WorkOrderDTO, WorkOrder>();
                cfg.CreateMap<WorkOrder, WorkOrderDTO>();
                cfg.CreateMap<WorkOrder, WorkOrderHistory>();
                cfg.CreateMap<WorkOrderHistory, WorkOrder>();
            });
        }
    }
}