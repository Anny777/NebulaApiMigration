namespace NebulaMigration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using NebulaMigration.Models;
    using NebulaMigration.Options;

    /// <summary>
    /// AccountController.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly NebulaAuthorizationOptions nebulaAuthorizationOptions;
        private readonly UserManager<User> userManager;

        public AccountController(
            IOptions<NebulaAuthorizationOptions> nebulaApiOptions,
            UserManager<User> userManager)
        {
            this.nebulaAuthorizationOptions = nebulaApiOptions.Value ?? throw new ArgumentNullException(nameof(nebulaApiOptions));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return this.BadRequest("Необходимо указать имя пользователя и пароль.");
            }

            var user = await this.userManager.FindByEmailAsync(model.Username).ConfigureAwait(false);
            if (user == null || !await this.userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false))
            {
                return this.BadRequest("Пользователь не найден или ошибка при вводе пароля.");
            }

            var encoded = await this.GetToken(user).ConfigureAwait(false);


            return this.Ok(new AuthenticateResponse(user, encoded));
        }

        private async Task<string> GetToken(User user)
        {
            var claims = new List<Claim>();
            claims.AddRange(await this.userManager.GetClaimsAsync(user).ConfigureAwait(false));
            claims.AddRange((await this.userManager.GetRolesAsync(user).ConfigureAwait(false)).Select(role => new Claim("roles", role)));
            claims.Add(new Claim("name", user.UserName));
            claims.Add(new Claim("email", user.Email));
            var issuer = "nebula";
            return new JwtSecurityTokenHandler().CreateEncodedJwt(
                issuer: issuer,
                audience: issuer,
                subject: new ClaimsIdentity(claims),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(14),
                issuedAt: null,
                signingCredentials: new SigningCredentials(this.nebulaAuthorizationOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ResetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = await this.userManager.FindByIdAsync(model.UserId).ConfigureAwait(false);
            if (user == null)
            {
                return this.BadRequest($"User with id {model.UserId} wasn't find.");
            }

            var result = await this.userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            return !result.Succeeded ? this.BadRequest(result) : (ActionResult)this.Ok();
        }
    }
}
