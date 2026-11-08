using PostService.DataAccess;
using PostService.Models;

namespace PostService.BusinessLogic;

public class PostingService(IPostingRepository repository) : IPostingService {
    public Posting Create(Posting posting) {
        PostingRules.Validate(posting);
        posting.Price = PostingRules.CalculatePrice(posting);
        posting.CreatedAt = DateTime.UtcNow;
        posting.Id = repository.Create(posting);
        return posting;
    }

    public List<Posting> GetAll() { return repository.GetList(); }
    public Posting? Find(int id) { return repository.GetById(id); }

    public Posting? Update(Posting posting) {
        PostingRules.Validate(posting);
        var previous = repository.GetById(posting.Id);
        if (previous is null) return null;

        posting.CreatedAt = previous.CreatedAt;
        posting.Price = PostingRules.CalculatePrice(posting);
        return repository.Update(posting) == 0 ? null : posting;
    }

    public int Delete(int id) { return repository.Delete(id); }
}
