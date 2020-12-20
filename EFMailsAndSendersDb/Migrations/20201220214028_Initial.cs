using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFMailsAndSendersDb.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Senders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Senders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: false),
                    Port = table.Column<int>(nullable: false),
                    UseSSL = table.Column<bool>(nullable: false),
                    Login = table.Column<string>(maxLength: 200, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Subject = table.Column<string>(maxLength: 255, nullable: false),
                    Body = table.Column<string>(maxLength: 500, nullable: true),
                    IsBodyHtml = table.Column<bool>(nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Recipients_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Recipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Senders_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Senders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SenderTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    ServerId = table.Column<int>(nullable: false),
                    TaskDate = table.Column<DateTime>(nullable: false),
                    SendDate = table.Column<DateTime>(nullable: true),
                    Attempt = table.Column<int>(nullable: false),
                    MessageId = table.Column<int>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    IsSendEnd = table.Column<bool>(nullable: false),
                    IsSuccessful = table.Column<bool>(nullable: false),
                    Error = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenderTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SenderTasks_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SenderTasks_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_SenderTasks_MessageId",
                table: "SenderTasks",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SenderTasks_ServerId",
                table: "SenderTasks",
                column: "ServerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SenderTasks");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "Recipients");

            migrationBuilder.DropTable(
                name: "Senders");
        }
    }
}
