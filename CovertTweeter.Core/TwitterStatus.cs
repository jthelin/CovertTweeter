namespace CovertTweeter.Core
{
    public class TwitterStatus
    {
        public string  CreatedAt { get; set; }
        public long Id { get; set; }
        public string Text { get; set; }

        public string InReplyToScreenName { get; set; }
        public long? InReplyToUserId { get; set; }
        public TwitterStatus RetweededStatus { get; set; }

        public TwitterUser User { get; set; }
    }
}