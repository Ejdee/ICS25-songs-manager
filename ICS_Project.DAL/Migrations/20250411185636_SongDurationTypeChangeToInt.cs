using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICS_Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SongDurationTypeChangeToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DurationInSeconds",
                table: "Songs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "DurationInSeconds",
                table: "Songs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
