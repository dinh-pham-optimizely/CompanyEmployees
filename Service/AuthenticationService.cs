using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.RequestFeatures;

namespace Service;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }
    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        // Map dto object to User object.
        var user = _mapper.Map<User>(userForRegistration);

        // Create new user in the database.
        var result = await _userManager.CreateAsync(user, userForRegistration.Password);

        if (result.Succeeded)
        {
            // Add roles to user.
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
        }

        return result;
    }
}
