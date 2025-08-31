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
        var roadmap1 = new Roadmap { LevelStartId = Guid.Empty, LevelEndId = level1.Id, Name = "Lộ trình TOPIK 0 -> 1" };
        var roadmap2 = new Roadmap { LevelStartId = level1.Id, LevelEndId = level2.Id, Name = "Lộ trình TOPIK 1 -> 2" };
        var roadmap3 = new Roadmap { LevelStartId = level2.Id, LevelEndId = level3.Id, Name = "Lộ trình TOPIK 2 -> 3" };
        var roadmap4 = new Roadmap { LevelStartId = level3.Id, LevelEndId = level4.Id, Name = "Lộ trình TOPIK 3 -> 4" };
        var roadmap5 = new Roadmap { LevelStartId = level4.Id, LevelEndId = level5.Id, Name = "Lộ trình TOPIK 4 -> 5" };
        var roadmap6 = new Roadmap { LevelStartId = level5.Id, LevelEndId = level6.Id, Name = "Lộ trình TOPIK 5 -> 6" };
        await context.Roadmaps.AddRangeAsync(roadmap1, roadmap2, roadmap3, roadmap4, roadmap5, roadmap6);
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
            new RoadmapElement { RoadmapId = roadmap1.Id, Order = 1, RangeId = doc1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1.Id, Order = 2, RangeId = nghe1ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1.Id, Order = 3, RangeId = doc1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1.Id, Order = 4, RangeId = nghe1ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap1.Id, Order = 5, RangeId = doc1ranges![2].Id, QuestionPerDay = 10 }
        );

        var nghe2ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe2.Id); // 4 range
        var doc2ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc2.Id); // 5 range
        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap2.Id, Order = 1, RangeId = doc2ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2.Id, Order = 2, RangeId = nghe2ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2.Id, Order = 3, RangeId = doc2ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2.Id, Order = 4, RangeId = nghe2ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2.Id, Order = 5, RangeId = doc2ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2.Id, Order = 6, RangeId = nghe2ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2.Id, Order = 7, RangeId = doc2ranges![3].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2.Id, Order = 8, RangeId = nghe2ranges![3].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap2.Id, Order = 9, RangeId = doc2ranges![4].Id, QuestionPerDay = 10 }
        );

        var nghe3ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe3.Id); // 3 range
        var doc3ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc3.Id); // 3 range
        var viet3ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(viet3.Id); // 0 range
        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap3.Id, Order = 1, RangeId = doc3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3.Id, Order = 2, RangeId = nghe3ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3.Id, Order = 3, RangeId = doc3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3.Id, Order = 4, RangeId = nghe3ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3.Id, Order = 5, RangeId = doc3ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap3.Id, Order = 6, RangeId = nghe3ranges![2].Id, QuestionPerDay = 10 }
        );

        var nghe4ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe4.Id); // 3 range
        var doc4ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc4.Id); // 3 range
        var viet4ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(viet4.Id); // 0 range
        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap4.Id, Order = 1, RangeId = doc4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4.Id, Order = 2, RangeId = nghe4ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4.Id, Order = 3, RangeId = doc4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4.Id, Order = 4, RangeId = nghe4ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4.Id, Order = 5, RangeId = doc4ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap4.Id, Order = 6, RangeId = nghe4ranges![2].Id, QuestionPerDay = 10 }
        );

        var nghe5ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe5.Id); // 3 range
        var doc5ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc5.Id); // 2 range
        var viet5ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(viet5.Id); // 0 range
        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap5.Id, Order = 1, RangeId = nghe5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5.Id, Order = 2, RangeId = doc5ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5.Id, Order = 3, RangeId = nghe5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5.Id, Order = 4, RangeId = doc5ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap5.Id, Order = 5, RangeId = nghe5ranges![2].Id, QuestionPerDay = 10 }
        );

        var nghe6ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(nghe6.Id); // 3 range
        var doc6ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(doc6.Id); // 3 range
        var viet6ranges = await questionbankService.GetAllRangesBySkillLevelIdAsync(viet6.Id); // 0 range
        await context.RoadmapElements.AddRangeAsync(
            new RoadmapElement { RoadmapId = roadmap6.Id, Order = 1, RangeId = doc6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6.Id, Order = 2, RangeId = nghe6ranges![0].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6.Id, Order = 3, RangeId = doc6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6.Id, Order = 4, RangeId = nghe6ranges![1].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6.Id, Order = 5, RangeId = doc6ranges![2].Id, QuestionPerDay = 10 },
            new RoadmapElement { RoadmapId = roadmap6.Id, Order = 6, RangeId = nghe6ranges![2].Id, QuestionPerDay = 10 }
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