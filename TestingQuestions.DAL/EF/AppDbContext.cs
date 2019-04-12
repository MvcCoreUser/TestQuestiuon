﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.DAL.Entities;

namespace TestingQuestions
{
    public class AppDbContext: DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(string connectionName): base(connectionName)
        {

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestQuestionAnswer> PersonQuestionAnswers { get; set; }
        public DbSet<TestResult> TestResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<TestQuestionAnswer>().HasIndex(p => new { p.TestResultId, p.QuestionId }).IsUnique();
            base.OnModelCreating(modelBuilder);
        }

    }
}