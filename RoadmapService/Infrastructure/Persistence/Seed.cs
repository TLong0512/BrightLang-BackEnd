using Application.Abstraction.Services;
using Application.Dtos.QuestionBankService;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class Seed
{
    public static async Task ApplyAsync(
        AppDbContext context,
        IServiceProvider serviceScopeProvider,
        CancellationToken cancellationToken = default)
    {
        // Check if database exists and seeded
        if (context.Database.CanConnect()) return;


        // Database does not exist → create and migrate
        context.Database.Migrate();

        // NOTE: This seed depends heavily on question bank service's seed data.
        // on question bank's service seed update, this seed must be updated accordingly.
        var questionbankService = serviceScopeProvider.GetRequiredService<IQuestionbankService>();

        // get Level table to prepare for seeding roadmap elements. snapshot:
        // [
        // {"id":"(random-guid)", "name":"Cấp 1", "examTypeName":"TOPIK I"},
        // {"id":"(random-guid)", "name":"Cấp 2", "examTypeName":"TOPIK I"},
        // {"id":"(random-guid)", "name":"Cấp 3", "examTypeName":"TOPIK II"},
        // {"id":"(random-guid)", "name":"Cấp 4", "examTypeName":"TOPIK II"},
        // {"id":"(random-guid)", "name":"Cấp 5", "examTypeName":"TOPIK II"},
        // {"id":"(random-guid)", "name":"Cấp 6", "examTypeName":"TOPIK II"}
        // ]
        List<LevelViewDto> levels = await questionbankService.GetAllLevelsAsync();
        var level1 = levels.Single(e => e.Name == "Cấp 1");
        var level2 = levels.Single(e => e.Name == "Cấp 2");
        var level3 = levels.Single(e => e.Name == "Cấp 3");
        var level4 = levels.Single(e => e.Name == "Cấp 4");
        var level5 = levels.Single(e => e.Name == "Cấp 5");
        var level6 = levels.Single(e => e.Name == "Cấp 6");

        // seed roadmap.
        // WARNING: FIRST ROADMAP ID IS NOT VALID.
        var roadmap1short = new Roadmap { LevelStartId = Guid.Empty, LevelEndId = level1.Id, Name = "Lộ trình TOPIK 0 -> 1" };
        var roadmap1normal = new Roadmap { LevelStartId = Guid.Empty, LevelEndId = level1.Id, Name = "Lộ trình TOPIK 0 -> 1" };
        var roadmap1long = new Roadmap { LevelStartId = Guid.Empty, LevelEndId = level1.Id, Name = "Lộ trình TOPIK 0 -> 1" };

        var roadmap2short = new Roadmap { LevelStartId = level1.Id, LevelEndId = level2.Id, Name = "Lộ trình TOPIK 1 -> 2" };
        var roadmap2normal = new Roadmap { LevelStartId = level1.Id, LevelEndId = level2.Id, Name = "Lộ trình TOPIK 1 -> 2" };
        var roadmap2long = new Roadmap { LevelStartId = level1.Id, LevelEndId = level2.Id, Name = "Lộ trình TOPIK 1 -> 2" };

        var roadmap3short = new Roadmap { LevelStartId = level2.Id, LevelEndId = level3.Id, Name = "Lộ trình TOPIK 2 -> 3" };
        var roadmap3normal = new Roadmap { LevelStartId = level2.Id, LevelEndId = level3.Id, Name = "Lộ trình TOPIK 2 -> 3" };
        var roadmap3long = new Roadmap { LevelStartId = level2.Id, LevelEndId = level3.Id, Name = "Lộ trình TOPIK 2 -> 3" };

        var roadmap4short = new Roadmap { LevelStartId = level3.Id, LevelEndId = level4.Id, Name = "Lộ trình TOPIK 3 -> 4" };
        var roadmap4normal = new Roadmap { LevelStartId = level3.Id, LevelEndId = level4.Id, Name = "Lộ trình TOPIK 3 -> 4" };
        var roadmap4long = new Roadmap { LevelStartId = level3.Id, LevelEndId = level4.Id, Name = "Lộ trình TOPIK 3 -> 4" };

        var roadmap5short = new Roadmap { LevelStartId = level4.Id, LevelEndId = level5.Id, Name = "Lộ trình TOPIK 4 -> 5" };
        var roadmap5normal = new Roadmap { LevelStartId = level4.Id, LevelEndId = level5.Id, Name = "Lộ trình TOPIK 4 -> 5" };
        var roadmap5long = new Roadmap { LevelStartId = level4.Id, LevelEndId = level5.Id, Name = "Lộ trình TOPIK 4 -> 5" };

        var roadmap6short = new Roadmap { LevelStartId = level5.Id, LevelEndId = level6.Id, Name = "Lộ trình TOPIK 5 -> 6" };
        var roadmap6normal = new Roadmap { LevelStartId = level5.Id, LevelEndId = level6.Id, Name = "Lộ trình TOPIK 5 -> 6" };
        var roadmap6long = new Roadmap { LevelStartId = level5.Id, LevelEndId = level6.Id, Name = "Lộ trình TOPIK 5 -> 6" };

        await context.Roadmaps.AddRangeAsync(
            roadmap1short, roadmap1normal, roadmap1long,
            roadmap2short, roadmap2normal, roadmap2long,
            roadmap3short, roadmap3normal, roadmap3long,
            roadmap4short, roadmap4normal, roadmap4long,
            roadmap5short, roadmap5normal, roadmap5long,
            roadmap6short, roadmap6normal, roadmap6long
        );
        await context.SaveChangesAsync(cancellationToken);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // get Skill table to prepare for seeding roadmap elements. snapshot:
        // [
        // {"id": "(random-guid)","skillName": "Nghe","levelName": "Cấp 1"},
        // {"id": "(random-guid)","skillName": "Đọc","levelName": "Cấp 1"},

        // {"id": "(random-guid)","skillName": "Nghe","levelName": "Cấp 2"},
        // {"id": "(random-guid)","skillName": "Đọc","levelName": "Cấp 2"},

        // {"id": "(random-guid)","skillName": "Nghe","levelName": "Cấp 3"},
        // {"id": "(random-guid)","skillName": "Đọc","levelName": "Cấp 3"},
        // {"id": "(random-guid)","skillName": "Viết","levelName": "Cấp 3"},

        // {"id": "(random-guid)","skillName": "Nghe","levelName": "Cấp 4"},
        // {"id": "(random-guid)","skillName": "Đọc","levelName": "Cấp 4"},
        // {"id": "(random-guid)","skillName": "Viết","levelName": "Cấp 4"},

        // {"id": "(random-guid)","skillName": "Nghe","levelName": "Cấp 5"},
        // {"id": "(random-guid)","skillName": "Đọc","levelName": "Cấp 5"},
        // {"id": "(random-guid)","skillName": "Viết","levelName": "Cấp 5"},

        // {"id": "(random-guid)","skillName": "Nghe","levelName": "Cấp 6"}
        // {"id": "(random-guid)","skillName": "Đọc","levelName": "Cấp 6"},
        // {"id": "(random-guid)","skillName": "Viết","levelName": "Cấp 6"},
        // ]
        List<SkillLevelViewDto> skillLevels = await questionbankService.GetAllSkillLevelsAsync();

        var nghe1 = skillLevels.Single(e => e.LevelName == "Cấp 1" && e.SkillName == "Nghe");
        var doc1 = skillLevels.Single(e => e.LevelName == "Cấp 1" && e.SkillName == "Đọc");

        var nghe2 = skillLevels.Single(e => e.LevelName == "Cấp 2" && e.SkillName == "Nghe");
        var doc2 = skillLevels.Single(e => e.LevelName == "Cấp 2" && e.SkillName == "Đọc");

        var nghe3 = skillLevels.Single(e => e.LevelName == "Cấp 3" && e.SkillName == "Nghe");
        var doc3 = skillLevels.Single(e => e.LevelName == "Cấp 3" && e.SkillName == "Đọc");
        var viet3 = skillLevels.Single(e => e.LevelName == "Cấp 3" && e.SkillName == "Viết");

        var nghe4 = skillLevels.Single(e => e.LevelName == "Cấp 4" && e.SkillName == "Nghe");
        var doc4 = skillLevels.Single(e => e.LevelName == "Cấp 4" && e.SkillName == "Đọc");
        var viet4 = skillLevels.Single(e => e.LevelName == "Cấp 4" && e.SkillName == "Viết");

        var nghe5 = skillLevels.Single(e => e.LevelName == "Cấp 5" && e.SkillName == "Nghe");
        var doc5 = skillLevels.Single(e => e.LevelName == "Cấp 5" && e.SkillName == "Đọc");
        var viet5 = skillLevels.Single(e => e.LevelName == "Cấp 5" && e.SkillName == "Viết");

        var nghe6 = skillLevels.Single(e => e.LevelName == "Cấp 6" && e.SkillName == "Nghe");
        var doc6 = skillLevels.Single(e => e.LevelName == "Cấp 6" && e.SkillName == "Đọc");
        var viet6 = skillLevels.Single(e => e.LevelName == "Cấp 6" && e.SkillName == "Viết");


        // seed roadmap elements.
        // each range count can be check with the snapshot at the bottom of this file.

        // TODO: update the roadmap element to more diversity:
        // question per day should reflect real world use case.
        // add more roadmap element for practice, re-study, ...
        var nghe1ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe1.Id); // 2 range
        var doc1ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc1.Id); // 3 range

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 1, RangeId = doc1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 2, RangeId = doc1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 3, RangeId = doc1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 4, RangeId = doc1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 5, RangeId = doc1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 6, RangeId = doc1ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 7, RangeId = nghe1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 8, RangeId = nghe1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 9, RangeId = nghe1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 10, RangeId = nghe1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 11, RangeId = nghe1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 12, RangeId = nghe1ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 13, RangeId = doc1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 14, RangeId = doc1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 15, RangeId = doc1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 16, RangeId = doc1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 17, RangeId = doc1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 18, RangeId = doc1ranges![1].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 19, RangeId = nghe1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 20, RangeId = nghe1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 21, RangeId = nghe1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 22, RangeId = nghe1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 23, RangeId = nghe1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 24, RangeId = nghe1ranges![1].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 25, RangeId = doc1ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 26, RangeId = doc1ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 27, RangeId = doc1ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 28, RangeId = doc1ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 29, RangeId = doc1ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1short.Id, Order = 30, RangeId = doc1ranges![2].Id, QuestionPerDay = 10 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 1, RangeId = doc1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 2, RangeId = doc1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 3, RangeId = doc1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 4, RangeId = doc1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 5, RangeId = doc1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 6, RangeId = doc1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 7, RangeId = doc1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 8, RangeId = doc1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 9, RangeId = doc1ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 10, RangeId = nghe1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 11, RangeId = nghe1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 12, RangeId = nghe1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 13, RangeId = nghe1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 14, RangeId = nghe1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 15, RangeId = nghe1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 16, RangeId = nghe1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 17, RangeId = nghe1ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 18, RangeId = nghe1ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 19, RangeId = doc1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 20, RangeId = doc1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 21, RangeId = doc1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 22, RangeId = doc1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 23, RangeId = doc1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 24, RangeId = doc1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 25, RangeId = doc1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 26, RangeId = doc1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 27, RangeId = doc1ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 28, RangeId = nghe1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 29, RangeId = nghe1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 30, RangeId = nghe1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 31, RangeId = nghe1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 32, RangeId = nghe1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 33, RangeId = nghe1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 34, RangeId = nghe1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 35, RangeId = nghe1ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 36, RangeId = nghe1ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 37, RangeId = doc1ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 38, RangeId = doc1ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 39, RangeId = doc1ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 40, RangeId = doc1ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 41, RangeId = doc1ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 42, RangeId = doc1ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 43, RangeId = doc1ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 44, RangeId = doc1ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap1normal.Id, Order = 45, RangeId = doc1ranges![2].Id, QuestionPerDay = 12 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 1, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 2, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 3, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 4, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 5, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 6, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 7, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 8, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 9, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 10, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 11, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 12, RangeId = doc1ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 13, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 14, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 15, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 16, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 17, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 18, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 19, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 20, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 21, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 22, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 23, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 24, RangeId = nghe1ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 25, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 26, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 27, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 28, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 29, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 30, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 31, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 32, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 33, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 34, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 35, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 36, RangeId = doc1ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 37, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 38, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 39, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 40, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 41, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 42, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 43, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 44, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 45, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 46, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 47, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 48, RangeId = nghe1ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 49, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 50, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 51, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 52, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 53, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 54, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 55, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 56, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 57, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 58, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 59, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap1long.Id, Order = 60, RangeId = doc1ranges![2].Id, QuestionPerDay = 15 }
        );

        var nghe2ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe2.Id); // 4 range
        var doc2ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc2.Id); // 5 range

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 1, RangeId = doc2ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 2, RangeId = doc2ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 3, RangeId = doc2ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 4, RangeId = nghe2ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 5, RangeId = nghe2ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 6, RangeId = nghe2ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 7, RangeId = doc2ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 8, RangeId = doc2ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 9, RangeId = doc2ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 10, RangeId = doc2ranges![1].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 11, RangeId = nghe2ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 12, RangeId = nghe2ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 13, RangeId = nghe2ranges![1].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 14, RangeId = doc2ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 15, RangeId = doc2ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 16, RangeId = doc2ranges![2].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 17, RangeId = nghe2ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 18, RangeId = nghe2ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 19, RangeId = nghe2ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 20, RangeId = nghe2ranges![2].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 21, RangeId = doc2ranges![3].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 22, RangeId = doc2ranges![3].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 23, RangeId = doc2ranges![3].Id, QuestionPerDay = 10 },
            
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 24, RangeId = nghe2ranges![3].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 25, RangeId = nghe2ranges![3].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 26, RangeId = nghe2ranges![3].Id, QuestionPerDay = 10 },
            
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 27, RangeId = doc2ranges![4].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 28, RangeId = doc2ranges![4].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 29, RangeId = doc2ranges![4].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2short.Id, Order = 30, RangeId = doc2ranges![4].Id, QuestionPerDay = 10 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 1, RangeId = doc2ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 2, RangeId = doc2ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 3, RangeId = doc2ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 4, RangeId = doc2ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 5, RangeId = doc2ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 6, RangeId = nghe2ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 7, RangeId = nghe2ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 8, RangeId = nghe2ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 9, RangeId = nghe2ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 10, RangeId = nghe2ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 11, RangeId = doc2ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 12, RangeId = doc2ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 13, RangeId = doc2ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 14, RangeId = doc2ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 15, RangeId = doc2ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 16, RangeId = nghe2ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 17, RangeId = nghe2ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 18, RangeId = nghe2ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 19, RangeId = nghe2ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 20, RangeId = nghe2ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 21, RangeId = doc2ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 22, RangeId = doc2ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 23, RangeId = doc2ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 24, RangeId = doc2ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 25, RangeId = doc2ranges![2].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 26, RangeId = nghe2ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 27, RangeId = nghe2ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 28, RangeId = nghe2ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 29, RangeId = nghe2ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 30, RangeId = nghe2ranges![2].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 31, RangeId = doc2ranges![3].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 32, RangeId = doc2ranges![3].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 33, RangeId = doc2ranges![3].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 34, RangeId = doc2ranges![3].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 35, RangeId = doc2ranges![3].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 36, RangeId = nghe2ranges![3].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 37, RangeId = nghe2ranges![3].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 38, RangeId = nghe2ranges![3].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 39, RangeId = nghe2ranges![3].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 40, RangeId = nghe2ranges![3].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 41, RangeId = doc2ranges![4].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 42, RangeId = doc2ranges![4].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 43, RangeId = doc2ranges![4].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 44, RangeId = doc2ranges![4].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap2normal.Id, Order = 45, RangeId = doc2ranges![4].Id, QuestionPerDay = 12 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 1, RangeId = doc2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 2, RangeId = doc2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 3, RangeId = doc2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 4, RangeId = doc2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 5, RangeId = doc2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 6, RangeId = doc2ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 7, RangeId = nghe2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 8, RangeId = nghe2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 9, RangeId = nghe2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 10, RangeId = nghe2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 11, RangeId = nghe2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 12, RangeId = nghe2ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 13, RangeId = nghe2ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 14, RangeId = doc2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 15, RangeId = doc2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 16, RangeId = doc2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 17, RangeId = doc2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 18, RangeId = doc2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 19, RangeId = doc2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 20, RangeId = doc2ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 21, RangeId = nghe2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 22, RangeId = nghe2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 23, RangeId = nghe2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 24, RangeId = nghe2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 25, RangeId = nghe2ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 26, RangeId = nghe2ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 27, RangeId = doc2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 28, RangeId = doc2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 29, RangeId = doc2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 30, RangeId = doc2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 31, RangeId = doc2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 32, RangeId = doc2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 33, RangeId = doc2ranges![2].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 34, RangeId = nghe2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 35, RangeId = nghe2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 36, RangeId = nghe2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 37, RangeId = nghe2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 38, RangeId = nghe2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 39, RangeId = nghe2ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 40, RangeId = nghe2ranges![2].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 41, RangeId = doc2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 42, RangeId = doc2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 43, RangeId = doc2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 44, RangeId = doc2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 45, RangeId = doc2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 46, RangeId = doc2ranges![3].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 47, RangeId = nghe2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 48, RangeId = nghe2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 49, RangeId = nghe2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 50, RangeId = nghe2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 51, RangeId = nghe2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 52, RangeId = nghe2ranges![3].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 53, RangeId = nghe2ranges![3].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 54, RangeId = doc2ranges![4].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 55, RangeId = doc2ranges![4].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 56, RangeId = doc2ranges![4].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 57, RangeId = doc2ranges![4].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 58, RangeId = doc2ranges![4].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 59, RangeId = doc2ranges![4].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap2long.Id, Order = 60, RangeId = doc2ranges![4].Id, QuestionPerDay = 15 }
        );

        var nghe3ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe3.Id); // 3 range
        var doc3ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc3.Id); // 3 range
        var viet3ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(viet3.Id); // 0 range

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 1, RangeId = doc3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 2, RangeId = doc3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 3, RangeId = doc3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 4, RangeId = doc3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 5, RangeId = doc3ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 6, RangeId = nghe3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 7, RangeId = nghe3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 8, RangeId = nghe3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 9, RangeId = nghe3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 10, RangeId = nghe3ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 11, RangeId = doc3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 12, RangeId = doc3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 13, RangeId = doc3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 14, RangeId = doc3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 15, RangeId = doc3ranges![1].Id, QuestionPerDay = 10 },
            
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 16, RangeId = nghe3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 17, RangeId = nghe3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 18, RangeId = nghe3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 19, RangeId = nghe3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 20, RangeId = nghe3ranges![1].Id, QuestionPerDay = 10 },
            
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 21, RangeId = doc3ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 22, RangeId = doc3ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 23, RangeId = doc3ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 24, RangeId = doc3ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 25, RangeId = doc3ranges![2].Id, QuestionPerDay = 10 },
            
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 26, RangeId = nghe3ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 27, RangeId = nghe3ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 28, RangeId = nghe3ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 29, RangeId = nghe3ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3short.Id, Order = 30, RangeId = nghe3ranges![2].Id, QuestionPerDay = 10 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 1, RangeId = doc3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 2, RangeId = doc3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 3, RangeId = doc3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 4, RangeId = doc3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 5, RangeId = doc3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 6, RangeId = doc3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 7, RangeId = doc3ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 8, RangeId = nghe3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 9, RangeId = nghe3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 10, RangeId = nghe3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 11, RangeId = nghe3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 12, RangeId = nghe3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 13, RangeId = nghe3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 14, RangeId = nghe3ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 15, RangeId = nghe3ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 16, RangeId = doc3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 17, RangeId = doc3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 18, RangeId = doc3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 19, RangeId = doc3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 20, RangeId = doc3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 21, RangeId = doc3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 22, RangeId = doc3ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 23, RangeId = nghe3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 24, RangeId = nghe3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 25, RangeId = nghe3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 26, RangeId = nghe3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 27, RangeId = nghe3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 28, RangeId = nghe3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 29, RangeId = nghe3ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 30, RangeId = nghe3ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 31, RangeId = doc3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 32, RangeId = doc3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 33, RangeId = doc3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 34, RangeId = doc3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 35, RangeId = doc3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 36, RangeId = doc3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 37, RangeId = doc3ranges![2].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 38, RangeId = nghe3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 39, RangeId = nghe3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 40, RangeId = nghe3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 41, RangeId = nghe3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 42, RangeId = nghe3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 43, RangeId = nghe3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 44, RangeId = nghe3ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap3normal.Id, Order = 45, RangeId = nghe3ranges![2].Id, QuestionPerDay = 12 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 1, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 2, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 3, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 4, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 5, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 6, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 7, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 8, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 9, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 10, RangeId = doc3ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 11, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 12, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 13, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 14, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 15, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 16, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 17, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 18, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 19, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 20, RangeId = nghe3ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 21, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 22, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 23, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 24, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 25, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 26, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 27, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 28, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 29, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 30, RangeId = doc3ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 31, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 32, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 33, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 34, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 35, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 36, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 37, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 38, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 39, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 40, RangeId = nghe3ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 41, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 42, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 43, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 44, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 45, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 46, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 47, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 48, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 49, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 50, RangeId = doc3ranges![2].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 51, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 52, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 53, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 54, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 55, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 56, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 57, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 58, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 59, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap3long.Id, Order = 60, RangeId = nghe3ranges![2].Id, QuestionPerDay = 15 }
        );

        var nghe4ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe4.Id); // 3 range
        var doc4ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc4.Id); // 3 range
        var viet4ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(viet4.Id); // 0 range

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 1, RangeId = doc4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 2, RangeId = doc4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 3, RangeId = doc4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 4, RangeId = doc4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 5, RangeId = doc4ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 6, RangeId = nghe4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 7, RangeId = nghe4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 8, RangeId = nghe4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 9, RangeId = nghe4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 10, RangeId = nghe4ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 11, RangeId = doc4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 12, RangeId = doc4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 13, RangeId = doc4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 14, RangeId = doc4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 15, RangeId = doc4ranges![1].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 16, RangeId = nghe4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 17, RangeId = nghe4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 18, RangeId = nghe4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 19, RangeId = nghe4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 20, RangeId = nghe4ranges![1].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 21, RangeId = doc4ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 22, RangeId = doc4ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 23, RangeId = doc4ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 24, RangeId = doc4ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 25, RangeId = doc4ranges![2].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 26, RangeId = nghe4ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 27, RangeId = nghe4ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 28, RangeId = nghe4ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 29, RangeId = nghe4ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4short.Id, Order = 30, RangeId = nghe4ranges![2].Id, QuestionPerDay = 10 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 1, RangeId = doc4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 2, RangeId = doc4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 3, RangeId = doc4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 4, RangeId = doc4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 5, RangeId = doc4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 6, RangeId = doc4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 7, RangeId = doc4ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 8, RangeId = nghe4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 9, RangeId = nghe4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 10, RangeId = nghe4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 11, RangeId = nghe4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 12, RangeId = nghe4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 13, RangeId = nghe4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 14, RangeId = nghe4ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 15, RangeId = nghe4ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 16, RangeId = doc4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 17, RangeId = doc4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 18, RangeId = doc4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 19, RangeId = doc4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 20, RangeId = doc4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 21, RangeId = doc4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 22, RangeId = doc4ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 23, RangeId = nghe4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 24, RangeId = nghe4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 25, RangeId = nghe4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 26, RangeId = nghe4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 27, RangeId = nghe4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 28, RangeId = nghe4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 29, RangeId = nghe4ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 30, RangeId = nghe4ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 31, RangeId = doc4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 32, RangeId = doc4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 33, RangeId = doc4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 34, RangeId = doc4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 35, RangeId = doc4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 36, RangeId = doc4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 37, RangeId = doc4ranges![2].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 38, RangeId = nghe4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 39, RangeId = nghe4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 40, RangeId = nghe4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 41, RangeId = nghe4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 42, RangeId = nghe4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 43, RangeId = nghe4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 44, RangeId = nghe4ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap4normal.Id, Order = 45, RangeId = nghe4ranges![2].Id, QuestionPerDay = 12 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 1, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 2, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 3, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 4, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 5, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 6, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 7, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 8, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 9, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 10, RangeId = doc4ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 11, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 12, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 13, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 14, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 15, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 16, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 17, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 18, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 19, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 20, RangeId = nghe4ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 21, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 22, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 23, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 24, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 25, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 26, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 27, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 28, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 29, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 30, RangeId = doc4ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 31, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 32, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 33, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 34, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 35, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 36, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 37, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 38, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 39, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 40, RangeId = nghe4ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 41, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 42, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 43, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 44, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 45, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 46, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 47, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 48, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 49, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 50, RangeId = doc4ranges![2].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 51, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 52, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 53, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 54, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 55, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 56, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 57, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 58, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 59, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap4long.Id, Order = 60, RangeId = nghe4ranges![2].Id, QuestionPerDay = 15 }
        );

        var nghe5ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe5.Id); // 3 range
        var doc5ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc5.Id); // 2 range
        var viet5ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(viet5.Id); // 0 range
        
        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 1, RangeId = nghe5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 2, RangeId = nghe5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 3, RangeId = nghe5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 4, RangeId = nghe5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 5, RangeId = nghe5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 6, RangeId = nghe5ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 7, RangeId = doc5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 8, RangeId = doc5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 9, RangeId = doc5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 10, RangeId = doc5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 11, RangeId = doc5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 12, RangeId = doc5ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 13, RangeId = nghe5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 14, RangeId = nghe5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 15, RangeId = nghe5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 16, RangeId = nghe5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 17, RangeId = nghe5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 18, RangeId = nghe5ranges![1].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 19, RangeId = doc5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 20, RangeId = doc5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 21, RangeId = doc5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 22, RangeId = doc5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 23, RangeId = doc5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 24, RangeId = doc5ranges![1].Id, QuestionPerDay = 10 },
            
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 25, RangeId = nghe5ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 26, RangeId = nghe5ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 27, RangeId = nghe5ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 28, RangeId = nghe5ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 29, RangeId = nghe5ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5short.Id, Order = 30, RangeId = nghe5ranges![2].Id, QuestionPerDay = 10 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 1, RangeId = nghe5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 2, RangeId = nghe5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 3, RangeId = nghe5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 4, RangeId = nghe5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 5, RangeId = nghe5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 6, RangeId = nghe5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 7, RangeId = nghe5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 8, RangeId = nghe5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 9, RangeId = nghe5ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 10, RangeId = doc5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 11, RangeId = doc5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 12, RangeId = doc5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 13, RangeId = doc5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 14, RangeId = doc5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 15, RangeId = doc5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 16, RangeId = doc5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 17, RangeId = doc5ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 18, RangeId = doc5ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 19, RangeId = nghe5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 20, RangeId = nghe5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 21, RangeId = nghe5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 22, RangeId = nghe5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 23, RangeId = nghe5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 24, RangeId = nghe5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 25, RangeId = nghe5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 26, RangeId = nghe5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 27, RangeId = nghe5ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 28, RangeId = doc5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 29, RangeId = doc5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 30, RangeId = doc5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 31, RangeId = doc5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 32, RangeId = doc5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 33, RangeId = doc5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 34, RangeId = doc5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 35, RangeId = doc5ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 36, RangeId = doc5ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 37, RangeId = nghe5ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 38, RangeId = nghe5ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 39, RangeId = nghe5ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 40, RangeId = nghe5ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 41, RangeId = nghe5ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 42, RangeId = nghe5ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 43, RangeId = nghe5ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 44, RangeId = nghe5ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap5normal.Id, Order = 45, RangeId = nghe5ranges![2].Id, QuestionPerDay = 12 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 1, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 2, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 3, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 4, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 5, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 6, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 7, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 8, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 9, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 10, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 11, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 12, RangeId = nghe5ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 13, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 14, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 15, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 16, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 17, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 18, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 19, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 20, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 21, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 22, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 23, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 24, RangeId = doc5ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 25, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 26, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 27, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 28, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 29, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 30, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 31, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 32, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 33, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 34, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 35, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 36, RangeId = nghe5ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 37, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 38, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 39, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 40, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 41, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 42, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 43, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 44, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 45, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 46, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 47, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 48, RangeId = doc5ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 49, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 50, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 51, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 52, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 53, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 54, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 55, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 56, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 57, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 58, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 59, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap5long.Id, Order = 60, RangeId = nghe5ranges![2].Id, QuestionPerDay = 15 }
        );

        var nghe6ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe6.Id); // 3 range
        var doc6ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc6.Id); // 3 range
        var viet6ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(viet6.Id); // 0 range

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 1, RangeId = doc6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 2, RangeId = doc6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 3, RangeId = doc6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 4, RangeId = doc6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 5, RangeId = doc6ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 6, RangeId = nghe6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 7, RangeId = nghe6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 8, RangeId = nghe6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 9, RangeId = nghe6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 10, RangeId = nghe6ranges![0].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 11, RangeId = doc6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 12, RangeId = doc6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 13, RangeId = doc6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 14, RangeId = doc6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 15, RangeId = doc6ranges![1].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 16, RangeId = nghe6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 17, RangeId = nghe6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 18, RangeId = nghe6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 19, RangeId = nghe6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 20, RangeId = nghe6ranges![1].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 21, RangeId = doc6ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 22, RangeId = doc6ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 23, RangeId = doc6ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 24, RangeId = doc6ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 25, RangeId = doc6ranges![2].Id, QuestionPerDay = 10 },

            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 26, RangeId = nghe6ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 27, RangeId = nghe6ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 28, RangeId = nghe6ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 29, RangeId = nghe6ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6short.Id, Order = 30, RangeId = nghe6ranges![2].Id, QuestionPerDay = 10 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 1, RangeId = doc6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 2, RangeId = doc6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 3, RangeId = doc6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 4, RangeId = doc6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 5, RangeId = doc6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 6, RangeId = doc6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 7, RangeId = doc6ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 8, RangeId = nghe6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 9, RangeId = nghe6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 10, RangeId = nghe6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 11, RangeId = nghe6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 12, RangeId = nghe6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 13, RangeId = nghe6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 14, RangeId = nghe6ranges![0].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 15, RangeId = nghe6ranges![0].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 16, RangeId = doc6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 17, RangeId = doc6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 18, RangeId = doc6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 19, RangeId = doc6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 20, RangeId = doc6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 21, RangeId = doc6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 22, RangeId = doc6ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 23, RangeId = nghe6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 24, RangeId = nghe6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 25, RangeId = nghe6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 26, RangeId = nghe6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 27, RangeId = nghe6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 28, RangeId = nghe6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 29, RangeId = nghe6ranges![1].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 30, RangeId = nghe6ranges![1].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 31, RangeId = doc6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 32, RangeId = doc6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 33, RangeId = doc6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 34, RangeId = doc6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 35, RangeId = doc6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 36, RangeId = doc6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 37, RangeId = doc6ranges![2].Id, QuestionPerDay = 12 },

            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 38, RangeId = nghe6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 39, RangeId = nghe6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 40, RangeId = nghe6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 41, RangeId = nghe6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 42, RangeId = nghe6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 43, RangeId = nghe6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 44, RangeId = nghe6ranges![2].Id, QuestionPerDay = 12 },
            new RoadmapElement { RoadmapId = roadmap6normal.Id, Order = 45, RangeId = nghe6ranges![2].Id, QuestionPerDay = 12 }
        );

        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 1, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 2, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 3, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 4, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 5, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 6, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 7, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 8, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 9, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 10, RangeId = doc6ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 11, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 12, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 13, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 14, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 15, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 16, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 17, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 18, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 19, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 20, RangeId = nghe6ranges![0].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 21, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 22, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 23, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 24, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 25, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 26, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 27, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 28, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 29, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 30, RangeId = doc6ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 31, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 32, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 33, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 34, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 35, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 36, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 37, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 38, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 39, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 40, RangeId = nghe6ranges![1].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 41, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 42, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 43, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 44, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 45, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 46, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 47, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 48, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 49, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 50, RangeId = doc6ranges![2].Id, QuestionPerDay = 15 },

            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 51, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 52, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 53, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 54, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 55, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 56, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 57, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 58, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 59, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 },
            new RoadmapElement { RoadmapId = roadmap6long.Id, Order = 60, RangeId = nghe6ranges![2].Id, QuestionPerDay = 15 }
        );

        await context.SaveChangesAsync(cancellationToken);
    }
}

// source: 
// range: /api/Range/filter/skill-level(levelskillId)
// [ // nghe 1
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":1,"endQuestionNumber":6},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":7,"endQuestionNumber":10},
// ]

// [ // đọc 1
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":1,"endQuestionNumber":5},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":6,"endQuestionNumber":10},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":11,"endQuestionNumber":15},
// ]

// [ // nghe 2. trùng range?
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":11,"endQuestionNumber":15},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":16,"endQuestionNumber":20},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":21,"endQuestionNumber":25},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":26,"endQuestionNumber":30},
// ]

// [ // đọc 2. thiếu?
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":16,"endQuestionNumber":20},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":21,"endQuestionNumber":25},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":26,"endQuestionNumber":30},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":31,"endQuestionNumber":35},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":36,"endQuestionNumber":40},
// ]

// [ // nghe 3
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":1,"endQuestionNumber":3},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":4,"endQuestionNumber":8},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":9,"endQuestionNumber":12},
// ]

// [ // đọc 3
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":1,"endQuestionNumber":4},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":5,"endQuestionNumber":8},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":9,"endQuestionNumber":12},
// ]

// [] // viết 3

// [ // nghe 4
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":13,"endQuestionNumber":16},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":17,"endQuestionNumber":21},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":22,"endQuestionNumber":24},
// ]

// [ // đọc 4
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":13,"endQuestionNumber":15},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":16,"endQuestionNumber":22},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":22,"endQuestionNumber":24},
// ]

// [] // viết 4

// [ // nghe 5
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":25,"endQuestionNumber":28},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":29,"endQuestionNumber":33},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":34,"endQuestionNumber":37},
// ]


// [ // đọc 5
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":28,"endQuestionNumber":31},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":32,"endQuestionNumber":38},
// ]

// [] // viết 5

// [ // nghe 6
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":38,"endQuestionNumber":40},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":41,"endQuestionNumber":43},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":44,"endQuestionNumber":50},
// ]

// [ // đọc 6
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":39,"endQuestionNumber":43},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":44,"endQuestionNumber":45},
// {"id": "(random-guid)","name":"(description)","startQuestionNumber":46,"endQuestionNumber":50},
// ]

// [] // viết 6