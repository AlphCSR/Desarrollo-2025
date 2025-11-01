using Microsoft.AspNetCore.Mvc;
using MediatR;
using UsersMS.Commons.Dtos.Request;
using UsersMS.Application.Commands;
using UsersMS.Application.Queries;
using Microsoft.AspNetCore.Authorization;

namespace UsersMS.Controllers
{
    [ApiController]
    [Authorize(Roles = "User,Administrador")] // Ajusta los roles según lo que tengas en Keycloak
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;

        public UsersController(ILogger<UsersController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger.LogInformation("UsersController instantiated");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            _logger.LogInformation("Received request to create a User");
            try
            {
                var command = new CreateUserCommand(createUserDto);
                var message = await _mediator.Send(command);
                return Ok(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating user: {Message}", e.Message);
                return StatusCode(500, "Error while creating user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            try
            {
                var command = new DeleteUserCommand(new DeleteUserDto { UserId = id });
                var message = await _mediator.Send(command);
                return Ok(message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error deleting user: {Message}", e.Message);
                return StatusCode(500, "Error while deleting user.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var query = new GetUserQuery(id);
                var user = await _mediator.Send(query);
                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting user: {Message}", e.Message);
                return StatusCode(500, "Error while getting user.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            try
            {
                var command = new UpdateUserCommand(updateUserDto);
                var msg = await _mediator.Send(command);
                return Ok(msg);
            }
            catch (Exception e)
            {
                _logger.LogError("Error updating user: {Message}", e.Message);
                return StatusCode(500, "Error while updating user.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var query = new GetAllUsersQuery();
                var users = await _mediator.Send(query);
                return Ok(users);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting users: {Message}", e.Message);
                return StatusCode(500, "Error while getting users.");
            }
        }
    }
}
