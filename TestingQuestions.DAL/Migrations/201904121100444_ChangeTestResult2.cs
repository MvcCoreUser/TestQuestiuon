namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTestResult2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestResults", "StartedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.TestResults", "FinishedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.TestResults", "Started");
            DropColumn("dbo.TestResults", "Finished");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TestResults", "Finished", c => c.DateTime(nullable: false));
            AddColumn("dbo.TestResults", "Started", c => c.DateTime(nullable: false));
            DropColumn("dbo.TestResults", "FinishedAt");
            DropColumn("dbo.TestResults", "StartedAt");
        }
    }
}
