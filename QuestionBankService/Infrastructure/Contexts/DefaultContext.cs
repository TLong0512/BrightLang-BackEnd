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
        }

    }
}
