using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "CountryList",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CountryList",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "CountryList",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "AF", "Afghanistan" },
                    { 157, "NC", "New Caledonia" },
                    { 158, "NZ", "New Zealand" },
                    { 159, "NI", "Nicaragua" },
                    { 160, "NE", "Niger" },
                    { 161, "NG", "Nigeria" },
                    { 162, "NU", "Niue" },
                    { 163, "NF", "Norfolk Island" },
                    { 164, "MP", "Northern Mariana Islands" },
                    { 165, "NO", "Norway" },
                    { 166, "OM", "Oman" },
                    { 167, "PK", "Pakistan" },
                    { 168, "PW", "Palau" },
                    { 156, "AN", "Netherlands Antilles" },
                    { 169, "PS", "Palestine" },
                    { 171, "PG", "Papua New Guinea" },
                    { 172, "PY", "Paraguay" },
                    { 173, "PE", "Peru" },
                    { 174, "PH", "Philippines" },
                    { 175, "PN", "Pitcairn" },
                    { 176, "PL", "Poland" },
                    { 177, "PT", "Portugal" },
                    { 178, "PR", "Puerto Rico" },
                    { 179, "QA", "Qatar" },
                    { 180, "RE", "Reunion" },
                    { 181, "RO", "Romania" },
                    { 182, "RU", "Russian Federation" },
                    { 170, "PA", "Panama" },
                    { 155, "NL", "Netherlands" },
                    { 154, "NP", "Nepal" },
                    { 153, "NR", "Nauru" },
                    { 126, "LI", "Liechtenstein" },
                    { 127, "LT", "Lithuania" },
                    { 128, "LU", "Luxembourg" },
                    { 129, "MO", "Macau" },
                    { 130, "MK", "Macedonia" },
                    { 131, "MG", "Madagascar" },
                    { 132, "MW", "Malawi" },
                    { 133, "MY", "Malaysia" },
                    { 134, "MV", "Maldives" },
                    { 135, "ML", "Mali" },
                    { 136, "MT", "Malta" },
                    { 137, "MH", "Marshall Islands" },
                    { 138, "MQ", "Martinique" },
                    { 139, "MR", "Mauritania" },
                    { 140, "MU", "Mauritius" },
                    { 141, "TY", "Mayotte" },
                    { 142, "MX", "Mexico" },
                    { 143, "FM", "Micronesia, Federated States of" },
                    { 144, "MD", "Moldova, Republic of" },
                    { 145, "MC", "Monaco" },
                    { 146, "MN", "Mongolia" },
                    { 147, "ME", "Montenegro" },
                    { 148, "MS", "Montserrat" },
                    { 149, "MA", "Morocco" },
                    { 150, "MZ", "Mozambique" },
                    { 151, "MM", "Myanmar" },
                    { 152, "NA", "Namibia" },
                    { 183, "RW", "Rwanda" },
                    { 184, "KN", "Saint Kitts and Nevis" },
                    { 185, "LC", "Saint Lucia" },
                    { 186, "VC", "Saint Vincent and the Grenadines" },
                    { 218, "TG", "Togo" },
                    { 219, "TK", "Tokelau" },
                    { 220, "TO", "Tonga" },
                    { 222, "TT", "Trinidad and Tobago" },
                    { 223, "TN", "Tunisia" },
                    { 224, "TR", "Turkey" },
                    { 225, "TM", "Turkmenistan" },
                    { 226, "TC", "Turks and Caicos Islands" },
                    { 227, "TV", "Tuvalu" },
                    { 228, "UG", "Uganda" },
                    { 229, "UA", "Ukraine" },
                    { 230, "AE", "United Arab Emirates" },
                    { 231, "GB", "United Kingdom" },
                    { 232, "US", "United States" },
                    { 233, "UM", "United States minor outlying islands" },
                    { 234, "UY", "Uruguay" },
                    { 235, "UZ", "Uzbekistan" },
                    { 236, "VU", "Vanuatu" },
                    { 237, "VA", "Vatican City State" },
                    { 238, "VE", "Venezuela" },
                    { 239, "VN", "Vietnam" },
                    { 240, "VG", "Virgin Islands (British)" },
                    { 241, "VI", "Virgin Islands (U.S.)" },
                    { 242, "WF", "Wallis and Futuna Islands" },
                    { 243, "EH", "Western Sahara" },
                    { 244, "YE", "Yemen" },
                    { 245, "ZR", "Zaire" },
                    { 217, "TH", "Thailand" },
                    { 125, "LY", "Libyan Arab Jamahiriya" },
                    { 216, "TZ", "Tanzania, United Republic of" },
                    { 214, "TW", "Taiwan" },
                    { 187, "WS", "Samoa" },
                    { 188, "SM", "San Marino" },
                    { 189, "ST", "Sao Tome and Principe" },
                    { 190, "SA", "Saudi Arabia" },
                    { 191, "SN", "Senegal" },
                    { 192, "RS", "Serbia" },
                    { 193, "SC", "Seychelles" },
                    { 194, "SL", "Sierra Leone" },
                    { 195, "SG", "Singapore" },
                    { 196, "SK", "Slovakia" },
                    { 197, "SI", "Slovenia" },
                    { 198, "SB", "Solomon Islands" },
                    { 199, "SO", "Somalia" },
                    { 200, "ZA", "South Africa" },
                    { 201, "GS", "South Georgia South Sandwich Islands" },
                    { 202, "SS", "South Sudan" },
                    { 203, "ES", "Spain" },
                    { 204, "LK", "Sri Lanka" },
                    { 205, "SH", "St. Helena" },
                    { 206, "PM", "St. Pierre and Miquelon" },
                    { 207, "SD", "Sudan" },
                    { 208, "SR", "Suriname" },
                    { 209, "SJ", "Svalbard and Jan Mayen Islands" },
                    { 210, "SZ", "Swaziland" },
                    { 211, "SE", "Sweden" },
                    { 212, "CH", "Switzerland" },
                    { 213, "SY", "Syrian Arab Republic" },
                    { 215, "TJ", "Tajikistan" },
                    { 124, "LR", "Liberia" },
                    { 123, "LS", "Lesotho" },
                    { 122, "LB", "Lebanon" },
                    { 33, "BG", "Bulgaria" },
                    { 34, "BF", "Burkina Faso" },
                    { 35, "BI", "Burundi" },
                    { 36, "KH", "Cambodia" },
                    { 37, "CM", "Cameroon" },
                    { 38, "CA", "Canada" },
                    { 39, "CV", "Cape Verde" },
                    { 40, "KY", "Cayman Islands" },
                    { 41, "CF", "Central African Republic" },
                    { 42, "TD", "Chad" },
                    { 43, "CL", "Chile" },
                    { 44, "CN", "China" },
                    { 45, "CX", "Christmas Island" },
                    { 46, "CC", "Cocos (Keeling) Islands" },
                    { 47, "CO", "Colombia" },
                    { 48, "KM", "Comoros" },
                    { 49, "CG", "Congo" },
                    { 50, "CK", "Cook Islands" },
                    { 51, "CR", "Costa Rica" },
                    { 52, "HR", "Croatia (Hrvatska)" },
                    { 53, "CU", "Cuba" },
                    { 54, "CY", "Cyprus" },
                    { 55, "CZ", "Czech Republic" },
                    { 56, "DK", "Denmark" },
                    { 57, "DJ", "Djibouti" },
                    { 58, "DM", "Dominica" },
                    { 59, "DO", "Dominican Republic" },
                    { 32, "BN", "Brunei Darussalam" },
                    { 60, "TP", "East Timor" },
                    { 31, "IO", "British Indian Ocean Territory" },
                    { 29, "BV", "Bouvet Island" },
                    { 2, "AL", "Albania" },
                    { 3, "DZ", "Algeria" },
                    { 4, "DS", "American Samoa" },
                    { 5, "AD", "Andorra" },
                    { 6, "AO", "Angola" },
                    { 7, "AI", "Anguilla" },
                    { 8, "AQ", "Antarctica" },
                    { 9, "AG", "Antigua and Barbuda" },
                    { 10, "AR", "Argentina" },
                    { 11, "AM", "Armenia" },
                    { 12, "AW", "Aruba" },
                    { 13, "AU", "Australia" },
                    { 14, "AT", "Austria" },
                    { 15, "AZ", "Azerbaijan" },
                    { 16, "BS", "Bahamas" },
                    { 17, "BH", "Bahrain" },
                    { 18, "BD", "Bangladesh" },
                    { 19, "BB", "Barbados" },
                    { 20, "BY", "Belarus" },
                    { 21, "BE", "Belgium" },
                    { 22, "BZ", "Belize" },
                    { 23, "BJ", "Benin" },
                    { 24, "BM", "Bermuda" },
                    { 25, "BT", "Bhutan" },
                    { 26, "BO", "Bolivia" },
                    { 27, "BA", "Bosnia and Herzegovina" },
                    { 28, "BW", "Botswana" },
                    { 30, "BR", "Brazil" },
                    { 246, "ZM", "Zambia" },
                    { 61, "EC", "Ecuador" },
                    { 63, "SV", "El Salvador" },
                    { 95, "HN", "Honduras" },
                    { 96, "HK", "Hong Kong" },
                    { 97, "HU", "Hungary" },
                    { 98, "IS", "Iceland" },
                    { 99, "IN", "India" },
                    { 100, "IM", "Isle of Man" },
                    { 101, "ID", "Indonesia" },
                    { 102, "IR", "Iran (Islamic Republic of)" },
                    { 103, "IQ", "Iraq" },
                    { 104, "IE", "Ireland" },
                    { 105, "IL", "Israel" },
                    { 106, "IT", "Italy" },
                    { 107, "CI", "Ivory Coast" },
                    { 108, "JE", "Jersey" },
                    { 109, "JM", "Jamaica" },
                    { 110, "JP", "Japan" },
                    { 111, "JO", "Jordan" },
                    { 112, "KZ", "Kazakhstan" },
                    { 113, "KE", "Kenya" },
                    { 114, "KI", "Kiribati" },
                    { 115, "KP", "Korea, Democratic Peopl's Republic of" },
                    { 116, "KR", "Korea, Republic of" },
                    { 117, "XK", "Kosovo" },
                    { 118, "KW", "Kuwait" },
                    { 119, "KG", "Kyrgyzstan" },
                    { 120, "LA", "Lao People's Democratic Republic" },
                    { 121, "LV", "Latvia" },
                    { 94, "HM", "Heard and Mc Donald Islands" },
                    { 62, "EG", "Egypt" },
                    { 93, "HT", "Haiti" },
                    { 91, "GW", "Guinea-Bissau" },
                    { 64, "GQ", "Equatorial Guinea" },
                    { 65, "ER", "Eritrea" },
                    { 66, "EE", "Estonia" },
                    { 67, "ET", "Ethiopia" },
                    { 68, "FK", "Falkland Islands (Malvinas)" },
                    { 69, "FO", "Faroe Islands" },
                    { 70, "FJ", "Fiji" },
                    { 71, "FI", "Finland" },
                    { 72, "FR", "France" },
                    { 73, "FX", "France, Metropolitan" },
                    { 74, "GF", "French Guiana" },
                    { 75, "PF", "French Polynesia" },
                    { 76, "TF", "French Southern Territories" },
                    { 77, "GA", "Gabon" },
                    { 78, "GM", "Gambia" },
                    { 79, "GE", "Georgia" },
                    { 80, "DE", "Germany" },
                    { 81, "GH", "Ghana" },
                    { 82, "GI", "Gibraltar" },
                    { 83, "GK", "Guernsey" },
                    { 84, "GR", "Greece" },
                    { 85, "GL", "Greenland" },
                    { 86, "GD", "Grenada" },
                    { 87, "GP", "Guadeloupe" },
                    { 88, "GU", "Guam" },
                    { 89, "GT", "Guatemala" },
                    { 90, "GN", "Guinea" },
                    { 92, "GY", "Guyana" },
                    { 247, "ZW", "Zimbabwe" }
                });
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

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CountryList");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CountryList",
                newName: "Details");
        }
    }
}
