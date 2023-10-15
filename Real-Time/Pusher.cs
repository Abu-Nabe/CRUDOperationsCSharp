using PusherServer;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using BackendAPI.Model;

namespace BackendAPI.RealTime
{
    public class RealTimeController : Controller
    {
        [Route("/send")]
        [HttpPost]
        public async Task<ActionResult> sendMessage([FromBody] IndividualModel individualModel)
        {
            // Sets up connection for pusher
            var options = new PusherOptions
            {
                Cluster = "ap1",
                Encrypted = true
            };

            var pusher = new Pusher(
              "hidden",
              "hidden",
              "hidden",
              options);

            // sends the message
            var result = await pusher.TriggerAsync(
              "everyone",
              "bioChange",
              new { username = individualModel.username, bio = individualModel.bio });

            return Json(new { message = result });
        }
    }
}

