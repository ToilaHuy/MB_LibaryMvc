using System;
using Bogus;
using Bogus.DataSets;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApplication8.Data;

#nullable disable

namespace WebApplication8.Migrations
{
   
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
    
    protected override void Up(MigrationBuilder migrationBuilder)
        {
      
        migrationBuilder.CreateTable(
                name: "Borrowers",
                columns: table => new
                {
                    BorrowerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowers", x => x.BorrowerId);
                });

          
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    NumberOfPages = table.Column<int>(type: "int", nullable: true),
                    RunTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });




            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassWord = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingHistories",
                columns: table => new
                {
                    BorrowingHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BorrowerId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingHistories", x => x.BorrowingHistoryId);
                    table.ForeignKey(
                        name: "FK_BorrowingHistories_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "BorrowerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoryDetails",
                columns: table => new
                {
                    HistoryDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BorrowingHistoryId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryDetails", x => x.HistoryDetailId);
                    table.ForeignKey(
                        name: "FK_HistoryDetails_BorrowingHistories_BorrowingHistoryId",
                        column: x => x.BorrowingHistoryId,
                        principalTable: "BorrowingHistories",
                        principalColumn: "BorrowingHistoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingHistories_BorrowerId",
                table: "BorrowingHistories",
                column: "BorrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryDetails_BorrowingHistoryId",
                table: "HistoryDetails",
                column: "BorrowingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryDetails_ItemId",
                table: "HistoryDetails",
                column: "ItemId");


            Randomizer.Seed = new(8675309);
            var fakeBorrower = new Faker<Borrower>()    
                .RuleFor(b => b.BorrowerName, f => f.Name.FullName())
                .RuleFor(b => b.Address, f => f.Address.City())
                .RuleFor(b => b.ImageUrl, f => f.Internet.Avatar());
            //insert data 
            for (int i = 0; i < 100; i++)
            {
                var borrower = fakeBorrower.Generate();

                migrationBuilder.InsertData(
                    table: "Borrowers",
                    columns: new[] { "BorrowerName", "Address", "ImageUrl" },
                    values: new object[]
                    {
                        borrower.BorrowerName,
                        borrower.Address,
                        borrower.ImageUrl
                    });
            }

            var fakeItems = new Faker<Item>()
                .RuleFor(i => i.Title, f => f.Lorem.Sentence())
                .RuleFor(i => i.PublicationDate,
                    f => f.Date.Between(new(2000, 1, 1), new(2023, 5, 1)))
                .RuleFor(i => i.Author, f => f.Name.FullName())
                .RuleFor(i => i.Price, f => f.Random.Number(10000, 100000))
                .RuleFor(i => i.NumberOfPages, 0)
                .RuleFor(i => i.RunTime, "")
                .RuleFor(i => i.ImageUrl, f => f.Internet.Avatar())
                .RuleFor(i => i.Quantity, 10)
                .RuleFor(i => i.AvailableQuantity, 10)
                .RuleFor(i => i.Category, f => f.PickRandom<Item.CategoryName>());



            Random rnd = new();
       
            for (int i = 0; i < 100000; i++)
            {
                var item = fakeItems.Generate();

                migrationBuilder.InsertData(
                    table: "Items",
                    columns: new[] { "Title", "PublicationDate", "Author", "Price", "NumberOfPages", "RunTime", "ImageUrl", "Quantity", "AvailableQuantity", "Category" },
                    values: new object[]
                    {
                        item.Title,
                        item.PublicationDate,
                        item.Author,
                        item.Price,
                        item.NumberOfPages,
                        item.RunTime,
                        item.ImageUrl,
                        item.Quantity,
                        item.AvailableQuantity,
                        rnd.Next(0,2)
                    });
            }

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BorrowingHistories");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Borrowers");
        }
    }
}
