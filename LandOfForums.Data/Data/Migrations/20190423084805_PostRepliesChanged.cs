using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LandOfForums.Data.Migrations
{
    public partial class PostRepliesChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostsReplies_Forums_ForumId",
                table: "PostsReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsReplies_Posts_PostId",
                table: "PostsReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_PostsReplies_AspNetUsers_UserId",
                table: "PostsReplies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostsReplies",
                table: "PostsReplies");

            migrationBuilder.RenameTable(
                name: "PostsReplies",
                newName: "PostReplies");

            migrationBuilder.RenameIndex(
                name: "IX_PostsReplies_UserId",
                table: "PostReplies",
                newName: "IX_PostReplies_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostsReplies_PostId",
                table: "PostReplies",
                newName: "IX_PostReplies_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostsReplies_ForumId",
                table: "PostReplies",
                newName: "IX_PostReplies_ForumId");

            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Forums",
                newName: "ImageUrl");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReplies",
                table: "PostReplies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReplies_Forums_ForumId",
                table: "PostReplies",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReplies_Posts_PostId",
                table: "PostReplies",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReplies_AspNetUsers_UserId",
                table: "PostReplies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReplies_Forums_ForumId",
                table: "PostReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReplies_Posts_PostId",
                table: "PostReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReplies_AspNetUsers_UserId",
                table: "PostReplies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReplies",
                table: "PostReplies");

            migrationBuilder.RenameTable(
                name: "PostReplies",
                newName: "PostsReplies");

            migrationBuilder.RenameIndex(
                name: "IX_PostReplies_UserId",
                table: "PostsReplies",
                newName: "IX_PostsReplies_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostReplies_PostId",
                table: "PostsReplies",
                newName: "IX_PostsReplies_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostReplies_ForumId",
                table: "PostsReplies",
                newName: "IX_PostsReplies_ForumId");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Forums",
                newName: "ImageURL");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostsReplies",
                table: "PostsReplies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostsReplies_Forums_ForumId",
                table: "PostsReplies",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsReplies_Posts_PostId",
                table: "PostsReplies",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsReplies_AspNetUsers_UserId",
                table: "PostsReplies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
