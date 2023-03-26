using DotNetCoreHangfire.Services.Interfaces;

namespace DotNetCoreHangfire.Services.Implements
{
    public class JobService : IJobService
    {
        public readonly ILogger<JobService> _logger;

        public JobService(ILogger<JobService> logger) 
        {
            _logger = logger;
        }

        public void ContinuationJob()
        {
            _logger.LogInformation($"{nameof(ContinuationJob)} started at {DateTime.UtcNow}");
        }

        public void DelayedJob()
        {
            _logger.LogInformation($"{nameof(DelayedJob)} started at {DateTime.UtcNow}");
        }

        public void FireAndForgetJob()
        {
            _logger.LogInformation($"{nameof(FireAndForgetJob)} started at {DateTime.UtcNow}");
        }

        public void ReccuringJob()
        {
            _logger.LogInformation($"{nameof(ReccuringJob)} started at {DateTime.UtcNow}");
        }
    }
}
