using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Forum_Team2
{
    public partial class comments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Member member = new Member();
            member.PasteDataTopicInTopics(phTopic);
            member.PasteDataComments(phComments);
        }

        protected void btnPostComment_Click(object sender, EventArgs e)
        {
            Member member = new Member();
            member.CreateComment(txtCommentText);
        }

        protected void btnVote_Click(object sender, EventArgs e)
        {
            Member member = new Member();
            member.UpVoteSystem(btnUpvote);
        }
    }
}