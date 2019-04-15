namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TestResults", "StartedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.TestResults", "FinishedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TestResults", "FinishedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TestResults", "StartedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
    }
}
