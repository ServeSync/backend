﻿namespace ServeSync.Application.MailSender.Interfaces;

public interface IEmailTemplateGenerator
{
    string GetForForgetPassword(string callBackUrl);

    string GetCancelEvent(string eventName);
    
    string GetApproveEventRegistration(string studentName, string eventName, string eventRole, DateTime eventDate, string eventAddress);
    
    string GetRejectEventRegistration(string studentName, string eventName, string eventRole, string rejectReason);

    string GetGrantAccountToEventOrganizer(string name, string email, string username, string password);

    string GetApproveCollaborationRequest(string name, string eventName);
    
    string GetRejectCollaborationRequest(string name, string eventName);

    string GetRejectEvent(string name, string eventName);

    string GetOrganizationInvitation(string name, string approveUrlCallBack, string rejectUrlCallBack);
    
    string GetOrganizationContactInvitation(string name, string organizationName, string approveUrlCallBack, string rejectUrlCallBack);
    
    string GetRejectProof(string studentName, string proofName, string rejectReason);
}