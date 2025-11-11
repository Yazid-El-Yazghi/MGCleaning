using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MGCleaning.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToGebouwArbeider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GebouwArbeiders",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GebouwArbeiders");
        }
    }
}
