using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    public interface IService
    {
        bool ServiceCall();
    }

    public class ClassWithDependencies : ControllerBase
    {
        private readonly IService _service;

        public ClassWithDependencies(IService service)
        {
            _service = service;
        }

        public void Do()
        {
            var res = _service.ServiceCall();
            if (!res)
            {
                throw new Exception();
            }
        }
    }
}
