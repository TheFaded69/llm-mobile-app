using System.Text.Json;
using BaseInfrastructure.DbContext;
using Main.Domain.Dialogs.Models;
using Main.Domain.Dictionary.Models;
using Main.Domain.Identity.Models;
using Main.Domain.Tests.Models;
using Main.Domain.Tests.Models.Answers;
using Main.Domain.Tests.Models.Questions;
using Main.Domain.Tutors.Models;
using Main.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.DbContext;

public class MainDataContext : DataContext 
{
    public MainDataContext(DbContextOptions<MainDataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        CreateDbUsersModel(modelBuilder);
        CreateDbIdentityModels(modelBuilder);
        CreateDbTestModels(modelBuilder);
        CreateDbDialogModels(modelBuilder);
        CreateDbDictionaryModels(modelBuilder);
        CreateDbTutorModels(modelBuilder);
    }
    
    private void CreateDbUsersModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        
        CreateBaseEntity<User, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<User>()
            .Property(e => e.UserName)
            .HasMaxLength(255);
        modelBuilder
            .Entity<User>()
            .Property(e => e.Role);
        modelBuilder
            .Entity<User>()
            .Property(e => e.UserType);
        modelBuilder
            .Entity<User>()
            .Property(e => e.Email)
            .HasMaxLength(255);
        modelBuilder
            .Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();
    }

    private void CreateDbIdentityModels(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUser>().ToTable("IdentityUsers");
        
        CreateBaseEntity<IdentityUser, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<IdentityUser>()
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);
        modelBuilder
            .Entity<IdentityUser>()
            .Property(x => x.PasswordHash)
            .IsRequired();
        modelBuilder
            .Entity<IdentityUser>()
            .Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(255);
        modelBuilder
            .Entity<IdentityUser>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder
            .Entity<RefreshToken>()
            .ToTable("RefreshTokens");
        
        CreateBaseEntity<RefreshToken, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<RefreshToken>()
            .Property(x => x.TokenHash)
            .IsRequired();
        modelBuilder
            .Entity<RefreshToken>()
            .HasIndex(x => x.TokenHash)
            .IsUnique();
        modelBuilder
            .Entity<RefreshToken>()
            .HasIndex(x => x.IdentityUserId);

        modelBuilder.Entity<PasswordResetToken>().ToTable("PasswordResetTokens");
        
        CreateBaseEntity<PasswordResetToken, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<PasswordResetToken>()
            .Property(x => x.TokenHash)
            .IsRequired();
        modelBuilder
            .Entity<PasswordResetToken>()
            .HasIndex(x => x.TokenHash)
            .IsUnique();

        modelBuilder.Entity<ExternalLogin>().ToTable("ExternalLogins");
        
        CreateBaseEntity<ExternalLogin, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<ExternalLogin>()
            .Property(x => x.Provider)
            .IsRequired();
        modelBuilder
            .Entity<ExternalLogin>()
            .Property(x => x.ProviderUserId)
            .IsRequired();
        modelBuilder.Entity<ExternalLogin>()
            .HasIndex(x => new { x.Provider, x.ProviderUserId })
            .IsUnique();
    }

    private void CreateDbTestModels(ModelBuilder modelBuilder)
    {
        #region Sets
        
        CreateBaseEntity<Set, Guid>(modelBuilder);
        modelBuilder.Entity<Set>(b =>
        {
            b.ToTable("Sets");
            b.HasOne(s => s.User)
                .WithMany(u => u.CreatedSets)
                .HasForeignKey(s => s.UserId);
            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);
            b.Property(x => x.Description)
                .IsRequired();
            b.Property(x => x.IsPublic)
                .IsRequired();
            b.Property(x => x.TestDifficult)
                .IsRequired();
            b.Property(x => x.Duration)
                .IsRequired();
            b.Property(x => x.SetStatus)
                .IsRequired();
        });
        
        CreateBaseEntity<SetItem, Guid>(modelBuilder);
        modelBuilder.Entity<SetItem>(b =>
        {
            b.ToTable("SetItems");
            b.HasOne(s => s.Set)
                .WithMany(u => u.SetItems)
                .HasForeignKey(s => s.SetId);
            b.Property(x => x.Term)
                .IsRequired();
            b.Property(x => x.Description)
                .IsRequired();
        });
        
        #endregion

        #region Session
        
        CreateBaseEntity<Session, Guid>(modelBuilder);
        modelBuilder.Entity<Session>(b =>
        {
            b.ToTable("Sessions");
            b.HasOne(s => s.Set)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.SetId);
            b.HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId);
            b.Property(x => x.DeviceId)
                .IsRequired(false);;
            b.Property(x => x.TestMode)
                .IsRequired();
            b.Property(x => x.SessionStatus)
                .IsRequired();
        });
        
        CreateBaseEntity<SessionItem, Guid>(modelBuilder);
        modelBuilder.Entity<SessionItem>(b =>
        {
            b.ToTable("SessionItems");
            b.HasOne(s => s.Session)
                .WithMany(u => u.SessionItems)
                .HasForeignKey(s => s.SessionId);
            b.HasOne(s => s.Question)
                .WithOne()
                .HasForeignKey<SessionItem>(s => s.QuestionId);
            b.HasOne(s => s.Answer)
                .WithOne()
                .HasForeignKey<SessionItem>(s => s.AnswerId)
                .IsRequired(false);
            b.Property(x => x.IsCorrect);
        });
        
        #endregion
        
        #region Questions
        
        CreateBaseEntity<TestQuestion, Guid>(modelBuilder);
        modelBuilder.Entity<TestQuestion>(b =>
        {
            b.ToTable("TestQuestions");

            b.Property(x => x.Definition)
                .IsRequired();
            b.Property(x => x.ExplainTerm)
                .IsRequired()
                .HasMaxLength(255);
            b.Property(x => x.ExplainText)
                .IsRequired()
                .HasMaxLength(255);
        });
        modelBuilder.Entity<TestQuestionQuestion>(b =>
        {
            b.ToTable("TestQuestionQuestions");
        });
        CreateBaseEntity<TestQuestionQuestionOption, Guid>(modelBuilder);
        modelBuilder.Entity<TestQuestionQuestionOption>(b =>
        {
            b.ToTable("TestQuestionQuestionOptions");
            
            b.HasOne(x => x.TestQuestionQuestion)
                .WithMany(x => x.Options)
                .HasForeignKey(x => x.TestQuestionQuestionId);
            
            b.Property(x => x.Option)
                .IsRequired()
                .HasMaxLength(255);
        });
        modelBuilder.Entity<TestQuestionSelection>(b =>
        {
            b.ToTable("TestQuestionSelections");
        });
        CreateBaseEntity<TestQuestionSelectionAnswer, Guid>(modelBuilder);
        modelBuilder.Entity<TestQuestionSelectionAnswer>(b =>
        {
            b.ToTable("TestQuestionSelectionAnswers");
            
            b.HasOne(x => x.TestQuestionSelection)
                .WithMany(x => x.AnswerPool)
                .HasForeignKey(x => x.TestQuestionSelectionId);
            
            b.Property(x => x.Answer)
                .IsRequired()
                .HasMaxLength(255);
        });
        modelBuilder.Entity<TestQuestionTrueFalse>(b =>
        {
            b.ToTable("TestQuestionTrueFalses");

            b.Property(x => x.Term)
                .IsRequired();
            b.Property(x => x.TermTranslation)
                .IsRequired();
        });
        
        #endregion

        #region Answers

        CreateBaseEntity<TestAnswer, Guid>(modelBuilder);
        modelBuilder.Entity<TestAnswer>(b =>
        {
            b.ToTable("TestAnswers");
        });
        modelBuilder.Entity<TestAnswerQuestion>(b =>
        {
            b.ToTable("TestAnswerQuestions");

            b.Property(x => x.SelectedIndex)
                .IsRequired();
        });
        modelBuilder.Entity<TestAnswerSelection>(b =>
        {
            b.ToTable("TestAnswerSelections");

            b.Property(x => x.SelectedLabel)
                .IsRequired()
                .HasMaxLength(255);
        });
        modelBuilder.Entity<TestAnswerTrueFalse>(b =>
        {
            b.ToTable("TestAnswerTrueFalses");

            b.Property(x => x.UserSaidTrue)
                .IsRequired();
        });
        modelBuilder.Entity<TestAnswerWritten>(b =>
        {
            b.ToTable("TestAnswerWrittens");

            b.Property(x => x.UserText)
                .IsRequired()
                .HasMaxLength(255);
        });

        #endregion
    }

    private void CreateDbTutorModels(ModelBuilder modelBuilder)
    {
        #region Tutor

        CreateBaseEntity<Tutor, Guid>(modelBuilder);

        modelBuilder.Entity<Tutor>(b =>
        {
            b.ToTable("Tutors");

            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Property(x => x.TutorRole)
                .IsRequired()
                .HasMaxLength(255);

            b.Property(x => x.Description)
                .IsRequired();

            b.Property(x => x.Personality)
                .IsRequired()
                .HasMaxLength(255);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            b.Property(x => x.IsPublic)
                .IsRequired();

            b.Property(x => x.Difficulty)
                .IsRequired();

            b.HasMany(x => x.Stories)
                .WithOne()
                .HasForeignKey(x => x.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.TargetWords)
                .WithOne()
                .HasForeignKey(x => x.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => x.UserId);
            b.HasIndex(x => x.IsPublic);
        });

        #endregion

        #region FavoriteTutor

        CreateBaseEntity<FavoriteTutor, Guid>(modelBuilder);

        modelBuilder.Entity<FavoriteTutor>(b =>
        {
            b.ToTable("FavoriteTutors");

            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<Tutor>()
                .WithMany()
                .HasForeignKey(x => x.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => x.UserId);

            b.HasIndex(x => x.TutorId);

            b.HasIndex(x => new { x.UserId, x.TutorId })
                .IsUnique();
        });

        #endregion

        #region StoryLine

        CreateBaseEntity<StoryLine, Guid>(modelBuilder);

        modelBuilder.Entity<StoryLine>(b =>
        {
            b.ToTable("StoryLines");

            b.Property(x => x.Story)
                .IsRequired();

            b.HasIndex(x => x.TutorId);
        });

        #endregion

        #region TargetWord

        CreateBaseEntity<TargetWord, Guid>(modelBuilder);

        modelBuilder.Entity<TargetWord>(b =>
        {
            b.ToTable("TargetWords");

            b.Property(x => x.Word)
                .IsRequired()
                .HasMaxLength(100);

            b.HasIndex(x => x.TutorId);
        });

        #endregion
    }

    private void CreateDbDialogModels(ModelBuilder modelBuilder)
    {
        #region Dialog

        CreateBaseEntity<Dialog, Guid>(modelBuilder);

        modelBuilder.Entity<Dialog>(b =>
        {
            b.ToTable("Dialogs");

            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne<Tutor>()
                .WithMany()
                .HasForeignKey(x => x.TutorId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(x => x.Messages)
                .WithOne()
                .HasForeignKey(x => x.DialogId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => x.UserId);

            b.HasIndex(x => x.TutorId);
        });

        #endregion

        #region Message

        CreateBaseEntity<Message, Guid>(modelBuilder);

        modelBuilder.Entity<Message>(b =>
        {
            b.ToTable("Messages");

            b.Property(x => x.Sender)
                .IsRequired();

            b.Property(x => x.Text)
                .IsRequired();

            b.Property(x => x.Translation)
                .IsRequired(false);

            b.HasIndex(x => x.DialogId);
        });

        #endregion
    }

    private void CreateDbDictionaryModels(ModelBuilder modelBuilder)
    {
        CreateBaseEntity<UserDictionary, Guid>(modelBuilder);
        modelBuilder.Entity<UserDictionary>(b =>
        {
            b.ToTable("UserDictionaries");
            
            b.HasOne<User>()
                .WithOne()
                .HasForeignKey<UserDictionary>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(d => d.Words)
                .WithOne()
                .HasForeignKey(d => d.DictionaryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        
        CreateBaseEntity<DictionaryWord, Guid>(modelBuilder);
        modelBuilder.Entity<DictionaryWord>(b =>
        {
            b.ToTable("DictionaryWords");
            
            b.Property(x => x.Word)
                .IsRequired()
                .HasMaxLength(30);
            
            b.Property(x => x.Translation)
                .IsRequired()
                .HasMaxLength(30);
        });
    }
}