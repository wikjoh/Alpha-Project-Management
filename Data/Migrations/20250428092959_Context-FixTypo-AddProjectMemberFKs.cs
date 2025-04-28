using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ContextFixTypoAddProjectMemberFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberAdresses_MemberProfiles_UserId",
                table: "MemberAdresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberAdresses",
                table: "MemberAdresses");

            migrationBuilder.RenameTable(
                name: "MemberAdresses",
                newName: "MemberAddresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberAddresses",
                table: "MemberAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberAddresses_MemberProfiles_UserId",
                table: "MemberAddresses",
                column: "UserId",
                principalTable: "MemberProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberAddresses_MemberProfiles_UserId",
                table: "MemberAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberAddresses",
                table: "MemberAddresses");

            migrationBuilder.RenameTable(
                name: "MemberAddresses",
                newName: "MemberAdresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberAdresses",
                table: "MemberAdresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberAdresses_MemberProfiles_UserId",
                table: "MemberAdresses",
                column: "UserId",
                principalTable: "MemberProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
