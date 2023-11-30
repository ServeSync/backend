﻿using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;

public class FilterProofSpecification : PagingAndSortingSpecification<Proof, Guid>
{
    private readonly string? _search;
    
    public FilterProofSpecification(int page, int size, string sorting, string? search) : base(page, size, sorting)
    {
        _search = search;
        
        AddInclude(x => x.Student!);
        AddInclude(x => x.ExternalProof);
        AddInclude(x => x.InternalProof);
        AddInclude(x => x.InternalProof.Event);
        AddInclude(x => x.InternalProof.Event.RepresentativeOrganization.Organization);
    }
    
    public override Expression<Func<Proof, bool>> ToExpression()
    {
        return x => string.IsNullOrWhiteSpace(_search) ||
                    x.ExternalProof.EventName.ToLower().Contains(_search.ToLower()) ||
                    x.InternalProof.Event.Name.ToLower().Contains(_search.ToLower()) ||
                    x.ExternalProof.Address.ToLower().Contains(_search.ToLower()) ||
                    x.InternalProof.Event.Address.FullAddress.ToLower().Contains(_search.ToLower()) ||
                    x.ExternalProof.OrganizationName.ToLower().Contains(_search.ToLower()) ||
                    x.InternalProof.Event.RepresentativeOrganization.Organization.Name.ToLower().Contains(_search.ToLower()) ||
                    x.Student.FullName.ToLower().Contains(_search.ToLower());
    }
}