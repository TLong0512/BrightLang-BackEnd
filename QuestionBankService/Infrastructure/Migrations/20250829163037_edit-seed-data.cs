using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editseeddata : Migration
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
                    { new Guid("0d3b1667-7f36-4247-a915-2e06eef6532b"), null, null, "TOPIK II (Cấp 3–6) là kỳ thi kiểm tra trình độ tiếng Hàn từ trung cấp đến cao cấp, đánh giá kỹ năng nghe, đọc và viết dành cho người không phải bản ngữ. Kỳ thi đánh giá khả năng giao tiếp từ mức thông thường đến gần như người bản xứ. Được sử dụng cho nhập học đại học, công việc chuyên môn, xin visa, học bổng và thăng tiến nghề nghiệp, đặc biệt ở Việt Nam, chứng chỉ TOPIK II có giá trị trong hai năm và được công nhận toàn cầu.", "TOPIK II", null, null },
                    { new Guid("e53b3aef-3a9f-4453-a54b-165626621689"), null, null, "TOPIK I (Cấp 1 và 2) là kỳ thi kiểm tra trình độ tiếng Hàn cấp độ sơ cấp, đánh giá kỹ năng nghe và đọc cơ bản dành cho người không phải bản ngữ. Kỳ thi kiểm tra khả năng giao tiếp đơn giản với 800–2.000 từ vựng. Được sử dụng cho nhập học đại học, xin visa (ví dụ: visa kết hôn hoặc định cư tại Hàn Quốc), công việc cấp nhập môn và mục tiêu học ngôn ngữ cá nhân, chứng chỉ TOPIK I có giá trị trong hai năm và được công nhận toàn cầu.", "TOPIK I", null, null }
                });

            migrationBuilder.InsertData(
                table: "Skill",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "SkillName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("38c7dca6-f7a5-4f6f-9649-f7da4a1cbb46"), null, null, "Đọc", null, null },
                    { new Guid("89e0201a-2d41-4a84-8468-7f9d33693679"), null, null, "Viết", null, null },
                    { new Guid("d657d124-b149-43e3-a84a-7b05dd037ffc"), null, null, "Nghe", null, null }
                });

            migrationBuilder.InsertData(
                table: "Level",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "ExamTypeId", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("0ae30081-a113-4652-af54-195763ce7045"), null, null, new Guid("e53b3aef-3a9f-4453-a54b-165626621689"), "Cấp 1", null, null },
                    { new Guid("346a2201-2a1f-41c8-a705-bd49ef2f5fa8"), null, null, new Guid("0d3b1667-7f36-4247-a915-2e06eef6532b"), "Cấp 6", null, null },
                    { new Guid("621f0da6-d50a-40b7-9be0-ba44df11b00f"), null, null, new Guid("0d3b1667-7f36-4247-a915-2e06eef6532b"), "Cấp 3", null, null },
                    { new Guid("87b10d91-5170-45f6-b6ed-639db7bacafc"), null, null, new Guid("0d3b1667-7f36-4247-a915-2e06eef6532b"), "Cấp 5", null, null },
                    { new Guid("e519693e-4a45-424a-a023-ed78e8ddd734"), null, null, new Guid("e53b3aef-3a9f-4453-a54b-165626621689"), "Cấp 2", null, null },
                    { new Guid("f0860c43-68d7-4ef2-87ff-b4a5dfb50162"), null, null, new Guid("0d3b1667-7f36-4247-a915-2e06eef6532b"), "Cấp 4", null, null }
                });

            migrationBuilder.InsertData(
                table: "SkillLevel",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "LevelId", "SkillId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("1297d427-1dc2-40ff-bc2c-0a47048aba12"), null, null, new Guid("87b10d91-5170-45f6-b6ed-639db7bacafc"), new Guid("89e0201a-2d41-4a84-8468-7f9d33693679"), null, null },
                    { new Guid("1484e946-1ace-4a2b-bafc-b23e6acb0c83"), null, null, new Guid("621f0da6-d50a-40b7-9be0-ba44df11b00f"), new Guid("38c7dca6-f7a5-4f6f-9649-f7da4a1cbb46"), null, null },
                    { new Guid("2af49ff8-3dcb-4b98-8401-de7eca0cd335"), null, null, new Guid("f0860c43-68d7-4ef2-87ff-b4a5dfb50162"), new Guid("38c7dca6-f7a5-4f6f-9649-f7da4a1cbb46"), null, null },
                    { new Guid("33c0fe80-ceb8-4e96-8c5d-eaf68abba27e"), null, null, new Guid("0ae30081-a113-4652-af54-195763ce7045"), new Guid("38c7dca6-f7a5-4f6f-9649-f7da4a1cbb46"), null, null },
                    { new Guid("494e3aa3-f49f-4202-bf7d-3b9ce975db18"), null, null, new Guid("f0860c43-68d7-4ef2-87ff-b4a5dfb50162"), new Guid("d657d124-b149-43e3-a84a-7b05dd037ffc"), null, null },
                    { new Guid("7deb2d52-b4d8-4f0e-ae98-a044a1dcc3e0"), null, null, new Guid("0ae30081-a113-4652-af54-195763ce7045"), new Guid("d657d124-b149-43e3-a84a-7b05dd037ffc"), null, null },
                    { new Guid("84f41ed6-c4a3-4962-bb01-35f344aff293"), null, null, new Guid("346a2201-2a1f-41c8-a705-bd49ef2f5fa8"), new Guid("89e0201a-2d41-4a84-8468-7f9d33693679"), null, null },
                    { new Guid("88833a7b-5af3-4513-a9fd-1647c67ff40c"), null, null, new Guid("f0860c43-68d7-4ef2-87ff-b4a5dfb50162"), new Guid("89e0201a-2d41-4a84-8468-7f9d33693679"), null, null },
                    { new Guid("a39bb6dc-07f2-44ea-bb10-9b69ae48dfed"), null, null, new Guid("e519693e-4a45-424a-a023-ed78e8ddd734"), new Guid("d657d124-b149-43e3-a84a-7b05dd037ffc"), null, null },
                    { new Guid("a920d435-bcdf-4543-b206-e65c597ad2b6"), null, null, new Guid("87b10d91-5170-45f6-b6ed-639db7bacafc"), new Guid("38c7dca6-f7a5-4f6f-9649-f7da4a1cbb46"), null, null },
                    { new Guid("b84fce53-5cb4-4739-9644-b4470c77c4ed"), null, null, new Guid("e519693e-4a45-424a-a023-ed78e8ddd734"), new Guid("38c7dca6-f7a5-4f6f-9649-f7da4a1cbb46"), null, null },
                    { new Guid("cb1297b6-f55a-4eec-99f4-3426014b3539"), null, null, new Guid("621f0da6-d50a-40b7-9be0-ba44df11b00f"), new Guid("89e0201a-2d41-4a84-8468-7f9d33693679"), null, null },
                    { new Guid("d767ccbc-c491-42fe-9273-12dc4dfc2aa8"), null, null, new Guid("346a2201-2a1f-41c8-a705-bd49ef2f5fa8"), new Guid("38c7dca6-f7a5-4f6f-9649-f7da4a1cbb46"), null, null },
                    { new Guid("d82536ef-234d-46bf-abba-5d2c91d5548d"), null, null, new Guid("621f0da6-d50a-40b7-9be0-ba44df11b00f"), new Guid("d657d124-b149-43e3-a84a-7b05dd037ffc"), null, null },
                    { new Guid("e1638ce4-e1d8-4ed8-96a4-949ea26fbf1f"), null, null, new Guid("87b10d91-5170-45f6-b6ed-639db7bacafc"), new Guid("d657d124-b149-43e3-a84a-7b05dd037ffc"), null, null },
                    { new Guid("f60f8658-fe49-4055-9c7e-1d22aeb66aca"), null, null, new Guid("346a2201-2a1f-41c8-a705-bd49ef2f5fa8"), new Guid("d657d124-b149-43e3-a84a-7b05dd037ffc"), null, null }
                });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "EndQuestionNumber", "Name", "SkillLevelId", "StartQuestionNumber", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("05224c3f-b985-4930-b085-dad20be3fbd2"), null, null, 50, "Điền từ/cụm từ vào chỗ trống hoặc chọn đáp án chính xác với mục đích của đoạn văn.", new Guid("d767ccbc-c491-42fe-9273-12dc4dfc2aa8"), 46, null, null },
                    { new Guid("0946f08f-b6dc-4757-87ec-137d73a359cd"), null, null, 20, "Chọn thông tin bổ sung từ thông báo hoặc đoạn hội thoại.", new Guid("a39bb6dc-07f2-44ea-bb10-9b69ae48dfed"), 16, null, null },
                    { new Guid("0a917d61-8a93-4c89-abb8-994d7519e0e1"), null, null, 10, "Chọn ngữ pháp phù hợp với câu.", new Guid("33c0fe80-ceb8-4e96-8c5d-eaf68abba27e"), 6, null, null },
                    { new Guid("0ee524c5-2e02-4baf-ae1d-fbb0e4e4cd83"), null, null, 28, "Nghe hội thoại trung bình/dài rồi điền thông tin hoặc chọn ý chính.", new Guid("e1638ce4-e1d8-4ed8-96a4-949ea26fbf1f"), 25, null, null },
                    { new Guid("1114d572-88b0-44a0-a90d-d28d0c81fed5"), null, null, 25, "Chọn thái độ người nói hoặc ý nghĩa câu nói.", new Guid("a39bb6dc-07f2-44ea-bb10-9b69ae48dfed"), 21, null, null },
                    { new Guid("13988bbf-a5fa-4f6b-be3f-8ef0413870f4"), null, null, 16, "Nghe hội thoại có tình huống rồi chọn hành động hoặc phản ứng thích hợp.", new Guid("494e3aa3-f49f-4202-bf7d-3b9ce975db18"), 13, null, null },
                    { new Guid("15d5e052-77a8-476d-b1c1-21dc25fdbfce"), null, null, 24, "Nghe hội thoại ngắn rồi chọn nội dung tương ứng với phần gạch chân.", new Guid("494e3aa3-f49f-4202-bf7d-3b9ce975db18"), 22, null, null },
                    { new Guid("24658e93-6c68-4646-9376-faf7dbbb2f5a"), null, null, 12, "Chọn nội dung tương ứng với biểu đồ hoặc thông báo.", new Guid("1484e946-1ace-4a2b-bafc-b23e6acb0c83"), 9, null, null },
                    { new Guid("252e0c20-70bd-4d60-8cf8-5cf5c810abac"), null, null, 31, "Điền từ/cụm từ vào chỗ trống trong đoạn văn.", new Guid("a920d435-bcdf-4543-b206-e65c597ad2b6"), 28, null, null },
                    { new Guid("2ee42488-2a13-4c3e-a0dd-df86ea4e7a3b"), null, null, 15, "Sắp xếp thứ tự câu văn sao cho hợp lý.", new Guid("2af49ff8-3dcb-4b98-8401-de7eca0cd335"), 13, null, null },
                    { new Guid("50258a43-e1a2-49b1-855d-d89803f05711"), null, null, 3, "Nghe hội thoại ngắn và chọn tranh/đáp án phù hợp.", new Guid("d82536ef-234d-46bf-abba-5d2c91d5548d"), 1, null, null },
                    { new Guid("5fcdbf81-4584-4e74-8d40-7307b6fda633"), null, null, 33, "Nghe bài giảng/thuyết trình ngắn rồi chọn đáp án phù hợp với nội dung hoặc thái độ người nói.", new Guid("e1638ce4-e1d8-4ed8-96a4-949ea26fbf1f"), 29, null, null },
                    { new Guid("639a0abc-6094-41a3-87ab-96eac5cb62fa"), null, null, 43, "Chọn đáp án phù hợp với nội dung gạch chân trong đoạn văn.", new Guid("d767ccbc-c491-42fe-9273-12dc4dfc2aa8"), 39, null, null },
                    { new Guid("699a7397-7baf-447f-a007-abbec207a33f"), null, null, 15, "Trả lời câu hỏi về thông báo, biển báo, hướng dẫn đơn giản.", new Guid("33c0fe80-ceb8-4e96-8c5d-eaf68abba27e"), 11, null, null },
                    { new Guid("6b30c4fc-000b-4cd6-85a1-213b4ac235ff"), null, null, 21, "Nghe đoạn hội thoại dài hơn rồi chọn đáp án đúng với mục đích hoặc ý chính.", new Guid("494e3aa3-f49f-4202-bf7d-3b9ce975db18"), 17, null, null },
                    { new Guid("6b319e61-6c3d-41a2-a7e2-a8bea8bcbb6a"), null, null, 10, "Chọn ý chính hoặc thông tin chi tiết trong hội thoại.", new Guid("7deb2d52-b4d8-4f0e-ae98-a044a1dcc3e0"), 7, null, null },
                    { new Guid("6c5bf929-56c8-491b-81bd-182c921af5ed"), null, null, 24, "Chọn đáp án phù hợp với phần gạch chân trong đoạn văn.", new Guid("2af49ff8-3dcb-4b98-8401-de7eca0cd335"), 22, null, null },
                    { new Guid("799d184c-da86-44d9-8089-6b9d7bb0478e"), null, null, 25, "Chọn thông tin chi tiết trong đoạn văn ngắn.", new Guid("b84fce53-5cb4-4739-9644-b4470c77c4ed"), 21, null, null },
                    { new Guid("8d9d23a7-98e2-4b3f-a8cf-ae4a6cb45925"), null, null, 43, "Nghe thảo luận/phát biểu rồi chọn đáp án phù hợp với nội dung gạch chân.", new Guid("f60f8658-fe49-4055-9c7e-1d22aeb66aca"), 41, null, null },
                    { new Guid("903931a9-e6dd-407e-be43-694441074ad1"), null, null, 40, "Chọn ý chính hoặc thông tin chi tiết trong đoạn văn.", new Guid("b84fce53-5cb4-4739-9644-b4470c77c4ed"), 36, null, null },
                    { new Guid("9ae24b34-77f3-4910-a1c0-7aea229c9e42"), null, null, 8, "Chọn chủ đề phù hợp dựa vào ảnh.", new Guid("1484e946-1ace-4a2b-bafc-b23e6acb0c83"), 5, null, null },
                    { new Guid("a7f085c1-128d-456c-850d-589854124aff"), null, null, 15, "Chọn thông tin chi tiết hoặc ý chính trong đoạn hội thoại dài hơn.", new Guid("a39bb6dc-07f2-44ea-bb10-9b69ae48dfed"), 11, null, null },
                    { new Guid("ac186a0f-91ea-4474-a209-6ebd4177755a"), null, null, 30, "Chọn mục đích người nói hoặc thông tin chi tiết nâng cao.", new Guid("a39bb6dc-07f2-44ea-bb10-9b69ae48dfed"), 26, null, null },
                    { new Guid("b4eaf804-0552-4de7-b296-fa080f95da03"), null, null, 37, "Nghe cuộc thảo luận/ý kiến trái chiều rồi chọn nội dung chính hoặc quan điểm nhân vật.", new Guid("e1638ce4-e1d8-4ed8-96a4-949ea26fbf1f"), 34, null, null },
                    { new Guid("c3a4c36d-cb82-43ac-8c47-ea098508bbf7"), null, null, 40, "Nghe bài giảng/hội thoại dài rồi chọn đáp án phù hợp với ý chính hoặc lập luận.", new Guid("f60f8658-fe49-4055-9c7e-1d22aeb66aca"), 38, null, null },
                    { new Guid("c6dcb040-a6de-44a7-b167-a1f01256e9b2"), null, null, 6, "Nghe câu ngắn, chọn đáp án đúng về tên, địa điểm, thời gian, hành động.", new Guid("7deb2d52-b4d8-4f0e-ae98-a044a1dcc3e0"), 1, null, null },
                    { new Guid("c8c1cb3f-73e8-4504-8944-239cb081a3a4"), null, null, 45, "Chọn đáp án phù hợp với chủ đề của đoạn văn.", new Guid("d767ccbc-c491-42fe-9273-12dc4dfc2aa8"), 44, null, null },
                    { new Guid("ca70d90e-2812-46dd-ba70-e71fa3f4f6b8"), null, null, 35, "Chọn ngữ pháp phù hợp trong đoạn văn.", new Guid("b84fce53-5cb4-4739-9644-b4470c77c4ed"), 31, null, null },
                    { new Guid("cc488c41-b2f8-4426-a6f8-6b66010749aa"), null, null, 30, "Điền từ/cụm từ vào câu trong đoạn văn.", new Guid("b84fce53-5cb4-4739-9644-b4470c77c4ed"), 26, null, null },
                    { new Guid("cd6a1252-60a5-440a-aa41-b10cf70ecf26"), null, null, 38, "Chọn đáp án phù hợp với nội dung đoạn văn.", new Guid("a920d435-bcdf-4543-b206-e65c597ad2b6"), 32, null, null },
                    { new Guid("d7fed0d1-1fcf-4454-9478-f2f1832f9558"), null, null, 8, "Nghe câu/đoạn hội thoại đơn giản rồi chọn thông tin đúng.", new Guid("d82536ef-234d-46bf-abba-5d2c91d5548d"), 4, null, null },
                    { new Guid("d8629bba-e900-40a5-949e-8c9bf2d42797"), null, null, 5, "Điền từ/cụm từ vào chỗ trống trong câu đơn giản.", new Guid("33c0fe80-ceb8-4e96-8c5d-eaf68abba27e"), 1, null, null },
                    { new Guid("db1a4fe2-d703-4d52-a23b-b9bb0aec1c1a"), null, null, 22, "Điền từ/cụm từ vào chỗ trống trong đoạn văn.", new Guid("2af49ff8-3dcb-4b98-8401-de7eca0cd335"), 16, null, null },
                    { new Guid("de568e8e-6760-460a-b81e-04f3398d1d13"), null, null, 4, "Chọn ngữ pháp phù hợp cho câu văn.", new Guid("1484e946-1ace-4a2b-bafc-b23e6acb0c83"), 1, null, null },
                    { new Guid("df7fc211-e1a2-4f59-bf22-353c9873f69d"), null, null, 20, "Điền từ/cụm từ trong đoạn văn ngắn.", new Guid("b84fce53-5cb4-4739-9644-b4470c77c4ed"), 16, null, null },
                    { new Guid("eac6e764-2b02-4766-afde-6c012f5b81eb"), null, null, 12, "Nghe thông báo/hội thoại ngắn rồi chọn đáp án phù hợp với nội dung chính.", new Guid("d82536ef-234d-46bf-abba-5d2c91d5548d"), 9, null, null },
                    { new Guid("f661d290-c244-4a28-851e-5d6f9c7c5b0b"), null, null, 50, "Nghe đoạn văn bản dài (tin tức, bài giảng, phỏng vấn) rồi chọn đáp án đúng với chủ đề, mục đích, thái độ, hoặc chi tiết quan trọng..", new Guid("f60f8658-fe49-4055-9c7e-1d22aeb66aca"), 44, null, null }
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
