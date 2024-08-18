using Microsoft.EntityFrameworkCore;
using DataLayer.Models;

namespace DataLayer
{
    public class ProfileContext : DbContext
    {
        public ProfileContext(DbContextOptions<ProfileContext> options)
        : base(options)
        {
           //Database.EnsureDeleted();
           Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<AnswerBlock> AnswerBlocks { get; set; }
        public DbSet<AnswerVariant> AnswerVariants { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormModuleConnection> FormModuleConnections { get; set; }
        public DbSet<QuestionModule> QuestionModules { get; set; }
        public DbSet<QuestionUnit> QuestionUnits { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<UsersStamp> UsersStamps { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<UserDelay> UserDelays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity(typeof(Match))
            .HasOne(typeof(User), "FoundUser")
            .WithMany()
            .HasForeignKey("FoundUserId")
            .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Form>().HasData(
                    new Form { Id = 1, Name = "Знакомства", Description = "для знакомств", Active = true },
                    new Form { Id = 2, Name = "Отношения", Description = "Для отношений", Active = true },
                    new Form { Id = 3, Name = "Соседа", Description = "для соседа", Active = true }
            );

            List<QuestionUnit> questions = new List<QuestionUnit>();
            questions.Add(new QuestionUnit { Id = 1, TextQuestion = "Укажите свой пол" });
            questions.Add(new QuestionUnit { Id = 2, TextQuestion = "Кого вы хотите найти?" });
            questions.Add(new QuestionUnit { Id = 3, TextQuestion = "Сколько вам лет?" });
            questions.Add(new QuestionUnit { Id = 4, TextQuestion = "Человека какого возраста вы ищете?" });
            questions.Add(new QuestionUnit { Id = 5, TextQuestion = "Укажите свой город \n(Мы постоянно работаем над добавлением новых городов)" });
            questions.Add(new QuestionUnit { Id = 6, TextQuestion = "Ваш рост?" });
            questions.Add(new QuestionUnit { Id = 7, TextQuestion = "Человека какого роста вы ищете?" });
            questions.Add(new QuestionUnit { Id = 8, TextQuestion = "Ваше телосложение?" });
            questions.Add(new QuestionUnit { Id = 9, TextQuestion = "Человека какого телосложения вы ищете?" });
            questions.Add(new QuestionUnit { Id = 10, TextQuestion = "Сколько у вас детей?" });
            questions.Add(new QuestionUnit { Id = 11, TextQuestion = "Сколько может быть детей у человека, которого вы ищете?" });
            questions.Add(new QuestionUnit { Id = 12, TextQuestion = "Вы курите?" });
            questions.Add(new QuestionUnit { Id = 13, TextQuestion = "Может ли курить человек, которого вы ищете?" });
            questions.Add(new QuestionUnit { Id = 14, TextQuestion = "Вы..." });
            questions.Add(new QuestionUnit { Id = 15, TextQuestion = "Можно ли дома курить?" });
            questions.Add(new QuestionUnit { Id = 16, TextQuestion = "Могут ли дома быть домашние животные?" });
            questions.Add(new QuestionUnit { Id = 17, TextQuestion = "Сколько вы готовы платить за аренду в месяц?" });


            List<AnswerBlock> answers = new List<AnswerBlock>();

            answers.Add(new AnswerBlock() { Id = 1, Description = "СвойПол" });
            answers.Add(new AnswerBlock() { Id = 2, Description = "НеСвойПол" });
            answers.Add(new AnswerBlock() { Id = 3, Description = "СколькоЛет?" });
            answers.Add(new AnswerBlock() { Id = 4, Description = "СколькоЛет?+Любого" });
            answers.Add(new AnswerBlock() { Id = 5, Description = "Город" });
            answers.Add(new AnswerBlock() { Id = 6, Description = "СвойРост" });
            answers.Add(new AnswerBlock() { Id = 7, Description = "НеСвойРост" });
            answers.Add(new AnswerBlock() { Id = 8, Description = "СвоёТелосложение" });
            answers.Add(new AnswerBlock() { Id = 9, Description = "НеСвоеТелосложение" });
            answers.Add(new AnswerBlock() { Id = 10, Description = "КолвоДетей" });
            answers.Add(new AnswerBlock() { Id = 11, Description = "Да\\Нет" });
            answers.Add(new AnswerBlock() { Id = 12, Description = "ЖаворонокСова" });
            answers.Add(new AnswerBlock() { Id = 13, Description = "Да\\НетХомячок" });
            answers.Add(new AnswerBlock() { Id = 14, Description = "Аренда" });

            modelBuilder.Entity<QuestionUnit>().HasData(questions);
            modelBuilder.Entity<AnswerBlock>().HasData(answers);

            modelBuilder.Entity<AnswerVariant>().HasData(
                new AnswerVariant { Id = 1, AnswerBlockId = 1, Caption = "Мужчина", Data = "1" },
                new AnswerVariant { Id = 2, AnswerBlockId = 1, Caption = "Женщина", Data = "2" },
                new AnswerVariant { Id = 3, AnswerBlockId = 1, Caption = "Человек", Data = "3" },

                new AnswerVariant { Id = 4, AnswerBlockId = 2, Caption = "Мужчину", Data = "1" },
                new AnswerVariant { Id = 5, AnswerBlockId = 2, Caption = "Женщину", Data = "2" },
                new AnswerVariant { Id = 6, AnswerBlockId = 2, Caption = "Человека", Data = "3" },

                new AnswerVariant { Id = 7, AnswerBlockId = 3, Caption = "18-23", Data = "1" },
                new AnswerVariant { Id = 8, AnswerBlockId = 3, Caption = "24-29", Data = "2" },
                new AnswerVariant { Id = 9, AnswerBlockId = 3, Caption = "30-35", Data = "3" },
                new AnswerVariant { Id = 10, AnswerBlockId = 3, Caption = "36-45", Data = "4" },
                new AnswerVariant { Id = 11, AnswerBlockId = 3, Caption = "56-55", Data = "5" },
                new AnswerVariant { Id = 12, AnswerBlockId = 3, Caption = "56+", Data = "6" },

                new AnswerVariant { Id = 13, AnswerBlockId = 4, Caption = "18-23", Data = "1" },
                new AnswerVariant { Id = 14, AnswerBlockId = 4, Caption = "24-29", Data = "2" },
                new AnswerVariant { Id = 15, AnswerBlockId = 4, Caption = "30-35", Data = "3" },
                new AnswerVariant { Id = 16, AnswerBlockId = 4, Caption = "36-45", Data = "4" },
                new AnswerVariant { Id = 17, AnswerBlockId = 4, Caption = "46-55", Data = "5" },
                new AnswerVariant { Id = 18, AnswerBlockId = 4, Caption = "56+", Data = "6" },
                new AnswerVariant { Id = 19, AnswerBlockId = 4, Caption = "Любого", Data = "7" },

                new AnswerVariant { Id = 20, AnswerBlockId = 5, Caption = "Москва", Data = "1" },
                new AnswerVariant { Id = 21, AnswerBlockId = 5, Caption = "Санкт-Петербург", Data = "2" },

                new AnswerVariant { Id = 22, AnswerBlockId = 6, Caption = "Низкий", Data = "1" },
                new AnswerVariant { Id = 23, AnswerBlockId = 6, Caption = "Средний", Data = "2" },
                new AnswerVariant { Id = 24, AnswerBlockId = 6, Caption = "Высокий", Data = "3" },
            
                new AnswerVariant { Id = 25, AnswerBlockId = 7, Caption = "Низкого", Data = "1" },
                new AnswerVariant { Id = 26, AnswerBlockId = 7, Caption = "Среднего", Data = "2" },
                new AnswerVariant { Id = 27, AnswerBlockId = 7, Caption = "Высокого", Data = "3" },
                new AnswerVariant { Id = 28, AnswerBlockId = 7, Caption = "Любого", Data = "4" },

                new AnswerVariant { Id = 29, AnswerBlockId = 8, Caption = "Худое", Data = "1" },
                new AnswerVariant { Id = 30, AnswerBlockId = 8, Caption = "Среднее", Data = "2" },
                new AnswerVariant { Id = 31, AnswerBlockId = 8, Caption = "Полное", Data = "3" },
                new AnswerVariant { Id = 32, AnswerBlockId = 8, Caption = "Атлетическое", Data = "4" },

                new AnswerVariant { Id = 33, AnswerBlockId = 9, Caption = "Худого", Data = "1" },
                new AnswerVariant { Id = 34, AnswerBlockId = 9, Caption = "Среднего", Data = "2" },
                new AnswerVariant { Id = 35, AnswerBlockId = 9, Caption = "Полного", Data = "3" },
                new AnswerVariant { Id = 36, AnswerBlockId = 9, Caption = "Атлетического", Data = "4" },
                new AnswerVariant { Id = 37, AnswerBlockId = 9, Caption = "Любого", Data = "5" },

                new AnswerVariant { Id = 38, AnswerBlockId = 10, Caption = "Ни одного", Data = "1" },
                new AnswerVariant { Id = 39, AnswerBlockId = 10, Caption = "Один", Data = "2" },
                new AnswerVariant { Id = 40, AnswerBlockId = 10, Caption = "Двое", Data = "3" },
                new AnswerVariant { Id = 41, AnswerBlockId = 10, Caption = "Больше двух", Data = "4" },

                new AnswerVariant { Id = 42, AnswerBlockId = 11, Caption = "Да", Data = "1" },
                new AnswerVariant { Id = 43, AnswerBlockId = 11, Caption = "Нет", Data = "2" },

                new AnswerVariant { Id = 44, AnswerBlockId = 12, Caption = "Жаворонок", Data = "1" },
                new AnswerVariant { Id = 45, AnswerBlockId = 12, Caption = "Сова", Data = "2" },
                new AnswerVariant { Id = 46, AnswerBlockId = 12, Caption = "Что-то среднее", Data = "3" },

                new AnswerVariant { Id = 47, AnswerBlockId = 13, Caption = "Да", Data = "1" },
                new AnswerVariant { Id = 48, AnswerBlockId = 13, Caption = "Нет", Data = "2" },
                new AnswerVariant { Id = 49, AnswerBlockId = 13, Caption = "Да, но не больше хомячка", Data = "3" },

                new AnswerVariant { Id = 50, AnswerBlockId = 14, Caption = "До 10 000", Data = "1" },
                new AnswerVariant { Id = 51, AnswerBlockId = 14, Caption = "10 000 - 20 000", Data = "2" },
                new AnswerVariant { Id = 52, AnswerBlockId = 14, Caption = "20 000 - 30 000", Data = "3" },
                new AnswerVariant { Id = 53, AnswerBlockId = 14, Caption = "30 000 и выше", Data = "4" }
                    );

            modelBuilder.Entity<QuestionModule>().HasData(
                        new QuestionModule
                        {
                            Id = 1,
                            Descripion = "Пол",
                            FirstQuestionId = 1,
                            SecondQuestionId = 2,
                            FirstQuestionAnswerBlockId = 1,
                            SecondQuestionAnswerBlockId = 2,
                            JSONAnswerMatrix = "{\"1\":[\"1\",\"3\"],\"2\":[\"2\",\"3\"],\"3\":[\"3\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = false
                        },
                        new QuestionModule
                        {
                            Id = 2,
                            Descripion = "Возраст",
                            FirstQuestionId = 3,
                            SecondQuestionId = 4,
                            FirstQuestionAnswerBlockId = 3,
                            SecondQuestionAnswerBlockId = 4,
                            JSONAnswerMatrix = "{\"1\":[\"1\",\"7\"],\"2\":[\"2\",\"7\"],\"3\":[\"3\",\"7\"],\"4\":[\"4\",\"7\"],\"5\":[\"5\",\"7\"],\"6\":[\"6\",\"7\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = true
                        },
                        new QuestionModule
                        {
                            Id = 3,
                            Descripion = "Город",
                            FirstQuestionId = 5,
                            SecondQuestionId = null,
                            FirstQuestionAnswerBlockId = 5,
                            SecondQuestionAnswerBlockId = null,
                            JSONAnswerMatrix = "{\"1\":[\"1\"],\"2\":[\"2\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = false
                        },
                        new QuestionModule
                        {
                            Id = 4,
                            Descripion = "Рост",
                            FirstQuestionId = 6,
                            SecondQuestionId = 7,
                            FirstQuestionAnswerBlockId = 6,
                            SecondQuestionAnswerBlockId = 7,
                            JSONAnswerMatrix = "{\"1\":[\"1\",\"4\"],\"2\":[\"2\",\"4\"],\"3\":[\"3\",\"4\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = true
                        },
                        new QuestionModule
                        {
                            Id = 5,
                            Descripion = "Телосложение",
                            FirstQuestionId = 8,
                            SecondQuestionId = 9,
                            FirstQuestionAnswerBlockId = 8,
                            SecondQuestionAnswerBlockId = 9,
                            JSONAnswerMatrix = "{\"1\":[\"5\",\"1\"],\"2\":[\"2\",\"5\"],\"3\":[\"3\",\"5\"],\"4\":[\"4\",\"5\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = true
                        },
                        new QuestionModule
                        {
                            Id = 6,
                            Descripion = "КолвоДетей",
                            FirstQuestionId = 10,
                            SecondQuestionId = 11,
                            FirstQuestionAnswerBlockId = 10,
                            SecondQuestionAnswerBlockId = 10,
                            JSONAnswerMatrix = "{\"1\":[\"1\"],\"2\":[\"2\"],\"3\":[\"3\"],\"4\":[\"4\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = true
                        },
                        new QuestionModule
                        {
                            Id = 7,
                            Descripion = "ВыКуритЕ?",
                            FirstQuestionId = 12,
                            SecondQuestionId = 13,
                            FirstQuestionAnswerBlockId = 11,
                            SecondQuestionAnswerBlockId = 11,
                            JSONAnswerMatrix = "{\"1\":[\"1\",\"2\"],\"2\":[\"2\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = false
                        },
                        new QuestionModule
                        {
                            Id = 8,
                            Descripion = "СоВАЖаворонок?",
                            FirstQuestionId = 14,
                            SecondQuestionId = null,
                            FirstQuestionAnswerBlockId = 12,
                            SecondQuestionAnswerBlockId = null,
                            JSONAnswerMatrix = "{\"1\":[\"1\"],\"2\":[\"2\"],\"3\":[\"3\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = false
                        },
                        new QuestionModule
                        {
                            Id = 9,
                            Descripion = "КуритьДома?",
                            FirstQuestionId = 15,
                            SecondQuestionId = null,
                            FirstQuestionAnswerBlockId = 11,
                            SecondQuestionAnswerBlockId = null,
                            JSONAnswerMatrix = "{\"1\":[\"1\",\"2\"],\"2\":[\"2\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = false
                        },
                        new QuestionModule
                        {
                            Id = 10,
                            Descripion = "ДОмЖивотные?",
                            FirstQuestionId = 16,
                            SecondQuestionId = null,
                            FirstQuestionAnswerBlockId = 13,
                            SecondQuestionAnswerBlockId = null,
                            JSONAnswerMatrix = "{\"1\":[\"1\",\"2\",\"3\"],\"2\":[\"2\"],\"3\":[\"2\",\"3\"]}",
                            FirstMultiselectable = false,
                            SecondMultiselectable = false
                        },
                        new QuestionModule
                        {
                            Id = 11,
                            Descripion = "Аренда",
                            FirstQuestionId = 17,
                            SecondQuestionId = null,
                            FirstQuestionAnswerBlockId = 14,
                            SecondQuestionAnswerBlockId = null,
                            JSONAnswerMatrix = "{\"1\":[\"1\"],\"2\":[\"2\"],\"3\":[\"3\"],\"4\":[\"4\"]}",
                            FirstMultiselectable = true,
                            SecondMultiselectable = false
                        }
                    );

            modelBuilder.Entity<FormModuleConnection>().HasData(
                    new FormModuleConnection
                    {
                        Id = 1,
                        FormId = 1,
                        QuestionModuleId = 1,
                        NumberInSequence = 1
                    },
                    new FormModuleConnection
                    {
                        Id = 2,
                        FormId = 1,
                        QuestionModuleId = 2,
                        NumberInSequence = 2
                    },
                    new FormModuleConnection
                    {
                        Id = 3,
                        FormId = 2,
                        QuestionModuleId = 3,
                        NumberInSequence = 1
                    },
                    new FormModuleConnection
                    {
                        Id = 4,
                        FormId = 2,
                        QuestionModuleId = 1,
                        NumberInSequence = 2
                    },
                    new FormModuleConnection
                    {
                        Id = 5,
                        FormId = 2,
                        QuestionModuleId = 2,
                        NumberInSequence = 3
                    },
                    new FormModuleConnection
                    {
                        Id = 6,
                        FormId = 2,
                        QuestionModuleId = 4,
                        NumberInSequence = 4
                    },
                    new FormModuleConnection
                    {
                        Id = 7,
                        FormId = 2,
                        QuestionModuleId = 5,
                        NumberInSequence = 5
                    },
                    new FormModuleConnection
                    {
                        Id = 8,
                        FormId = 2,
                        QuestionModuleId = 6,
                        NumberInSequence = 6
                    },
                    new FormModuleConnection
                    {
                        Id = 9,
                        FormId = 2,
                        QuestionModuleId = 7,
                        NumberInSequence = 7
                    },
                    new FormModuleConnection
                    {
                        Id = 10,
                        FormId = 3,
                        QuestionModuleId = 3,
                        NumberInSequence = 1
                    },
                    new FormModuleConnection
                    {
                        Id = 11,
                        FormId = 3,
                        QuestionModuleId = 1,
                        NumberInSequence = 2
                    },
                    new FormModuleConnection
                    {
                        Id = 12,
                        FormId = 3,
                        QuestionModuleId = 2,
                        NumberInSequence = 3
                    },
                    new FormModuleConnection
                    {
                        Id = 13,
                        FormId = 3,
                        QuestionModuleId = 8,
                        NumberInSequence = 4
                    },
                    new FormModuleConnection
                    {
                        Id = 14,
                        FormId = 3,
                        QuestionModuleId = 9,
                        NumberInSequence = 5
                    },
                    new FormModuleConnection
                    {
                        Id = 15,
                        FormId = 3,
                        QuestionModuleId = 10,
                        NumberInSequence = 6
                    },
                    new FormModuleConnection
                    {
                        Id = 16,
                        FormId = 3,
                        QuestionModuleId = 11,
                        NumberInSequence = 7
                    }
                    );
        }

    }
}