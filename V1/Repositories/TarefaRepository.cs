using MinhasTarefasAPI.DataBase;
using MinhasTarefasAPI.V1.Models;
using MinhasTarefasAPI.V1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.V1.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly ApiContext _banco;
        public TarefaRepository(ApiContext banco)
        {
            _banco = banco;
        }
        public List<Tarefa> Restauracao(ApplicationUser usuario, DateTime dataUltimaSincronizacao)
        {
            var query = _banco.Tarefas.Where(a => a.UsuarioId == usuario.Id).AsQueryable();

            if (dataUltimaSincronizacao != null)
            {
                query.Where(a => a.Criado >= dataUltimaSincronizacao || a.Atualizado >= dataUltimaSincronizacao);
            }
            return query.ToList<Tarefa>();

        }

        public List<Tarefa> Sincronizacao(List<Tarefa> tarefas)
        {
           
            var novasTarefas = tarefas.Where(a => a.IdTarefaApi == 0).ToList();            
            var TarefasAtualizadas = tarefas.Where(a => a.IdTarefaApi != 0).ToList();

            if (novasTarefas.Count() > 0)
            {
                foreach (var tarefa in novasTarefas)
                {
                    _banco.Tarefas.Add(tarefa);
                }

            }


            if (TarefasAtualizadas.Count() > 0)
            {
                foreach (var item in TarefasAtualizadas)
                {
                    _banco.Tarefas.Update(item);

                }

            }
            _banco.SaveChanges();

            return novasTarefas.ToList();
        }
    }
}
