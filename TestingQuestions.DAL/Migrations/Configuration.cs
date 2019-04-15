namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            List<Question> questions = new List<Question>()
            {
                new Question()
                {
                    Description="Вторая планета Солнечной системы",
                    Answer1="Венера",
                    Answer2="Меркурий",
                    Answer3="Земля",
                    RightAnswerNum=1
                },
                new Question()
                {
                    Description="Число 27 в двоичной системе исчисления",
                    Answer1="111000",
                    Answer2="101010",
                    Answer3="11011",
                    RightAnswerNum=3
                },
                new Question()
                {
                    Description="Примерное количество людей на Земле",
                    Answer1="7 млрд.",
                    Answer2="10 млрд.",
                    Answer3="5млрд.",
                    RightAnswerNum=1
                },
                new Question()
                {
                    Description="Кто написал «Сказка о царе Салтане",
                    Answer1="Лермонтов",
                    Answer2="Пушкин",
                    Answer3="Некрасов",
                    RightAnswerNum=2
                },
                new Question()
                {
                    Description="Сколько граней у куба?",
                    Answer1="6",
                    Answer2="8",
                    Answer3="12",
                    RightAnswerNum=2
                },
            };
            questions.ForEach(q =>  context.Questions.AddOrUpdate(q));
            context.SaveChanges();
        }
    }
}
