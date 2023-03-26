using DotNetCoreHangfire.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreHangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public JobController(IJobService jobService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _jobService = jobService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager; 
        }

        [HttpGet("/FireAndForgetJob")]
        public IActionResult CreateFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _jobService.FireAndForgetJob());
            return Ok();
        }

        [HttpGet("/DelayedJob")]
        public IActionResult CreateDelayedJob(int delayedInSeconds)
        {
            _backgroundJobClient.Schedule(() => _jobService.DelayedJob(), TimeSpan.FromSeconds(delayedInSeconds));
            return Ok();
        }

        [HttpGet("/ReccuringJob")]
        public IActionResult CreateReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _jobService.ReccuringJob(), Cron.Minutely);
            return Ok();
        }

        [HttpGet("/ReccuringJobDaily")]
        public IActionResult CreateReccuringJobDaily(int hour, int minute)
        {
            _recurringJobManager.AddOrUpdate("dailyJobId", () => _jobService.ReccuringJob(), Cron.Daily(hour, minute), TimeZoneInfo.Local); // deault is utc time, use TimeZoneInfo.Local for local time
            return Ok();
        }

        [HttpGet("/ContinuationJob")]
        public IActionResult CreateContinuationJob()
        {
            var parentJobId = _backgroundJobClient.Enqueue(() => _jobService.FireAndForgetJob());
            _backgroundJobClient.ContinueJobWith(parentJobId, () => _jobService.ContinuationJob());

            return Ok();
        }
    }
}
