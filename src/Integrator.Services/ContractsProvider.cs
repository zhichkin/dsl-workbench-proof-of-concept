using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneCSharp.Integrator.Services
{
    public interface IContractsProvider
    {
        List<Assembly> GetContracts();
        void AddContract(Assembly assembly);
    }
    public sealed class ContractsProvider : IContractsProvider
    {
        private readonly List<Assembly> _contracts = new List<Assembly>();
        public void AddContract(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (_contracts.Contains(assembly)) return;
            _contracts.Add(assembly);
        }
        public List<Assembly> GetContracts()
        {
            return _contracts;
        }
    }
}