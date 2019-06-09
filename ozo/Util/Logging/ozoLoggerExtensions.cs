using Microsoft.Extensions.Logging;
using System;

namespace ozo.Util.Logging
{
  public static class ozoLoggerExtensions
  {
    public static ILoggerFactory AddozoLogger(this ILoggerFactory factory, IServiceProvider serviceProvider,  Func<LogLevel, bool> filter = null)
    {
      factory.AddProvider(new ozoLoggerProvider(serviceProvider, filter));
      return factory;
    }
  }
}
