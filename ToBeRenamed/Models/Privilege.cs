using System.Collections.Generic;
using System.Linq;

namespace ToBeRenamed.Models
{
    public class Privilege
    {
        public static Privilege CanSubmitVideo = new Privilege("can_submit_video");
        public static Privilege CanRemoveOwnVideo = new Privilege("can_remove_own_video");
        public static Privilege CanRemoveAnyVideo = new Privilege("can_remove_any_video");

        public static ISet<Privilege> All()
        {
            return new[]
            {
                CanSubmitVideo,
                CanRemoveOwnVideo,
                CanRemoveAnyVideo
            }.ToHashSet();
        }

        private Privilege(string alias)
        {
            Alias = alias;
        }

        public string Alias { get; }
    }
}
