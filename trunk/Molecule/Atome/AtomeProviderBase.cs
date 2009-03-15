using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Molecule.Configuration;
using Molecule.Runtime;
using Molecule.Web;

namespace Molecule.Atome
{
    public abstract class AtomeProviderBase<TAtome, IProvider>
        where TAtome : AtomeProviderBase<TAtome, IProvider>
        where IProvider : class, Molecule.Atome.IProvider
    {
        private string providerName;
        private IProvider providerInstance;

        protected static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TAtome));

        protected AtomeProviderBase()
        {
            providerName = ConfigurationClient.Client.Get<string>(ConfigurationNamespace, "LibraryProvider", DefaultProvider);
            OnProviderUpdated();
        }

        protected static TAtome instance
        {
            get
            {
                return Singleton<TAtome>.Instance;
            }
        }

        private object providerInstanceLock = new object();

        private void resetProviderInstance()
        {
            lock (providerInstanceLock)
            {
                providerInstance = null;
            }
        }

        private IProvider getProviderInstance()
        {
            if (providerInstance == null)
            {
                lock (providerInstanceLock)
                {
                    if (providerInstance == null)
                    {
                        providerInstance = Plugin<IProvider>.CreateInstance(providerName, ProviderDirectory);
                        providerInstance.Initialize();
                    }
                }
            }
            return providerInstance;
        }

        protected void CallProvider(Action<IProvider> action)
        {
            try
            {
                action(getProviderInstance());
            }
            catch (Exception ex)
            {
                throw new ProviderException(providerName, ex);
            }
        }

        public static string CurrentProvider
        {
            get { return instance.providerName; }
            set
            {
                instance.providerName = value;
                instance.resetProviderInstance();
                ConfigurationClient.Client.Set<string>(instance.ConfigurationNamespace, "LibraryProvider", value);
                instance.OnProviderUpdated();
            }
        }

        protected abstract void OnProviderUpdated();

        protected abstract string ConfigurationNamespace
        {
            get;
        }

        protected abstract string ProviderDirectory
        {
            get;
        }

        protected abstract string DefaultProvider
        {
            get;
        }

        public static IEnumerable<ProviderInfo> Providers
        {
            get
            {
                foreach (var provider in Plugin<IProvider>.List(instance.ProviderDirectory))
                    yield return new ProviderInfo() { Description = provider.Description, Name = provider.Name };
            }
        }
    }
}
