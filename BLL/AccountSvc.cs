using Common.BLL;
using Common.Req;
using Common.Req.Account;
using Common.Rsp;
using DAL;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AccountSvc : GenericSvc<AccountRep, Account>
    {
        private readonly AccountRep _accountRep = new AccountRep();
        private readonly AppSettings _appSettings = new AppSettings();

        public SingleRsp CheckValidToken(TokenReq TokenReq)
        {
            var token = TokenReq.token;
            var rsp = new SingleRsp();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Pjnis7DgRYABE9aShtatwK16j3BuIchX");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            // Giải mã token và lấy danh sách các Claim
            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;

                // Sử dụng claimsIdentity để xác thực và phân quyền cho người dùng
                if (claimsIdentity == null)
                {
                    // Token không hợp lệ, không có ClaimIdentity
                }
                else if (claimsIdentity.IsAuthenticated && validatedToken.ValidTo < DateTime.UtcNow)
                {
                    // Token hợp lệ và thời gian hết hạn
                    rsp.SetError("Out of time token");
                }
                else
                {
                    // Token không hết hạn
                    rsp.Data = new
                    {
                        IDUser = claimsIdentity.FindFirst("IDUser").Value,
                        Role = claimsIdentity.FindFirst(ClaimTypes.Role).Value,
                    };
                    
                }
            }
            catch (SecurityTokenException)
            {
                rsp.SetError("Token is invalid");
            }
            
            return rsp;

        }

        public SingleRsp GetAccountByID(Guid id)
        {
            var rsp = new SingleRsp();
            if(_accountRep.GetAccountByID(id) != null)
            {
                rsp.Data = _accountRep.GetAccountByID(id);
            }
            else
            {
                rsp.SetError("Not found acc");
            }
            return rsp;
        }

        public SingleRsp Login(LoginReq loginReq)
        {
            var rsp = new SingleRsp();
            if(_accountRep.GetUserByLogin(loginReq.Username, loginReq.Password) != null)
            {
                rsp.Data = GenerateToken(_accountRep.GetUserByLogin(loginReq.Username, loginReq.Password));
            }
            else
            {
                rsp.SetError("Not found account");
            }
            return rsp;
        }

        public SingleRsp Register(RegisterReq registerReq)
        {
            var rsp = new SingleRsp();
            if (_accountRep.ValidRegister(registerReq))
            {
                rsp.SetError("Existed Email");
                return rsp;
            }
            Guid id = Guid.NewGuid();
            Account acc = new Account
            {
                IdAccount = id,
                DateCreate = DateTime.Now,
                Password = registerReq.Password,
                Username = registerReq.Username,
            };
            _accountRep.AddAccount(acc);
            _accountRep.AddUser(new User
            {
                IdUser = Guid.NewGuid(),
                IdAccount = id,
                Avatar = null,
                DateOfBirth = null,
                Email = registerReq.Username,
                Name = registerReq.Name,
                IdCard = null,
                PhoneNumber = null,
                IdBankAccount = null,
                IdTypeOfUser = registerReq.TypeOfUser,
                Status = registerReq.TypeOfUser == 1 ? -2 : 1,
            });

            rsp.Data = acc;

            return rsp;

        }

        private string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("IDUser", Convert.ToString(user.IdUser)),
                new Claim("Name", user.Name),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, user.IdTypeOfUserNavigation!.TypeOfUserName!),
                new Claim("Role", user.IdTypeOfUserNavigation!.TypeOfUserName!)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.SecretKey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
