using PostService.Models;

namespace PostService.Services;

public interface IPostingService {
    Posting Create(Posting posting);
    List<Posting> GetAll();
    Posting? Find(int id);
    Posting? Update(Posting posting);
    int Delete(int id);
}
