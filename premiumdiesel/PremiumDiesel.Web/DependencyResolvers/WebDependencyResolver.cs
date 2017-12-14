using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace PremiumDiesel.Web
{
    internal class WebDependencyResolver : IDependencyResolver
    {
        private IUnityContainer _unityContainer;

        public WebDependencyResolver(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            if (_unityContainer == null)
            {
                throw new ArgumentNullException("container");
                }
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _unityContainer.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _unityContainer.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            _unityContainer.Dispose();
        }
    }
}