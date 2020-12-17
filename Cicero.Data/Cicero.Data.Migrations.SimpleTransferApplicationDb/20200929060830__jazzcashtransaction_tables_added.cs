using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _jazzcashtransaction_tables_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JazzCashTransaction",
                columns: table => new
                {
                    JazzCashTransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentRequestId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    BankBranchId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    BeneficiaryId = table.Column<int>(nullable: false),
                    SendAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayoutAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    TransferFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GST = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SendCountryId = table.Column<int>(nullable: false),
                    PayoutCountryId = table.Column<int>(nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_JazzCashTransaction", x => x.JazzCashTransactionId);
                });

            migrationBuilder.CreateTable(
                name: "JazzCashTransactionHistory",
                columns: table => new
                {
                    JazzCashTransactionHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionRefNo = table.Column<string>(type: "varchar(50)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SupplierTxnRefNo = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    SupplierTxnStatus = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    RemarkBy = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JazzCashTransactionHistory", x => x.JazzCashTransactionHistoryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JazzCashTransaction");

            migrationBuilder.DropTable(
                name: "JazzCashTransactionHistory");
        }
    }
}
