namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStringLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "Description", c => c.String(maxLength: 256));
            AlterColumn("dbo.Questions", "Answer1", c => c.String(maxLength: 50));
            AlterColumn("dbo.Questions", "Answer2", c => c.String(maxLength: 50));
            AlterColumn("dbo.Questions", "Answer3", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "Answer3", c => c.String());
            AlterColumn("dbo.Questions", "Answer2", c => c.String());
            AlterColumn("dbo.Questions", "Answer1", c => c.String());
            AlterColumn("dbo.Questions", "Description", c => c.String());
        }
    }
}
