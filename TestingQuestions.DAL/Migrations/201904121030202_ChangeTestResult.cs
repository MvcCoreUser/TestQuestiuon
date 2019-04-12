namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTestResult : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PersonQuestionAnswers", "PersonId", "dbo.People");
            DropIndex("dbo.PersonQuestionAnswers", new[] { "PersonId", "QuestionId" });
            DropIndex("dbo.PersonQuestionAnswers", new[] { "TestResultId" });
            AddColumn("dbo.TestResults", "PersonId", c => c.Int(nullable: false));
            CreateIndex("dbo.PersonQuestionAnswers", new[] { "TestResultId", "QuestionId" }, unique: true);
            CreateIndex("dbo.TestResults", "PersonId");
            AddForeignKey("dbo.TestResults", "PersonId", "dbo.People", "Id", cascadeDelete: true);
            DropColumn("dbo.PersonQuestionAnswers", "PersonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PersonQuestionAnswers", "PersonId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TestResults", "PersonId", "dbo.People");
            DropIndex("dbo.TestResults", new[] { "PersonId" });
            DropIndex("dbo.PersonQuestionAnswers", new[] { "TestResultId", "QuestionId" });
            DropColumn("dbo.TestResults", "PersonId");
            CreateIndex("dbo.PersonQuestionAnswers", "TestResultId");
            CreateIndex("dbo.PersonQuestionAnswers", new[] { "PersonId", "QuestionId" }, unique: true);
            AddForeignKey("dbo.PersonQuestionAnswers", "PersonId", "dbo.People", "Id", cascadeDelete: true);
        }
    }
}
