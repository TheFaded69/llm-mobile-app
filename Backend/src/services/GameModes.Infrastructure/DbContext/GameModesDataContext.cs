using BaseInfrastructure.DbContext;
using GameModes.Contracts.V1;
using GameModes.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace GameModes.Infrastructure.DbContext;

public class GameModesDataContext : DataContext
{
    public GameModesDataContext(DbContextOptions<GameModesDataContext> options) : base(options)
    {
        Database.Migrate();
    }

    public DbSet<GameModeEntity> GameModes => Set<GameModeEntity>();
    public DbSet<TestSetEntity> TestSets => Set<TestSetEntity>();
    public DbSet<QuestionEntity> Questions => Set<QuestionEntity>();
    public DbSet<TestSessionEntity> TestSessions => Set<TestSessionEntity>();
    public DbSet<SessionAnswerEntity> SessionAnswers => Set<SessionAnswerEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var listConverter = new ValueConverter<List<string>?, string?>(
            list => list == null ? null : JsonSerializer.Serialize(list, (JsonSerializerOptions?)null),
            json => string.IsNullOrWhiteSpace(json)
                ? null
                : JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null));

        modelBuilder.Entity<GameModeEntity>().ToTable("GameModes");
        modelBuilder.Entity<GameModeEntity>().HasKey(x => x.Mode);

        modelBuilder.Entity<TestSetEntity>().ToTable("TestSets");
        modelBuilder.Entity<TestSetEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<TestSetEntity>().Property(x => x.SetAnswerPool).HasConversion(listConverter);
        modelBuilder.Entity<TestSetEntity>().HasMany(x => x.Questions).WithOne(x => x.Set).HasForeignKey(x => x.SetId);

        modelBuilder.Entity<QuestionEntity>().ToTable("Questions");
        modelBuilder.Entity<QuestionEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<QuestionEntity>().HasIndex(x => new { x.SetId, x.QuestionId }).IsUnique();
        modelBuilder.Entity<QuestionEntity>().Property(x => x.Options).HasConversion(listConverter);
        modelBuilder.Entity<QuestionEntity>().Property(x => x.AnswerPool).HasConversion(listConverter);

        modelBuilder.Entity<TestSessionEntity>().ToTable("TestSessions");
        modelBuilder.Entity<TestSessionEntity>().HasKey(x => x.SessionId);
        modelBuilder.Entity<TestSessionEntity>().HasMany(x => x.Answers).WithOne(x => x.Session).HasForeignKey(x => x.SessionId);

        modelBuilder.Entity<SessionAnswerEntity>().ToTable("SessionAnswers");
        modelBuilder.Entity<SessionAnswerEntity>().HasKey(x => x.Id);

        modelBuilder.Entity<GameModeEntity>().HasData(
            new GameModeEntity { Mode = TestModes.TrueFalse, Title = "True / False", SupportsPerQuestionFeedback = true, SupportsFinalSubmitOnly = true },
            new GameModeEntity { Mode = TestModes.Questions, Title = "Questions", SupportsPerQuestionFeedback = true, SupportsFinalSubmitOnly = true },
            new GameModeEntity { Mode = TestModes.Selection, Title = "Selection", SupportsPerQuestionFeedback = true, SupportsFinalSubmitOnly = true },
            new GameModeEntity { Mode = TestModes.Written, Title = "Written", SupportsPerQuestionFeedback = true, SupportsFinalSubmitOnly = true }
        );

        modelBuilder.Entity<TestSetEntity>().HasData(
            new TestSetEntity
            {
                Id = "questions-ux-core",
                Mode = TestModes.Questions,
                Title = "Термины UX и мобильной разработки",
                Category = "Вопросы с выбором",
                Difficulty = "Средний",
                DurationLabel = "~ 10 мин",
                Author = "Команда курса",
                QuestionCount = 1,
                TotalInCourse = 12,
                ProgressPercent = 0,
                SectionDate = "Март 2026"
            },
            new TestSetEntity
            {
                Id = "ux-gestures",
                Mode = TestModes.TrueFalse,
                Title = "Термины UX и мобильной разработки",
                Category = "UX термины",
                Difficulty = "Средний",
                DurationLabel = "~ 10 мин",
                Author = "Команда курса",
                QuestionCount = 1,
                TotalInCourse = 12,
                ProgressPercent = 0,
                SectionDate = "Март 2026"
            },
            new TestSetEntity
            {
                Id = "selection-ux-core",
                Mode = TestModes.Selection,
                Title = "Термины UX и мобильной разработки",
                Category = "Подбор из списка",
                Difficulty = "Средний",
                DurationLabel = "~ 10 мин",
                Author = "Команда курса",
                QuestionCount = 1,
                TotalInCourse = 12,
                ProgressPercent = 0,
                SectionDate = "Март 2026",
                SetAnswerPool = ["Drag and drop", "Deployment", "Database"]
            },
            new TestSetEntity
            {
                Id = "written-ux-core",
                Mode = TestModes.Written,
                Title = "Термины UX и мобильной разработки",
                Category = "Письменный ответ",
                Difficulty = "Средний",
                DurationLabel = "~ 10 мин",
                Author = "Команда курса",
                QuestionCount = 1,
                TotalInCourse = 12,
                ProgressPercent = 0,
                SectionDate = "Март 2026"
            }
        );

        modelBuilder.Entity<QuestionEntity>().HasData(
            new QuestionEntity
            {
                Id = "q_questions-ux-core_1",
                SetId = "questions-ux-core",
                Kind = TestModes.Questions,
                QuestionId = "choice-1",
                Definition = "Изменение положения интерфейса с помощью перетягивания; дословно «тащи и бросай».",
                ExplainTerm = "Drag and drop",
                ExplainText = "Перетаскивание элементов интерфейса — drag and drop.",
                Options = ["Drag and drop", "Deployment", "Database", "Safe area"]
            },
            new QuestionEntity
            {
                Id = "q_ux-gestures_1",
                SetId = "ux-gestures",
                Kind = TestModes.TrueFalse,
                QuestionId = "choice-1",
                Definition = "Изменение положения интерфейса с помощью перетягивания; дословно «тащи и бросай».",
                ExplainTerm = "Drag and drop",
                ExplainText = "Перетаскивание элементов интерфейса — drag and drop.",
                Term = "Drag and drop",
                TermTranslation = "Перетаскивание"
            },
            new QuestionEntity
            {
                Id = "q_selection-ux-core_1",
                SetId = "selection-ux-core",
                Kind = TestModes.Selection,
                QuestionId = "choice-1",
                Definition = "Изменение положения интерфейса с помощью перетягивания; дословно «тащи и бросай».",
                ExplainTerm = "Drag and drop",
                ExplainText = "Перетаскивание элементов интерфейса — drag and drop."
            },
            new QuestionEntity
            {
                Id = "q_written-ux-core_1",
                SetId = "written-ux-core",
                Kind = TestModes.Written,
                QuestionId = "choice-1",
                Definition = "Изменение положения интерфейса с помощью перетягивания; дословно «тащи и бросай».",
                ExplainTerm = "Drag and drop",
                ExplainText = "Перетаскивание элементов интерфейса — drag and drop."
            }
        );
    }
}
