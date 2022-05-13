using Microsoft.EntityFrameworkCore.Migrations;

namespace HULK.Migrations
{
    public partial class addA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ApplicationType_ApplicatonTypeId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ApplicatonTypeId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ApplicatonTypeId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Product",
                newName: "ApplicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ApplicationTypeId",
                table: "Product",
                column: "ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ApplicationType_ApplicationTypeId",
                table: "Product",
                column: "ApplicationTypeId",
                principalTable: "ApplicationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ApplicationType_ApplicationTypeId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ApplicationTypeId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ApplicationTypeId",
                table: "Product",
                newName: "ApplicationId");

            migrationBuilder.AddColumn<int>(
                name: "ApplicatonTypeId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ApplicatonTypeId",
                table: "Product",
                column: "ApplicatonTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ApplicationType_ApplicatonTypeId",
                table: "Product",
                column: "ApplicatonTypeId",
                principalTable: "ApplicationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
