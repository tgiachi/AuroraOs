using Microsoft.Practices.Unity;
using System;
using System.Web.Http;
using Unity.WebApi;

namespace AuroraOs.WebApi
{
    public class UnityConfig
    {
        public Lazy<UnityContainer> Container = new Lazy<UnityContainer>(() =>
        {
            return new UnityContainer();

        });
    }
}