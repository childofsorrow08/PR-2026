using PostService.Models;

namespace PostService.BusinessLogic;

public class PostingService : IPostingService {
    private static readonly object sync = new();
    private static readonly List<Posting> postings = [];
    private static int next_id = 1;

    public Posting Create(Posting posting) {
        PostingRules.Validate(posting);
        posting.Price = PostingRules.CalculatePrice(posting);
        posting.CreatedAt = DateTime.UtcNow;

        lock (sync) {
            posting.Id = next_id++;
            postings.Add(posting);
            return posting;
        }
    }

    public List<Posting> GetAll() { lock (sync) return postings.ToList(); }
    public Posting? Find(int id) { lock (sync) return postings.Find(item => item.Id == id); }

    public Posting? Update(Posting posting) {
        PostingRules.Validate(posting);
        posting.Price = PostingRules.CalculatePrice(posting);

        lock (sync) {
            int index = postings.FindIndex(item => item.Id == posting.Id);
            if (index < 0) return null;

            posting.CreatedAt = postings[index].CreatedAt;
            postings[index] = posting;
            return posting;
        }
    }

    public int Delete(int id) { lock (sync) return postings.RemoveAll(item => item.Id == id); }
}
