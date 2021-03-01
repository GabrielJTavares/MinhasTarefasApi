using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinhasTarefasAPI.V1.Models;
using MinhasTarefasAPI.V1.Repositories.Contracts;
using Newtonsoft.Json;

namespace MinhasTarefasAPI.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly UserManager<ApplicationUser> _usermanager;
        public TarefaController(ITarefaRepository tarefaRepository, UserManager<ApplicationUser> usermanager)
        {
            _tarefaRepository = tarefaRepository;
            _usermanager = usermanager;
        }
        /// <summary>
        /// Verifica qual tarefa esta o app e não está no banco
        /// </summary>
        /// <param name="tarefas">lista de tarefas</param>
        /// <returns></returns>
        [HttpPost("Sincronizacao")]
        [Authorize]
        public ActionResult Sincronizacao([FromBody]List<Tarefa> tarefas)
        {
            
            return Ok(_tarefaRepository.Sincronizacao(tarefas));
        }
        /// <summary>
        /// faz a restauração
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("restaurar")]
        public ActionResult Restaurar(DateTime data)
        {
            var usuario=_usermanager.GetUserAsync(HttpContext.User).Result;
            return Ok(_tarefaRepository.Restauracao(usuario, data));
        }
    }
}