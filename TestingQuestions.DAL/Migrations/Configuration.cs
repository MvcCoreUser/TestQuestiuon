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
                    Description="������ ������� ��������� �������",
                    Answer1="������",
                    Answer2="��������",
                    Answer3="�����",
                    RightAnswerNum=1
                },
                new Question()
                {
                    Description="����� 27 � �������� ������� ����������",
                    Answer1="111000",
                    Answer2="101010",
                    Answer3="11011",
                    RightAnswerNum=3
                },
                new Question()
                {
                    Description="��������� ���������� ����� �� �����",
                    Answer1="7 ����.",
                    Answer2="10 ����.",
                    Answer3="5����.",
                    RightAnswerNum=1
                },
                new Question()
                {
                    Description="��� ������� ������� � ���� �������",
                    Answer1="���������",
                    Answer2="������",
                    Answer3="��������",
                    RightAnswerNum=2
                },
                new Question()
                {
                    Description="������� ������ � ����?",
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
