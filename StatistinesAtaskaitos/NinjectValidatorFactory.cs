using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Ninject;

namespace StatistinesAtaskaitos
{
    public class NinjectValidatorFactory : IValidatorFactory
    {
        private readonly IKernel _kernel;

        public NinjectValidatorFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IValidator<T> GetValidator<T>()
        {
            return (IValidator<T>)GetValidator(typeof(T));
        }

        public IValidator GetValidator(Type type)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(type);
            return (IValidator)_kernel.TryGet(validatorType);
        }
    }
}