using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPLV2.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Inits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MfAreaId = table.Column<int>(type: "int", nullable: false),
                    entitytype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    displaylabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phaseid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    priority_c = table.Column<int>(type: "int", nullable: false),
                    category_c = table.Column<int>(type: "int", nullable: false),
                    Category_c_DisplayLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subcategory_c_DisplayLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subcategory_c = table.Column<int>(type: "int", nullable: false),
                    impmcategory_c = table.Column<bool>(type: "bit", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MfCatId = table.Column<int>(type: "int", nullable: false),
                    entitytype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    displaylabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phaseid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category_c = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    impmcategory_c = table.Column<bool>(type: "bit", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MfDeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    entity_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SubType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateTime = table.Column<long>(type: "bigint", nullable: false),
                    DisplayLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MacAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentVersion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceRunningLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShutDownTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceRunningLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SystemId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLogs",
                columns: table => new
                {
                    PKExceptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExceptionSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogs", x => x.PKExceptionId);
                });

            migrationBuilder.CreateTable(
                name: "MachineRegistration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    systemid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mac_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ipaddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    agent_version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    install_ram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hard_disk = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineRegistration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MfpersonId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    entity_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVIP = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Upn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateTime = table.Column<long>(type: "bigint", nullable: false),
                    Location = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MfAreaId = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StarRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ONGCCAT_c = table.Column<int>(type: "int", nullable: false),
                    ONGCSUB_c = table.Column<int>(type: "int", nullable: false),
                    ONGCAREA_c = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    starcount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarRating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MfSubCatId = table.Column<int>(type: "int", nullable: false),
                    entitytype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    displaylabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category_c_DisplayLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phaseid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category_c = table.Column<int>(type: "int", nullable: false),
                    impmcategory_c = table.Column<bool>(type: "bit", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceDeskGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredForActualService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedByPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredForDevice_c = table.Column<int>(type: "int", nullable: true),
                    ONGCCAT_c = table.Column<int>(type: "int", nullable: true),
                    ONGCSUB_c = table.Column<int>(type: "int", nullable: true),
                    ONGCAREA_c = table.Column<int>(type: "int", nullable: true),
                    ContactPerson = table.Column<int>(type: "int", nullable: true),
                    DisplayLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketGenerated = table.Column<bool>(type: "bit", nullable: true),
                    TicketId = table.Column<int>(type: "int", nullable: true),
                    TicketStatus = table.Column<int>(type: "int", nullable: true),
                    FeedbackCount = table.Column<int>(type: "int", nullable: true),
                    FeedBackReceived = table.Column<bool>(type: "bit", nullable: true),
                    FeedBackReceivedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextFeedBackSchedule = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SystemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedBackRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Completion_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    starcount = table.Column<int>(type: "int", nullable: false),
                    CurrentStatus_c = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpertAssignee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpertAssigneeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedByPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredForLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    close_count = table.Column<int>(type: "int", nullable: false),
                    feedback_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    feedback_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    feedback_action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    feedback_rating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userSystemHardwares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocal = table.Column<bool>(type: "bit", nullable: false),
                    IsArray = table.Column<bool>(type: "bit", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualifires = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userSystemHardwares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSystemSoftware",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstalledOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    SystemId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSystemSoftware", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DeviceDetails");

            migrationBuilder.DropTable(
                name: "DeviceRunningLog");

            migrationBuilder.DropTable(
                name: "EventHistory");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropTable(
                name: "MachineRegistration");

            migrationBuilder.DropTable(
                name: "PersonDetails");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "StarRating");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "TicketRecords");

            migrationBuilder.DropTable(
                name: "userSystemHardwares");

            migrationBuilder.DropTable(
                name: "UserSystemSoftware");
        }
    }
}
