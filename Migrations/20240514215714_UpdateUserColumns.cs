using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POE_CLVD.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "ContactMessages",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "ContactMessages",
                newName: "UserEmail");
        }
    }
}
