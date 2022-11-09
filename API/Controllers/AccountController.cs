using System.IdentityModel.Tokens.Jwt;
using API.DTOs;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly Jwt _jwt;
        private readonly UserManager<AppUser> _userManager;
        
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IOptions<Jwt> jwt, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _jwt = jwt.Value;
        }

        // [Authorize]
        // [HttpGet]
        // public async Task<ActionResult<UserDto>> GetCurrentUser()
        // {
        //     var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(User);

        //     return new UserDto
        //     {
        //         Email = user.Email,
        //         DisplayName = user.DisplayName,
        //         Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
        //         TokenExpirationTime = jwtSecurityToken.ValidTo,
        //         RefreshToken = _tokenService.CreateRefreshToken(),
        //         RefreshTokenExpirationTime = user.RefreshTokenExpirationDate
        //     };
        // }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(User);
            if (user is null)
                return NotFound("User not found");

            return _mapper.Map<AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(User);
            if (user is null)
                return NotFound("User not found");

            user.Address = _mapper.Map<Address>(addressDto);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return _mapper.Map<AddressDto>(user.Address);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                DisplayName = registerDto.DisplayName
            };

            SetUserRefreshToken(user);

            await _userManager.UpdateAsync(user);

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var jwtSecurityToken = _tokenService.CreateToken(user);

            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                TokenExpirationTime = jwtSecurityToken.ValidTo,
                RefreshToken = _tokenService.CreateRefreshToken(),
                RefreshTokenExpirationTime = user.RefreshTokenExpirationDate
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("Username or password not valid");

            SetUserRefreshToken(user);

            await _userManager.UpdateAsync(user);

            var jwtSecurityToken = _tokenService.CreateToken(user);

            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                TokenExpirationTime = jwtSecurityToken.ValidTo,
                RefreshToken = _tokenService.CreateRefreshToken(),
                RefreshTokenExpirationTime = user.RefreshTokenExpirationDate
            };
        }

        private void SetUserRefreshToken(AppUser user)
        {
            var newRefreshToken = _tokenService.CreateRefreshToken();
            var newRefreshTokenExpirationTime = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDurationInDays);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpirationDate = newRefreshTokenExpirationTime;
        }
    }
}