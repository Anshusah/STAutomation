using Cicero.Service.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleTransferAPI.Jobs
{
    public class GetDailyRatesJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            WebApiService.Instance.Get("api/getcountries");
            return Task.CompletedTask;
        }
    }
}
