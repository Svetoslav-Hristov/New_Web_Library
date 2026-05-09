using Microsoft.AspNetCore.Mvc.Rendering;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Author;


namespace New_Web_Library.Service.Core.Interfaces
{
    public interface IAuthorService
    {
        Task <ServiceResult<IEnumerable<AuthorPreviewDetails>>> GetAllAuthorsAsync (string? search);

        Task<ServiceResult<AuthorDetailsForm>> CreateNewAuthorProfileAsync(Guid creatorId);

        Task <ServiceResult<Guid>> ConfirmNewAuthorProfileAsync(AuthorDetailsForm model, Guid creatorId);

        Task<ServiceResult<AuthorPreviewDetails>> GetAllDetailsAuthorAsync(Guid Id);

        Task<ServiceResult<AuthorDetailsForm>> EditAuthorProfileAsync(Guid Id, Guid changerId);

        Task<ServiceResult<bool>> ConfirmEditAuthorProfileAsync(AuthorDetailsForm model, Guid Id, Guid changerId);

        Task<ServiceResult<bool>> HardDeleteAuthorProfileAsync(Guid Id,Guid changerId);

        void AuthorModelImageFiling(AuthorDetailsForm model);
    }
}
