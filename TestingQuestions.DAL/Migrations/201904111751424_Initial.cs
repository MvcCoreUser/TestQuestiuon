namespace TestingQuestions.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonQuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        AnswerNum = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Answer1 = c.String(),
                        Answer2 = c.String(),
                        Answer3 = c.String(),
                        RightAnswerNum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonQuestionAnswers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.PersonQuestionAnswers", "PersonId", "dbo.People");
            DropIndex("dbo.PersonQuestionAnswers", new[] { "QuestionId" });
            DropIndex("dbo.PersonQuestionAnswers", new[] { "PersonId" });
            DropTable("dbo.Questions");
            DropTable("dbo.People");
            DropTable("dbo.PersonQuestionAnswers");
        }
    }
}
