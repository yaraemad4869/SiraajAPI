using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiraajApi.Migrations
{
    /// <inheritdoc />
    public partial class Sprint1_AddStudentSupervisorFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferralDiscounts_Users_ReferredUserId",
                table: "ReferralDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferralDiscounts_Users_ReferrerId",
                table: "ReferralDiscounts");

            migrationBuilder.CreateTable(
                name: "QuranSurahs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersesCount = table.Column<int>(type: "int", nullable: false),
                    JuzNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuranSurahs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionFeedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionDuration = table.Column<int>(type: "int", nullable: false),
                    QuranRecitation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tajweed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewMemorization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OverallRating = table.Column<int>(type: "int", nullable: false),
                    TeacherNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Homework = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextSessionFocus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionFeedbacks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionFeedbacks_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supervisors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurahCompletionReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurahName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurahNumber = table.Column<int>(type: "int", nullable: false),
                    CompletionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersesCount = table.Column<int>(type: "int", nullable: false),
                    VersesStart = table.Column<int>(type: "int", nullable: false),
                    VersesEnd = table.Column<int>(type: "int", nullable: false),
                    MemorizationAccuracy = table.Column<int>(type: "int", nullable: false),
                    TajweedAccuracy = table.Column<int>(type: "int", nullable: false),
                    Fluency = table.Column<int>(type: "int", nullable: false),
                    Strengths = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weaknesses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextSurahRecommended = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewFrequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurahCompletionReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurahCompletionReports_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurahCompletionReports_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TajweedRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TajweedRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurahsMemorized = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentSurah = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentJuz = table.Column<int>(type: "int", nullable: false),
                    TotalVersesMemorized = table.Column<int>(type: "int", nullable: false),
                    TotalSurahsCompleted = table.Column<int>(type: "int", nullable: false),
                    PreviousKnowledge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousInstitution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousTeacher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TajweedLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SpecialNeeds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Goals = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentDetails_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentDetails_Supervisors_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Supervisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentDetails_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuranSurahs_Number",
                table: "QuranSurahs",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionFeedbacks_StudentId",
                table: "SessionFeedbacks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionFeedbacks_TeacherId",
                table: "SessionFeedbacks",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDetails_StudentId",
                table: "StudentDetails",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDetails_SupervisorId",
                table: "StudentDetails",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDetails_TeacherId",
                table: "StudentDetails",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_Email",
                table: "Supervisors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_UserId",
                table: "Supervisors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SurahCompletionReports_StudentId",
                table: "SurahCompletionReports",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SurahCompletionReports_TeacherId",
                table: "SurahCompletionReports",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralDiscounts_Users_ReferredUserId",
                table: "ReferralDiscounts",
                column: "ReferredUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralDiscounts_Users_ReferrerId",
                table: "ReferralDiscounts",
                column: "ReferrerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferralDiscounts_Users_ReferredUserId",
                table: "ReferralDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferralDiscounts_Users_ReferrerId",
                table: "ReferralDiscounts");

            migrationBuilder.DropTable(
                name: "QuranSurahs");

            migrationBuilder.DropTable(
                name: "SessionFeedbacks");

            migrationBuilder.DropTable(
                name: "StudentDetails");

            migrationBuilder.DropTable(
                name: "SurahCompletionReports");

            migrationBuilder.DropTable(
                name: "TajweedRules");

            migrationBuilder.DropTable(
                name: "Supervisors");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralDiscounts_Users_ReferredUserId",
                table: "ReferralDiscounts",
                column: "ReferredUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralDiscounts_Users_ReferrerId",
                table: "ReferralDiscounts",
                column: "ReferrerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
