using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projett.Migrations
{
    public partial class NomDeVotreMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    mots_de_passe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "commandes",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    article = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Qte = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    prix = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__commande__465962296A70EE6E", x => x.order_id);
                    table.ForeignKey(
                        name: "FK__commandes__user___398D8EEE",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_commandes_user_id",
                table: "commandes",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commandes");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
