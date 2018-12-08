using System.Collections.Generic;
using System.Linq;

namespace ToBeRenamed.Models
{
    public class Privilege
    {
        public static Privilege CanManageMembersAndRoles = new Privilege("can_manage_members_and_roles");
        public static Privilege CanEditLibrary = new Privilege("can_edit_library");
        public static Privilege CanRemoveLibrary = new Privilege("can_remove_library");
        public static Privilege CanEditVideo = new Privilege("can_edit_video");
        public static Privilege CanSubmitVideo = new Privilege("can_submit_video");
        public static Privilege CanRemoveOwnVideo = new Privilege("can_remove_own_video");
        public static Privilege CanRemoveAnyVideo = new Privilege("can_remove_any_video");
        public static Privilege CanSeeAllAnnotations = new Privilege("can_see_all_annotations");

        public static ISet<Privilege> All()
        {
            return new[]
            {
                CanManageMembersAndRoles,
                CanEditLibrary,
                CanRemoveLibrary,
                CanEditVideo,
                CanSubmitVideo,
                CanRemoveOwnVideo,
                CanRemoveAnyVideo,
                CanSeeAllAnnotations
            }.ToHashSet();
        }

        private Privilege(string alias)
        {
            Alias = alias;
        }

        public string Alias { get; }
    }
}
