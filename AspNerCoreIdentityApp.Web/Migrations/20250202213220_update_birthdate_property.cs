using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNerCoreIdentityApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class update_birthdate_property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "AspNetUsers",
                newName: "Birthdate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthdate",
                table: "AspNetUsers",
                newName: "Birthday");
        }
    }
}
