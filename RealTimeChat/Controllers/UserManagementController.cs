using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Core.Dtos;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Services.Interfaces;
using RealTimeChat.Hubs;
using System.Net.WebSockets;

namespace RealTimeChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagmentService;
        public UserManagementController(IUserManagementService userManagmentService)
        {
            _userManagmentService = userManagmentService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
        {
            if (!await _userManagmentService.Login(request))
            {
                return BadRequest("User Doesn't Exist");
            }
            return Ok(request.Username); // simulate returning token just for sake of poc 
        }

        [HttpPost]
        [Route("Register")]

        public async Task<ActionResult> Register([FromBody] User request)
        {
            if (!await _userManagmentService.Register(request))
            {
                return BadRequest("User Already Exist");
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetUsersStatus")]
        [AuthorizationHeaderFilter]

        public async Task<List<UserStatusDto>> GetUsersStatus()
        {
            return await _userManagmentService.GetUsersStatus();
        }

        [HttpGet]
        [Route("GetUserMessages")]
        [AuthorizationHeaderFilter]

        public async Task<Dictionary<string, List<ChatMessage>>> GetUserMessages()
        {
            var callerUserName = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split(" ")[1];

           return await _userManagmentService.GetUnRecievedMessages(callerUserName);
         
        }
    }
}
