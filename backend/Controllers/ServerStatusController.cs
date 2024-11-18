using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("server/status")]
    public class ServerStatusController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetServerStatus()
        {
            var data = new
            {
                ServerOnline = true
            };
            var response = new
            {
                Status = "Success",
                Message = "Server is operational",
                Data = data,
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };

            return Ok(response);

        }
    }
}