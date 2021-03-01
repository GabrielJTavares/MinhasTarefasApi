using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinhasTarefasAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.DataBase
{
    public class ApiContext:IdentityDbContext<ApplicationUser>
    {

        public ApiContext(DbContextOptions<ApiContext> options): base(options)
        {

        }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
