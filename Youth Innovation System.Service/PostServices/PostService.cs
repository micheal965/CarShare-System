using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.IRepositories;
using Youth_Innovation_System.Core.IServices.Cloudinary;
using Youth_Innovation_System.Core.IServices.Post;
using Youth_Innovation_System.Core.Roles;
using Youth_Innovation_System.Core.Specifications.PostSpecifications;
using Youth_Innovation_System.Shared.DTOs.Post;
using Youth_Innovation_System.Shared.Exceptions;
using Youth_Innovation_System.Shared.Pagination;

namespace Youth_Innovation_System.Service.PostServices
{
    public class PostService : IPostService
    {
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork,
                          ICloudinaryServices cloudinaryServices,
                          IMapper mapper,
                          UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryServices = cloudinaryServices;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<PostResponseDto> CreatePostAsync(string userId, CreatePostDto createPostDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NotFoundException("User not found");

            var postRepo = _unitOfWork.Repository<CarPost>();

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newPost = _mapper.Map<CarPost>(createPostDto);

                await postRepo.AddAsync(newPost);
                await _unitOfWork.CompleteAsync();//Save changes to get postId

                //Upload images if provided
                if (createPostDto.Images != null && createPostDto.Images.Count > 0)
                {
                    var postImageRepo = _unitOfWork.Repository<PostImage>();

                    var uploadResults = await _cloudinaryServices.UploadImagesAsync(createPostDto.Images);
                    var postImages = uploadResults.Select(result => new PostImage
                    {
                        PostId = newPost.Id,
                        imageUrl = result.imageUrl,
                        imagePublicId = result.publicId,
                    }).ToList();
                    await postImageRepo.AddRangeAsync(postImages);
                    await _unitOfWork.CompleteAsync();
                }
                await transaction.CommitAsync();
                return _mapper.Map<PostResponseDto>(newPost);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeletePostAsync(int postId, string userId)
        {
            var postRepo = _unitOfWork.Repository<CarPost>();

            //Car owner can't delete car post if it is rented.
            UpdateOrDeletePostSpecification spec = new UpdateOrDeletePostSpecification(postId, userId);
            var post = await postRepo.GetWithSpecAsync(spec);
            if (post == null) return false;

            bool DeletePhotosResult = false;
            if (post.postImages.Count > 0)
            {
                DeletePhotosResult = await _cloudinaryServices.DeleteImagesAsync(post.postImages.Select(pi => pi.imageUrl).ToList());
            }
            if (DeletePhotosResult)
            {
                postRepo.Delete(post);
                if (await _unitOfWork.CompleteAsync() > 0) return true;
            }
            return false;
        }
        public async Task UpdatePostAsync(string userId, UpdatePostDto updatePostDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NotFoundException("User not found.");
            //PostRepository
            var postRepo = _unitOfWork.Repository<CarPost>();

            UpdateOrDeletePostSpecification spec = new UpdateOrDeletePostSpecification(updatePostDto.Id, userId);
            var post = await postRepo.GetWithSpecAsync(spec);
            if (post == null) throw new NotFoundException("There is no post to modify");

            post = _mapper.Map<CarPost>(updatePostDto);

            try
            {
                //Ensuring there are new images
                if (updatePostDto.Images is { Count: > 0 })
                {
                    var DeleteImagesResult = await _cloudinaryServices.DeleteImagesAsync(post.postImages.Select(pi => pi.imageUrl).ToList());
                    var uploadImagesResult = await _cloudinaryServices.UploadImagesAsync(updatePostDto.Images);
                    if (!DeleteImagesResult || uploadImagesResult == null || !uploadImagesResult.Any())
                        throw new Exception("Failed to update post");
                    else
                    {
                        //Delete the old postImages
                        post.postImages.Clear();
                        //Upload new images
                        var postImages = uploadImagesResult.Select(upload => new PostImage()
                        {
                            PostId = post.Id,
                            imagePublicId = upload.publicId,
                            imageUrl = upload.imageUrl,
                        }).ToList();
                        post.postImages.AddRange(postImages);
                    }
                }
                postRepo.Update(post);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PagedResult<PostResponseDto>> GetAllPostsAsync(int pageNumber, int pageSize)
        {
            var postRepo = _unitOfWork.Repository<CarPost>();
            //Specification for pagination
            GetAllPostsSpecification specForPagination = new GetAllPostsSpecification(pageNumber, pageSize);
            var posts = await postRepo.GetAllWithSpecAsync(specForPagination);
            if (posts.Count == 0) throw new NotFoundException("There are no posts");

            //Specification for total count
            GetAllPostsSpecification specForTotalCount = new GetAllPostsSpecification();
            var totalPosts = await postRepo.CountAsyncWithSpec(specForTotalCount);

            var mappedPosts = _mapper.Map<List<PostResponseDto>>(posts);

            return new PagedResult<PostResponseDto>(mappedPosts, totalPosts, pageSize);
        }
        public async Task<PagedResult<PostResponseDto>> GetPostsAfterSearchAsync(CarPostSearchParamaters searchParamaters)
        {
            var postRepo = _unitOfWork.Repository<CarPost>();
            //Specification for Search
            PostSearchSpecification specForSearch = new PostSearchSpecification(searchParamaters.CarType,
                                                                                searchParamaters.RentalPrice,
                                                                                searchParamaters.pageNumber,
                                                                                searchParamaters.PageSize);

            var posts = await postRepo.GetAllWithSpecAsync(specForSearch);
            if (posts.Count == 0) throw new NotFoundException("There are no posts");

            var searchedPosts = _mapper.Map<List<PostResponseDto>>(posts);
            //Specification for total count
            PostSearchSpecification specForTotalCount = new PostSearchSpecification(searchParamaters.CarType,
                                                                                searchParamaters.RentalPrice);

            var totalCount = await postRepo.CountAsyncWithSpec(specForTotalCount);
            return new PagedResult<PostResponseDto>(searchedPosts, totalCount, searchParamaters.PageSize);
        }
        public async Task<PagedResult<PostResponseDto>> GetAllUserPostsAsync(string userId, int pageNumber, int pageSize)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NotFoundException("User not found.");

            var postRepo = _unitOfWork.Repository<CarPost>();
            //for Pagination
            GetAllUserPosts specWithPagination = new GetAllUserPosts(userId, pageNumber, pageSize);
            var posts = await postRepo.GetAllWithSpecAsync(specWithPagination);
            if (posts.Count == 0) throw new NotFoundException("There are no posts");
            //Total count
            GetAllUserPosts specForTotalCount = new GetAllUserPosts(userId);
            int totalRecords = await postRepo.CountAsyncWithSpec(specForTotalCount);

            var postsDto = _mapper.Map<List<PostResponseDto>>(posts);
            return new PagedResult<PostResponseDto>(postsDto, totalRecords, pageSize);
        }
        public async Task<PostResponseDto> GetPostAsync(int postId)
        {
            var postRepo = _unitOfWork.Repository<CarPost>();

            //specifications of Get post (including postimages)
            GetPostSpecification spec = new GetPostSpecification(postId);

            var post = await postRepo.GetWithSpecAsync(spec);
            if (post == null) throw new NotFoundException("Post not found.");

            return _mapper.Map<PostResponseDto>(post);
        }

        public async Task ManagePostAsync(int postId, bool IsAccepted)
        {
            var postRepo = _unitOfWork.Repository<CarPost>();

            var post = await postRepo.GetAsync(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");
            post.RentalStatus = IsAccepted ? CarStatus.Accepted.ToString() :
                                             CarStatus.Rejected.ToString();
            postRepo.Update(post);
            await _unitOfWork.CompleteAsync();
        }

    }
}
