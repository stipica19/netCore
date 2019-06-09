using ozo.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ozo.Util.Logging
{
  public class ozoLoggerProvider : ILoggerProvider
  {
    private IServiceProvider serviceProvider;
    private Func<LogLevel, bool> filter;
    public ozoLoggerProvider(IServiceProvider serviceProvider, Func<LogLevel, bool> filter)
    {
      this.filter = filter;
      this.serviceProvider = serviceProvider;
    }
    public ILogger CreateLogger(string categoryName)
    {      
      return new ozoLogger(serviceProvider, filter);
    }

    public void Dispose()
    {
      
    }
  }
}
