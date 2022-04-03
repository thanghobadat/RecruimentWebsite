using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSave = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Careers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Careers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInformations",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkerNumber = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInformations", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_CompanyInformations_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2022, 4, 1, 15, 26, 50, 207, DateTimeKind.Local).AddTicks(9790))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AppUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInformations",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcademicLevel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInformations", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserInformations_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAvatars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FizeSize = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAvatars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyAvatars_CompanyInformations_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyBranches",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBranches", x => new { x.BranchId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_CompanyBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyBranches_CompanyInformations_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyCoverImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FizeSize = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCoverImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyCoverImages_CompanyInformations_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FizeSize = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyImages_CompanyInformations_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recruitments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "none"),
                    DetailedExperience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruitments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recruitments_CompanyInformations_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2022, 4, 1, 15, 26, 50, 181, DateTimeKind.Local).AddTicks(2862)),
                    Performer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_CompanyInformations_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chats_UserInformations_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => new { x.UserId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_Follows_CompanyInformations_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follows_UserInformations_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAvatars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FizeSize = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvatars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAvatars_UserInformations_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchRecruiments",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    RecruimentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchRecruiments", x => new { x.BranchId, x.RecruimentId });
                    table.ForeignKey(
                        name: "FK_BranchRecruiments_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchRecruiments_Recruitments_RecruimentId",
                        column: x => x.RecruimentId,
                        principalTable: "Recruitments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CareerRecruitments",
                columns: table => new
                {
                    RecruimentId = table.Column<int>(type: "int", nullable: false),
                    CareerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerRecruitments", x => new { x.CareerId, x.RecruimentId });
                    table.ForeignKey(
                        name: "FK_CareerRecruitments_Careers_CareerId",
                        column: x => x.CareerId,
                        principalTable: "Careers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CareerRecruitments_Recruitments_RecruimentId",
                        column: x => x.RecruimentId,
                        principalTable: "Recruitments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecruimentId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubcommentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AppUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Recruitments_RecruimentId",
                        column: x => x.RecruimentId,
                        principalTable: "Recruitments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CurriculumVitaes",
                columns: table => new
                {
                    RecruimentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2022, 4, 1, 15, 26, 50, 203, DateTimeKind.Local).AddTicks(6042))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumVitaes", x => new { x.UserId, x.RecruimentId });
                    table.ForeignKey(
                        name: "FK_CurriculumVitaes_Recruitments_RecruimentId",
                        column: x => x.RecruimentId,
                        principalTable: "Recruitments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurriculumVitaes_UserInformations_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInformations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("71b20899-4673-47f7-84fb-40fe7e10d137"), "cea14a8d-678d-45ce-9036-3553cee20c50", "Company role", "company", "company" },
                    { new Guid("05ddc91e-18ae-4ed6-b804-36b75e50d880"), "71871563-1d74-421d-80db-a4026441a188", "User role", "user", "user" },
                    { new Guid("5ca85098-3392-4e71-b85c-fa00171d1def"), "a2e2d531-f163-4c32-8cd9-ee32e59796ad", "Admin role", "admin", "admin" }
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("5ca85098-3392-4e71-b85c-fa00171d1def"), new Guid("42560043-fbe2-4520-a7d1-86c5854ea59e") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateCreated", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("42560043-fbe2-4520-a7d1-86c5854ea59e"), 0, "75d9b258-821a-4adb-8408-f6bce8a867de", new DateTime(2022, 4, 1, 15, 26, 50, 239, DateTimeKind.Local).AddTicks(1991), "hoangthanh01022000@gmail.com", true, false, null, "hoangthanh01022000@gmail.com", "admin", "AQAAAAEAACcQAAAAEHn3TojVYRXeVsDJFMRvOMVuWHiLz6LL8KNp0pdrDpCHe5wT7SJsKcOQXKtDHJu5uA==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "MailSettings",
                columns: new[] { "Id", "DisplayName", "Email", "Host", "Password", "Port" },
                values: new object[] { 1, "Admin", "hoangthanh01022000@gmail.com", "smtp.gmail.com", "Thanhngo@123", 587 });

            migrationBuilder.CreateIndex(
                name: "IX_BranchRecruiments_RecruimentId",
                table: "BranchRecruiments",
                column: "RecruimentId");

            migrationBuilder.CreateIndex(
                name: "IX_CareerRecruitments_RecruimentId",
                table: "CareerRecruitments",
                column: "RecruimentId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_CompanyId",
                table: "Chats",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserId",
                table: "Chats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AccountId",
                table: "Comments",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RecruimentId",
                table: "Comments",
                column: "RecruimentId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAvatars_CompanyId",
                table: "CompanyAvatars",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBranches_CompanyId",
                table: "CompanyBranches",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCoverImages_CompanyId",
                table: "CompanyCoverImages",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyImages_CompanyId",
                table: "CompanyImages",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumVitaes_RecruimentId",
                table: "CurriculumVitaes",
                column: "RecruimentId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_CompanyId",
                table: "Follows",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AccountId",
                table: "Notifications",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruitments_CompanyId",
                table: "Recruitments",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_UserId",
                table: "UserAvatars",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "BranchRecruiments");

            migrationBuilder.DropTable(
                name: "CareerRecruitments");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CompanyAvatars");

            migrationBuilder.DropTable(
                name: "CompanyBranches");

            migrationBuilder.DropTable(
                name: "CompanyCoverImages");

            migrationBuilder.DropTable(
                name: "CompanyImages");

            migrationBuilder.DropTable(
                name: "CurriculumVitaes");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropTable(
                name: "MailSettings");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "UserAvatars");

            migrationBuilder.DropTable(
                name: "Careers");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Recruitments");

            migrationBuilder.DropTable(
                name: "UserInformations");

            migrationBuilder.DropTable(
                name: "CompanyInformations");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
