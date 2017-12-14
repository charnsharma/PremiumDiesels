using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;

namespace PremiumDiesel.Web
{
    public class MyHelper
    {
        //public int CurrentClientId
        //{
        //    get
        //    {
        //        // get the current userId get the corresponding clientId

        //    }
        //}

        public static string CurrentUserId
        {
            get
            {
                return System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
        }

        public static ApplicationUser CurrentUser
        {
            get
            {
                //return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var currentUser = new ApplicationDbContext().Users.FirstOrDefault(x => x.Id == currentUserId);
                return currentUser;
            }
        }

        public static ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public static ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        public static List<SelectListItem> GetAllRolesAsSelectList()
        {
            List<SelectListItem> SelectRoleListItems = new List<SelectListItem>();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var colRoleSelectList = roleManager.Roles.OrderBy(x => x.Name).ToList();

            SelectRoleListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in colRoleSelectList)
            {
                SelectRoleListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Name.ToString(),
                        Value = item.Name.ToString()
                    });
            }

            return SelectRoleListItems;
        }

        #region Public Methods

        public static string GetFullExceptionMessage(Exception ex)
        {
            string exMessage = ex.Message;

            if (ex.TargetSite != null)
                exMessage = "\"" + exMessage + "\" in " + ex.TargetSite.ReflectedType.Name + "." + ex.TargetSite.Name + "()";

            if (ex.InnerException != null)
            {
                if (!string.IsNullOrEmpty(ex.InnerException.Message))
                    exMessage = exMessage + " ---> " + ex.InnerException.Message;
                if (ex.InnerException.InnerException != null)
                {
                    if (!string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                        exMessage = exMessage + " --->" + ex.InnerException.InnerException.Message;
                }
            }

            return exMessage;
        }

        public static bool UserIsAtLeast(string minimumRole)
        {
            bool retval = false;

            try
            {
                var currentUserRole = HttpContext.Current.User;
                switch (minimumRole)
                {
                    case UserRoles.SuperAdmin:
                        if (currentUserRole.IsInRole(UserRoles.SuperAdmin))
                            retval = true;
                        break;

                    case UserRoles.Admin:
                        if (currentUserRole.IsInRole(UserRoles.SuperAdmin) || currentUserRole.IsInRole(UserRoles.Admin))
                            retval = true;
                        break;

                    case UserRoles.User:
                        if (currentUserRole.IsInRole(UserRoles.SuperAdmin) || currentUserRole.IsInRole(UserRoles.Admin) || currentUserRole.IsInRole(UserRoles.User))
                            retval = true;
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return retval;
        }

        #endregion Public Methods
    }
}