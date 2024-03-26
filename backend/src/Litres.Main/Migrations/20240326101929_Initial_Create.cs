using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Litres.Main.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalService",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ServiceToken = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PickupPoint",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FiasAdress = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    WorkingHours = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupPoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    BooksAllowed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Series_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValue: "/"),
                    SubscriptionActiveUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsModerator = table.Column<bool>(type: "bit", nullable: false),
                    Wallet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubscriptionId = table.Column<long>(type: "bigint", nullable: true, defaultValue: 1L),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupPointId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_PickupPoint_PickupPointId",
                        column: x => x.PickupPointId,
                        principalTable: "PickupPoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Publisher",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ContractId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publisher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publisher_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Publisher_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExternalServices",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ExternalServiceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExternalServices", x => new { x.UserId, x.ExternalServiceId });
                    table.ForeignKey(
                        name: "FK_UserExternalServices_ExternalService_ExternalServiceId",
                        column: x => x.ExternalServiceId,
                        principalTable: "ExternalService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserExternalServices_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    CoverUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ContentUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Isbn = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsReadable = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    BookGenres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false),
                    SeriesId = table.Column<long>(type: "bigint", nullable: true),
                    PublisherId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Book_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Book_Publisher_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publisher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Book_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => new { x.UserId, x.BookId });
                    table.ForeignKey(
                        name: "FK_Favourites_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Favourites_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purchased",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchased", x => new { x.UserId, x.BookId });
                    table.ForeignKey(
                        name: "FK_Purchased_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchased_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    PublisherId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBookId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Request_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Request_Book_UpdatedBookId",
                        column: x => x.UpdatedBookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Request_Publisher_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publisher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: true),
                    ParentReviewId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_Review_ParentReviewId",
                        column: x => x.ParentReviewId,
                        principalTable: "Review",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewLike",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    ReviewId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewLike_Review_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Review",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewLike_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Author",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1L, "J.K. Rowling is the author of the much-loved series of seven Harry Potter novels, originally published between 1997 and 2007. Along with the three companion books written for charity, the series has sold over 500 million copies, been translated into 80 languages, and made into eight blockbuster films.", "J. K. Rowling" },
                    { 2L, "Donald John Trump (born June 14, 1946) is an American politician, media personality, and businessman who served as the 45th president of the United States from 2017 to 2021.", "Donald Trump" },
                    { 3L, "Leo Tolstoy – Russian writer and thinker, participated in the defense of Sevastopol, was engaged in educational and journalistic activities. He was at the origins of Tolstoyism, a new religious movement.", "Leo Tolstoy" },
                    { 4L, "Multi-Michelin starred chef and star of the small screen, Gordon Ramsay has opened a string of successful restaurants across the globe.", "Gordon Ramsey" }
                });

            migrationBuilder.InsertData(
                table: "Contract",
                columns: new[] { "Id", "SerialNumber" },
                values: new object[,]
                {
                    { 1L, "AA-0001/01" },
                    { 2L, "AA-0002/01" }
                });

            migrationBuilder.InsertData(
                table: "ExternalService",
                columns: new[] { "Id", "Name", "ServiceToken" },
                values: new object[] { 1L, "Google", "Google Litres 1234" });

            migrationBuilder.InsertData(
                table: "PickupPoint",
                columns: new[] { "Id", "Address", "FiasAdress", "WorkingHours" },
                values: new object[,]
                {
                    { 1L, "ул. Саид-Галеева, д.27", "0ccc56fa-3cb7-4fc9-9627-8417383806bd", "10:00-22:00" },
                    { 2L, "ул. Сабан, д.7А", "9797a9b6-607d-40ed-83c6-2d473be17224", "09:00-21:00" },
                    { 3L, "ул. Ленинградская, д.32", "620d672a-17a7-4db2-821c-46dca70b5817", "09:00-23:00" },
                    { 4L, "ул. Проспет Победы, д.46", "cc72fb9d-4468-449a-a1fe-0d61effb2a5a", "10:00-22:00" },
                    { 5L, "ул. Бондаренко, д.34", "a049a83e-8775-43df-adb4-d1c0f7d9b7a6", "10:00-22:00" },
                    { 6L, "ул. Минская, д.46", "1166b9da-0d95-4179-a1d1-6fafb8250bc6", "09:00-23:00" },
                    { 7L, "ул. Фатыха Амирхана, д.21", "f18d8663-31c9-42ba-819b-6274b1202980", "10:00-22:00" },
                    { 8L, "ул. Щапова, д.2", "2a3a1e2f-f50b-4781-ab9a-450131086bc0", "00:00-23:59" },
                    { 9L, "ул. Нурсултана Назарбаева, д.45А", "383e0f18-63b5-400d-8abb-cacd9f7f1854", "09:00-23:00" },
                    { 10L, "ул. Петра Полушкина, д.6", "f717cf33-87d3-474a-93b3-0d4e804a5462", "08:00-21:00" }
                });

            migrationBuilder.InsertData(
                table: "Subscription",
                columns: new[] { "Id", "BooksAllowed", "Name", "Price" },
                values: new object[,]
                {
                    { 1L, "[1]", "Free", 0 },
                    { 2L, "[1,9,10,19]", "Students", 150 },
                    { 3L, "[1,4,5,9,10,19]", "Scientific", 200 },
                    { 4L, "[]", "Premium", 0 },
                    { 5L, "[1,6,8,11]", "Custom", 0 },
                    { 6L, "[7,16,17,18]", "Custom", 0 }
                });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "Id", "AuthorId", "BookGenres", "ContentUrl", "Count", "CoverUrl", "Description", "IsApproved", "IsAvailable", "IsReadable", "Isbn", "Name", "Price", "PublicationDate", "PublisherId", "Rating", "SeriesId" },
                values: new object[,]
                {
                    { 9L, 3L, "[6,15]", "", 5000, "", "The standard Russian text of War and Peace is divided into four books (comprising fifteen parts) and an epilogue in two parts. ... Although the book is mainly in Russian, significant portions of dialogue are in French. It has been suggested[14] that the use of French is a deliberate literary device, to portray artifice while Russian emerges as a language of sincerity, honesty, and seriousness.", false, false, true, "978-5-907577-08-4", "War and Peace", 400, new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4.5999999999999996, null },
                    { 10L, 4L, "[7]", "", 1000, "", "Do you want to cook a delicious homemade lunch, but don't know where to start? Are you confused in kitchen cabinets and afraid to pick up a mixer? Start your first steps in the challenging world of cooking with our beginner series!Here you will find the best recipes on the most important topics, a detailed description of the cooking stages, step-by-step photos and useful tips. The recipes are divided into three difficulty groups, from the easiest to those that will take a little work. This is a basic tutorial for an inexperienced cook. With it, you will learn how to stir, whip, stew, sift, smear, scroll and much more. Learn to cook with our books!", false, false, false, "978-5-04-155311-1", "Learning how to cook quick recipes for every day", 900, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5.0, null }
                });

            migrationBuilder.InsertData(
                table: "Series",
                columns: new[] { "Id", "AuthorId", "Name" },
                values: new object[] { 1L, 1L, "The Adventures of Harry Potter" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsModerator", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "SubscriptionActiveUntil", "SubscriptionId", "TwoFactorEnabled", "UserName", "Wallet" },
                values: new object[,]
                {
                    { 1L, 0, "", "c2eb1e57-f4e5-4eb0-8bd9-b8e6568728c5", "a@mail.com", false, false, false, null, "User A", null, null, "aaa", null, false, null, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, false, null, 0.00m },
                    { 2L, 0, "", "43ebf569-010b-40a7-89be-f80b64d4e189", "b@mail.com", false, false, false, null, "User B", null, null, "bbb", null, false, null, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, false, null, 3.59m },
                    { 3L, 0, "", "30a40fa2-a6d4-4d85-ac9e-2a3d830128e0", "c@mail.com", false, false, false, null, "User C", null, null, "ccc", null, false, null, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, false, null, 10.00m },
                    { 4L, 0, "", "4ec97b3b-5b11-4e8f-b35b-14e95b085d56", "d@mail.com", false, false, false, null, "User D", null, null, "ddd", null, false, null, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, false, null, 5.32m },
                    { 5L, 0, "", "0f97be1e-4c74-4c4d-a395-7420a907fe91", "e@mail.com", false, false, false, null, "User E", null, null, "eee", null, false, null, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5L, false, null, 1.99m },
                    { 6L, 0, "", "d1090d20-710b-40ab-91ce-831c1a4e2cc7", "f@mail.com", false, false, false, null, "User F", null, null, "fff", null, false, null, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6L, false, null, 7.25m },
                    { 7L, 0, "", "8ed2d651-fe28-45ad-a933-47d5a6cd9246", "g@mail.com", false, false, false, null, "User G", null, null, "ggg", null, false, null, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, false, null, 0.50m }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsModerator", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "SubscriptionActiveUntil", "TwoFactorEnabled", "UserName", "Wallet" },
                values: new object[,]
                {
                    { 8L, 0, "", "fe2c0033-9960-4a20-b16d-7dc68584387e", "h@mail.com", false, true, false, null, "User H", null, null, "hhh", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 0m },
                    { 9L, 0, "aa", "f165ed6b-6d61-40d5-9a63-813dc1edda01", "pA@mail.com", false, false, false, null, "Publisher A", null, null, "aaa", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 6.78m },
                    { 10L, 0, "bb", "405af72b-763f-4b91-8dd1-6e34d498e118", "pB@mail.com", false, false, false, null, "Publisher B", null, null, "bbb", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 4.50m }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "Id", "Description", "PickupPointId", "UserId" },
                values: new object[,]
                {
                    { 1L, "Harry Potter and the Philosopher's Stone * 1; Harry Potter and the Chamber of SecretsЫ * 1;", 1L, 1L },
                    { 2L, "The Lord of the Rings: The Fellowship of the Ring * 1; The Lord of the Rings: The Two Towers * 1;", 2L, 2L },
                    { 3L, "Pride and Prejudice by Jane Austen * 1; Sense and Sensibility by Jane Austen * 1;", 3L, 3L },
                    { 4L, "To Kill a Mockingbird by Harper Lee * 1; The Great Gatsby by F. Scott Fitzgerald * 1;", 4L, 4L },
                    { 5L, "1984 by George Orwell * 1; Animal Farm by George Orwell * 1;", 5L, 5L },
                    { 6L, "The Catcher in the Rye by J.D. Salinger * 1; Catch-22 by Joseph Heller * 1;", 6L, 6L },
                    { 7L, "The Chronicles of Narnia by C.S. Lewis * 1; The Hobbit by J.R.R. Tolkien * 1;", 7L, 7L },
                    { 8L, "The Da Vinci Code by Dan Brown * 1; Angels & Demons by Dan Brown * 1;", 8L, 8L },
                    { 9L, "The Catcher in the Rye by J.D. Salinger * 1; Catch-22 by Joseph Heller * 1;", 9L, 1L },
                    { 10L, "The Chronicles of Narnia by C.S. Lewis * 1; The Hobbit by J.R.R. Tolkien * 1;", 10L, 2L }
                });

            migrationBuilder.InsertData(
                table: "Publisher",
                columns: new[] { "Id", "ContractId" },
                values: new object[,]
                {
                    { 9L, 1L },
                    { 10L, 2L }
                });

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "Id", "BookId", "Content", "CreatedAt", "ParentReviewId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 17L, 9L, "An absolute gem! Emotional and heartwarming.", new DateTime(2024, 3, 9, 10, 17, 0, 0, DateTimeKind.Unspecified), null, 5, 7L },
                    { 18L, 9L, "Couldn't finish it. Lost interest halfway through.", new DateTime(2024, 3, 9, 14, 55, 0, 0, DateTimeKind.Unspecified), null, 2, 8L }
                });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "Id", "AuthorId", "BookGenres", "ContentUrl", "Count", "CoverUrl", "Description", "IsApproved", "IsAvailable", "IsReadable", "Isbn", "Name", "Price", "PublicationDate", "PublisherId", "Rating", "SeriesId" },
                values: new object[,]
                {
                    { 1L, 1L, "[2,3,6,12]", "", 3000, "", "Harry Potter and the Philosopher's Stone is a fantasy novel written by British author J. K. Rowling. The first novel in the Harry Potter series and Rowling's debut novel, it follows Harry Potter, a young wizard who discovers his magical heritage on his eleventh birthday, when he receives a letter of acceptance to Hogwarts School of Witchcraft and Wizardry. Harry makes close friends and a few enemies during his first year at the school and with the help of his friends, Ron Weasley and Hermione Granger, he faces an attempted comeback by the dark wizard Lord Voldemort, who killed Harry's parents, but failed to kill Harry when he was just 15 months old.", false, false, true, "978-5-38-907435-4", "Harry Potter and the Philosopher Stone", 500, new DateTime(1997, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, 4.7000000000000002, 1L },
                    { 2L, 1L, "[2,3,6,12]", "", 3000, "", "Harry Potter and the Chamber of Secrets is a fantasy novel written by British author J. K. Rowling. The second novel in the Harry Potter series, Rowling continues the story of Harry Potter's second year at Hogwarts School of Witchcraft and Wizardry. Along with his friends Ron Weasley and Hermione Granger, Harry investigates a series of mysterious attacks that have targeted students at the school, leaving them petrified. The trio discover that the attacks are being carried out by the mysterious Chamber of Secrets and they must uncover the truth before it is too late.", false, false, true, "978-5-38-907436-1", "Harry Potter and the Chamber of Secrets", 550, new DateTime(1998, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, 4.5, 1L },
                    { 3L, 1L, "[2,3,6,12]", "", 3000, "", "Harry Potter and the Prisoner of Azkaban is a fantasy novel written by British author J. K. Rowling. The third novel in the Harry Potter series, the book follows Harry Potter, a young wizard, in his third year at Hogwarts School of Witchcraft and Wizardry. Along with his friends Ron Weasley and Hermione Granger, Harry investigates the criminal Sirius Black, who is believed to be a supporter of the dark wizard Lord Voldemort. The trio uncover the truth about Sirius and a number of secrets from the past that have a profound impact on Harry's life.", false, false, true, "978-5-38-907437-8", "Harry Potter and the Prisoner of Azkaban", 600, new DateTime(1999, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, 4.7999999999999998, 1L },
                    { 4L, 1L, "[2,3,6,12]", "", 3000, "", "Harry Potter and the Goblet of Fire is a fantasy novel written by British author J. K. Rowling. The fourth novel in the Harry Potter series, the book follows Harry Potter, a young wizard, in his fourth year at Hogwarts School of Witchcraft and Wizardry. The story revolves around the Triwizard Tournament, where three magical schools compete against each other. However, during the tournament, Harry encounters the resurrected Lord Voldemort and his plans for revenge against Harry.", false, false, true, "978-5-38-907438-5", "Harry Potter and the Goblet of Fire", 650, new DateTime(2000, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, 4.7000000000000002, 1L },
                    { 5L, 1L, "[2,3,6,12]", "", 3000, "", "Harry Potter and the Order of the Phoenix is a fantasy novel written by British author J. K. Rowling. The fifth novel in the Harry Potter series, the book follows Harry Potter, a young wizard, in his fifth year at Hogwarts School of Witchcraft and Wizardry. Harry and his friends form a secret defensive group called Dumbledore's Army to prepare for Lord Voldemort's return and the wizarding war that is about to erupt. As Harry faces challenges, he also experiences the emotional struggles of adolescence and deals with the oppressive authority at Hogwarts.", false, false, true, "978-5-38-907439-2", "Harry Potter and the Order of the Phoenix", 700, new DateTime(2003, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, 4.9000000000000004, 1L },
                    { 6L, 1L, "[2,3,6,12]", "", 3000, "", "Harry Potter and the Half-Blood Prince is a fantasy novel written by British author J. K. Rowling. The sixth novel in the Harry Potter series, the book follows Harry Potter, a young wizard, in his sixth year at Hogwarts School of Witchcraft and Wizardry. As Harry prepares for the battle against Lord Voldemort, he uncovers important information about Voldemort's past and learns about the concept of Horcruxes. The story explores the darker side of magic and the sacrifices that need to be made in the face of ultimate evil.", false, false, true, "978-5-38-907440-8", "Harry Potter and the Half-Blood Prince", 750, new DateTime(2005, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, 4.7999999999999998, 1L },
                    { 7L, 1L, "[2,3,6,12]", "", 3000, "", "Harry Potter and the Deathly Hallows is a fantasy novel written by British author J. K. Rowling. The seventh and final novel in the Harry Potter series, the book follows Harry Potter, a young wizard, in his seventh year at Hogwarts School of Witchcraft and Wizardry. As the wizarding war reaches its climax, Harry, Ron, and Hermione set out on a mission to destroy the remaining Horcruxes and defeat Lord Voldemort once and for all. The book explores themes of sacrifice, friendship, and the power of love in the face of evil.", false, false, true, "978-5-38-907441-5", "Harry Potter and the Deathly Hallows", 800, new DateTime(2007, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, 4.9000000000000004, 1L },
                    { 8L, 2L, "[4]", "", 3000, "", "To become a truly rich person, you need to learn how to think like a billionaire. And here the real estate genius, bestselling author and TV star Donald Trump will come to your aid. He will show you how to properly treat money, career, your own talents and life in general. In this book, you will find excellent advice from a recognized expert on investing in real estate: from methods of communicating with brokers to recommendations on building renovation and real estate valuation methods.", false, false, true, "978-5-96-142785-1", "Think like a billionaire", 1000, new DateTime(2016, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 10L, 3.8999999999999999, null }
                });

            migrationBuilder.InsertData(
                table: "ReviewLike",
                columns: new[] { "Id", "IsLike", "ReviewId", "UserId" },
                values: new object[,]
                {
                    { 17L, true, 17L, 8L },
                    { 18L, false, 18L, 9L }
                });

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "Id", "BookId", "Content", "CreatedAt", "ParentReviewId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L, "This is the greatest book I've read. Made my childhood", new DateTime(2024, 3, 3, 19, 42, 0, 0, DateTimeKind.Unspecified), null, 5, 1L },
                    { 3L, 2L, "A masterpiece! Couldn't put it down.", new DateTime(2024, 3, 4, 9, 27, 0, 0, DateTimeKind.Unspecified), null, 5, 3L },
                    { 4L, 2L, "I found it quite boring. Couldn't get into the story.", new DateTime(2024, 3, 4, 10, 9, 0, 0, DateTimeKind.Unspecified), null, 2, 4L },
                    { 5L, 3L, "Highly recommend! Amazing plot twists.", new DateTime(2024, 3, 4, 15, 56, 0, 0, DateTimeKind.Unspecified), null, 5, 5L },
                    { 6L, 3L, "Waste of time. Predictable and unoriginal.", new DateTime(2024, 3, 4, 17, 21, 0, 0, DateTimeKind.Unspecified), null, 1, 6L },
                    { 7L, 4L, "Not my cup of tea. Didn't connect with the characters.", new DateTime(2024, 3, 5, 8, 50, 0, 0, DateTimeKind.Unspecified), null, 2, 7L },
                    { 8L, 4L, "Absolutely brilliant! A must-read for any fantasy lover.", new DateTime(2024, 3, 5, 12, 37, 0, 0, DateTimeKind.Unspecified), null, 5, 8L },
                    { 9L, 5L, "Incredible book. Touching and thought-provoking.", new DateTime(2024, 3, 5, 19, 15, 0, 0, DateTimeKind.Unspecified), null, 4, 9L },
                    { 10L, 5L, "Couldn't get into it. Slow-paced and uninteresting.", new DateTime(2024, 3, 5, 20, 59, 0, 0, DateTimeKind.Unspecified), null, 2, 10L },
                    { 11L, 6L, "Loved every page! Captivating and beautifully written.", new DateTime(2024, 3, 6, 11, 42, 0, 0, DateTimeKind.Unspecified), null, 5, 1L },
                    { 12L, 6L, "Disappointing. Expected much more from the hype.", new DateTime(2024, 3, 6, 14, 17, 0, 0, DateTimeKind.Unspecified), null, 3, 2L },
                    { 13L, 7L, "Couldn't put it down! Gripping and suspenseful.", new DateTime(2024, 3, 7, 9, 38, 0, 0, DateTimeKind.Unspecified), null, 5, 3L },
                    { 14L, 7L, "Hated it. Didn't understand the point of the story.", new DateTime(2024, 3, 7, 13, 25, 0, 0, DateTimeKind.Unspecified), null, 1, 4L },
                    { 15L, 8L, "Beautifully written. The author's words touched my soul.", new DateTime(2024, 3, 8, 16, 59, 0, 0, DateTimeKind.Unspecified), null, 4, 5L },
                    { 16L, 8L, "Overrated. Too much hype for a mediocre story.", new DateTime(2024, 3, 8, 18, 42, 0, 0, DateTimeKind.Unspecified), null, 2, 6L },
                    { 2L, null, "Can't disagree more. Awful book", new DateTime(2024, 3, 3, 20, 13, 0, 0, DateTimeKind.Unspecified), 1L, 0, 2L },
                    { 21L, null, "Totally agree", new DateTime(2024, 3, 11, 20, 13, 0, 0, DateTimeKind.Unspecified), 3L, 0, 2L },
                    { 22L, null, "What a great review!", new DateTime(2024, 3, 11, 22, 17, 0, 0, DateTimeKind.Unspecified), 3L, 0, 4L }
                });

            migrationBuilder.InsertData(
                table: "ReviewLike",
                columns: new[] { "Id", "IsLike", "ReviewId", "UserId" },
                values: new object[,]
                {
                    { 1L, true, 1L, 1L },
                    { 3L, true, 3L, 1L },
                    { 4L, false, 4L, 2L },
                    { 5L, true, 5L, 2L },
                    { 6L, false, 6L, 3L },
                    { 7L, true, 7L, 3L },
                    { 8L, false, 8L, 4L },
                    { 9L, true, 9L, 4L },
                    { 10L, false, 10L, 5L },
                    { 11L, true, 11L, 5L },
                    { 12L, false, 12L, 6L },
                    { 13L, true, 13L, 6L },
                    { 14L, false, 14L, 7L },
                    { 15L, true, 15L, 7L },
                    { 16L, false, 16L, 8L },
                    { 24L, false, 1L, 2L },
                    { 26L, false, 3L, 3L },
                    { 27L, true, 4L, 3L },
                    { 28L, false, 5L, 4L },
                    { 29L, true, 6L, 4L },
                    { 30L, false, 7L, 5L },
                    { 31L, true, 8L, 5L },
                    { 32L, false, 9L, 6L },
                    { 33L, true, 10L, 6L }
                });

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "Id", "BookId", "Content", "CreatedAt", "ParentReviewId", "Rating", "UserId" },
                values: new object[] { 23L, null, "Thx :3", new DateTime(2024, 3, 12, 12, 37, 0, 0, DateTimeKind.Unspecified), 22L, 0, 2L });

            migrationBuilder.InsertData(
                table: "ReviewLike",
                columns: new[] { "Id", "IsLike", "ReviewId", "UserId" },
                values: new object[,]
                {
                    { 2L, false, 2L, 1L },
                    { 21L, true, 21L, 9L },
                    { 22L, false, 22L, 10L },
                    { 25L, true, 2L, 2L },
                    { 23L, true, 23L, 10L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorId",
                table: "Book",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_PublisherId",
                table: "Book",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_SeriesId",
                table: "Book",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_BookId",
                table: "Favourites",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PickupPointId",
                table: "Order",
                column: "PickupPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Publisher_ContractId",
                table: "Publisher",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchased_BookId",
                table: "Purchased",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_BookId",
                table: "Request",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_PublisherId",
                table: "Request",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_UpdatedBookId",
                table: "Request",
                column: "UpdatedBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_BookId",
                table: "Review",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ParentReviewId",
                table: "Review",
                column: "ParentReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId",
                table: "Review",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLike_ReviewId",
                table: "ReviewLike",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLike_UserId",
                table: "ReviewLike",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_AuthorId",
                table: "Series",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SubscriptionId",
                table: "User",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExternalServices_ExternalServiceId",
                table: "UserExternalServices",
                column: "ExternalServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favourites");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Purchased");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "ReviewLike");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserExternalServices");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "PickupPoint");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "ExternalService");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Publisher");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
