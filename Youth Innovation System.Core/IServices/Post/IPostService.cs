using Youth_Innovation_System.Core.Specifications.PostSpecifications;
using Youth_Innovation_System.Shared.DTOs.Post;
using Youth_Innovation_System.Shared.Pagination;

namespace Youth_Innovation_System.Core.IServices.Post
{
    public interface IPostService
    {
        Task<PostResponseDto> CreatePostAsync(string userId, CreatePostDto createPostDto);
        Task<bool> DeletePostAsync(int postId, string userId);
        Task UpdatePostAsync(string userId, UpdatePostDto updatePostDto);
        Task<PagedResult<PostResponseDto>> GetPostsAfterSearchAsync(CarPostSearchParamaters searchParamaters);
        Task<PagedResult<PostResponseDto>> GetAllPostsAsync(int pageNumber, int pageSize);
        Task<PagedResult<PostResponseDto>> GetAllUserPostsAsync(string userId, int pageNumber, int pageSize);
        Task<PostResponseDto> GetPostAsync(int postId);
        Task ManagePostAsync(int postId, bool IsAccepted);
        Task<IReadOnlyList<PostResponseDto>> GetPendingPostsAsync();

    }
}
