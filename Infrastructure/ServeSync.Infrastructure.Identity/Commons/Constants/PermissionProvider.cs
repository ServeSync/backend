using ServeSync.Application.Common;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Commons.Constants;

public class PermissionProvider
{
    public static List<ApplicationPermission> Provider => new()
    {
        new(AppPermissions.Roles.Management, "Quản lý roles."),
        new(AppPermissions.Roles.Create, "Tạo role."),
        new(AppPermissions.Roles.View, "Xem role."),
        new(AppPermissions.Roles.Edit, "Chỉnh sửa roles."),
        new(AppPermissions.Roles.Delete, "Xóa roles."),
        new(AppPermissions.Roles.ViewPermission, "Xem quyền của role."),
        new(AppPermissions.Roles.UpdatePermission, "Cập nhật quyền cho role."),

        new(AppPermissions.PermissionManagement.View, "Xem danh sách quyền."),
        
        new(AppPermissions.Users.Management, "Quản lý người dùng."),
        new(AppPermissions.Users.ViewPermissions, "Xem quyền của người dùng."),
        new(AppPermissions.Users.ViewProfile, "Xem thông tin cá nhân."),
        new(AppPermissions.Users.View, "Xem danh sách người dùng."),
        new(AppPermissions.Users.ViewRoles, "Xem vai trò người dùng."),
        new(AppPermissions.Users.EditRoles, "Cập nhật vai trò người dùng."),

        new(AppPermissions.Students.Management, "Quản lý sinh viên."),
        new(AppPermissions.Students.View, "Xem danh sách sinh viên."),
        new(AppPermissions.Students.Delete, "Xóa sinh viên."),
        new(AppPermissions.Students.Create, "Tạo sinh viên."),
        new(AppPermissions.Students.Edit, "Chỉnh sửa sinh viên."),
        new(AppPermissions.Students.ViewProfile, "Xem thông tin cá nhân của sinh viên."),
        new(AppPermissions.Students.EditProfile, "Chỉnh sửa thông tin cá nhân của sinh viên."),
        new(AppPermissions.Students.Import, "Nhập danh sách sinh viên."),
        new(AppPermissions.Students.Export, "Xuất thông tin sinh viên."),
        
        new(AppPermissions.Events.Management, "Quản lý sự kiện."),
        new(AppPermissions.Events.View, "Xem danh sách sự kiện."),
        new(AppPermissions.Events.Delete, "Xóa sự kiện."),
        new(AppPermissions.Events.Create, "Tạo sự kiện."),
        new(AppPermissions.Events.Edit, "Chỉnh sửa sự kiện."),
        new(AppPermissions.Events.Approve, "Duyệt sự kiện."),
        new(AppPermissions.Events.Cancel, "Hủy sự kiện."),
        new(AppPermissions.Events.Reject, "Từ chối sự kiện."),
        new(AppPermissions.Events.ApproveRegistration, "Duyệt đơn đăng ký tham gia."),
        new(AppPermissions.Events.RejectRegistration, "Từ chối đơn đăng ký tham gia."),
        
        new(AppPermissions.EventCollaborationRequests.Management, "Quản lý đề nghị hợp tác."),
        new(AppPermissions.EventCollaborationRequests.View, "Xem thông tin đề nghị hợp tác."),
        new(AppPermissions.EventCollaborationRequests.Approve, "Duyệt đề nghị hợp tác."),
        new(AppPermissions.EventCollaborationRequests.Reject, "Từ chối đề nghị hợp tác."),
        
        new(AppPermissions.EventOrganizations.Management, "Quản lý nhà tổ chức sự kiện."),
        new (AppPermissions.EventOrganizations.View, "Xem danh sách nhà tổ chức sự kiện."),
        new(AppPermissions.EventOrganizations.Create, "Tạo nhà tổ chức sự kiện."),
        new(AppPermissions.EventOrganizations.Update, "Cập nhật nhà tổ chức sự kiện."),
        new(AppPermissions.EventOrganizations.Delete, "Xóa nhà tổ chức sự kiện."),
        new(AppPermissions.EventOrganizations.AddContact, "Thêm liên hệ cho nhà tổ chức sự kiện."),
        new(AppPermissions.EventOrganizations.ViewContact, "Xem liên hệ của nhà tổ chức sự kiện."),
        new(AppPermissions.EventOrganizations.RemoveContact, "Xóa liên hệ của nhà tổ chức sự kiện."),
        new(AppPermissions.EventOrganizations.UpdateContact, "Cập nhật liên hệ của nhà tổ chức sự kiện."),
        
        new(AppPermissions.Proofs.Management, "Quản lý minh chứng."),
        new(AppPermissions.Proofs.View, "Xem danh sách minh chứng."),
        new(AppPermissions.Proofs.Create, "Tạo minh chứng."),
        new(AppPermissions.Proofs.Update, "Cập nhật minh chứng."),
        new(AppPermissions.Proofs.Delete, "Xóa minh chứng."),
        new(AppPermissions.Proofs.Approve, "Duyệt minh chứng."),
        new(AppPermissions.Proofs.Reject, "Từ chối minh chứng."),
    };
}