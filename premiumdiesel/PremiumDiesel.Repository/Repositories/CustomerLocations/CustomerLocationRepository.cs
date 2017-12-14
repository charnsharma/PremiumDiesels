using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.CustomerLocations
{
    public class CustomerLocationRepository : Repository<CustomerLocation>, ICustomerLocationRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public CustomerLocationRepository(ApplicationDbContext context) : base(context)
        {
        }

        #region Read

        /// <summary>
        /// Returns al the locations of a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IEnumerable<CustomerLocation> GetByCustomer(int customerId)
        {
            Expression<Func<CustomerLocation, bool>> customerLocationPredicate = cl => cl.CustomerId == customerId;
            var customerLocations = Find(customerLocationPredicate);

            return customerLocations;
        }

        /// <summary>
        /// Returns the (active) head office location of a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public CustomerLocation GetHeadOffice(int customerId)
        {
            //Expression<Func<CustomerLocation, bool>> customerLocationPredicate = cl => cl.CustomerId == customerId && cl.Type == LocationType.HeadOffice;
            //var customerLocation = Find(customerLocationPredicate).SingleOrDefault();

            var customerLocation = (from cl in ApplicationContext.CustomerLocations
                                    where
                                    cl.CustomerId == customerId
                                    && cl.Type == LocationType.HeadOffice
                                    && cl.Status == Status.Active
                                    select cl).SingleOrDefault();

            return customerLocation;
        }

        #endregion Read
    }
}