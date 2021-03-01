using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinhasTarefasAPI.V1.Models;
using MinhasTarefasAPI.V1.Repositories.Contracts;

namespace MinhasTarefasAPI.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsuarioController(IUsuarioRepository usuarioRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            _usuarioRepository = usuarioRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        /// <summary>
        /// Faz o Login
        /// </summary>
        /// <param name="usuarioDTO">Email</param>
        /// <returns>retorna um token</returns>
        [HttpPost("login")]
        public ActionResult Login([FromBody]UsuarioDTO usuarioDTO)
        {
            ModelState.Remove("ConfirmarSenha");
            ModelState.Remove("Nome");
            if (ModelState.IsValid)
            {
                ApplicationUser usuario =_usuarioRepository.Find(usuarioDTO.Email, usuarioDTO.Senha);
                if (usuario != null)
                {
                    return GerarNovoToken(usuario);
                }
                else
                {
                    return NotFound("Usuário não localizado!");
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        
        /// <summary>
        /// faz a renovação do token para não ser necessario um novo login
        /// </summary>
        /// <param name="tokenDTO">token</param>
        /// <returns>um novo token</returns>
        [HttpPost("renovar")]
        public ActionResult Renovar([FromBody]TokenDTO tokenDTO)
        {
            var refreshTokenDB = _tokenRepository.FindToken(tokenDTO.RefreshToken);

            if (refreshTokenDB == null)
                return NotFound();

            //atualizado token
            refreshTokenDB.Atualizado = DateTime.Now;
            refreshTokenDB.Utilizado = true;
            _tokenRepository.UpdateToken(refreshTokenDB);

            //gera um novo token
            var usuario = _usuarioRepository.Find(refreshTokenDB.UsuarioId);

            return GerarNovoToken(usuario);


        }

        /// <summary>
        /// Cadastra um novo usuario
        /// </summary>
        /// <param name="usuarioDTO">email</param>
        /// <returns>o novo usuario</returns>
        [HttpPost("cadastrar")]
        public ActionResult Cadastrar([FromBody] UsuarioDTO usuarioDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser usuario = new ApplicationUser();
                usuario.FullName = usuarioDTO.Nome;
                usuario.UserName= usuarioDTO.Email;
                usuario.Email = usuarioDTO.Email;
                var resultado = _userManager.CreateAsync(usuario, usuarioDTO.Senha).Result;

                if (!resultado.Succeeded)
                {
                    List<string> erros = new List<string>();
                    foreach (var erro in resultado.Errors)
                    {
                        erros.Add(erro.Description);
                    }
                    return UnprocessableEntity(erros);
                }
                else
                {
                    return Ok(usuario);
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }
        
        private TokenDTO BuildToken(ApplicationUser usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email,usuario.Email),
                 new Claim(JwtRegisteredClaimNames.Sub,usuario.Id)
            };
            var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes("CH4V35-jwt!@e-m1nh45T4r3f4a5"));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer:null,
                audience:"",
                claims: claims,
                expires:exp,
                signingCredentials:sign

                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = Guid.NewGuid().ToString();
            var expritationRefreshToken = DateTime.UtcNow.AddHours(2);

            var TokenDTO= new TokenDTO { Token = tokenString, Expiration = exp,ExpirationRefreshToken= expritationRefreshToken,RefreshToken= refreshToken };
            return TokenDTO;
        }

        private ActionResult GerarNovoToken(ApplicationUser usuario)
        {
            var token = BuildToken(usuario);
            var tokenModel = new Token()
            {
                RefreshToken = token.RefreshToken,
                ExpirtationRefreshToken = token.ExpirationRefreshToken,
                ExpirtationToken = token.Expiration,
                Usuario = usuario,
                Criado = DateTime.Now,
                Utilizado = false
            };

            _tokenRepository.RegisterToken(tokenModel);
            return Ok(token);
        }
    }
}