using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
using Tiveria.Common;
using Tiveria.Common.Extensions;
using System.Diagnostics;

namespace Toolsfactory.Home.Adapters.Common
{
    public class ServiceHost<T> where T : class
    {
        public readonly string ServiceName;
        private readonly string _envName;
        private readonly bool _debug;
        private readonly string _configFile = string.Empty;
        private readonly ILogger _Logger;
        private string _configDir = String.Empty;
        private bool _useConfigDir = true;

        public ServiceHost(string servicename, bool debug = false, string? configDirectory = null, string? configFile = null)
        {
            Ensure.That(servicename).IsNotNullOrWhiteSpace();
            var myWriter = new ConsoleTraceListener();
            Trace.Listeners.Add(myWriter); 
            Debug.AutoFlush = true;
            _debug = debug;
            _configFile = configFile ?? String.Empty;
            _envName = Environment.GetEnvironmentVariable("environment")?.ToLowerInvariant() ?? "production";
            ServiceName = servicename;

            SetupConfiguration(configDirectory);
            _Logger = SetupLogging();
            LogBasics();
        }

        private void SetupConfiguration(string? configDirectory)
        {
            WriteDebug("Checking configuration");
            if (_configFile.IsNullOrEmpty())
            {
                _configDir = GetConfigurationDirectory(configDirectory);
                WriteDebug($"Using config dir: {_configDir}");
                _useConfigDir = true;
            }
            else
            {
                WriteDebug($"Using config file: {_configFile}");
                _useConfigDir = false;
            }
        }

        private ILogger SetupLogging()
        {
            WriteDebug("Setting up Logging");
            if (_debug)
                Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            var config = CreateBaselineConfig(new ConfigurationBuilder()).Build();
            WriteDebug("Baseline Configuration:");
            WriteDebug(config.GetDebugView());

            var loggerconfig = new LoggerConfiguration()
                .WriteTo.Console();
            if (_debug)
                loggerconfig.MinimumLevel.Debug();
            else
                loggerconfig.MinimumLevel.Information();
            Log.Logger = loggerconfig.CreateBootstrapLogger();

            WriteDebug("Handing over logging to full logger");
            Log.Logger.Debug("Logger created");

            return Log.Logger;
        }

        private void LogBasics()
        {
            _Logger.Information("Service host created for: {ServiceName}", ServiceName);
            _Logger.Information("Hosting environment: {HostingEnvironment}", _envName);
            if (_useConfigDir)
                _Logger.Information("Configuration directory: {ConfigDir}", _configDir);
            else
                _Logger.Information("Configuration file: {ConfigFile}", _configFile);
        }

        private IConfigurationBuilder CreateBaselineConfig(IConfigurationBuilder builder)
        {
            if (_useConfigDir)
            {

                builder
                    .SetBasePath(_configDir)
                    .AddJsonFile($"appsettings.json", false)
                    .AddJsonFile($"appsettings.{_envName}.json", optional: true, reloadOnChange: true);
            }
            else
            {
                builder
                    .AddJsonFile(_configFile);
            }
            return  builder
                        .AddEnvironmentVariables()
                        .AddUserSecrets<T>(true);
        }

        public IHostBuilder BuildBaslineHost(string[] args)
        {
            _Logger.Debug("Creating HostBuilder");
            var builder = new HostBuilder();
            ConfigureHost(args, builder);
            ConfigureApp(args, builder);
            ConfigureServices(builder);
            ConfigureLogging(builder);
            ConfigureSystemD(builder);
            return builder;
        }

        private void ConfigureSystemD(HostBuilder builder)
        {
            builder.UseSystemd();
            _Logger.Debug("Systemd configured");
        }

        private void ConfigureLogging(HostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services);
            });
            _Logger.Debug("AppLogging added");
        }

        private void ConfigureServices(HostBuilder builder)
        {
            builder.ConfigureServices((hostContext, services) =>
            {
                services
                    .AddOptions()
                    .AddHttpClient();
            });
            _Logger.Debug("Basic services configured");
        }

        private void ConfigureApp(string[] args, HostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                CreateBaselineConfig(config)
                    .AddCommandLine(args);
            });
            _Logger.Debug("AppConfiguration configured");
        }

        private void ConfigureHost(string[] args, HostBuilder builder)
        {
            builder.ConfigureHostConfiguration((config) =>
            {
                config.SetBasePath(_configDir)
                    .AddJsonFile("hostsettings.json", true)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<T>(true)
                    .AddCommandLine(args);
            });
            _Logger.Debug("HostConfiguration configured");
        }

        private string GetConfigurationDirectory(string? configDirArg)
        {
            var confEnv = Environment.GetEnvironmentVariable(ServiceName.ToUpperInvariant() + "_CONFIGDIR");
            var fallback = Directory.GetCurrentDirectory();

            WriteDebug($"ConfigDir Parameter: {configDirArg}, Exists: {Directory.Exists(configDirArg)}");
            WriteDebug($"ConfigDir environment variable: {confEnv}, Exists: {Directory.Exists(confEnv)}");
            WriteDebug($"ConfigDir fallback: {fallback}, Exists: {Directory.Exists(fallback)}");

            if (!configDirArg.IsNullOrWhiteSpace() && Directory.Exists(configDirArg))
                return configDirArg;
            else
            {
                if (confEnv == null || !Directory.Exists(confEnv))
                    return Directory.GetCurrentDirectory();
                return confEnv;
            }
        }

        private void WriteDebug(string text)
        {
            /*if (_debug) */ Console.WriteLine("DBG: " + text);
            //Debug.WriteLineIf(_debug, text);
        }
    }
}