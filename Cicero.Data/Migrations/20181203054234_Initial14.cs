using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 1,
                column: "FieldValue",
                value: "");

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 2,
                column: "FieldValue",
                value: "");

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 3,
                column: "FieldValue",
                value: "");

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 4,
                column: "FieldValue",
                value: "");

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "FieldDisplay", "FieldGridSize", "FieldKey", "FieldType", "FieldValue" },
                values: new object[] { "Starting Claim", "6", "app_claim", "TENANTCLAIM", "" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 7,
                column: "FieldValue",
                value: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 1,
                column: "FieldValue",
                value: "Cicero");

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 2,
                column: "FieldValue",
                value: "Cargo Carrier Levitate");

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 3,
                column: "FieldValue",
                value: "9851189071");

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 4,
                column: "FieldValue",
                value: "info@cep.com");

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "FieldDisplay", "FieldGridSize", "FieldKey", "FieldType", "FieldValue" },
                values: new object[] { "Sea Ports", "12", "app_ports", "TEXTAREA", "Algeciras,Antwerp,Bremerhaven,Busan,Colombo,Colon,Dalian,Dongguan,Dubai,Felixstowe,Guangzhou,Hamburg,Hong Kong,Jeddah,Kaohsiung,Khor Fakkan,Laem Chabang,Lianyungang,Long Beach,Los Angeles,Manila,Marsaxlokk,Mumbai,Mundra,Nanjing,New York,Ningbo-Zhoushan,Piraeus,Port Klang,Port Said,Qingdao,Rizhao,Rotterdam,Saigon,Salalah,Santos,Savannah,Seattle/Tacoma,Shanghai,Shenzhen,Singapore,Taicang,Tanjung Pelepas,Tanjung Perak,Tanjung Priok,Tianjin,Tokyo,Valencia,Xiamen,Yingkou" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 7,
                column: "FieldValue",
                value: "User");
        }
    }
}
