namespace NebulaMigration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using NebulaMigration.Models;
    using NebulaMigration.Options;
    using NebulaMigration.ViewModels;
    using JsonClaimValueTypes = Microsoft.IdentityModel.JsonWebTokens.JsonClaimValueTypes;

    /// <summary>
    /// AccountController.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly NebulaAuthorizationOptions nebulaAuthorizationOptions;
        private readonly UserManager<User> userManager;

        /// <inheritdoc />
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
            if (user == null)
            {
                return this.BadRequest("Пользователь не найден");
            }

            if (!await this.userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false))
            {
                return this.BadRequest("Oшибка при вводе пароля.");
            }

            var encoded = await this.GetToken(user).ConfigureAwait(false);
            return this.Ok(new AuthenticateResponse(user, encoded));
        }

        private async Task<string> GetToken(User user)
        {
            var claims = new List<Claim>();
            claims.AddRange(await this.userManager.GetClaimsAsync(user).ConfigureAwait(false));
            claims.Add(await this.GetRoles(user).ConfigureAwait(false));
            claims.Add(new Claim("name", user.UserName));
            claims.Add(new Claim("email", user.Email));
            var issuer = "nebula";
            return new JwtSecurityTokenHandler()
                .CreateEncodedJwt(
                issuer: issuer,
                audience: issuer,
                subject: new ClaimsIdentity(claims),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(14),
                issuedAt: null,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.nebulaAuthorizationOptions.SymmetricSecurityKey)), SecurityAlgorithms.HmacSha256));
        }

        private async Task<Claim> GetRoles(User user)
        {
            var roles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
            return new Claim("roles", JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray);
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <returns>Action result.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetUserInfo")]
        public async Task<ActionResult> GetUserInfo(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return this.BadRequest("Имя пользователя не может быть пустым.");
            }

            var user = await this.userManager.FindByEmailAsync(userName).ConfigureAwait(false);
            if (user == null)
            {
                return this.BadRequest("Пользователь не найден.");
            }
            var roles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
            var userInfo = new UserInfoViewModel
            {
                Email = userName,
                Roles = roles,
                HasRegistered = true,
            };
            return this.Ok(userInfo);
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
