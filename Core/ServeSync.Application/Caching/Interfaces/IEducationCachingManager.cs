﻿using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;

namespace ServeSync.Application.Caching.Interfaces;

public interface IEducationCachingManager
{
    Task SetAsync(IList<EducationProgramDto> educationProgram);
    
    Task<IList<EducationProgramDto>?> GetAsync();

    Task<EducationProgramDto?> GetByIdAsync(Guid id);

    Task<IList<EducationProgramDto>> GetOrAddAsync();
}