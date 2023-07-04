namespace The49.Maui.Toolkit.Sample.Models;

public class SocialPost
{
    public string Text { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTimeOffset DateCreated { get; set; }

    internal static IEnumerable<SocialPost> GetSample()
    {
        return new List<SocialPost>
        {
            new SocialPost
            {
                UserName = "socialUser1",
                AvatarUrl = "user.png",
                Text = "Just had an amazing day! 😃"
            },
            new SocialPost
            {
                UserName = "socialUser2",
                AvatarUrl = "user.png",
                Text = "My favorite song on repeat. Can't get enough of it! 🎶"
            },
            new SocialPost
            {
                UserName = "socialUser3",
                AvatarUrl = "user.png",
                Text = "Coffee + good company = perfect morning! ☕️❤️"
            },
            new SocialPost
            {
                UserName = "socialUser4",
                AvatarUrl = "user.png",
                Text = "Volunteered at a local animal shelter today. Seeing the wagging tails and grateful eyes of the rescued pets brought me so much joy. Adopt, don't shop! 🐾❤️🏠"
            },
            new SocialPost
            {
                UserName = "socialUser5",
                AvatarUrl = "user.png",
                Text = "Spent the day exploring a historic city and immersing myself in its rich culture. From ancient architecture to mouthwatering street food, it was a sensory overload! 🏰🍲📸"
            },
            new SocialPost
            {
                UserName = "socialUser6",
                AvatarUrl = "user.png",
                Text = "Weekend getaway with friends. Fun times guaranteed! 🚀🌴"
            },
            new SocialPost
            {
                UserName = "socialUser7",
                AvatarUrl = "user.png",
                Text = "Movie marathon tonight! Any recommendations? 🎥🍿"
            },
            new SocialPost
            {
                UserName = "socialUser8",
                AvatarUrl = "user.png",
                Text = "Hiked to the top of a mountain. The view was breathtaking! ⛰️✨"
            },
            new SocialPost
            {
                UserName = "socialUser9",
                AvatarUrl = "user.png",
                Text = "Visited a local farmer's market today and discovered an array of fresh, organic produce. Can't wait to cook up a healthy feast! 🥦🍅🍆"
            },
            new SocialPost
            {
                UserName = "socialUser10",
                AvatarUrl = "user.png",
                Text = "New recipe experiment in the kitchen. Fingers crossed it turns out delicious! 🍳👩‍🍳"
            },
            new SocialPost
            {
                UserName = "socialUser11",
                AvatarUrl = "user.png",
                Text = "Embarked on a solo backpacking trip through the mountains. The solitude and serenity are invigorating. It's a journey of self-discovery and personal growth. 🏞️🎒💪"
            },
            new SocialPost
            {
                UserName = "socialUser12",
                AvatarUrl = "user.png",
                Text = "Lost in a captivating book. The characters feel so real! 📚"
            },
            new SocialPost
            {
                UserName = "socialUser13",
                AvatarUrl = "user.png",
                Text = "Excited about the upcoming concert. Can't wait to sing along! 🎤🎵"
            },
            new SocialPost
            {
                UserName = "socialUser14",
                AvatarUrl = "user.png",
                Text = "Took a stroll along the beach at sunset and witnessed a mesmerizing display of colors painting the sky. Nature's artwork never ceases to amaze me. 🌅🎨✨"
            },
            new SocialPost
            {
                UserName = "socialUser15",
                AvatarUrl = "user.png",
                Text = "Spontaneous road trip with friends. Adventure awaits! 🚗💨"
            },
            new SocialPost
            {
                UserName = "socialUser16",
                AvatarUrl = "user.png",
                Text = "Found a hidden gem of a cafe. Their pastries are heavenly! 🥐😋"
            },
            new SocialPost
            {
                UserName = "socialUser17",
                AvatarUrl = "user.png",
                Text = "Reflecting on the beauty of nature. Grateful for moments like these. 🌳🌸"
            },
            new SocialPost
            {
                UserName = "socialUser18",
                AvatarUrl = "user.png",
                Text = "Attended a thought-provoking lecture on the impact of technology in our society. It sparked lively discussions and challenged my perspective. 📱💡💬"
            }
        };
    }
}
