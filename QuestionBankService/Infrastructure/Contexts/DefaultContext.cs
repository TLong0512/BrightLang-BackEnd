using Domain.Entities;
using Infrastructure.Cofigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Range = Domain.Entities.Range;

namespace Infrastructure.Contexts
{
    public class DefaultContext : DbContext
    {
        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<Domain.Entities.Range> Ranges { get; set; }
        public DbSet<Context> Contexts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public DefaultContext(DbContextOptions<DefaultContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ExamTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LevelConfiguration());
            modelBuilder.ApplyConfiguration(new SkillConfiguration());
            modelBuilder.ApplyConfiguration(new SkillLevelConfiguration());
            modelBuilder.ApplyConfiguration(new RangeConfiguration());
            modelBuilder.ApplyConfiguration(new ContextConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new AnswerConfiguration());

            List<Guid> guids = new List<Guid>();
            for (int i = 0; i < 64; i++)
            {
                guids.Add(Guid.NewGuid());
            }

            //seed data for examtype
            modelBuilder.Entity<ExamType>().HasData(
                new ExamType
                {
                    Id = guids[0],
                    Name = "TOPIK I",
                    Description = "TOPIK I (Cấp 1 và 2) là kỳ thi kiểm tra trình độ tiếng Hàn " +
                    "cấp độ sơ cấp, đánh giá kỹ năng nghe và đọc cơ bản dành cho người không phải " +
                    "bản ngữ. Kỳ thi kiểm tra khả năng giao tiếp đơn giản với 800–2.000 từ vựng. " +
                    "Được sử dụng cho nhập học đại học, xin visa (ví dụ: visa kết hôn hoặc định cư tại Hàn Quốc)," +
                    " công việc cấp nhập môn và mục tiêu học ngôn ngữ cá nhân, chứng chỉ TOPIK I có giá trị trong" +
                    " hai năm và được công nhận toàn cầu."
                },
                new ExamType
                {
                    Id = guids[1],
                    Name = "TOPIK II",
                    Description = "TOPIK II (Cấp 3–6) là kỳ thi kiểm tra " +
                    "trình độ tiếng Hàn từ trung cấp đến cao cấp, đánh giá kỹ " +
                    "năng nghe, đọc và viết dành cho người không phải bản ngữ." +
                    " Kỳ thi đánh giá khả năng giao tiếp từ mức thông thường đến " +
                    "gần như người bản xứ. Được sử dụng cho nhập học đại học, " +
                    "công việc chuyên môn, xin visa, học bổng và thăng tiến nghề nghiệp, " +
                    "đặc biệt ở Việt Nam, chứng chỉ TOPIK II có giá trị trong hai năm và được công nhận toàn cầu."
                }
                );

            //seed data for level
            modelBuilder.Entity<Level>().HasData(
                 new Level
                 {
                     Id = guids[2],
                     Name = "Cấp 1",
                     ExamTypeId = guids[0]
                 },
                 new Level
                 {
                     Id = guids[3],
                     Name = "Cấp 2",
                     ExamTypeId = guids[0]
                 },
                 new Level
                 {
                     Id = guids[4],
                     Name = "Cấp 3",
                     ExamTypeId = guids[1]
                 },
                 new Level
                 {
                     Id = guids[5],
                     Name = "Cấp 4",
                     ExamTypeId = guids[1]
                 },
                 new Level
                 {
                     Id = guids[6],
                     Name = "Cấp 5",
                     ExamTypeId = guids[1]
                 },
                 new Level
                 {
                     Id = guids[7],
                     Name = "Cấp 6",
                     ExamTypeId = guids[1]
                 }
                );

            //seed data for skill
            modelBuilder.Entity<Skill>().HasData(
                   new Skill
                   {
                       Id = guids[8],
                       SkillName = "Nghe"
                   },
                   new Skill
                   {
                       Id = guids[9],
                       SkillName = "Đọc"
                   },
                   new Skill
                   {
                       Id = guids[10],
                       SkillName = "Viết"
                   }
                );

            //seed data for level - skill
            modelBuilder.Entity<SkillLevel>().HasData(
                new SkillLevel
                {
                    //listening - topik I - level 1
                    Id = guids[11],
                    SkillId = guids[8],
                    LevelId = guids[2]
                },
                new SkillLevel
                {
                    //listening - topik I - level 2 
                    Id = guids[12],
                    SkillId = guids[8],
                    LevelId = guids[3]
                },
                new SkillLevel
                {
                    //listening - topik II - level 3
                    Id = guids[13],
                    SkillId = guids[8],
                    LevelId = guids[4]
                },
                new SkillLevel
                {
                    //listening - topik II - level 4
                    Id = guids[14],
                    SkillId = guids[8],
                    LevelId = guids[5]
                },
                new SkillLevel
                {
                    //listening - topik II - level 5
                    Id = guids[15],
                    SkillId = guids[8],
                    LevelId = guids[6]
                },
                new SkillLevel
                {
                    //listening - topik II - level 6
                    Id = guids[16],
                    SkillId = guids[8],
                    LevelId = guids[7]
                },
                new SkillLevel
                {
                    //reading - topik I - level 1 
                    Id = guids[17],
                    SkillId = guids[9],
                    LevelId = guids[2]
                },
                new SkillLevel
                {
                    //reading - topik I - level 2
                    Id = guids[18],
                    SkillId = guids[9],
                    LevelId = guids[3]
                },
                new SkillLevel
                {
                    //reading - topik II - level 3
                    Id = guids[19],
                    SkillId = guids[9],
                    LevelId = guids[4]
                },
                new SkillLevel
                {
                    //reading - topik II - level 4
                    Id = guids[20],
                    SkillId = guids[9],
                    LevelId = guids[5]
                },
                new SkillLevel
                {
                    //reading - topik II - level 5
                    Id = guids[21],
                    SkillId = guids[9],
                    LevelId = guids[6]
                },
                new SkillLevel
                {
                    //reading - topik II - level 6
                    Id = guids[22],
                    SkillId = guids[9],
                    LevelId = guids[7]
                },
                new SkillLevel
                {
                    // writting - topik II - level 3
                    Id = guids[23],
                    SkillId = guids[10],
                    LevelId = guids[4]
                },
                new SkillLevel
                {
                    // writting - topik II - level 4
                    Id = guids[24],
                    SkillId = guids[10],
                    LevelId = guids[5]
                },
                new SkillLevel
                {
                    // writting - topik II - level 5
                    Id = guids[25],
                    SkillId = guids[10],
                    LevelId = guids[6]
                },
                new SkillLevel
                {
                    // writting - topik II - level 6
                    Id = guids[26],
                    SkillId = guids[10],
                    LevelId = guids[7]
                }
                );

            //seed data for range
            //level 1 
            //listening ranges
            modelBuilder.Entity<Range>().HasData(
                new Range
                {
                    Id = guids[27],
                    SkillLevelId = guids[11],
                    StartQuestionNumber = 1,
                    EndQuestionNumber = 6,
                    Name = "Nghe câu ngắn, chọn đáp án đúng về tên, địa điểm, thời gian, hành động."
                },
                new Range
                {
                    Id = guids[28],
                    SkillLevelId = guids[11],
                    StartQuestionNumber = 7,
                    EndQuestionNumber = 10,
                    Name = "Chọn ý chính hoặc thông tin chi tiết trong hội thoại."
                }
                );
            //reading ranges
            modelBuilder.Entity<Range>().HasData(
                new Range
                {
                    Id = guids[29],
                    SkillLevelId = guids[17],
                    StartQuestionNumber = 1,
                    EndQuestionNumber = 5,
                    Name = "Điền từ/cụm từ vào chỗ trống trong câu đơn giản."
                },
                new Range
                {
                    Id = guids[30],
                    SkillLevelId = guids[17],
                    StartQuestionNumber = 6,
                    EndQuestionNumber = 10,
                    Name = "Chọn ngữ pháp phù hợp với câu."
                },
                new Range
                {
                    Id = guids[31],
                    SkillLevelId = guids[17],
                    StartQuestionNumber = 11,
                    EndQuestionNumber = 15,
                    Name = "Trả lời câu hỏi về thông báo, biển báo, hướng dẫn đơn giản."
                }
                );
            //level 2
            //listening ranges
            modelBuilder.Entity<Range>().HasData(
               new Range
               {
                   Id = guids[32],
                   SkillLevelId = guids[12],
                   StartQuestionNumber = 11,
                   EndQuestionNumber = 15,
                   Name = "Chọn thông tin chi tiết hoặc ý chính trong đoạn hội thoại dài hơn."
               },
               new Range
               {
                   Id = guids[33],
                   SkillLevelId = guids[12],
                   StartQuestionNumber = 16,
                   EndQuestionNumber = 20,
                   Name = "Chọn thông tin bổ sung từ thông báo hoặc đoạn hội thoại."
               },
               new Range
               {
                   Id = guids[34],
                   SkillLevelId = guids[12],
                   StartQuestionNumber = 21,
                   EndQuestionNumber = 25,
                   Name = "Chọn thái độ người nói hoặc ý nghĩa câu nói."
               },
               new Range
               {
                   Id = guids[35],
                   SkillLevelId = guids[12],
                   StartQuestionNumber = 26,
                   EndQuestionNumber = 30,
                   Name = "Chọn mục đích người nói hoặc thông tin chi tiết nâng cao."
               }
               );
            //reading ranges
            modelBuilder.Entity<Range>().HasData(
              new Range
              {
                  Id = guids[36],
                  SkillLevelId = guids[18],
                  StartQuestionNumber = 16,
                  EndQuestionNumber = 20,
                  Name = "Điền từ/cụm từ trong đoạn văn ngắn."
              },
              new Range
              {
                  Id = guids[37],
                  SkillLevelId = guids[12],
                  StartQuestionNumber = 21,
                  EndQuestionNumber = 25,
                  Name = "Chọn thông tin chi tiết trong đoạn văn ngắn."
              },
              new Range
              {
                  Id = guids[38],
                  SkillLevelId = guids[12],
                  StartQuestionNumber = 26,
                  EndQuestionNumber = 30,
                  Name = "Điền từ/cụm từ vào câu trong đoạn văn."
              },
              new Range
              {
                  Id = guids[39],
                  SkillLevelId = guids[12],
                  StartQuestionNumber = 31,
                  EndQuestionNumber = 35,
                  Name = "Chọn ngữ pháp phù hợp trong đoạn văn."
              },
              new Range
              {
                  Id = guids[40],
                  SkillLevelId = guids[12],
                  StartQuestionNumber = 36,
                  EndQuestionNumber = 40,
                  Name = "Chọn ý chính hoặc thông tin chi tiết trong đoạn văn."
              }
              );
            //level 3
            //listening ranges
            modelBuilder.Entity<Range>().HasData(
              new Range
              {
                  Id = guids[41],
                  SkillLevelId = guids[13],
                  StartQuestionNumber = 1,
                  EndQuestionNumber = 3,
                  Name = "Nghe hội thoại ngắn và chọn tranh/đáp án phù hợp."
              },
              new Range
              {
                  Id = guids[42],
                  SkillLevelId = guids[13],
                  StartQuestionNumber = 4,
                  EndQuestionNumber = 8,
                  Name = "Nghe câu/đoạn hội thoại đơn giản rồi chọn thông tin đúng."
              },
             new Range
             {
                 Id = guids[43],
                 SkillLevelId = guids[13],
                 StartQuestionNumber = 9,
                 EndQuestionNumber = 12,
                 Name = "Nghe thông báo/hội thoại ngắn rồi chọn đáp án phù hợp với nội dung chính."
             }
              );
            //reading ranges
            modelBuilder.Entity<Range>().HasData(
              new Range
              {
                  Id = guids[44],
                  SkillLevelId = guids[19],
                  StartQuestionNumber = 1,
                  EndQuestionNumber = 4,
                  Name = "Chọn ngữ pháp phù hợp cho câu văn."
              },
              new Range
              {
                  Id = guids[45],
                  SkillLevelId = guids[19],
                  StartQuestionNumber = 5,
                  EndQuestionNumber = 8,
                  Name = "Chọn chủ đề phù hợp dựa vào ảnh."
              },
             new Range
             {
                 Id = guids[46],
                 SkillLevelId = guids[19],
                 StartQuestionNumber = 9,
                 EndQuestionNumber = 12,
                 Name = "Chọn nội dung tương ứng với biểu đồ hoặc thông báo."
             }
              );
            //level 4
            //listening ranges
            modelBuilder.Entity<Range>().HasData(
              new Range
              {
                  Id = guids[47],
                  SkillLevelId = guids[14],
                  StartQuestionNumber = 13,
                  EndQuestionNumber = 16,
                  Name = "Nghe hội thoại có tình huống rồi chọn hành động hoặc phản ứng thích hợp."
              },
              new Range
              {
                  Id = guids[48],
                  SkillLevelId = guids[14],
                  StartQuestionNumber = 17,
                  EndQuestionNumber = 21,
                  Name = "Nghe đoạn hội thoại dài hơn rồi chọn đáp án đúng với mục đích hoặc ý chính."
              },
             new Range
             {
                 Id = guids[49],
                 SkillLevelId = guids[14],
                 StartQuestionNumber = 22,
                 EndQuestionNumber = 24,
                 Name = "Nghe hội thoại ngắn rồi chọn nội dung tương ứng với phần gạch chân."
             }
              );
            //reading ranges
            modelBuilder.Entity<Range>().HasData(
              new Range
              {
                  Id = guids[50],
                  SkillLevelId = guids[20],
                  StartQuestionNumber = 13,
                  EndQuestionNumber = 15,
                  Name = "Sắp xếp thứ tự câu văn sao cho hợp lý."
              },
              new Range
              {
                  Id = guids[51],
                  SkillLevelId = guids[20],
                  StartQuestionNumber = 16,
                  EndQuestionNumber = 22,
                  Name = "Điền từ/cụm từ vào chỗ trống trong đoạn văn."
              },
             new Range
             {
                 Id = guids[52],
                 SkillLevelId = guids[20],
                 StartQuestionNumber = 22,
                 EndQuestionNumber = 24,
                 Name = "Chọn đáp án phù hợp với phần gạch chân trong đoạn văn."
             }
              );
            //level 5
            //listening ranges
            modelBuilder.Entity<Range>().HasData(
             new Range
             {
                 Id = guids[53],
                 SkillLevelId = guids[15],
                 StartQuestionNumber = 25,
                 EndQuestionNumber = 28,
                 Name = "Nghe hội thoại trung bình/dài rồi điền thông tin hoặc chọn ý chính."
             },
             new Range
             {
                 Id = guids[54],
                 SkillLevelId = guids[15],
                 StartQuestionNumber = 29,
                 EndQuestionNumber = 33,
                 Name = "Nghe bài giảng/thuyết trình ngắn rồi chọn đáp án phù hợp với nội dung hoặc thái độ người nói."
             },
            new Range
            {
                Id = guids[55],
                SkillLevelId = guids[15],
                StartQuestionNumber = 34,
                EndQuestionNumber = 37,
                Name = "Nghe cuộc thảo luận/ý kiến trái chiều rồi chọn nội dung chính hoặc quan điểm nhân vật."
            }
             );
            //reading ranges
            modelBuilder.Entity<Range>().HasData(
            new Range
            {
                Id = guids[56],
                SkillLevelId = guids[21],
                StartQuestionNumber = 28,
                EndQuestionNumber = 31,
                Name = "Điền từ/cụm từ vào chỗ trống trong đoạn văn."
            },
            new Range
            {
                Id = guids[57],
                SkillLevelId = guids[21],
                StartQuestionNumber = 32,
                EndQuestionNumber = 38,
                Name = "Chọn đáp án phù hợp với nội dung đoạn văn."
            }
            );
            //level 6
            //listening ranges
            modelBuilder.Entity<Range>().HasData(
            new Range
            {
                Id = guids[58],
                SkillLevelId = guids[16],
                StartQuestionNumber = 38,
                EndQuestionNumber = 40,
                Name = "Nghe bài giảng/hội thoại dài rồi chọn đáp án phù hợp với ý chính hoặc lập luận."
            },
            new Range
            {
                Id = guids[59],
                SkillLevelId = guids[16],
                StartQuestionNumber = 41,
                EndQuestionNumber = 43,
                Name = "Nghe thảo luận/phát biểu rồi chọn đáp án phù hợp với nội dung gạch chân."
            },
            new Range
            {
                Id = guids[60],
                SkillLevelId = guids[16],
                StartQuestionNumber = 44,
                EndQuestionNumber = 50,
                Name = "Nghe đoạn văn bản dài (tin tức, bài giảng, phỏng vấn)" +
                " rồi chọn đáp án đúng với chủ đề, mục đích, thái độ, hoặc chi tiết quan trọng.."
            }
            );
            //reading ranges
            modelBuilder.Entity<Range>().HasData(
            new Range
            {
                Id = guids[61],
                SkillLevelId = guids[22],
                StartQuestionNumber = 39,
                EndQuestionNumber = 43,
                Name = "Chọn đáp án phù hợp với nội dung gạch chân trong đoạn văn."
            },
            new Range
            {
                Id = guids[62],
                SkillLevelId = guids[22],
                StartQuestionNumber = 44,
                EndQuestionNumber = 45,
                Name = "Chọn đáp án phù hợp với chủ đề của đoạn văn."
            },
            new Range
            {
                Id = guids[63],
                SkillLevelId = guids[22],
                StartQuestionNumber = 46,
                EndQuestionNumber = 50,
                Name = "Điền từ/cụm từ vào chỗ trống hoặc chọn đáp án chính xác với mục đích của đoạn văn."
            }
            );
        }

    }
}
