using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class Permissions
{
    public const string Default = "ServeSync.Permissions";

    public static List<ApplicationPermission> Provider => new()
    {
        new(Roles.Create, "Tạo role."),
        new(Roles.View, "Xem role."),
        new(Roles.Edit, "Chỉnh sửa roles."),
        new(Roles.Delete, "Xóa roles."),
        new(Roles.ViewPermission, "Xem quyền của role."),
        new(Roles.UpdatePermission, "Cập nhật quyền cho role."),

        new(PermissionManagement.View, "Xem danh sách quyền."),
        
        new(Users.ViewPermissions, "Xem quyền của người dùng."),
        new(Users.ViewProfile, "Xem thông tin cá nhân."),
        
        new(Faculties.View, "Xem danh sách khoa."),
        new(HomeRooms.View, "Xem danh sách lớp sinh hoạt."),
        new(EducationPrograms.View, "Xem danh sách chương trình học."),
        
        new(Students.View, "Xem danh sách sinh viên."),
        new(Students.Delete, "Xóa sinh viên."),
        new(Students.Create, "Tạo sinh viên."),
        new(Students.Edit, "Chỉnh sửa sinh viên."),
        new(Students.ViewProfile, "Xem thông tin cá nhân của sinh viên."),
        new(Students.EditProfile, "Chỉnh sửa thông tin cá nhân của sinh viên."),
        
        new(Events.View, "Xem danh sách sự kiện."),
        new(Events.Delete, "Xóa sự kiện."),
        new(Events.Create, "Tạo sự kiện."),
        new(Events.Edit, "Chỉnh sửa sự kiện."),
        new(Events.Approve, "Duyệt sự kiện."),
        new(Events.Cancel, "Hủy sự kiện."),
        new(Events.ApproveRegistration, "Duyệt đơn đăng ký tham gia."),
        new(Events.RejectRegistration, "Từ chối đơn đăng ký tham gia."),
        
        new(EventCollaborationRequests.View, "Xem thông tin đề nghị hợp tác."),
        new(EventCollaborationRequests.Approve, "Duyệt đề nghị hợp tác."),
        new(EventCollaborationRequests.Reject, "Từ chối đề nghị hợp tác."),
        
        new (EventOrganizations.View, "Xem danh sách nhà tổ chức sự kiện."),
        new(EventOrganizations.Create, "Tạo nhà tổ chức sự kiện."),
        new(EventOrganizations.Update, "Cập nhật nhà tổ chức sự kiện."),
        new(EventOrganizations.Delete, "Xóa nhà tổ chức sự kiện."),
    };

    public static class Roles
    {
        public const string Group = $"{Default}.Roles";

        public const string View = $"{Group}.View";
        public const string Edit = $"{Group}.Edit";
        public const string Create = $"{Group}.Create";
        public const string Delete = $"{Group}.Delete";

        public const string ViewPermission = $"{Group}.ViewPermissions";
        public const string UpdatePermission = $"{Group}.UpdatePermissions";
    }

    public static class PermissionManagement
    {
        public const string Group = $"{Default}.PermissionManagement";

        public const string View = $"{Group}.View";
    }

    public static class Users
    {
        public const string Group = $"{Default}.PermissionManagement";

        public const string ViewProfile = $"{Group}.ViewProfile";
        public const string ViewPermissions = $"{Group}.ViewPermissions";
    }

    public static class Faculties
    {
        public const string Group = $"{Default}.Faculties";
        
        public const string View = $"{Group}.View";
    }
    
    public static class EducationPrograms
    {
        public const string Group = $"{Default}.EducationPrograms";
        
        public const string View = $"{Group}.View";
    }
    
    public static class HomeRooms
    {
        public const string Group = $"{Default}.HomeRooms";
        
        public const string View = $"{Group}.View";
    }
    
    public static class Students
    {
        public const string Group = $"{Default}.Students";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Delete = $"{Group}.Delete";
        public const string Edit = $"{Group}.Edit";
        
        public const string ViewProfile = $"{Group}.ViewProfile";
        public const string EditProfile = $"{Group}.EditProfile";
    }
    
    public static class Events
    {
        public const string Group = $"{Default}.Events";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Delete = $"{Group}.Delete";
        public const string Edit = $"{Group}.Edit";
        
        public const string Approve = $"{Group}.Approve";
        public const string Cancel = $"{Group}.Cancel";
        public const string Reject = $"{Group}.Reject";

        public const string ApproveRegistration = $"{Group}.ApproveRegistration";
        public const string RejectRegistration = $"{Group}.RejectRegistration";
    }

    public static class EventCollaborationRequests
    {
        public const string Group = $"{Default}.EventCollaborationRequests";
        
        public const string View = $"{Group}.View";
        public const string Approve = $"{Group}.Approve";
        public const string Reject = $"{Group}.Reject";
    }

    public static class EventOrganizations
    {
        public const string Group = $"{Default}.EventOrganizations";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Update = $"{Group}.Update";
        public const string Delete = $"{Group}.Delete";
    }

    public static List<string> GeneratePermissionsForModule(string module)
    {
        return new List<string>()
        {
            $"{Default}.{module}.Create",
            $"{Default}.{module}.View",
            $"{Default}.{module}.Edit",
            $"{Default}.{module}.Delete",
        };
    }
}