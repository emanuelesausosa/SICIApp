using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SICIApp.Interfaces;
using SICIApp.Services;
using SICIApp.Entities;
using SICIApp.Dominio;
using Ninject;

namespace SICIApp.Infrastructure
{
    public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            this.ninjectKernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return this.ninjectKernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.ninjectKernel.GetAll(serviceType);
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {

            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<ILogger>().To<Logger>();
            ninjectKernel.Bind<IDBRepository>().To<DBRepository>();
        }
    }
}