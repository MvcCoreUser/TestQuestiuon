namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestQuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        AnswerNum = c.Int(nullable: false),
                        TestResultId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.TestResults", t => t.TestResultId, cascadeDelete: true)
                .Index(t => new { t.TestResultId, t.QuestionId }, unique: true);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 256),
                        Answer1 = c.String(maxLength: 50),
                        Answer2 = c.String(maxLength: 50),
                        Answer3 = c.String(maxLength: 50),
                        RightAnswerNum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        FinishedAt = c.DateTime(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestQuestionAnswers", "TestResultId", "dbo.TestResults");
            DropForeignKey("dbo.TestResults", "PersonId", "dbo.People");
            DropForeignKey("dbo.TestQuestionAnswers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.People", new[] { "Name" });
            DropIndex("dbo.TestResults", new[] { "PersonId" });
            DropIndex("dbo.TestQuestionAnswers", new[] { "TestResultId", "QuestionId" });
            DropTable("dbo.People");
            DropTable("dbo.TestResults");
            DropTable("dbo.Questions");
            DropTable("dbo.TestQuestionAnswers");
        }
    }
}
