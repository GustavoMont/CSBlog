using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSBlog.Migrations
{
    public partial class UserTypeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "Users",
                type: "varchar(20)",
                nullable: true,
                defaultValue: "READER",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldDefaultValue: "READER")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "Users",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "READER",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true,
                oldDefaultValue: "READER")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
