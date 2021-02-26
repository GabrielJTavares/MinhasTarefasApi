using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinhasTarefasAPI.Models;
using MinhasTarefasAPI.Repositories.Contracts;

namespace MinhasTarefasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly UserManager<ApplicationUser> _usermanager;
        public TarefaController(ITarefaRepository tarefaRepository, UserManager<ApplicationUser> usermanager)
        {
            _tarefaRepository = tarefaRepository;
            _usermanager = usermanager;
        }
        [Authorize]
        [HttpPost("Sincronizacao")]
        public ActionResult Sincronizacao([FromBody]List<Tarefa> tarefas)
        {
            return Ok(_tarefaRepository.Sincronizacao(tarefas));
        }

        [HttpGet("modelo")]
        public ActionResult Modelo()
        {
            return Ok(new Tarefa());
        }
        [Authorize]
        [HttpGet("restaurar")]
        public ActionResult Restaurar(DateTime data)
        {
            var usuario=_usermanager.GetUserAsync(HttpContext.User).Result;
            return Ok(_tarefaRepository.Restauracao(usuario, data));
        }
    }
}