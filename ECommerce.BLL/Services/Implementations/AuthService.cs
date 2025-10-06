using ECommerce.BLL.DTOs;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null) return null;

            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null) return null;

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return null;

           

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                
                user = await _userManager.FindByEmailAsync(model.Username);
                if (user == null) return null;
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid) return null;

           
            user.LastLoginTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            
            var user = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.RefreshToken != null && u.RefreshToken == refreshToken);

            if (user == null) return null;

            if (!user.RefreshTokenExpiry.HasValue || user.RefreshTokenExpiry.Value < DateTime.UtcNow)
            {
                return null; 
            }

       
            return await GenerateAuthResponseAsync(user, rotateRefreshToken: true);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _userManager.UpdateAsync(user);
            return true;
        }

      
        private async Task<AuthResponseDto> GenerateAuthResponseAsync(ApplicationUser user, bool rotateRefreshToken = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenExpiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "30");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
               
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

          
            string refreshToken = user.RefreshToken ?? string.Empty;
            DateTime? refreshExpiry = user.RefreshTokenExpiry;

            if (rotateRefreshToken || string.IsNullOrWhiteSpace(refreshToken) || !refreshExpiry.HasValue || refreshExpiry.Value < DateTime.UtcNow)
            {
                refreshToken = GenerateRefreshToken();
                refreshExpiry = DateTime.UtcNow.AddDays(7); 
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = refreshExpiry;
                await _userManager.UpdateAsync(user);
            }

            return new AuthResponseDto
            {
                Token = jwt,
                RefreshToken = refreshToken,
                TokenExpiry = tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(tokenExpiryMinutes)
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
