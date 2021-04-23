

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

        [Post("/api/app/task/search-my-tasks")]
        Task<ApiResponse<TaskDto[]>> SearchMyTasks(GetMyTaskRequestDto request);

        [Get("/api/app/task/my-actions")]
        Task<ApiResponse<TaskDto[]>> GetMyActions();

        [Get("/api/app/task/{id}/task")]
        Task<ApiResponse<FullTaskDto>> GetTask(long id);

        [Get("/api/app/task/{id}/reference-tasks")]
        Task<ApiResponse<FullTaskDto[]>> GetReferenceTasks(long id);

        [Get("/api/app/task/{id}/task-comments")]
        Task<ApiResponse<TaskCommentDto[]>> GetTaskComments(long id);

        [Get("/api/app/task/{id}/task-history")]
        Task<ApiResponse<TaskHistoryDto[]>> GetHistory(long id);

        [Get("/api/app/task/{id}/note")]
        Task<ApiResponse<string>> GetNote(long id);

        [Get("/api/app/task/{id}/task-files")]
        Task<ApiResponse<FileDto[]>> GetTaskFiles(long id);

        [Get("/api/app/task/assignee?target={target}")]
        Task<ApiResponse<SimpleUserDto>> GetAssignee(int target);
        
        [Get("/api/app/task/member?target={target}")]
        Task<ApiResponse<SimpleUserDto[]>> GetMembers(int target);

        [Multipart]
        [Post("/api/File/UploadFile")]
        Task<ApiResponse<FileDto>> Upload([AliasAs("file")] StreamPart bytes);

        [Get("/api/File/DownloadFile?id={id}")]
        Task<ApiResponse<byte[]>> DownloadFile(Guid id);

        [Post("/api/app/task/task")]
        Task<ApiResponse<CreateTaskResultDto>> CreateTask(CreateTaskRequestDto request);

        [Put("/api/app/task/task")]
        Task<ApiResponse<CreateTaskResultDto>> UpdateTask(UpdateTaskRequestDto request);

        [Post("/api/app/task/process-task")]
        Task<ApiResponse<bool>> ApproveTask(ApproveTaskRequestDto request);

        [Post("/api/app/task/finish-task")]
        Task<ApiResponse<bool>> FinishTask(FinishTaskRequestDto request);

        [Post("/api/app/task/follow-task")]
        Task<ApiResponse<bool>> FollowTask(FollowTaskRequestDto request);

        [Post("/api/app/task/reopen-task")]
        Task<ApiResponse<bool>> ReopenTask(ReopenTaskRequestDto request);

        [Post("/api/app/task/request-task")]
        Task<ApiResponse<bool>> RequestTask(ReopenTaskRequestDto request);

        [Post("/api/app/task/{id}/send-comment")]
        Task<ApiResponse<bool>> SendComment(long id, CommentRequestDto request);
        
        [Post("/api/app/device-token/save-device-token")]
        Task<ApiResponse<bool>> SaveDeviceToken(SaveDeviceTokenRequestDto request);


        [Get("/api/app/guide/guide")]
        Task<ApiResponse<GuideResultDto>> GetGuide();

        [Get("/api/app/department/my-departments")]
        Task<ApiResponse<DepartmentUserDto[]>> GetMyDepartment();

        [Get("/api/app/task/users-in-my-tasks")]
        Task<ApiResponse<SimpleUserDto[]>> GetUsersInMyTasks();

        [Get("/api/app/target/targets")]
        Task<ApiResponse<TargetDto[]>> GetAllTargets();

        [Post("/api/app/task/rate-task/{taskId}")]
        Task<ApiResponse<bool>> Rate(long taskId, [Body]RatingRequestDto rating);

    }
}