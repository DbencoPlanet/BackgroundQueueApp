using BackgroundQueueApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundQueueApp.Controllers
{
    public class AuthenticateController : Controller
    {
        // and more..
        private readonly IMailService _mailService;
        private readonly IBackgroundTaskQueue _queue;
        // and more..
        public AuthenticateController(
            IMailService mailService,
            IBackgroundTaskQueue queue)
        {

            _mailService = mailService;
            _queue = queue;
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            // and more...
            // Queue processing
            await _queue.QueueBackgroundWorkItemAsync(async (token) =>
            {
                await _mailService.SendAsync(mailData, token);
            });

            return Ok();
        }
       
    }

   
}
