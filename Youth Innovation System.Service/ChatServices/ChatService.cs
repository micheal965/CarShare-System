using Microsoft.AspNetCore.Identity;
using Youth_Innovation_System.Core.Entities.Chat;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.IRepositories;
using Youth_Innovation_System.Core.IServices.IChat;
using Youth_Innovation_System.Core.Specifications.ChatSpecifications;

namespace Youth_Innovation_System.Service.ChatServices
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<bool> DeleteMessageAsync(int messageId, string userId)
        {
            var messageRepo = _unitOfWork.Repository<Message>();

            DeleteMessageSpecifications spec = new DeleteMessageSpecifications(messageId, userId);
            var message = await messageRepo.GetWithSpecAsync(spec);

            if (message == null) return false;

            messageRepo.Delete(message);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IReadOnlyList<Message>> GetChatHistoryAsync(string senderId, string receiverId)
        {
            var sender = await _userManager.FindByIdAsync(senderId);
            if (sender == null)
                throw new UnauthorizedAccessException("User is unauthorized");

            FetchChatHistorySpecifications spec = new FetchChatHistorySpecifications(senderId, receiverId);
            return await _unitOfWork.Repository<Message>().GetAllWithSpecAsync(spec);
        }
    }
}
