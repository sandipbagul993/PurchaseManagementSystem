using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Data.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a4662b8c-ce3a-4f15-8eb7-2d3e4e2cc919", "AQAAAAIAAYagAAAAEFlpsXvL3qwGG7kCchc6WVMbtaLWn1GDLiEiN/VfG8b/S7NzMN10CHNNdt8yG7kkLA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "80d8a704-e5aa-47e8-b4a3-8c10f1b419cf", "AQAAAAIAAYagAAAAEKnWg6CHS9PHHHoPOo2Z1gg6weuWPt/EcYZfWlslemafI0UJEEViyp9qO0blX/JPLQ==" });
        }
    }
}
