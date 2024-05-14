using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Core.Dtos;
using RealTimeChat.Core.Services.Interfaces;

namespace RealTimeChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserManagmentService _userManagmentService;
        public LoginController(IUserManagmentService userManagmentService) 
        {
            _userManagmentService = userManagmentService;
        }

        [HttpPost]
        public void Login([FromBody] LoginRequest request)
        {
            _userManagmentService.Login(request);
        }
    }
}
