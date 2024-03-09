using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSizaMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContentMessage",
                table: "Campaigns",
                type: "nchar(300)",
                fixedLength: true,
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(160)",
                oldFixedLength: true,
                oldMaxLength: 160);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContentMessage",
                table: "Campaigns",
                type: "nchar(160)",
                fixedLength: true,
                maxLength: 160,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(300)",
                oldFixedLength: true,
                oldMaxLength: 300);
        }
    }
}
