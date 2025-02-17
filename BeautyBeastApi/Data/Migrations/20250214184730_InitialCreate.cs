using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeautyBeastApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "TEXT", nullable: true),
                    UserType = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    Bio = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtistAchievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArtistId = table.Column<int>(type: "INTEGER", nullable: false),
                    Achievement = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtistAchievements_Users_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    MediaUrls = table.Column<string>(type: "TEXT", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Likes = table.Column<int>(type: "INTEGER", nullable: false),
                    ArtistId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Users_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Likes = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statuses_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Statuses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    PreCareInstructions = table.Column<string>(type: "TEXT", nullable: false),
                    AfterCareInstructions = table.Column<string>(type: "TEXT", nullable: false),
                    ConsentFormUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ArtistId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treatments_Users_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserUser",
                columns: table => new
                {
                    FollowersId = table.Column<int>(type: "INTEGER", nullable: false),
                    FollowingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUser", x => new { x.FollowersId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_UserUser_Users_FollowersId",
                        column: x => x.FollowersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUser_Users_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TheComment = table.Column<string>(type: "TEXT", nullable: false),
                    Likes = table.Column<int>(type: "INTEGER", nullable: false),
                    PostId = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    TreatmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    BookingDateAndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Bio", "Email", "FullName", "ProfilePictureUrl", "UserType" },
                values: new object[,]
                {
                    { 1, "Master of PMU", "leavinci@gmail.com", "Lea Vinci", "lea.jpg", "Artist" },
                    { 2, "Make-up Artist", "rachelhertz@gmail.com", "Rachel Hertzler", "rachel.jpg", "Artist" },
                    { 3, "Hairdresser", "vivas@gmail.com", "Vivian A", "viv.jpg", "Artist" },
                    { 4, "Aesthetician", "fridaleon@gmail.com", "Frida Leon", "frida.jpg", "Artist" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "ProfilePictureUrl", "UserType" },
                values: new object[,]
                {
                    { 5, "johndoe@gmail.com", "John Doe", "john.jpg", "Client" },
                    { 6, "emma@gmail.com", "Emma Watson", "emma.jpg", "Client" }
                });

            migrationBuilder.InsertData(
                table: "ArtistAchievements",
                columns: new[] { "Id", "Achievement", "ArtistId" },
                values: new object[,]
                {
                    { 1, "PMU Certification", 1 },
                    { 2, "Bridal Makeup Specialist", 1 },
                    { 3, "Master Stylist", 2 },
                    { 4, "Color Correction Expert", 2 },
                    { 5, "Skin Care Specialist", 3 },
                    { 6, "Hydrafacial Expert", 3 },
                    { 7, "Permanent Makeup Trainer", 4 },
                    { 8, "Aesthetic Specialist", 4 }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "ArtistId", "DatePosted", "Description", "Likes", "MediaUrls" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 2, 20, 16, 0, 0, 0, DateTimeKind.Unspecified), "My first PMU work!", 0, "[\"pmu-work.jpg\"]" },
                    { 2, 2, new DateTime(2024, 2, 21, 15, 30, 0, 0, DateTimeKind.Unspecified), "Chiaroscuro masterpiece!", 0, "[\"chiaroscuro.jpg\"]" }
                });

            migrationBuilder.InsertData(
                table: "Treatments",
                columns: new[] { "Id", "AfterCareInstructions", "ArtistId", "ConsentFormUrl", "Description", "Name", "PreCareInstructions" },
                values: new object[,]
                {
                    { 1, "Follow aftercare card", 1, "microblading-consent.pdf", "Semi-permanent eyebrows", "Microblading", "Avoid caffeine" },
                    { 2, "Moisturize daily", 1, "ombre-brows-consent.pdf", "Powdered shading brows", "Ombre Brows", "No makeup before session" },
                    { 3, "Use oil-free remover", 2, "bridal-consent.pdf", "Full bridal makeup", "Bridal Makeup", "Clean face before" },
                    { 4, "Use gentle remover", 2, "evening-makeup-consent.pdf", "Makeup for special occasions", "Evening Glam Makeup", "Moisturize before" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingDateAndTime", "ClientId", "Status", "TreatmentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), 5, "Pending", 1 },
                    { 2, new DateTime(2024, 3, 12, 10, 30, 0, 0, DateTimeKind.Unspecified), 6, "Pending", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistAchievements_ArtistId",
                table: "ArtistAchievements",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ClientId",
                table: "Bookings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TreatmentId",
                table: "Bookings",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_StatusId",
                table: "Comments",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ArtistId",
                table: "Posts",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_ClientId",
                table: "Statuses",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_UserId",
                table: "Statuses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_ArtistId",
                table: "Treatments",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_FollowingId",
                table: "UserUser",
                column: "FollowingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistAchievements");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "UserUser");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
