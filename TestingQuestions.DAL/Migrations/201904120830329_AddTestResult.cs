namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTestResult : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Started = c.DateTime(nullable: false),
                        Finished = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PersonQuestionAnswers", "TestResultId", c => c.Int(nullable: false));
            CreateIndex("dbo.PersonQuestionAnswers", "TestResultId");
            AddForeignKey("dbo.PersonQuestionAnswers", "TestResultId", "dbo.TestResults", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonQuestionAnswers", "TestResultId", "dbo.TestResults");
            DropIndex("dbo.PersonQuestionAnswers", new[] { "TestResultId" });
            DropColumn("dbo.PersonQuestionAnswers", "TestResultId");
            DropTable("dbo.TestResults");
        }
    }
}
