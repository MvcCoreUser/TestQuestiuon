namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonAnswerIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PersonQuestionAnswers", new[] { "PersonId" });
            DropIndex("dbo.PersonQuestionAnswers", new[] { "QuestionId" });
            CreateIndex("dbo.PersonQuestionAnswers", new[] { "PersonId", "QuestionId" }, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.PersonQuestionAnswers", new[] { "PersonId", "QuestionId" });
            CreateIndex("dbo.PersonQuestionAnswers", "QuestionId");
            CreateIndex("dbo.PersonQuestionAnswers", "PersonId");
        }
    }
}
