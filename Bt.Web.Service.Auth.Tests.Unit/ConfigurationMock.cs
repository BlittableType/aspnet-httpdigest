using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Bt.Web.Service.Auth.Tests.Unit
{
    public class ConfigurationMock : IConfiguration
    {
        private readonly IDictionary<string, string> _entries;

        public ConfigurationMock(IDictionary<string, string> entries)
        {
            _entries = entries;
        }
        
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new System.NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new System.NotImplementedException();
        }

        public string this[string key]
        {
            get => _entries[key];
            set => throw new System.NotImplementedException();
        }
    }
}