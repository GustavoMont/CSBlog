using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSBlog.Migrations
{
    public partial class UpdateFirstAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "UserType" },
                values: new object[] { "$2a$11$bgdG3e/zdnlqTr1hLrgOT.SUz8ltVr7jWW4Dq5NFnti9tokEZa4/u", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "UserType" },
                values: new object[] { "$2a$11$NE2fNBJAPdxi4nCPwulNseIQRIV0Fk1o0l2iloKjBpP8XzQIKvNhS", null });
        }
    }
}
