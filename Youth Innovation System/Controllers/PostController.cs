using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Youth_Innovation_System.Core.IServices.Post;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.Core.Specifications.PostSpecifications;
using Youth_Innovation_System.Shared.ApiResponses;
using Youth_Innovation_System.Shared.DTOs.Post;
using Youth_Innovation_System.Shared.Exceptions;

namespace Youth_Innovation_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize(Roles = nameof(UserRoles.CarOwner))]
        [HttpPost("Create-Post")]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                var Result = await _postService.CreatePostAsync(userId, createPostDto);
                return Ok(Result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }
        //if its rented so cant delete it
        [Authorize(Roles = nameof(UserRoles.CarOwner))]
        [HttpDelete("Delete-Post/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _postService.DeletePostAsync(postId, userId);
            if (!result)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Can't delete post"));

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Post deleted successfully"));
        }

        [Authorize(Roles = nameof(UserRoles.CarOwner))]
        [HttpPut("Update-Post")]
        public async Task<IActionResult> UpdatePost(UpdatePostDto updatePostDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                await _postService.UpdatePostAsync(userId, updatePostDto);
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Post updated successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }

        }
        [HttpGet("Get-Post/{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            try
            {
                return Ok(await _postService.GetPostAsync(postId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, ex.Message));
            }

        }
        [HttpGet("Get-User-Posts")]
        public async Task<IActionResult> GetUserPosts(GetUserPostsDto getUserPostsDto)
        {
            try
            {
                var pagedPosts = await _postService.GetAllUserPostsAsync(getUserPostsDto.userId,
                                                                       getUserPostsDto.pageNumber,
                                                                       getUserPostsDto.pageSize);
                return Ok(pagedPosts);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }
        [HttpGet("Get-All-Posts")]
        public async Task<IActionResult> GetAllPosts(GetAllPostsDto getAllPostsDto)
        {
            try
            {
                //With Pagination
                var pagedPosts = await _postService.GetAllPostsAsync(getAllPostsDto.pageNumber, getAllPostsDto.pageSize);
                return Ok(pagedPosts);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }
        [HttpGet("Search-For-Cars")]
        public async Task<IActionResult> SearchForCars([FromQuery] CarPostSearchParamaters searchParamaters)
        {
            try
            {
                var SearchedPosts = await _postService.GetPostsAfterSearchAsync(searchParamaters);
                return Ok(SearchedPosts);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpPost("Manage-Post/{postId}")]
        public async Task<IActionResult> ManagePost(ManagePostDto managePostDto)
        {
            try
            {
                await _postService.ManagePostAsync(managePostDto.postId, managePostDto.IsAccepted);
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Post managed successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }
        //[Authorize]
        //[HttpGet("Get-Feedbacks/{postId}")]
        //public async Task<IActionResult> GetFeedbacks(int postId)
        //{
        //    var result = await _rentalApplicationService.GetFeedbacksAsync(postId);
        //    return StatusCode(result.StatusCode, result);

        //}
    }
}
