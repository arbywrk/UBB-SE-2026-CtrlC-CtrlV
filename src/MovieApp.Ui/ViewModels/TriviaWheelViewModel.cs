using MovieApp.Core.Models;
using MovieApp.Core.Repositories;

namespace MovieApp.Ui.ViewModels;

public sealed class TriviaWheelViewModel : ViewModelBase
{
    private readonly ITriviaRepository _triviaRepository;

    private string _selectedCategory = string.Empty;
    private bool _canSpin = true;
    private bool _isPlaying;
    private bool _isSessionComplete;
    private TriviaQuestion? _currentQuestion;
    private int _currentQuestionIndex;
    private int _score;
    private bool _hintUsed;

    public TriviaWheelViewModel(ITriviaRepository triviaRepository)
    {
        _triviaRepository = triviaRepository;
    }

    public IReadOnlyList<string> Categories { get; } = new List<string>
    {
        "Actors",
        "Directors",
        "Movie Quotes",
        "Oscars and Awards",
        "General Movie Trivia"
    };

    public bool CanSpin
    {
        get => _canSpin;
        private set
        {
            _canSpin = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(RemainingSpinsText));
        }
    }

    public string RemainingSpinsText => CanSpin
        ? "1 spin available today"
        : "Next spin available tomorrow";

    public string SelectedCategory
    {
        get => _selectedCategory;
        private set
        {
            _selectedCategory = value;
            OnPropertyChanged();
        }
    }

    public bool IsPlaying
    {
        get => _isPlaying;
        private set
        {
            _isPlaying = value;
            OnPropertyChanged();
        }
    }

    public bool IsSessionComplete
    {
        get => _isSessionComplete;
        private set
        {
            _isSessionComplete = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasEarnedReward));
        }
    }

    public TriviaQuestion? CurrentQuestion
    {
        get => _currentQuestion;
        private set
        {
            _currentQuestion = value;
            OnPropertyChanged();
        }
    }

    public int CurrentQuestionIndex
    {
        get => _currentQuestionIndex;
        private set
        {
            _currentQuestionIndex = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ProgressValue));
            OnPropertyChanged(nameof(ProgressText));
        }
    }

    public int Score
    {
        get => _score;
        private set
        {
            _score = value;
            OnPropertyChanged();
        }
    }

    public bool HintUsed
    {
        get => _hintUsed;
        private set
        {
            _hintUsed = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsHintAvailable));
        }
    }

    public bool IsHintAvailable => !HintUsed;

    public bool HasEarnedReward => Score == 20;

    public double ProgressValue => CurrentQuestionIndex / 20.0 * 100;

    public string ProgressText => $"{CurrentQuestionIndex}/20";
}