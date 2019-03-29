namespace ContestPark.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LogController : BaseController
    {
        #region Private Variables

        private readonly IHostingEnvironment _env;

        #endregion Private Variables

        #region Constructors

        public LogController(IHostingEnvironment env)
        {
            _env = env;
        }

        #endregion Constructors

        #region Services

        /// <summary>
        /// Get logs
        /// </summary>
        /// <returns>Log files</returns>
        [HttpGet]
        public IActionResult LogList()
        {
            IEnumerable<string> files = Directory.GetFiles(_env.ContentRootPath + "/logs/MyLogs");
            string logText = "";
            foreach (string logTxt in files)
            {
                string fil = _env.ContentRootPath + "/logs/MyLogs/temp-log.txt";
                System.IO.File.Copy(logTxt, fil);
                logText += System.IO.File.ReadAllText(fil);
                System.IO.File.Delete(fil);
            }

            string[] log = logText.Split(new string[] { "[INF]" }, System.StringSplitOptions.None);
            List<LogModel> logs = new List<LogModel>();
            foreach (var logTxt in log)
            {
                logs.Add(new LogModel { LogText = logTxt });
            }
            return Ok(logs);
        }

        #endregion Services
    }
}