

using PMVOnline.Api.Dtos.Accounts;
using PMVOnline.Api.Dtos.Authorizations;
using PMVOnline.Api.Dtos.Tasks;
using Refit;
using System;
using System.Threading.Tasks;

namespace PMVOnline.Api
{
    public interface AppApi
    {
        [Post("/connect/token")]
        Task<ApiResponse<TokenDto>> ConnectToken([Body(BodySerializationMethod.UrlEncoded)] ConnectTokenRequestDto request);

        [Get("/api/identity/my-profile")]
        Task<ApiResponse<ProfileDto>> GetMyProfile();

        [Get("/api/app/task/my-tasks?MaxResultCount={max}&SkipCount={skip}")]
        Task<ApiResponse<TaskDto[]>> GetMyTasks(int skip, int max);

        [Get("/api/app/task/my-actions")]
        Task<ApiResponse<TaskDto[]>> GetMyActions();

        [Get("/api/app/task/{id}/task")]
        Task<ApiResponse<FullTaskDto>> GetTask(long id);

        [Get("/api/app/task/{id}/task-comments")]
        Task<ApiResponse<TaskCommentDto[]>> GetTaskComments(long id);

        [Get("/api/app/task/{id}/task-history?SkipCount={skip}&MaxResultCount={max}")]
        Task<ApiResponse<TaskHistoryDto[]>> GetHistory(long id, int skip = 0,int max = 20);

        [Get("/api/app/task/{id}/task-files")]
        Task<ApiResponse<FileDto[]>> GetTaskFiles(long id);
        
        [Get("/api/app/task/assignee?target={target}")]
        Task<ApiResponse<SimpleUserDto>> GetAssignee(int target);

        [Multipart]
        [Post("/api/File/UploadFile")]
        Task<ApiResponse<FileDto>> Upload([AliasAs("file")] StreamPart bytes);

        [Get("/api/File/DownloadFile?id={id}")]
        Task<ApiResponse<byte[]>> DownloadFile(Guid id);

        [Post("/api/app/task/task")]
        Task<ApiResponse<CreateTaskResultDto>> CreateTask(CreateTaskRequestDto request);
    }
}