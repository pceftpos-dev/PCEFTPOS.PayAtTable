﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using TinyIoC;

namespace PayAtTable.API.Helpers
{
    public class TinyIocWebApiDependencyResolver : IDependencyResolver
    {
        private bool _disposed;
        private TinyIoCContainer _container;

        public TinyIocWebApiDependencyResolver(TinyIoCContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            if (_disposed)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            return new TinyIocWebApiDependencyResolver(_container.GetChildContainer());
        }

        public object GetService(Type serviceType)
        {
            if (_disposed)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            try
            {
                return _container.Resolve(serviceType);
            }
            catch (TinyIoCResolutionException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (_disposed)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (TinyIoCResolutionException)
            {
                return Enumerable.Empty<object>();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _container.Dispose();

            _disposed = true;
        }
    }

}