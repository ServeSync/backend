using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public static class Permissions
{
    public const string Default = "ServeSync.Permissions";

    public static List<ApplicationPermission> Provider => new()
    {
        new(Roles.Management, "Quản lý roles."),
        new(Roles.Create, "Tạo role."),
        new(Roles.View, "Xem role."),
        new(Roles.Edit, "Chỉnh sửa roles."),
        new(Roles.Delete, "Xóa roles."),
        new(Roles.ViewPermission, "Xem quyền của role."),
        new(Roles.UpdatePermission, "Cập nhật quyền cho role."),

        new(PermissionManagement.View, "Xem danh sách quyền."),
        
        new(Users.ViewPermissions, "Xem quyền của người dùng."),
        new(Users.ViewProfile, "Xem thông tin cá nhân."),
        new(Users.View, "Xem danh sách người dùng."),

        new(Students.Management, "Quản lý sinh viên."),
        new(Students.View, "Xem danh sách sinh viên."),
        new(Students.Delete, "Xóa sinh viên."),
        new(Students.Create, "Tạo sinh viên."),
        new(Students.Edit, "Chỉnh sửa sinh viên."),
        new(Students.ViewProfile, "Xem thông tin cá nhân của sinh viên."),
        new(Students.EditProfile, "Chỉnh sửa thông tin cá nhân của sinh viên."),
        new(Students.Import, "Nhập danh sách sinh viên."),
        new(Students.Export, "Xuất thông tin sinh viên."),
        
        new(Events.Management, "Quản lý sự kiện."),
        new(Events.View, "Xem danh sách sự kiện."),
        new(Events.Delete, "Xóa sự kiện."),
        new(Events.Create, "Tạo sự kiện."),
        new(Events.Edit, "Chỉnh sửa sự kiện."),
        new(Events.Approve, "Duyệt sự kiện."),
        new(Events.Cancel, "Hủy sự kiện."),
        new(Events.Reject, "Từ chối sự kiện."),
        new(Events.ApproveRegistration, "Duyệt đơn đăng ký tham gia."),
        new(Events.RejectRegistration, "Từ chối đơn đăng ký tham gia."),
        
        new(EventCollaborationRequests.Management, "Quản lý đề nghị hợp tác."),
        new(EventCollaborationRequests.View, "Xem thông tin đề nghị hợp tác."),
        new(EventCollaborationRequests.Approve, "Duyệt đề nghị hợp tác."),
        new(EventCollaborationRequests.Reject, "Từ chối đề nghị hợp tác."),
        
        new(EventOrganizations.Management, "Quản lý nhà tổ chức sự kiện."),
        new (EventOrganizations.View, "Xem danh sách nhà tổ chức sự kiện."),
        new(EventOrganizations.Create, "Tạo nhà tổ chức sự kiện."),
        new(EventOrganizations.Update, "Cập nhật nhà tổ chức sự kiện."),
        new(EventOrganizations.Delete, "Xóa nhà tổ chức sự kiện."),
        new(EventOrganizations.AddContact, "Thêm liên hệ cho nhà tổ chức sự kiện."),
        new(EventOrganizations.ViewContact, "Xem liên hệ của nhà tổ chức sự kiện."),
        new(EventOrganizations.RemoveContact, "Xóa liên hệ của nhà tổ chức sự kiện."),
        new(EventOrganizations.UpdateContact, "Cập nhật liên hệ của nhà tổ chức sự kiện."),
        
        new(Proofs.Management, "Quản lý minh chứng."),
        new(Proofs.View, "Xem danh sách minh chứng."),
        new(Proofs.Create, "Tạo minh chứng."),
        new(Proofs.Update, "Cập nhật minh chứng."),
        new(Proofs.Delete, "Xóa minh chứng."),
        new(Proofs.Approve, "Duyệt minh chứng."),
        new(Proofs.Reject, "Từ chối minh chứng."),
    };

    public static class Roles
    {
        public const string Group = $"{Default}.Roles";
        
        public const string Management = $"{Group}.Management";

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
        public const string View = $"{Group}.View";
    }
    
    public static class Students
    {
        public const string Group = $"{Default}.Students";

        public const string Management = $"{Group}.Management";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Delete = $"{Group}.Delete";
        public const string Edit = $"{Group}.Edit";
        
        public const string ViewProfile = $"{Group}.ViewProfile";
        public const string EditProfile = $"{Group}.EditProfile";
        
        public const string Import = $"{Group}.Import";
        public const string Export = $"{Group}.Export";
    }
    
    public static class Events
    {
        public const string Group = $"{Default}.Events";
        
        public const string Management = $"{Group}.Management";
        
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
        
        public const string Management = $"{Group}.Management";
        public const string View = $"{Group}.View";
        public const string Approve = $"{Group}.Approve";
        public const string Reject = $"{Group}.Reject";
    }

    public static class EventOrganizations
    {
        public const string Group = $"{Default}.EventOrganizations";
        
        public const string Management = $"{Group}.Management";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Update = $"{Group}.Update";
        public const string Delete = $"{Group}.Delete";
        
        public const string AddContact = $"{Group}.AddContact";
        public const string ViewContact = $"{Group}.ViewContact";
        public const string RemoveContact = $"{Group}.RemoveContact";
        public const string UpdateContact = $"{Group}.UpdateContact";
    }

    public static class Proofs
    {
        public const string Group = $"{Default}.Proofs";
        
        public const string Management = $"{Group}.Management";
        
        public const string View = $"{Group}.View";
        public const string Create = $"{Group}.Create";
        public const string Update = $"{Group}.Update";
        public const string Delete = $"{Group}.Delete";
        
        public const string Approve = $"{Group}.Approve";
        public const string Reject = $"{Group}.Reject";
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