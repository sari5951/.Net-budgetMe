﻿using Logic.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IAccountService
    {
        string CheckEmail(ContactDTO model);
        UserDTO SignIn(LoginDTO model);
        JWTResponseToken GetToken(User user);
    }
    public class AccountService : IAccountService
    {
        private IDBService dbService;
        private IUserService userService;
        private IConfiguration configuration;

        public AccountService(IDBService dbService, IConfiguration configuration,IUserService userService)
        {
            this.dbService = dbService;
            this.configuration = configuration;
            this.userService = userService;
        }

        public string CheckEmail(ContactDTO model)
        {
            return "";
        }
        public UserDTO SignIn(LoginDTO model)
        {
            var dbUser = dbService.entities.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password && x.IsActive);

            if (dbUser == null)
            {
                return null;
            }
            var token = GetToken(dbUser);

            var userdto = new UserDTO()
            {
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                UserType = (UserTypeDTO)dbUser.UserTypeId,
                Token = token,
                Email = dbUser.Email,
                Id = dbUser.Id,
                IsActive = dbUser.IsActive

            };
            return userdto;
        }

        public JWTResponseToken GetToken(User user)
        {

            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("loginDate", DateTime.Now.ToString()),
                new Claim("isActive", user.IsActive.ToString()),
                new Claim("managerId",userService.GetManagerId(user.Id).ToString())

             };
            var token = CreateToken(authClaims, false);
            var jwt = new JWT()
            {
                Aud = token.Audiences.ToString(),
                Exp = token.ValidTo.Ticks,
                Iss = token.Issuer,
            };

            var res = new JWTResponseToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Id = user.Id,
                JWT = jwt,
                AllowAllPermissions = "true",
                FullNameUser = user.FirstName + " " + user.LastName,
            };

            return res;
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims, bool remember)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: remember ? DateTime.Now.AddHours(24 * 14) : DateTime.Now.AddHours(24 * 2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
