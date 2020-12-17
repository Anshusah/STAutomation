using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class addtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Beneficiary",
                columns: table => new
                {
                    BeneficiaryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(type: "varchar(200)", nullable: true),
                    MiddleName = table.Column<string>(type: "varchar(200)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(200)", nullable: true),
                    ShortName = table.Column<string>(type: "varchar(200)", nullable: true),
                    AddressLine1 = table.Column<string>(type: "varchar(500)", nullable: true),
                    AddressLine2 = table.Column<string>(type: "varchar(500)", nullable: true),
                    Suburb = table.Column<string>(type: "varchar(200)", nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(200)", nullable: true),
                    MobileNo = table.Column<string>(type: "varchar(20)", nullable: true),
                    PhoneNo = table.Column<string>(type: "varchar(20)", nullable: true),
                    RelationshipToBeneId = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiary", x => x.BeneficiaryId);
                });

            migrationBuilder.CreateTable(
                name: "BeneficiaryRelationship",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    RelationshipName = table.Column<string>(type: "varchar(100)", nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficiaryRelationship", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentPurpose",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    PurposeName = table.Column<string>(type: "varchar(100)", nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentPurpose", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SupplierId = table.Column<int>(nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    BankBranchId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    BeneficiaryId = table.Column<int>(nullable: false),
                    SendAmount = table.Column<decimal>(nullable: false),
                    PayoutAmount = table.Column<decimal>(nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    TransferFee = table.Column<decimal>(nullable: false),
                    GST = table.Column<decimal>(nullable: false),
                    SendCountryId = table.Column<int>(nullable: false),
                    PayoutCountryId = table.Column<int>(nullable: false),
                    ExchangeRate = table.Column<decimal>(nullable: false),
                    IsFreePayment = table.Column<bool>(nullable: false),
                    PaymentPurpose = table.Column<int>(nullable: false),
                    SourceOfFund = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(type: "varchar(500)", nullable: true),
                    TransactionRefNo = table.Column<string>(type: "varchar(50)", nullable: true),
                    TransactionBookingNo = table.Column<string>(type: "varchar(50)", nullable: true),
                    SupplierTxnRefNo = table.Column<string>(type: "varchar(50)", nullable: true),
                    SupplierTxnStatus = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                });          
           }
    }
}
