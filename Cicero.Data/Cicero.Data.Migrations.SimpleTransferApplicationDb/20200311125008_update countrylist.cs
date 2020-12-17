using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class updatecountrylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CountryList",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "CurrencyCode", "CurrencyName", "DisplayOrder", "FlagCode", "FlagImageUrl", "IsActive", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "AF", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Afghanistan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 158, "NZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "New Zealand", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 159, "NI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Nicaragua", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 160, "NE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Niger", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 161, "NG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Nigeria", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 162, "NU", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Niue", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 163, "NF", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Norfolk Island", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 164, "MP", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Northern Mariana Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 165, "NO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Norway", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 166, "OM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Oman", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 167, "PK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Pakistan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 168, "PW", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Palau", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 169, "PS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Palestine", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 170, "PA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Panama", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 171, "PG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Papua New Guinea", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 172, "PY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Paraguay", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 173, "PE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Peru", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 174, "PH", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Philippines", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 175, "PN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Pitcairn", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 176, "PL", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Poland", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 177, "PT", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Portugal", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 178, "PR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Puerto Rico", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 179, "QA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Qatar", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 180, "RE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Reunion", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 181, "RO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Romania", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 182, "RU", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Russian Federation", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 183, "RW", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Rwanda", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 184, "KN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Saint Kitts and Nevis", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 157, "NC", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "New Caledonia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 185, "LC", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Saint Lucia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 156, "AN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Netherlands Antilles", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 154, "NP", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Nepal", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 127, "LT", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Lithuania", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 128, "LU", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Luxembourg", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 129, "MO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Macau", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 130, "MK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Macedonia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 131, "MG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Madagascar", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 132, "MW", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Malawi", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 133, "MY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Malaysia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 134, "MV", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Maldives", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 135, "ML", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Mali", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 136, "MT", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Malta", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 137, "MH", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Marshall Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 138, "MQ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Martinique", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 139, "MR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Mauritania", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 140, "MU", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Mauritius", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 141, "TY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Mayotte", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 142, "MX", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Mexico", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 143, "FM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Micronesia, Federated States of", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 144, "MD", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Moldova, Republic of", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 145, "MC", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Monaco", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 146, "MN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Mongolia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 147, "ME", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Montenegro", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 148, "MS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Montserrat", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 149, "MA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Morocco", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 150, "MZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Mozambique", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 151, "MM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Myanmar", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 152, "NA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Namibia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 153, "NR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Nauru", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 155, "NL", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Netherlands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 126, "LI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Liechtenstein", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 186, "VC", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Saint Vincent and the Grenadines", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 188, "SM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "San Marino", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 220, "TO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Tonga", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 222, "TT", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Trinidad and Tobago", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 223, "TN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Tunisia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 224, "TR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Turkey", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 225, "TM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Turkmenistan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 226, "TC", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Turks and Caicos Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 227, "TV", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Tuvalu", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 228, "UG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Uganda", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 229, "UA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Ukraine", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 230, "AE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "United Arab Emirates", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 231, "GB", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "United Kingdom", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 232, "US", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "United States", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 233, "UM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "United States minor outlying islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 234, "UY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Uruguay", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 235, "UZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Uzbekistan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 236, "VU", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Vanuatu", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 237, "VA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Vatican City State", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 238, "VE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Venezuela", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 239, "VN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Vietnam", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 240, "VG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Virgin Islands (British)", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 241, "VI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Virgin Islands (U.S.)", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 242, "WF", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Wallis and Futuna Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 243, "EH", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Western Sahara", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 244, "YE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Yemen", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 245, "ZR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Zaire", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 246, "ZM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Zambia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 247, "ZW", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Zimbabwe", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 219, "TK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Tokelau", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 187, "WS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Samoa", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 218, "TG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Togo", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 216, "TZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Tanzania, United Republic of", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 189, "ST", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Sao Tome and Principe", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 190, "SA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Saudi Arabia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 191, "SN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Senegal", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 192, "RS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Serbia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 193, "SC", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Seychelles", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 194, "SL", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Sierra Leone", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 195, "SG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Singapore", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 196, "SK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Slovakia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 197, "SI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Slovenia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 198, "SB", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Solomon Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 199, "SO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Somalia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 200, "ZA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "South Africa", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 201, "GS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "South Georgia South Sandwich Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 202, "SS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "South Sudan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 203, "ES", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Spain", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 204, "LK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Sri Lanka", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 205, "SH", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "St. Helena", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 206, "PM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "St. Pierre and Miquelon", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 207, "SD", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Sudan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 208, "SR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Suriname", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 209, "SJ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Svalbard and Jan Mayen Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 210, "SZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Swaziland", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 211, "SE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Sweden", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 212, "CH", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Switzerland", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 213, "SY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Syrian Arab Republic", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 214, "TW", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Taiwan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 215, "TJ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Tajikistan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 217, "TH", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Thailand", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 124, "LR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Liberia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 125, "LY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Libyan Arab Jamahiriya", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 61, "EC", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Ecuador", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 34, "BF", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Burkina Faso", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 35, "BI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Burundi", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 36, "KH", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Cambodia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 37, "CM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Cameroon", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 38, "CA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Canada", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 39, "CV", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Cape Verde", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 40, "KY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Cayman Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 41, "CF", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Central African Republic", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 42, "TD", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Chad", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 43, "CL", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Chile", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 44, "CN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "China", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 45, "CX", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Christmas Island", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 46, "CC", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Cocos (Keeling) Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 47, "CO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Colombia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 48, "KM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Comoros", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 49, "CG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Congo", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 50, "CK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Cook Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 51, "CR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Costa Rica", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 52, "HR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Croatia (Hrvatska)", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 53, "CU", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Cuba", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 54, "CY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Cyprus", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 55, "CZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Czech Republic", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 56, "DK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Denmark", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 57, "DJ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Djibouti", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 58, "DM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Dominica", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 59, "DO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Dominican Republic", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 60, "TP", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "East Timor", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 33, "BG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Bulgaria", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 123, "LS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Lesotho", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 32, "BN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Brunei Darussalam", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 30, "BR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Brazil", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 3, "DZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Algeria", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 4, "DS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "American Samoa", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 5, "AD", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Andorra", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 6, "AO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Angola", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 7, "AI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Anguilla", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) }
                });

            migrationBuilder.InsertData(
                table: "CountryList",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "CurrencyCode", "CurrencyName", "DisplayOrder", "FlagCode", "FlagImageUrl", "IsActive", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 8, "AQ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Antarctica", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 9, "AG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Antigua and Barbuda", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 10, "AR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Argentina", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 11, "AM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Armenia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 12, "AW", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Aruba", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 13, "AU", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Australia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 14, "AT", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Austria", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 15, "AZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Azerbaijan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 16, "BS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Bahamas", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 17, "BH", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Bahrain", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 18, "BD", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Bangladesh", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 19, "BB", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Barbados", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 20, "BY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Belarus", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 21, "BE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Belgium", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 22, "BZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Belize", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 23, "BJ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Benin", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 24, "BM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Bermuda", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 25, "BT", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Bhutan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 26, "BO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Bolivia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 27, "BA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Bosnia and Herzegovina", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 28, "BW", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Botswana", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 29, "BV", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Bouvet Island", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 31, "IO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "British Indian Ocean Territory", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 2, "AL", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Albania", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 62, "EG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Egypt", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 64, "GQ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Equatorial Guinea", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 96, "HK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Hong Kong", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 97, "HU", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Hungary", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 98, "IS", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Iceland", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 99, "IN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "India", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 100, "IM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Isle of Man", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 101, "ID", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Indonesia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 102, "IR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Iran (Islamic Republic of)", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 103, "IQ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Iraq", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 104, "IE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Ireland", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 105, "IL", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Israel", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 106, "IT", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Italy", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 107, "CI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Ivory Coast", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 108, "JE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Jersey", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 109, "JM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Jamaica", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 110, "JP", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Japan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 111, "JO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Jordan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 112, "KZ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Kazakhstan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 113, "KE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Kenya", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 114, "KI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Kiribati", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 115, "KP", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Korea, Democratic Peopl's Republic of", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 116, "KR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Korea, Republic of", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 117, "XK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Kosovo", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 118, "KW", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Kuwait", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 119, "KG", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Kyrgyzstan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 120, "LA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Lao People's Democratic Republic", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 121, "LV", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Latvia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 122, "LB", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Lebanon", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 95, "HN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Honduras", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 63, "SV", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "El Salvador", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 94, "HM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Heard and Mc Donald Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 92, "GY", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Guyana", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 65, "ER", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Eritrea", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 66, "EE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Estonia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 67, "ET", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Ethiopia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 68, "FK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Falkland Islands (Malvinas)", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 69, "FO", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Faroe Islands", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 70, "FJ", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Fiji", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 71, "FI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Finland", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 72, "FR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "France", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 73, "FX", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "France, Metropolitan", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 74, "GF", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "French Guiana", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 75, "PF", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "French Polynesia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 76, "TF", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "French Southern Territories", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 77, "GA", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Gabon", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 78, "GM", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Gambia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 79, "GE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Georgia", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 80, "DE", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Germany", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 81, "GH", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Ghana", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 82, "GI", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Gibraltar", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 83, "GK", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Guernsey", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 84, "GR", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Greece", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 85, "GL", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Greenland", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 86, "GD", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Grenada", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 87, "GP", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Guadeloupe", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 88, "GU", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Guam", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 89, "GT", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Guatemala", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 90, "GN", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Guinea", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 91, "GW", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Guinea-Bissau", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) },
                    { 93, "HT", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local), null, null, 0, null, null, false, "Haiti", "df0d5fc1-b3c9-448f-afea-a43cd08005a6", new DateTime(2020, 3, 11, 18, 35, 7, 538, DateTimeKind.Local) }
                });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 11, 18, 35, 7, 537, DateTimeKind.Local), new DateTime(2020, 3, 11, 18, 35, 7, 535, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 11, 18, 35, 7, 537, DateTimeKind.Local), new DateTime(2020, 3, 11, 18, 35, 7, 537, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 11, 18, 35, 7, 537, DateTimeKind.Local), new DateTime(2020, 3, 11, 18, 35, 7, 537, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 11, 18, 32, 11, 870, DateTimeKind.Local), new DateTime(2020, 3, 11, 18, 32, 11, 869, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 11, 18, 32, 11, 870, DateTimeKind.Local), new DateTime(2020, 3, 11, 18, 32, 11, 870, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 11, 18, 32, 11, 870, DateTimeKind.Local), new DateTime(2020, 3, 11, 18, 32, 11, 870, DateTimeKind.Local) });
        }
    }
}
