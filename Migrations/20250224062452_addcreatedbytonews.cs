using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParwatPiyushNewsPortal.Migrations
{
    /// <inheritdoc />
    public partial class addcreatedbytonews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "News");
        }
    }
}
