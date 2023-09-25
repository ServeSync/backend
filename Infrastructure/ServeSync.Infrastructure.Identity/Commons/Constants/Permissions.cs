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