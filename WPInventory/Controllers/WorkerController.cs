//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using WPInventory.Data.Models.EF;

//namespace WPInventory.Controllers
//{

//    [ApiController]
//    [Route("api/[controller]")]
//    public class WorkerController : Controller
//    {
//        private readonly IConfiguration _config;
//        private readonly ComputerInfoContext _context;
//        private readonly ILogger<WorkerController> _logger;

//        public WorkerController(IConfiguration config, ComputerInfoContext context, ILogger<WorkerController> logger)
//        {
//            _config = config;
//            _context = context;
//            _logger = logger;
//        }
//        [HttpPost("RunSearchWorker")]
//        public async Task<ActionResult<WorkerStartingResult>> RunSearchWorker()
//        {
//            var workerName = _config.GetSection("WorkerPath").ToString();
//            var process = Process.Start(workerName);
//            var result = new WorkerStartingResult();
//            if (process == null)
//            {
//                var message = "Не удалось запустить процесс";
//                _logger.LogError(message);
//                result.Message = message;
//            }
//            process.WaitForExit();
//            result.ExitCode = process.ExitCode;
//            return Ok(result);
//        }
//    }
//}
