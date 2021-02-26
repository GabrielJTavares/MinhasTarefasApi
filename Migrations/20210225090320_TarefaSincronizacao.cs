using Microsoft.EntityFrameworkCore.Migrations;

namespace MinhasTarefasAPI.Migrations
{
    public partial class TarefaSincronizacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tarefas",
                table: "Tarefas");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tarefas",
                newName: "MyProperty");

            migrationBuilder.AlterColumn<int>(
                name: "MyProperty",
                table: "Tarefas",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "IdTarefaApi",
                table: "Tarefas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<bool>(
                name: "Excluido",
                table: "Tarefas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdTarefaApp",
                table: "Tarefas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tarefas",
                table: "Tarefas",
                column: "IdTarefaApi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tarefas",
                table: "Tarefas");

            migrationBuilder.DropColumn(
                name: "IdTarefaApi",
                table: "Tarefas");

            migrationBuilder.DropColumn(
                name: "Excluido",
                table: "Tarefas");

            migrationBuilder.DropColumn(
                name: "IdTarefaApp",
                table: "Tarefas");

            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Tarefas",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Tarefas",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tarefas",
                table: "Tarefas",
                column: "Id");
        }
    }
}
