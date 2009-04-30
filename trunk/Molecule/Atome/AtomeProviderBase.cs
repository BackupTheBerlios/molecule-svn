using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Molecule.Configuration;
using Molecule.Runtime;
using Molecule.Web;
using System.Timers;

namespace Molecule.Atome
{
    public abstract class AtomeProviderBase<TAtome, IProvider>
        where TAtome : AtomeProviderBase<TAtome, IProvider>
        where IProvider : class, Molecule.Atome.IProvider
    {
        const string libraryProviderConfKey = "LibraryProvider";
        const string CommonConfigurationNamespace = "AtomeProviderBase";
        const string autoUpdateIntervalConfKey = "AutoUpdateInterval";
        const int defaultUpdateInterval = 3600;//seconds

        private string providerName;
        private IProvider provider;
        private Timer timer;

        protected static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TAtome));

        protected AtomeProviderBase()
        {
            providerName = ConfigurationClient.Get<string>(
                ConfigurationNamespace, libraryProviderConfKey, DefaultProvider);
            
            var autoUpdateInterval = TimeSpan.FromSeconds(
                ConfigurationClient.Get(CommonConfigurationNamespace, "AutoUpdateInterval", defaultUpdateInterval));
            timer = new Timer(autoUpdateInterval.TotalMilliseconds);
            timer.AutoReset = true;
            timer.Elapsed += (s, t) =>
            {
                Singleton<TAtome>.Instance.resetProvider();
                if (log.IsInfoEnabled)
                    log.Info("Provider reset.");
            };
        }

        protected static TAtome Instance
        {
            get
            {
                var instance = Singleton<TAtome>.Instance;
                if (instance.provider == null)
                {
                    if (log.IsInfoEnabled)
                        log.Info("Updating data with provider "+CurrentProvider);
                    instance.OnProviderUpdated();
                    if (log.IsInfoEnabled)
                        log.Info("Data updated.");
                }
                return instance;
            }
        }

        private object providerLock = new object();

        private void resetProvider()
        {
            lock (providerLock)
            {
                provider = null;
            }
        }

        private IProvider getProvider()
        {
            if (provider == null)
            {
                lock (providerLock)
                {
                    if (provider == null)
                    {
                        provider = Plugin<IProvider>.CreateInstance(providerName, ProviderDirectory);
                        provider.Initialize();
                    }
                }
            }
            return provider;
        }

        protected void CallProvider(Action<IProvider> action)
        {
            try
            {
                action(getProvider());
            }
            catch (Exception ex)
            {
                throw new ProviderException(providerName, ex);
            }
        }

        public static string CurrentProvider
        {
            get { return Singleton<TAtome>.Instance.providerName; }
            set
            {
                Singleton<TAtome>.Instance.providerName = value;
                Singleton<TAtome>.Instance.resetProvider();
                ConfigurationClient.Set<string>(ConfigurationNamespace, libraryProviderConfKey, value);
            }
        }

        protected abstract void OnProviderUpdated();

        protected static string ConfigurationNamespace
        {
            get { return typeof(TAtome).Name; }
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
                foreach (var provider in Plugin<IProvider>.List(Singleton<TAtome>.Instance.ProviderDirectory))
                    yield return new ProviderInfo() { Description = provider.Description, Name = provider.Name };
            }
        }
    }
}
