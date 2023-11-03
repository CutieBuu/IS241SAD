using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentAttendanceTracker.Migrations
{
    /// <inheritdoc />
    public partial class attendanceupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseTime",
                table: "Courses",
                newName: "CourseStartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "CourseEndTime",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Tardy",
                table: "AttendanceLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 1,
                column: "CourseEndTime",
                value: new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 2,
                column: "CourseEndTime",
                value: new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 3,
                column: "CourseEndTime",
                value: new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 4,
                column: "CourseEndTime",
                value: new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 5,
                column: "CourseEndTime",
                value: new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 6,
                column: "CourseEndTime",
                value: new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 7,
                column: "CourseEndTime",
                value: new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 8,
                column: "CourseEndTime",
                value: new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 9,
                column: "CourseEndTime",
                value: new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseEndTime",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Tardy",
                table: "AttendanceLogs");

            migrationBuilder.RenameColumn(
                name: "CourseStartTime",
                table: "Courses",
                newName: "CourseTime");
        }
    }
}
