//C# and Razor code written by Zaid Abuisba
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentAttendanceTracker.Models;

public partial class AttendanceTrackerContext : IdentityDbContext<User>
{
   

    public AttendanceTrackerContext(DbContextOptions<AttendanceTrackerContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Settings for configuring the database. uses Microsoft Sql Server provider
    /// </summary>
    /// <param name="optionsBuilder"></param>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;Database=AttendanceTracker;Trusted_Connection=True;MultipleActiveResultSets=true;");
	}

    /// <summary>
    /// List of assistance objects generated from the database rows
    /// </summary>
	public virtual DbSet<Assistance> Assistances { get; set; }

    /// <summary>
    /// List of course objects generated from the database rows
    /// </summary>
    public virtual DbSet<Course> Courses { get; set; }

    /// <summary>
    /// List of qualified staff objects generated from the database rows
    /// </summary>
    public virtual DbSet<QualifiedStaff> QualifiedStaff { get; set; }

    /// <summary>
    /// List of qualified staff objects generated from the database rows
    /// </summary>
    public virtual DbSet<Professor> Professors { get; set; }

    /// <summary>
    /// List of student objects generated from the database rows
    /// </summary>
    public virtual DbSet<Student> Students { get; set; }

    /// <summary>
    /// Populates database with seed data on creation of database.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>().HasData(new Course { CourseId = 1, CourseName = "Introduction To C++ Programming"},
            new Course { CourseId = 2, CourseName = "Data Structures"},
            new Course { CourseId = 3, CourseName = "Theory of Computer Science" },
            new Course { CourseId = 4, CourseName = "Algorithms" },
            new Course { CourseId = 5, CourseName = "Web Scripting Technologies" },
            new Course { CourseId = 6, CourseName = "Database Management" },
            new Course { CourseId = 7, CourseName = "Systems Analysis and Design" },
            new Course { CourseId = 8, CourseName = "C# Programming" },
            new Course { CourseId = 9, CourseName = "Web Publishing" }) ;


    }

}
