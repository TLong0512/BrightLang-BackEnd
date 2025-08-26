using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Level",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Level_ExamType_ExamTypeId",
                        column: x => x.ExamTypeId,
                        principalTable: "ExamType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillLevel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillLevel_Level_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Level",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillLevel_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Range",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartQuestionNumber = table.Column<int>(type: "int", nullable: false),
                    EndQuestionNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Range", x => x.Id);
                    table.CheckConstraint("CK_QuestionRange_StartLessThanEnd", "[StartQuestionNumber] < [EndQuestionNumber]");
                    table.ForeignKey(
                        name: "FK_Range_SkillLevel_SkillLevelId",
                        column: x => x.SkillLevelId,
                        principalTable: "SkillLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Context",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBelongTest = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Context", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Context_Range_RangeId",
                        column: x => x.RangeId,
                        principalTable: "Range",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionNumber = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Context_ContextId",
                        column: x => x.ContextId,
                        principalTable: "Context",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ExamType",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("38af4f71-911a-422f-9424-f3a24a3427f2"), null, null, "TOPIK II (Cấp 3–6) là kỳ thi kiểm tra trình độ tiếng Hàn từ trung cấp đến cao cấp, đánh giá kỹ năng nghe, đọc và viết dành cho người không phải bản ngữ. Kỳ thi đánh giá khả năng giao tiếp từ mức thông thường đến gần như người bản xứ. Được sử dụng cho nhập học đại học, công việc chuyên môn, xin visa, học bổng và thăng tiến nghề nghiệp, đặc biệt ở Việt Nam, chứng chỉ TOPIK II có giá trị trong hai năm và được công nhận toàn cầu.", "TOPIK II", null, null },
                    { new Guid("b0f20da4-09d4-4ab8-ab62-b031e00ba9f2"), null, null, "TOPIK I (Cấp 1 và 2) là kỳ thi kiểm tra trình độ tiếng Hàn cấp độ sơ cấp, đánh giá kỹ năng nghe và đọc cơ bản dành cho người không phải bản ngữ. Kỳ thi kiểm tra khả năng giao tiếp đơn giản với 800–2.000 từ vựng. Được sử dụng cho nhập học đại học, xin visa (ví dụ: visa kết hôn hoặc định cư tại Hàn Quốc), công việc cấp nhập môn và mục tiêu học ngôn ngữ cá nhân, chứng chỉ TOPIK I có giá trị trong hai năm và được công nhận toàn cầu.", "TOPIK I", null, null }
                });

            migrationBuilder.InsertData(
                table: "Skill",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "SkillName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("1bb8514e-a1a6-4c41-b176-f20124f4044e"), null, null, "Viết", null, null },
                    { new Guid("56e413be-c614-41b9-934b-c8b60fb24cfc"), null, null, "Đọc", null, null },
                    { new Guid("ba618367-1681-49ae-9d3a-be194de54403"), null, null, "Nghe", null, null }
                });

            migrationBuilder.InsertData(
                table: "Level",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "ExamTypeId", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("3d463174-b4f8-4604-9056-e3638571bc40"), null, null, new Guid("38af4f71-911a-422f-9424-f3a24a3427f2"), "Cấp 3", null, null },
                    { new Guid("40945347-4a3d-4938-9ba5-76901120f951"), null, null, new Guid("38af4f71-911a-422f-9424-f3a24a3427f2"), "Cấp 6", null, null },
                    { new Guid("58b35613-caf6-42f3-ace9-195f39f2ca86"), null, null, new Guid("38af4f71-911a-422f-9424-f3a24a3427f2"), "Cấp 4", null, null },
                    { new Guid("7dc94003-4c5e-44d9-bc58-cf935d8f5fbc"), null, null, new Guid("38af4f71-911a-422f-9424-f3a24a3427f2"), "Cấp 5", null, null },
                    { new Guid("e7b61c72-a094-4f81-806e-1a75a878557d"), null, null, new Guid("b0f20da4-09d4-4ab8-ab62-b031e00ba9f2"), "Cấp 1", null, null },
                    { new Guid("fcffbd73-5454-4af6-942d-645d120edac6"), null, null, new Guid("b0f20da4-09d4-4ab8-ab62-b031e00ba9f2"), "Cấp 2", null, null }
                });

            migrationBuilder.InsertData(
                table: "SkillLevel",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "LevelId", "SkillId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("038dc84d-84bf-4017-8b51-49041d7c69f1"), null, null, new Guid("fcffbd73-5454-4af6-942d-645d120edac6"), new Guid("56e413be-c614-41b9-934b-c8b60fb24cfc"), null, null },
                    { new Guid("04484630-4111-4afb-aab2-73e703ace410"), null, null, new Guid("40945347-4a3d-4938-9ba5-76901120f951"), new Guid("1bb8514e-a1a6-4c41-b176-f20124f4044e"), null, null },
                    { new Guid("05fd8057-f016-481d-9edb-52cdee594d01"), null, null, new Guid("58b35613-caf6-42f3-ace9-195f39f2ca86"), new Guid("1bb8514e-a1a6-4c41-b176-f20124f4044e"), null, null },
                    { new Guid("1127b458-58e3-482c-8c9a-7cb897482375"), null, null, new Guid("3d463174-b4f8-4604-9056-e3638571bc40"), new Guid("56e413be-c614-41b9-934b-c8b60fb24cfc"), null, null },
                    { new Guid("203ee981-d694-40e4-8a30-b7c1a1e6ec09"), null, null, new Guid("fcffbd73-5454-4af6-942d-645d120edac6"), new Guid("ba618367-1681-49ae-9d3a-be194de54403"), null, null },
                    { new Guid("2dbcff6d-8ebd-404d-b89c-d88b71733925"), null, null, new Guid("7dc94003-4c5e-44d9-bc58-cf935d8f5fbc"), new Guid("1bb8514e-a1a6-4c41-b176-f20124f4044e"), null, null },
                    { new Guid("32dba996-050b-48bc-832a-efbef20c5df4"), null, null, new Guid("3d463174-b4f8-4604-9056-e3638571bc40"), new Guid("1bb8514e-a1a6-4c41-b176-f20124f4044e"), null, null },
                    { new Guid("5bda5fed-c4d5-4707-879d-b6abe6f6b787"), null, null, new Guid("7dc94003-4c5e-44d9-bc58-cf935d8f5fbc"), new Guid("ba618367-1681-49ae-9d3a-be194de54403"), null, null },
                    { new Guid("6d63b5d7-8fc3-45be-a9f1-6af0f651b5e3"), null, null, new Guid("40945347-4a3d-4938-9ba5-76901120f951"), new Guid("56e413be-c614-41b9-934b-c8b60fb24cfc"), null, null },
                    { new Guid("83128ff4-d33d-40d6-87ee-b0013eb2521a"), null, null, new Guid("40945347-4a3d-4938-9ba5-76901120f951"), new Guid("ba618367-1681-49ae-9d3a-be194de54403"), null, null },
                    { new Guid("8aa5e695-9c2f-4f2b-a71d-482618517617"), null, null, new Guid("58b35613-caf6-42f3-ace9-195f39f2ca86"), new Guid("ba618367-1681-49ae-9d3a-be194de54403"), null, null },
                    { new Guid("aca5da63-e3b8-4d4f-a337-982e7585ef95"), null, null, new Guid("7dc94003-4c5e-44d9-bc58-cf935d8f5fbc"), new Guid("56e413be-c614-41b9-934b-c8b60fb24cfc"), null, null },
                    { new Guid("b163baf3-c72b-4763-b23b-001f7747e36c"), null, null, new Guid("58b35613-caf6-42f3-ace9-195f39f2ca86"), new Guid("56e413be-c614-41b9-934b-c8b60fb24cfc"), null, null },
                    { new Guid("bcb3d3cd-a148-4562-8b67-ee37c0263bc6"), null, null, new Guid("3d463174-b4f8-4604-9056-e3638571bc40"), new Guid("ba618367-1681-49ae-9d3a-be194de54403"), null, null },
                    { new Guid("c5afb485-fb72-4f70-842d-5f785281b516"), null, null, new Guid("e7b61c72-a094-4f81-806e-1a75a878557d"), new Guid("56e413be-c614-41b9-934b-c8b60fb24cfc"), null, null },
                    { new Guid("e8d1e161-73a9-46c0-abc3-1552188a0ead"), null, null, new Guid("e7b61c72-a094-4f81-806e-1a75a878557d"), new Guid("ba618367-1681-49ae-9d3a-be194de54403"), null, null }
                });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "EndQuestionNumber", "Name", "SkillLevelId", "StartQuestionNumber", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("094a3b15-91fc-43af-9fac-b84f07043a34"), null, null, 50, "Điền từ/cụm từ vào chỗ trống hoặc chọn đáp án chính xác với mục đích của đoạn văn.", new Guid("6d63b5d7-8fc3-45be-a9f1-6af0f651b5e3"), 46, null, null },
                    { new Guid("096d3210-1a35-4861-a58e-0988ad989521"), null, null, 24, "Chọn đáp án phù hợp với phần gạch chân trong đoạn văn.", new Guid("b163baf3-c72b-4763-b23b-001f7747e36c"), 22, null, null },
                    { new Guid("0f434d8f-8fef-427d-aa9c-b4130077d8ac"), null, null, 37, "Nghe cuộc thảo luận/ý kiến trái chiều rồi chọn nội dung chính hoặc quan điểm nhân vật.", new Guid("5bda5fed-c4d5-4707-879d-b6abe6f6b787"), 34, null, null },
                    { new Guid("0fbcf9e9-3585-4fc5-b85a-6773b8affb4d"), null, null, 20, "Điền từ/cụm từ trong đoạn văn ngắn.", new Guid("038dc84d-84bf-4017-8b51-49041d7c69f1"), 16, null, null },
                    { new Guid("118d0b47-3831-45e9-9ffa-1feaa8cfa678"), null, null, 43, "Nghe thảo luận/phát biểu rồi chọn đáp án phù hợp với nội dung gạch chân.", new Guid("83128ff4-d33d-40d6-87ee-b0013eb2521a"), 41, null, null },
                    { new Guid("135b5eea-7144-4ee2-94fd-2a4b4ca26402"), null, null, 5, "Điền từ/cụm từ vào chỗ trống trong câu đơn giản.", new Guid("c5afb485-fb72-4f70-842d-5f785281b516"), 1, null, null },
                    { new Guid("176ea83c-dff6-4b0f-a2d0-886487a7b989"), null, null, 35, "Chọn ngữ pháp phù hợp trong đoạn văn.", new Guid("203ee981-d694-40e4-8a30-b7c1a1e6ec09"), 31, null, null },
                    { new Guid("19bdbded-e2c6-4595-adb8-06a33bd918f4"), null, null, 25, "Chọn thái độ người nói hoặc ý nghĩa câu nói.", new Guid("203ee981-d694-40e4-8a30-b7c1a1e6ec09"), 21, null, null },
                    { new Guid("21919199-2703-4d0f-92b5-b3046cdf7005"), null, null, 10, "Chọn ngữ pháp phù hợp với câu.", new Guid("c5afb485-fb72-4f70-842d-5f785281b516"), 6, null, null },
                    { new Guid("23390cda-3b35-44fa-9e65-14c71ccfdf3d"), null, null, 24, "Nghe hội thoại ngắn rồi chọn nội dung tương ứng với phần gạch chân.", new Guid("8aa5e695-9c2f-4f2b-a71d-482618517617"), 22, null, null },
                    { new Guid("291ab660-46c5-441e-bf10-76c5b516e706"), null, null, 20, "Chọn thông tin bổ sung từ thông báo hoặc đoạn hội thoại.", new Guid("203ee981-d694-40e4-8a30-b7c1a1e6ec09"), 16, null, null },
                    { new Guid("2d76b832-7ce6-44d8-8c21-8693d992fe1a"), null, null, 30, "Chọn mục đích người nói hoặc thông tin chi tiết nâng cao.", new Guid("203ee981-d694-40e4-8a30-b7c1a1e6ec09"), 26, null, null },
                    { new Guid("40996dd9-4290-432e-aee7-251e20bcb1bc"), null, null, 45, "Chọn đáp án phù hợp với chủ đề của đoạn văn.", new Guid("6d63b5d7-8fc3-45be-a9f1-6af0f651b5e3"), 44, null, null },
                    { new Guid("4a044cb3-57e1-4530-b0d5-ca157cd95420"), null, null, 25, "Chọn thông tin chi tiết trong đoạn văn ngắn.", new Guid("203ee981-d694-40e4-8a30-b7c1a1e6ec09"), 21, null, null },
                    { new Guid("4ee1ec87-0115-479b-a433-344dd1796364"), null, null, 15, "Trả lời câu hỏi về thông báo, biển báo, hướng dẫn đơn giản.", new Guid("c5afb485-fb72-4f70-842d-5f785281b516"), 11, null, null },
                    { new Guid("4fcdbda9-66eb-4dfa-ac25-f3c9477afb43"), null, null, 50, "Nghe đoạn văn bản dài (tin tức, bài giảng, phỏng vấn) rồi chọn đáp án đúng với chủ đề, mục đích, thái độ, hoặc chi tiết quan trọng..", new Guid("83128ff4-d33d-40d6-87ee-b0013eb2521a"), 44, null, null },
                    { new Guid("505d8202-28a9-4d86-bd97-94985fa4b05d"), null, null, 12, "Nghe thông báo/hội thoại ngắn rồi chọn đáp án phù hợp với nội dung chính.", new Guid("bcb3d3cd-a148-4562-8b67-ee37c0263bc6"), 9, null, null },
                    { new Guid("6056c922-bbac-4ad7-8109-1d83e54a1977"), null, null, 12, "Chọn nội dung tương ứng với biểu đồ hoặc thông báo.", new Guid("1127b458-58e3-482c-8c9a-7cb897482375"), 9, null, null },
                    { new Guid("65d010e8-10c3-4336-9d1a-d01160dec39e"), null, null, 15, "Chọn thông tin chi tiết hoặc ý chính trong đoạn hội thoại dài hơn.", new Guid("203ee981-d694-40e4-8a30-b7c1a1e6ec09"), 11, null, null },
                    { new Guid("67d9372e-53b5-42f6-b942-fb9403c511df"), null, null, 10, "Chọn ý chính hoặc thông tin chi tiết trong hội thoại.", new Guid("e8d1e161-73a9-46c0-abc3-1552188a0ead"), 7, null, null },
                    { new Guid("756aefc0-5d21-4aaa-8c42-e4f2a7ef168f"), null, null, 40, "Chọn ý chính hoặc thông tin chi tiết trong đoạn văn.", new Guid("203ee981-d694-40e4-8a30-b7c1a1e6ec09"), 36, null, null },
                    { new Guid("78531118-1460-470a-9bbf-bfe41a1f49fc"), null, null, 3, "Nghe hội thoại ngắn và chọn tranh/đáp án phù hợp.", new Guid("bcb3d3cd-a148-4562-8b67-ee37c0263bc6"), 1, null, null },
                    { new Guid("78b16532-6721-421b-be68-5f811e6f33f0"), null, null, 33, "Nghe bài giảng/thuyết trình ngắn rồi chọn đáp án phù hợp với nội dung hoặc thái độ người nói.", new Guid("5bda5fed-c4d5-4707-879d-b6abe6f6b787"), 29, null, null },
                    { new Guid("79e07d80-703d-48b6-8327-e7396bc2e410"), null, null, 43, "Chọn đáp án phù hợp với nội dung gạch chân trong đoạn văn.", new Guid("6d63b5d7-8fc3-45be-a9f1-6af0f651b5e3"), 39, null, null },
                    { new Guid("815f931d-ac7d-4052-bfe0-c1eb7f48c37a"), null, null, 4, "Chọn ngữ pháp phù hợp cho câu văn.", new Guid("1127b458-58e3-482c-8c9a-7cb897482375"), 1, null, null },
                    { new Guid("90514cf2-aa9a-4bb9-b759-7258fb4e9eb2"), null, null, 31, "Điền từ/cụm từ vào chỗ trống trong đoạn văn.", new Guid("aca5da63-e3b8-4d4f-a337-982e7585ef95"), 28, null, null },
                    { new Guid("9136138a-c6d3-4411-a6fd-75e2d946905b"), null, null, 28, "Nghe hội thoại trung bình/dài rồi điền thông tin hoặc chọn ý chính.", new Guid("5bda5fed-c4d5-4707-879d-b6abe6f6b787"), 25, null, null },
                    { new Guid("98d23989-7ed5-4680-9ef0-acde9f106a95"), null, null, 15, "Sắp xếp thứ tự câu văn sao cho hợp lý.", new Guid("b163baf3-c72b-4763-b23b-001f7747e36c"), 13, null, null },
                    { new Guid("9c63602d-30b7-433b-8091-44656cfb4bbc"), null, null, 21, "Nghe đoạn hội thoại dài hơn rồi chọn đáp án đúng với mục đích hoặc ý chính.", new Guid("8aa5e695-9c2f-4f2b-a71d-482618517617"), 17, null, null },
                    { new Guid("9d5b8a10-c8f6-40a5-8ad3-3fb17098b0c1"), null, null, 16, "Nghe hội thoại có tình huống rồi chọn hành động hoặc phản ứng thích hợp.", new Guid("8aa5e695-9c2f-4f2b-a71d-482618517617"), 13, null, null },
                    { new Guid("ab9f96b1-dd7a-4361-82a5-98b849cb0a7c"), null, null, 8, "Chọn chủ đề phù hợp dựa vào ảnh.", new Guid("1127b458-58e3-482c-8c9a-7cb897482375"), 5, null, null },
                    { new Guid("b47f4a10-b0e1-4e21-a403-0dcafcc6eec2"), null, null, 40, "Nghe bài giảng/hội thoại dài rồi chọn đáp án phù hợp với ý chính hoặc lập luận.", new Guid("83128ff4-d33d-40d6-87ee-b0013eb2521a"), 38, null, null },
                    { new Guid("b824e232-ea18-4b28-9211-f646fd1896ef"), null, null, 6, "Nghe câu ngắn, chọn đáp án đúng về tên, địa điểm, thời gian, hành động.", new Guid("e8d1e161-73a9-46c0-abc3-1552188a0ead"), 1, null, null },
                    { new Guid("cc07c3d7-3e45-4114-abfe-ba8d21889bda"), null, null, 22, "Điền từ/cụm từ vào chỗ trống trong đoạn văn.", new Guid("b163baf3-c72b-4763-b23b-001f7747e36c"), 16, null, null },
                    { new Guid("d1754294-90ea-42d0-86c2-5d6e07a1b04f"), null, null, 38, "Chọn đáp án phù hợp với nội dung đoạn văn.", new Guid("aca5da63-e3b8-4d4f-a337-982e7585ef95"), 32, null, null },
                    { new Guid("ef5c612c-2d81-4254-9bfb-dc8aadc8435b"), null, null, 8, "Nghe câu/đoạn hội thoại đơn giản rồi chọn thông tin đúng.", new Guid("bcb3d3cd-a148-4562-8b67-ee37c0263bc6"), 4, null, null },
                    { new Guid("f79f4122-dfd6-4c73-b818-2f29dbef43e8"), null, null, 30, "Điền từ/cụm từ vào câu trong đoạn văn.", new Guid("203ee981-d694-40e4-8a30-b7c1a1e6ec09"), 26, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Context_RangeId",
                table: "Context",
                column: "RangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Level_ExamTypeId",
                table: "Level",
                column: "ExamTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ContextId",
                table: "Question",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_Range_SkillLevelId",
                table: "Range",
                column: "SkillLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillLevel_LevelId",
                table: "SkillLevel",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillLevel_SkillId",
                table: "SkillLevel",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Context");

            migrationBuilder.DropTable(
                name: "Range");

            migrationBuilder.DropTable(
                name: "SkillLevel");

            migrationBuilder.DropTable(
                name: "Level");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "ExamType");
        }
    }
}
