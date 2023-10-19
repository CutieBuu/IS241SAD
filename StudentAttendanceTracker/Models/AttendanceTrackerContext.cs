//C# and Razor code written by Zaid Abuisba
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public virtual DbSet<Instructor> Instructors { get; set; }

    /// <summary>
    /// List of student objects generated from the database rows
    /// </summary>
    public virtual DbSet<Student> Students { get; set; }

    /// <summary>
    /// Populates database with seed data on creation of database.
    /// </summary>
    /// 
    public virtual DbSet<AccessCode> AccessCodes { get; set; }


    public virtual DbSet<Attendance> AttendanceLogs { get; set; }

    public virtual DbSet<Administrator> Admins { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>().HasData(new Course { CourseId = 1, CourseName = "Introduction To C++ Programming", CourseTime = new DateTime(0001,1,1,9,30,0)},
            new Course { CourseId = 2, CourseName = "Data Structures", CourseTime = new DateTime(0001,1,1,9,30,0) },
            new Course { CourseId = 3, CourseName = "Theory of Computer Science", CourseTime = new DateTime(0001,1,1,9,30,0) },
            new Course { CourseId = 4, CourseName = "Algorithms", CourseTime = new DateTime(0001,1,1,9,30,0) },
            new Course { CourseId = 5, CourseName = "Web Scripting Technologies", CourseTime = new DateTime(0001,1,1,9,30,0) },
            new Course { CourseId = 6, CourseName = "Database Management", CourseTime = new DateTime(0001,1,1,9,30,0) },
            new Course { CourseId = 7, CourseName = "Systems Analysis and Design", CourseTime = new DateTime(0001,1,1,15,50,0) },
            new Course { CourseId = 8, CourseName = "C# Programming", CourseTime = new DateTime(0001,1,1,9,30,0) },
            new Course { CourseId = 9, CourseName = "Web Publishing" , CourseTime = new DateTime(0001,1,1,9,30,0) }) ;


    }

}
