namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTestRepo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PersonQuestionAnswers", newName: "TestQuestionAnswers");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TestQuestionAnswers", newName: "PersonQuestionAnswers");
        }
    }
}
