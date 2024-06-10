using Microsoft.AspNetCore.Identity;

namespace Snackis.Services
{
    public interface IHelpers
    {
        Task<string> FindUserName(string userId);

        Task<string> FindUsername(string userId);

        Task<string> FindUserProfilePicture(string userId);

        Task<bool> AlreadyFlaggedByMe(int forumPostId, string userId);

        Task<Models.ForumPost> GetCurrentForumPost(int forumPostId);

        Task<string> GetGroupName(int groupId);

        Task<string> GetGropOwnerUsername(int groupId);

        Task<int> FindTopLevelPostId(int postId);
    }
}
