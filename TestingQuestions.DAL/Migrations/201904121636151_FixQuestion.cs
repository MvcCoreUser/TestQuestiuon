namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixQuestion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TestQuestionAnswers", "AnswerNum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TestQuestionAnswers", "AnswerNum", c => c.Int());
        }
    }
}
