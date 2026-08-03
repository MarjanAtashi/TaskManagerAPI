using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAuthenticationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Email column
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");


            // Add PasswordHash column
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");


            // Add Role column
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "User");


            // Copy old Password values into PasswordHash
            migrationBuilder.Sql(
                "UPDATE Users SET PasswordHash = Password");


            // Remove old Password column
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");


            // Update Seed Data
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "PasswordHash", "Role" },
                values: new object[]
                {
                    "DeletedUser@gmail.com",
                    "SystemUserPassword",
                    "User"
                });
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restore Password column
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");


            // Copy PasswordHash back to Password
            migrationBuilder.Sql(
                "UPDATE Users SET Password = PasswordHash");


            // Remove new columns
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");


            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");


            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");
        }
    }
}