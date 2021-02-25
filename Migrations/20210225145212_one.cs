using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechnicalSupport.Migrations
{
    public partial class one : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Statuses_StatusNavigationId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_Employees_EmployeeId",
                table: "Dialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_Users_UserId",
                table: "Dialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Sexes_SexId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WorkTimes_WorkTimeNavigationId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Statuses_Dialogs_DialogId",
                table: "Statuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Employees_GuidemployyNavigationId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Sexes_SexNavigationSexId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SexNavigationSexId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_EmployeeId",
                table: "Dialogs");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_UserId",
                table: "Dialogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkTimes",
                table: "WorkTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_GuidemployyNavigationId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sexes",
                table: "Sexes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RoleId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_WorkTimeNavigationId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SexNavigationSexId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GuidemployyNavigationId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WorkTimeNavigationId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LocalHash",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "WorkTimes",
                newName: "WorkTime");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Task");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "Status");

            migrationBuilder.RenameTable(
                name: "Sexes",
                newName: "Sex");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "Application");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "Guidemployy",
                table: "Task",
                newName: "GUIDEmployy");

            migrationBuilder.RenameColumn(
                name: "Status1",
                table: "Status",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_Statuses_DialogId",
                table: "Status",
                newName: "IX_Status_DialogId");

            migrationBuilder.RenameColumn(
                name: "Sex1",
                table: "Sex",
                newName: "Sex");

            migrationBuilder.RenameColumn(
                name: "SexId",
                table: "Employee",
                newName: "Sex");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_SexId",
                table: "Employee",
                newName: "IX_Employee_Sex");

            migrationBuilder.RenameColumn(
                name: "SexId",
                table: "User",
                newName: "Sex");

            migrationBuilder.RenameColumn(
                name: "StatusNavigationId",
                table: "Application",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_StatusNavigationId",
                table: "Application",
                newName: "IX_Application_ChatId");

            migrationBuilder.AlterDatabase(
                collation: "Cyrillic_General_CI_AS");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<byte[]>(
                name: "LocalHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "Dialogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Dialogs",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TaskCount",
                table: "Task",
                type: "int",
                nullable: false,
                defaultValueSql: "((1))",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Status",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "Sex",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecondName",
                table: "Employee",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employee",
                type: "nvarchar(22)",
                maxLength: 22,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employee",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employee",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employee",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeGUID",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())");

            migrationBuilder.AlterColumn<string>(
                name: "UserIp",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecondName",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "User",
                type: "nvarchar(22)",
                maxLength: 22,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Topic",
                table: "Application",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Application",
                type: "int",
                nullable: false,
                defaultValueSql: "((1))",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkTime",
                table: "WorkTime",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                table: "Status",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sex",
                table: "Sex",
                column: "SexId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Employee_EmployeeGUID",
                table: "Employee",
                column: "EmployeeGUID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Application",
                table: "Application",
                column: "ApplicationId");

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    ChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GUIDEmployee = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GUIDUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationType",
                columns: table => new
                {
                    CommunicationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommunicationType = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationType", x => x.CommunicationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RequestType",
                columns: table => new
                {
                    RequestTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestType = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestType", x => x.RequestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    DetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatingDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Data = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Chat = table.Column<int>(type: "int", nullable: true),
                    RequestType = table.Column<int>(type: "int", nullable: true),
                    CommunicationType = table.Column<int>(type: "int", nullable: true),
                    GUIDInteracting = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Details__BAC8628C5494B5B4", x => x.DetailsId);
                    table.ForeignKey(
                        name: "FK_DetailsChat",
                        column: x => x.Chat,
                        principalTable: "Chat",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetailsCommunicationType",
                        column: x => x.CommunicationType,
                        principalTable: "CommunicationType",
                        principalColumn: "CommunicationTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestType",
                        column: x => x.RequestType,
                        principalTable: "RequestType",
                        principalColumn: "RequestTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_EmployeeId1",
                table: "Dialogs",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_UserId1",
                table: "Dialogs",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Task_GUIDEmployy",
                table: "Task",
                column: "GUIDEmployy");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_WorkTime",
                table: "Employee",
                column: "WorkTime");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeGUID",
                table: "Employee",
                column: "EmployeeGUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Sex",
                table: "User",
                column: "Sex");

            migrationBuilder.CreateIndex(
                name: "IX_Application_Status",
                table: "Application",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Details_Chat",
                table: "Details",
                column: "Chat");

            migrationBuilder.CreateIndex(
                name: "IX_Details_CommunicationType",
                table: "Details",
                column: "CommunicationType");

            migrationBuilder.CreateIndex(
                name: "IX_Details_RequestType",
                table: "Details",
                column: "RequestType");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Chat_ChatId",
                table: "Application",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "ChatId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Status",
                table: "Application",
                column: "Status",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_Employee_EmployeeId1",
                table: "Dialogs",
                column: "EmployeeId1",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_Users_UserId1",
                table: "Dialogs",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSex",
                table: "Employee",
                column: "Sex",
                principalTable: "Sex",
                principalColumn: "SexId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTime",
                table: "Employee",
                column: "WorkTime",
                principalTable: "WorkTime",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Status_Dialogs_DialogId",
                table: "Status",
                column: "DialogId",
                principalTable: "Dialogs",
                principalColumn: "DialogId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_GUIDEmployy",
                table: "Task",
                column: "GUIDEmployy",
                principalTable: "Employee",
                principalColumn: "EmployeeGUID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSex",
                table: "User",
                column: "Sex",
                principalTable: "Sex",
                principalColumn: "SexId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Role_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_Chat_ChatId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Status",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_Employee_EmployeeId1",
                table: "Dialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_Users_UserId1",
                table: "Dialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSex",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkTime",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Status_Dialogs_DialogId",
                table: "Status");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_GUIDEmployy",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSex",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Role_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "CommunicationType");

            migrationBuilder.DropTable(
                name: "RequestType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_EmployeeId1",
                table: "Dialogs");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_UserId1",
                table: "Dialogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkTime",
                table: "WorkTime");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Sex",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_GUIDEmployy",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                table: "Status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sex",
                table: "Sex");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Employee_EmployeeGUID",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_WorkTime",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeGUID",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Application",
                table: "Application");

            migrationBuilder.DropIndex(
                name: "IX_Application_Status",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocalHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeGUID",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "WorkTime",
                newName: "WorkTimes");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Clients");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "Status",
                newName: "Statuses");

            migrationBuilder.RenameTable(
                name: "Sex",
                newName: "Sexes");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "Application",
                newName: "Applications");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Sex",
                table: "Clients",
                newName: "SexId");

            migrationBuilder.RenameColumn(
                name: "GUIDEmployy",
                table: "Tasks",
                newName: "Guidemployy");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Statuses",
                newName: "Status1");

            migrationBuilder.RenameIndex(
                name: "IX_Status_DialogId",
                table: "Statuses",
                newName: "IX_Statuses_DialogId");

            migrationBuilder.RenameColumn(
                name: "Sex",
                table: "Sexes",
                newName: "Sex1");

            migrationBuilder.RenameColumn(
                name: "Sex",
                table: "Employees",
                newName: "SexId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_Sex",
                table: "Employees",
                newName: "IX_Employees_SexId");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Applications",
                newName: "StatusNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_Application_ChatId",
                table: "Applications",
                newName: "IX_Applications_StatusNavigationId");

            migrationBuilder.AlterDatabase(
                oldCollation: "Cyrillic_General_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SexNavigationSexId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserIp",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecondName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(22)",
                oldMaxLength: 22,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "LocalHash",
                table: "Clients",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Clients",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TaskCount",
                table: "Tasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "((1))");

            migrationBuilder.AddColumn<Guid>(
                name: "GuidemployyNavigationId",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status1",
                table: "Statuses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Sex1",
                table: "Sexes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "SecondName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(22)",
                oldMaxLength: 22,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkTimeNavigationId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Topic",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Applications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "((1))");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkTimes",
                table: "WorkTimes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sexes",
                table: "Sexes",
                column: "SexId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "ApplicationId");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DialogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedirectTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Dialogs_DialogId",
                        column: x => x.DialogId,
                        principalTable: "Dialogs",
                        principalColumn: "DialogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SexNavigationSexId",
                table: "Users",
                column: "SexNavigationSexId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_EmployeeId",
                table: "Dialogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_UserId",
                table: "Dialogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_GuidemployyNavigationId",
                table: "Tasks",
                column: "GuidemployyNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkTimeNavigationId",
                table: "Employees",
                column: "WorkTimeNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DialogId",
                table: "Messages",
                column: "DialogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Statuses_StatusNavigationId",
                table: "Applications",
                column: "StatusNavigationId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_Employees_EmployeeId",
                table: "Dialogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_Users_UserId",
                table: "Dialogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Sexes_SexId",
                table: "Employees",
                column: "SexId",
                principalTable: "Sexes",
                principalColumn: "SexId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WorkTimes_WorkTimeNavigationId",
                table: "Employees",
                column: "WorkTimeNavigationId",
                principalTable: "WorkTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statuses_Dialogs_DialogId",
                table: "Statuses",
                column: "DialogId",
                principalTable: "Dialogs",
                principalColumn: "DialogId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Employees_GuidemployyNavigationId",
                table: "Tasks",
                column: "GuidemployyNavigationId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Sexes_SexNavigationSexId",
                table: "Users",
                column: "SexNavigationSexId",
                principalTable: "Sexes",
                principalColumn: "SexId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
