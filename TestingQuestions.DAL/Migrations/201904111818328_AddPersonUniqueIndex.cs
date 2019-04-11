namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonUniqueIndex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "Name", c => c.String(maxLength: 128));
            CreateIndex("dbo.People", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.People", new[] { "Name" });
            AlterColumn("dbo.People", "Name", c => c.String());
        }
    }
}
